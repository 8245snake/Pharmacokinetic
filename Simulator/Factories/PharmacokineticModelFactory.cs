using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simulator.Models;

namespace Simulator.Factories
{

    /// <summary>
    /// 薬物動態モデルのファクトリクラス。
    /// </summary>
    public class PharmacokineticModelFactory : IPharmacokineticFactory
    {
        private double _weight;
        private bool _weightInitialized = false;
        private double _stat;
        private bool _statInitialized = false;
        private double _age;
        private bool _ageInitialized = false;
        private bool _isMale;
        private bool _isMaleInitialized = false;

        /// <summary>
        /// 体重
        /// </summary>
        public double Weight
        {
            get => _weight;
            set { _weight = value; _weightInitialized = true; }
        }

        /// <summary>
        /// 身長
        /// </summary>
        public double Stat
        {
            get => _stat;
            set { _stat = value; _statInitialized = true; }
        }

        /// <summary>
        /// 年齢
        /// </summary>
        public double Age
        {
            get => _age;
            set { _age = value; _ageInitialized = true; }
        }

        /// <summary>
        /// 性別（True：男性、False：女性）
        /// </summary>
        public bool IsMale
        {
            get => _isMale;
            set { _isMale = value; _isMaleInitialized = true; }
        }


        // パラメータを保持する辞書
        private Dictionary<string, double> _CustomParams = new Dictionary<string, double>();
        // カスタムパラメータを読み終わったかのフラグ
        private bool _IsCustomParamsComliled = false;
        // パラメータ解析用の正規表現
        private Regex _ParamRegex = new Regex("<(.*?)>", RegexOptions.Compiled);

        /// <summary>
        /// パラメータを指定せずにファクトリを生成します。
        /// </summary>
        public PharmacokineticModelFactory()
        {
        }

        /// <summary>
        /// 体重、身長、年齢、性別を指定してファクトリを生成します。
        /// </summary>
        /// <param name="weight">体重</param>
        /// <param name="stat">身長</param>
        /// <param name="age">年齢</param>
        /// <param name="isMale">性別（True：男性、False：女性）</param>
        public PharmacokineticModelFactory(double weight, double stat, double age, bool isMale)
        {
            Weight = weight;
            Stat = stat;
            Age = age;
            IsMale = isMale;

            // パラメータをパースする
            CompileCustomParams();
        }

        /// <summary>
        /// 個人データを使用してファクトリを生成します。
        /// </summary>
        /// <param name="individual">個人データ</param>
        public PharmacokineticModelFactory(IndividualModel individual)
        : this(individual.Weight, individual.Stat, individual.Age, individual.IsMale)
        {
        }

        /// <summary>
        /// 個人パラメータをセットします。
        /// </summary>
        /// <param name="individual">個人データ</param>
        public void SetIndivisual(IndividualModel individual)
        {
            Weight = individual.Weight;
            Stat = individual.Stat;
            Age = individual.Age;
            IsMale = individual.IsMale;

            // パラメータを初期化
            _IsCustomParamsComliled = false;
            _CustomParams = new Dictionary<string, double>();
            // パラメータをパースする
            CompileCustomParams();
        }

        /// <summary>
        /// 薬物動態モデルを作成します。
        /// </summary>
        /// <param name="modelName">モデル名。Model.iniのmodel_XXXキーのXXXの部分。</param>
        /// <returns>薬物動態モデル</returns>
        /// <exception cref="ParseErrorException">式のパースに失敗した場合</exception>
        public PharmacokineticModel Create(string modelName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// パラメータを置換して文字列の式を計算した結果を返す。
        /// </summary>
        /// <param name="expression">式</param>
        /// <returns>計算結果</returns>
        /// <exception cref="ParseErrorException">式のパースに失敗した場合</exception>
        private double Evaluate(string expression)
        {
            var exceptions = new List<string>();

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
                    else
                    {
                        exceptions.Add(item.Value);
                    }
                }
            }

            if (exceptions.Count > 0)
            {
                // 解析不能なら例外を出す
                throw new ParseErrorException(exceptions);
            }

            Type t = Type.GetTypeFromProgID("MSScriptControl.ScriptControl");
            object obj = Activator.CreateInstance(t);
            t.InvokeMember("Language", BindingFlags.SetProperty, null, obj, new object[] { "vbscript" });
            //Eval関数で計算を実行して結果を取得
            double result = (double)t.InvokeMember("Eval", BindingFlags.InvokeMethod, null, obj, new object[] { replaced });
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

            if (_weightInitialized)
            {
                replaced = replaced.Replace("<体重>", Weight.ToString());
            }
            if (_statInitialized)
            {
                replaced = replaced.Replace("<身長>", Stat.ToString());
            }
            if (_ageInitialized)
            {
                replaced = replaced.Replace("<年齢>", Age.ToString());

            }
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
            if (_CustomParams.ContainsKey($"{key}_{suffix}") && _isMaleInitialized)
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
            //// 依存関係が複雑なパラメータがあるかもしれないので10回まで繰り返す
            //for (int i = 0; i < 10; i++)
            //{
            //    foreach (var param in Configurations.Config.Sections["CustomParams"].GetIniValues())
            //    {
            //        if (_CustomParams.ContainsKey(param.KeyName))
            //        {
            //            continue;
            //        }

            //        string replaced = ReplaceProperty(param.Value);

            //        if (_ParamRegex.IsMatch(replaced))
            //        {

            //            foreach (Match item in _ParamRegex.Matches(replaced))
            //            {
            //                if (TryGetParam(item.Groups[1].Value, out var val))
            //                {
            //                    replaced = replaced.Replace(item.Value, val.ToString());
            //                }
            //            }

            //            // 変換してパラメータが残っていなければ完了
            //            if (!_ParamRegex.IsMatch(replaced))
            //            {
            //                _CustomParams.Add(param.KeyName, Evaluate(replaced));
            //            }
            //        }
            //        else
            //        {
            //            _CustomParams.Add(param.KeyName, Evaluate(param.Value));
            //        }
            //    }
            //}

            _IsCustomParamsComliled = true;
        }


    }
}
