using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Simulator.Models;

namespace Simulator.Factories
{

    /// <summary>
    /// 薬物動態モデルのファクトリクラス。
    /// 設定読み込みやポリモルフィズムができていないので現状ではテスト用モデルを作成するだけのクラス。
    /// </summary>
    public class PharmacokineticModelFactory
    {
        public double Weight { get; set; }
        public double Stat { get; set; }
        public double Age { get; set; }
        public bool IsMale { get; set; }

        // パラメータを保持
        private Dictionary<string, double> _CustomParams = new Dictionary<string, double>();

        private bool _IsCustomParamsComliled = false;

        // Compute関数を呼ぶためだけのオブジェクト
        private DataTable _ParamTable = new DataTable();

        private Regex _ParamRegex = new Regex("<(.*?)>", RegexOptions.Compiled);

        public PharmacokineticModelFactory()
        {
        }

        public PharmacokineticModelFactory(double weight, double stat, double age, bool isMale)
        {
            Weight = weight;
            Stat = stat;
            Age = age;
            IsMale = isMale;

            CompileCustomParams();

        }

        public PharmacokineticModelFactory(IndividualModel individual)
        : this(individual.Weight, individual.Stat, individual.Age, individual.IsMale)
        {
        }


        public PharmacokineticModel Create(int modelNumber)
        {
            var section = Configurations.Config.Sections[$"model_{modelNumber}"];
            string name = section.Keys["name"].Value;
            double k10 = Evaluate(section.Keys["k10"].Value);
            double k12 = Evaluate(section.Keys["k12"].Value);
            double k13 = Evaluate(section.Keys["k13"].Value);
            double k21 = Evaluate(section.Keys["k21"].Value);
            double k31 = Evaluate(section.Keys["k31"].Value);
            double ke0 = Evaluate(section.Keys["ke0"].Value);
            double v1 = Evaluate(section.Keys["V1"].Value);

            var model = new PharmacokineticModel(name, k10, k12, k13, k21, k31, ke0, Weight);
            model.V1 = v1;
            model.V2 = (section.Keys.ContainsKey("V2")) ? Evaluate(section.Keys["V2"].Value) : k12 * v1 / k21;
            model.V3 = (section.Keys.ContainsKey("V3")) ? Evaluate(section.Keys["V3"].Value) : k13 * v1 / k31;
            return model;

        }

        private double Evaluate(string expression)
        {
            // 基本的な情報で置換する
            string replaced = ReplaceProperty(expression);

            // 辞書からカスタムパラメータを取得して置換する
            if (_IsCustomParamsComliled && _ParamRegex.IsMatch(replaced))
            {
                foreach (Match item in _ParamRegex.Matches(replaced))
                {
                    if (TryGetParam(item.Groups[1].Value, out var val))
                    {
                        replaced = replaced.Replace(item.Value, val.ToString());
                    }
                }
            }

            Type t = Type.GetTypeFromProgID("MSScriptControl.ScriptControl");
            object obj = Activator.CreateInstance(t);
            t.InvokeMember("Language", BindingFlags.SetProperty, null, obj, new object[] {"vbscript"});
            //Eval関数で計算を実行して結果を取得
            double result = (double) t.InvokeMember("Eval", BindingFlags.InvokeMethod, null, obj, new object[] { replaced });
            return result;
        }

        /// <summary>
        /// プロパティによって置換できるパラメータを置換する
        /// </summary>
        /// <param name="expression">置換対象の式</param>
        /// <returns>置換された式</returns>
        private string ReplaceProperty(string expression)
        {
            // 基本的な情報で置換する
            string replaced = expression;
            replaced = replaced.Replace("<体重>", Weight.ToString());
            replaced = replaced.Replace("<年齢>", Age.ToString());
            replaced = replaced.Replace("<身長>", Stat.ToString());
            return replaced;
        }

        /// <summary>
        /// 辞書からパラメータを探す
        /// </summary>
        /// <param name="key">パラメータ名</param>
        /// <param name="value">値</param>
        /// <returns>取得に成功したらTrueを返す</returns>
        private bool TryGetParam(string key, out double value)
        {
            // 先に性別分岐パラメータを探す
            string suffix = IsMale ? "M" : "F";
            if (_CustomParams.ContainsKey($"{key}_{suffix}"))
            {
                value = _CustomParams[$"{key}_{suffix}"];
                return true;
            }

            if (_CustomParams.ContainsKey(key))
            {
                value = _CustomParams[key];
                return true;
            }

            value = 0;
            return false;
        }

        /// <summary>
        /// カスタムパラメータを読み込む
        /// </summary>
        private void CompileCustomParams()
        {
            // 依存関係が複雑なパラメータがあるかもしれないので10回まで繰り替えす
            for (int i = 0; i < 10; i++)
            {
                foreach (var param in Configurations.Config.Sections["CustomParams"].GetIniValues())
                {
                    if (_CustomParams.ContainsKey(param.KeyName))
                    {
                        continue;
                    }

                    string replaced = ReplaceProperty(param.Value);

                    if (_ParamRegex.IsMatch(replaced))
                    {
                        
                        foreach (Match item in _ParamRegex.Matches(replaced))
                        {
                            if (TryGetParam(item.Groups[1].Value, out var val))
                            {
                                replaced = replaced.Replace(item.Value, val.ToString());
                            }
                        }

                        // 変換してパラメータが残っていなければ完了
                        if (!_ParamRegex.IsMatch(replaced))
                        {
                            _CustomParams.Add(param.KeyName, Evaluate(replaced));
                        }
                    }
                    else
                    {
                        _CustomParams.Add(param.KeyName, Evaluate(param.Value));
                    }
                }
            }

            _IsCustomParamsComliled = true;
        }


    }
}
