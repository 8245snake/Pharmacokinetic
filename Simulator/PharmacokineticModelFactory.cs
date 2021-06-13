using System.IO;
using System.Reflection;
using IniUtils;

namespace Simulator
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

        public PharmacokineticModelFactory()
        {
        }

        public PharmacokineticModelFactory(double weight, double stat, double age, bool isMale)
        {
            Weight = weight;
            Stat = stat;
            Age = age;
            IsMale = isMale;
        }


        public PharmacokineticModel Create(int modelNumber)
        {
            var section = Configurations.Config?.Sections[$"model_{modelNumber}"];
            string name = section.Keys["name"].Value;
            double k10 =Evaluate( section.Keys["k10"].Value);
            double k12 =Evaluate( section.Keys["k12"].Value);
            double k13 =Evaluate( section.Keys["k13"].Value);
            double k21 =Evaluate( section.Keys["k21"].Value);
            double k31 =Evaluate( section.Keys["k31"].Value);
            double ke0 =Evaluate( section.Keys["ke0"].Value);
            double v1 = Evaluate(section.Keys["V1"].Value);


            var model = new PharmacokineticModel(name, k10, k12, k13, k21, k31, ke0, Weight);
            model.V1 = v1;
            model.V2 = (section.Keys.ContainsKey("V2")) ? Evaluate(section.Keys["V2"].Value) : k12 * v1 / k21;
            model.V3 = (section.Keys.ContainsKey("V3")) ? Evaluate(section.Keys["V3"].Value) : k13 * v1 / k31;
            return model;

        }

        private double Evaluate(string expression)
        {
            string replaced = expression;
            replaced = replaced.Replace("<体重>", Weight.ToString());
            replaced = replaced.Replace("<年齢>", Age.ToString());
            replaced = replaced.Replace("<身長>", Stat.ToString());

            var dt = new System.Data.DataTable();
            return double.Parse(dt.Compute(replaced, "").ToString());
        }


        /// <summary>
        /// フェンタニル（Shaferモデル）を作成するテスト関数
        /// </summary>
        /// <param name="weight">体重(kg)</param>
        /// <returns>モデル</returns>
        public static PharmacokineticModel CreateFentanyl1(int weight)
        {
            //速度定数(h^-1)
            double k10 = 0.083;
            double k12 = 0.471;
            double k13 = 0.225;
            double k21 = 0.102;
            double k31 = 0.006;
            double ke0 = 0.114;
            //分布容積
            double v1 = 0.105;

            var model = new PharmacokineticModel("ﾌｪﾝﾀﾆﾙ_Shafer", k10, k12, k13, k21, k31, ke0, v1, weight);
            return model;

        }

        /// <summary>
        /// プロポフォール（Marshモデル）を作成するテスト関数
        /// </summary>
        /// <param name="weight">体重(kg)</param>
        /// <returns>モデル</returns>
        public static PharmacokineticModel CreatePropofol1(int weight)
        {
            //速度定数(h^-1)
            double k10 = 0.119;
            double k12 = 0.114;
            double k13 = 0.042;
            double k21 = 0.055;
            double k31 = 0.003;
            double ke0 = 0.260;
            //分布容積
            double v1 = 0.228;

            return new PharmacokineticModel("ﾌﾟﾛﾎﾟﾌｫｰﾙ_Marsh", k10, k12, k13, k21, k31, ke0, v1, weight);
        }

        /// <summary>
        /// プロポフォール（Schniderモデル）を作成するテスト関数
        /// </summary>
        /// <param name="weight">体重(kg)</param>
        /// <returns>モデル</returns>
        /// <remarks>体重50kgで固定する</remarks>
        public static PharmacokineticModel CreatePropofol2(int weight = 50)
        {
            var model = new PharmacokineticModel("ﾌﾟﾛﾎﾟﾌｫｰﾙ_Schnider", weight)
            {
                K10 = 0.276,
                K12 = 0.375,
                K13 = 0.196,
                K21 = 0.067,
                K31 = 0.004,
                Ke0 = 0.456,
                V1 = 4.270,
                V2 = 23.983,
                V3 = 238.000,
            };

            return model ;
        }

        /// <summary>
        /// プロポフォール（Katrinaモデル）を作成するテスト関数
        /// </summary>
        /// <param name="weight">体重(kg)</param>
        /// <returns>モデル</returns>
        /// <remarks>体重50kgで固定する</remarks>
        public static PharmacokineticModel CreatePropofol3(int weight = 50)
        {
            var model = new PharmacokineticModel("ﾌﾟﾛﾎﾟﾌｫｰﾙ_Katrina", weight)
            {
                K10 = 0.065,
                K12 = 0.111,
                K13 = 0.050,
                K21 = 0.057,
                K31 = 0.003,
                Ke0 = 0.260,
                V1 = 26.050,
                V2 = 50.500,
                V3 = 410.500,
            };

            return model;
        }

    }
}
