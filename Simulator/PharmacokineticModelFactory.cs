

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
        public static PharmacokineticModel CreatePropofol(int weight)
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

            var model = new PharmacokineticModel("ﾌﾟﾛﾎﾟﾌｫｰﾙ_Marshモデル", k10, k12, k13, k21, k31, ke0, v1, weight);
            return model;

        }

    }
}
