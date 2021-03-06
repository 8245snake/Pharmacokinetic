using System;
using  Simulator.Models;
using  Simulator.Factories;

namespace Simulator.Factories
{

    /// <summary>
    /// Eleveldモデル専用のファクトリークラス
    /// </summary>
    public class EleveldModelFactory : PharmacokineticModelFactory, IPharmacokineticFactory
    {

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
        public double BMI => Weight / Stat / Stat;
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
            Weight = weight;
            Stat = stat;
            Age = age;
            PMA = pma;
            IsMale = isMale;
            Opiates = hasOpiates;
        }

        /// <summary>
        /// 個人データを使用してファクトリを生成します。
        /// </summary>
        /// <param name="individual">個人データ</param>
        public EleveldModelFactory(IndividualModel individual)
            : this(individual.Weight, individual.Stat, individual.Age, individual.PMA, individual.IsMale,
                individual.Opiates)
        {
        }

        /// <summary>
        /// 個人パラメータをセットします。
        /// </summary>
        /// <param name="individual">個人データ</param>
        public new void SetIndivisual(IndividualModel individual)
        {
            Weight = individual.Weight;
            Stat = individual.Stat;
            Age = individual.Age;
            PMA = individual.PMA;
            IsMale = individual.IsMale;
            Opiates = individual.Opiates;
        }


        private double AgingFunction(double x)
        {
            return Math.Exp(x * (Age - AGE_ref));
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
            return SigmoidFunction((Age * 54 + 40) / 54,THETA_14, 1);
        }

        private double Q3MaturationFunctionRef()
        {
            return SigmoidFunction((AGE_ref * 54 + 40) / 54, THETA_14, 1);
        }

        private double OpiatesFunction(double x)
        {
            return (Opiates) ? Math.Exp(x * Age) : 1;
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



        private double V1Arterial => THETA_1 * (CentralFunction(Weight) / CentralFunction(WGT_ref)) * Math.Exp(ETA_1);
        private double V1Venous => V1Arterial * (1 + THETA_17 * (1 - CentralFunction(Weight)));

        private double V2 => THETA_2 * (Weight / WGT_ref) * AgingFunction(THETA_10) * Math.Exp(ETA_2);
        private double V2_ref => THETA_2 * AgingFunction(THETA_10) * Math.Exp(ETA_2);

        private double V3 => THETA_3 * (A1Sallami(Age, Weight, BMI) / A1Sallami(AGE_ref, WGT_ref, BMI_ref)) *
                             OpiatesFunction(THETA_13) * Math.Exp(ETA_3);

        private double V3_ref => THETA_3 * OpiatesFunction(THETA_13) * Math.Exp(ETA_3);


        private double CL1
        {
            get
            {
                double k = IsMale ? THETA_4 : THETA_15;
                return k * Math.Pow(Weight/WGT_ref, 0.75) * (CLMaturationFunction() / CLMaturationFunctionRef()) * OpiatesFunction(THETA_11) * Math.Exp(ETA_4);
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


        private double Ke0Arterial => THETA_2 * Math.Pow(Weight / 70, -0.25) * Math.Exp(ETA_2);
        private double Ke0Venous => THETA_8 * Math.Pow(Weight / 70, -0.25) * Math.Exp(ETA_2);


        /// <summary>
        /// Eleveldモデルを作成する
        /// </summary>
        /// <param name="modelName">任意の名前</param>
        /// <returns>モデル</returns>
        public new PharmacokineticModel Create(string modelName)
        {
            var model = new PharmacokineticModel(modelName, Weight)
            {
                V1 = V1Venous,
                V2 = V2,
                V3 = V3,
                Ke0 = Ke0Venous,
                CL1 = CL1,
                CL2 = Q2Venous,
                CL3 = Q3
            };

            // min^-1 から h^-1 に補正
            model.Ke0 /= 60;
            return model;
        }

        /// <summary>
        /// Eleveldモデルを作成する
        /// </summary>
        /// <param name="name">モデル名</param>
        /// <param name="weight">体重(kg)</param>
        /// <param name="stat">身長(m)</param>
        /// <param name="age">年齢(year)</param>
        /// <param name="pma">在胎週数</param>
        /// <param name="isMale">True:男、False:女</param>
        /// <param name="hasOpiates">オピオイド有無（True:あり, False:なし）</param>
        /// <returns>Eleveldモデル</returns>
        public static PharmacokineticModel Create(string name, double weight, double stat, double age, double pma, bool isMale, bool hasOpiates)
        {
            var factory = new EleveldModelFactory(weight, stat, age, pma, isMale, hasOpiates);
            return factory.Create(name);
        }



    }
}