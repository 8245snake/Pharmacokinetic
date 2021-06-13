

namespace Simulator
{

    /// <summary>
    /// 薬物動態モデルのファクトリクラス。
    /// 設定読み込みやポリモルフィズムができていないので現状ではテスト用モデルを作成するだけのクラス。
    /// </summary>
    public class PharmacokineticModelFactory
    {

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

        /// <summary>
        /// プロポフォール（Kennyモデル）を作成するテスト関数
        /// </summary>
        /// <param name="weight">体重(kg)</param>
        /// <returns>モデル</returns>
        /// <remarks>体重50kgで固定する</remarks>
        public static PharmacokineticModel CreatePropofol4(int weight = 50)
        {
            var model = new PharmacokineticModel("ﾌﾟﾛﾎﾟﾌｫｰﾙ_Kenny", weight)
            {
                K10 = 0.119,
                K12 = 0.114,
                K13 = 0.042,
                K21 = 0.055,
                K31 = 0.003,
                Ke0 = 0.260,
                V1 = 11.400,
                V2 = 23.629,
                V3 = 144.745,
            };

            return model;
        }

    }
}
