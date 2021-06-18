using System;
using  Simulator.Models;

namespace Simulator
{

    /// <summary>
    /// Eleveldモデル専用のファクトリークラス
    /// </summary>
    public class EleveldModelFactory
    {
        /// <summary>
        /// 血管モード
        /// </summary>
        public enum BloodVessels
        {
            /// <summary>
            /// 動脈
            /// </summary>
            Arterial,
            /// <summary>
            /// 静脈
            /// </summary>
            Venous
        }

        // 参照用データ（文献では70kg, 170cm, 35歳の男性をrefとしている）
        private const double AGE_ref = 35;
        private const double WGT_ref = 70;
        private const double STAT_ref = 1.7;
        private const double BMI_ref = WGT_ref / STAT_ref / STAT_ref;

        // モデル内パラメータ
        private const double THETA_1 = 6.28;
        private const double THETA_2 = 25.5;
        private const double THETA_3 = 273;
        private const double THETA_4 = 1.79;
        private const double THETA_5 = 1.75;
        private const double THETA_6 = 1.11;
        private const double THETA_7 = 0.191;
        private const double THETA_8 = 42.3;
        private const double THETA_9 = 9.06;
        private const double THETA_10 = -0.0156;
        private const double THETA_11 = -0.00286;
        private const double THETA_12 = 33.6;
        private const double THETA_13 = -0.0138;
        private const double THETA_14 = 68.3;
        private const double THETA_15 = 2.10;
        private const double THETA_16 = 1.30;
        private const double THETA_17 = 1.42;
        private const double THETA_18 = 0.68;

        // 確率変数（個体間の誤差）
        private const double ETA_1 = 0.610;
        private const double ETA_2 = 0.565;
        private const double ETA_3 = 0.597;
        private const double ETA_4 = 0.265;
        private const double ETA_5 = 0.346;
        private const double ETA_6 = 0.209;
        private const double ETA_7 = 0.463;

        public double PMA { get; set; }
        public double AGE { get; set; }
        public double WGT { get; set; }
        public double STAT { get; set; }
        public double BMI => WGT / STAT / STAT;
        public bool IsMale { get; set; }
        public bool Opiates { get; set; }

        /// <summary>
        /// Eleveldモデル（プロポフォール）のファクトリーを作成する
        /// </summary>
        /// <param name="weight">体重(kg)</param>
        /// <param name="stat">身長(m)</param>
        /// <param name="age">年齢(year)</param>
        /// <param name="pma">在胎週数</param>
        /// <param name="isMale">True:男、False:女</param>
        /// <param name="hasOpiates">オピオイド有無（True:あり, False:なし）</param>
        public EleveldModelFactory(double weight, double stat, double age, double pma, bool isMale, bool hasOpiates)
        {
            WGT = weight;
            STAT = stat;
            AGE = age;
            PMA = pma;
            IsMale = isMale;
            Opiates = hasOpiates;
        }


        private double AgingFunction(double x)
        {
            return Math.Exp(x * (AGE - AGE_ref));
        }

        private double SigmoidFunction(double x, double e50, double lambda)
        {
            return Math.Pow(x, lambda) / (Math.Pow(x, lambda) + Math.Pow(e50, lambda));
        }

        private double CentralFunction(double x)
        {
            return SigmoidFunction(x, THETA_12, 1);
        }

        private double CLMaturationFunction()
        {
            return SigmoidFunction(PMA, THETA_8, THETA_9);
        }

        private double CLMaturationFunctionRef()
        {
            return SigmoidFunction(PMA, THETA_8, THETA_9);
        }

        private double Q3MaturationFunction()
        {
            return SigmoidFunction((AGE * 54 + 40) / 54,THETA_14, 1);
        }

        private double Q3MaturationFunctionRef()
        {
            return SigmoidFunction((AGE_ref * 54 + 40) / 54, THETA_14, 1);
        }

        private double OpiatesFunction(double x)
        {
            return (Opiates) ? Math.Exp(x * AGE) : 1;
        }

        private double A1Sallami(double age, double weight, double bmi)
        {
            if (IsMale)
            {
                double left = 0.88 + (1 - 0.88) / (1 + Math.Pow(age / 13.4, -12.7));
                double right = (9270 * weight) / (6680 + 216 * bmi);
                return left * right;
            }
            else
            {
                double left = 1.11 + (1 - 1.11) / (1 + Math.Pow(age / 7.1, -1.1));
                double right = (9270 * weight) / (8780 + 244 * bmi);
                return left * right;
            }
        }



        private double V1Arterial => THETA_1 * (CentralFunction(WGT) / CentralFunction(WGT_ref)) * Math.Exp(ETA_1);
        private double V1Venous => V1Arterial * (1 + THETA_17 * (1 - CentralFunction(WGT)));

        private double V2 => THETA_2 * (WGT / WGT_ref) * AgingFunction(THETA_10) * Math.Exp(ETA_2);
        private double V2_ref => THETA_2 * AgingFunction(THETA_10) * Math.Exp(ETA_2);

        private double V3 => THETA_3 * (A1Sallami(AGE, WGT, BMI) / A1Sallami(AGE_ref, WGT_ref, BMI_ref)) *
                             OpiatesFunction(THETA_13) * Math.Exp(ETA_3);

        private double V3_ref => THETA_3 * OpiatesFunction(THETA_13) * Math.Exp(ETA_3);


        private double CL1
        {
            get
            {
                double k = IsMale ? THETA_4 : THETA_15;
                return k * Math.Pow(WGT/WGT_ref, 0.75) * (CLMaturationFunction() / CLMaturationFunctionRef()) * OpiatesFunction(THETA_11) * Math.Exp(ETA_4);
            }
        }

        private double Q2Arterial =>
            THETA_5 * Math.Pow(V2 / V2_ref, 0.75) * 
            (1 + THETA_16 * (1 - Q3MaturationFunction())) *
            Math.Exp(ETA_5);


        private double Q2Venous => Q2Arterial * THETA_18;

        private double Q3 => THETA_6 * Math.Pow(V3 / V3_ref, 0.75) *
                             (Q3MaturationFunction() / (Q3MaturationFunctionRef())) * Math.Exp(ETA_6);

        private double Ce50 => THETA_1 * AgingFunction(THETA_7) * Math.Exp(ETA_1);


        private double Ke0Arterial => THETA_2 * Math.Pow(WGT / 70, -0.25) * Math.Exp(ETA_2);
        private double Ke0Venous => THETA_8 * Math.Pow(WGT / 70, -0.25) * Math.Exp(ETA_2);

        /// <summary>
        /// Eleveldモデルを作成する
        /// </summary>
        /// <param name="name">任意の名前</param>
        /// <param name="mode">動脈 or 静脈</param>
        /// <returns>モデル</returns>
        public PharmacokineticModel Create(string name)
        {
            var model = new PharmacokineticModel(name, WGT);

            model.V1 = V1Venous;
            model.V2 = V2;
            model.V3 = V3;
            model.Ke0 = Ke0Venous;
            model.CL1 = CL1;
            model.CL2 = Q2Venous;
            model.CL3 = Q3;

            // min^-1 から h^-1 に補正
            model.Ke0 /= 60;
            return model;
        }



    }
}