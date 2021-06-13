using System;

namespace Simulator
{
    /// <summary>
    /// 薬物動態モデル
    /// </summary>
    public class PharmacokineticModel
    {
        /// <summary>
        /// 薬剤名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// k10速度定数(h^-1)
        /// </summary>
        public double K10 { get; set; }

        /// <summary>
        /// k12速度定数(h^-1)
        /// </summary>
        public double K12 { get; set; }

        /// <summary>
        /// k13速度定数(h^-1)
        /// </summary>
        public double K13 { get; set; }

        /// <summary>
        /// k21速度定数(h^-1)
        /// </summary>
        public double K21 { get; set; }

        /// <summary>
        /// k31速度定数(h^-1)
        /// </summary>
        public double K31 { get; set; }

        /// <summary>
        /// 消失速度定数(h^-1)
        /// </summary>
        public double Ke0 { get; set; }

        /// <summary>
        /// 体重(kg)
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 分布容量(L)
        /// </summary>
        public double V1 { get; set; }

        /// <summary>
        /// 分布容量(L)
        /// </summary>
        public double V2 { get; set; }

        /// <summary>
        /// 分布容量(L)
        /// </summary>
        public double V3 { get; set; }

        /// <summary>
        /// 血中からのクリアランス
        /// </summary>
        public double CL1
        {
            get => K10 * V1;
            set => K10 = value / V1;
        }

        /// <summary>
        /// C2へのクリアランス
        /// </summary>
        public double CL2
        {
            get => K12 * V1;
            set
            {
                K12 = value / V1;
                K21 = CL2 / V2;
            }
        }

        /// <summary>
        /// C3へのクリアランス
        /// </summary>
        public double CL3
        {
            get => K13 * V1;
            set
            {
                K13 = value / V1;
                K31 = CL3 / V3;
            }
        }

        public PharmacokineticModel(string name, double weight)
        {
            Name = name;
            Weight = weight;
        }

        /// <summary>
        /// モデルを作成します。
        /// </summary>
        /// <param name="name">薬剤名称</param>
        /// <param name="k10">k10速度定数(h^-1)</param>
        /// <param name="k12">k12速度定数(h^-1)</param>
        /// <param name="k13">k13速度定数(h^-1)</param>
        /// <param name="k21">k21速度定数(h^-1)</param>
        /// <param name="k31">k31速度定数(h^-1)</param>
        /// <param name="ke0">消失速度定数(h^-1)</param>
        /// <param name="weight">体重(kg)</param>
        public PharmacokineticModel(string name, double k10, double k12, double k13, double k21, double k31, double ke0, double weight)
        {
            Name = name;
            K10 = k10;
            K12 = k12;
            K13 = k13;
            K21 = k21;
            K31 = k31;
            Ke0 = ke0;
            Weight = weight;
        }

        /// <summary>
        /// モデルを作成します。
        /// </summary>
        /// <param name="name">薬剤名称</param>
        /// <param name="k10">k10速度定数(h^-1)</param>
        /// <param name="k12">k12速度定数(h^-1)</param>
        /// <param name="k13">k13速度定数(h^-1)</param>
        /// <param name="k21">k21速度定数(h^-1)</param>
        /// <param name="k31">k31速度定数(h^-1)</param>
        /// <param name="ke0">消失速度定数(h^-1)</param>
        /// <param name="v1">Vd分布容量(L/kg)</param>
        /// <param name="weight">体重(kg)</param>
        public PharmacokineticModel(string name, double k10, double k12, double k13, double k21, double k31, double ke0, double v1, double weight)
        {
            Name = name;
            K10 = k10;
            K12 = k12;
            K13 = k13;
            K21 = k21;
            K31 = k31;
            Ke0 = ke0;
            Weight = weight;
            V1 = v1 * weight;
            V2 = k12 * V1 / k21;
            V3 = k13 * V1 / k31;
        }

        /// <summary>
        /// 中央区画の速度変化
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="c3"></param>
        /// <param name="h"></param>
        /// <param name="bolus"></param>
        /// <param name="continuous"></param>
        /// <returns></returns>
        private double GetDeltaC1(double c1, double c2, double c3, double h, double bolus, double continuous)
        {
            return (-c1 * (K10 + K12 + K13) + c2 * K21 + c3 * K31) + continuous / h;
        }


        /// <summary>
        /// 抹消区画(fast)の速度変化
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        private double GetDeltaC2(double c1, double c2)
        {
            return (c1 * K12 - c2 * K21);
        }

        /// <summary>
        /// 抹消区画(slow)の速度変化
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c3"></param>
        /// <returns></returns>
        private double GetDeltaC3(double c1, double c3)
        {
            return (c1 * K13 - c3 * K31);
        }

        /// <summary>
        /// 効果部位の速度変化
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="ce"></param>
        /// <returns></returns>
        private double GetDeltaCe(double c1, double ce)
        {
            return (c1 * Ke0 - ce * Ke0);
        }

        /// <summary>
        /// ルンゲクッタ法でh分だけ未来の値を予測する
        /// </summary>
        /// <param name="c1">血中濃度</param>
        /// <param name="c2">C2濃度</param>
        /// <param name="c3">C3濃度</param>
        /// <param name="ce">効果部位濃度</param>
        /// <param name="bolus">ボーラス投与量</param>
        /// <param name="continuous">持続投与量</param>
        /// <param name="h">刻み(分)</param>
        /// <returns></returns>
        public RungeKuttaResult RungeKuttaCalculation(double c1, double c2, double c3, double ce, double bolus, double continuous, double h)
        {
            double d1c1 = GetDeltaC1(c1, c2, c3, h, bolus, continuous);
            double d1c2 = GetDeltaC2(c1, c2);
            double d1c3 = GetDeltaC3(c1, c3);
            double d1ce = GetDeltaCe(c1, ce);

            double d2c1 = GetDeltaC1(c1 + d1c1 * 0.5, c2 + d1c2 * 0.5, c3 + d1c3 * 0.5, h, bolus, continuous);
            double d2c2 = GetDeltaC2(c1 + d1c1 * 0.5, c2 + d1c2 * 0.5);
            double d2c3 = GetDeltaC3(c1 + d1c1 * 0.5, c3 + d1c3 * 0.5);
            double d2ce = GetDeltaCe(c1 + d1c1 * 0.5, ce + d1ce * 0.5);

            double d3c1 = GetDeltaC1(c1 + d2c1 * 0.5, c2 + d2c2 * 0.5, c3 + d2c3 * 0.5, h, bolus, continuous);
            double d3c2 = GetDeltaC2(c1 + d2c1 * 0.5, c2 + d2c2 * 0.5);
            double d3c3 = GetDeltaC3(c1 + d2c1 * 0.5, c3 + d2c3 * 0.5);
            double d3ce = GetDeltaCe(c1 + d2c1 * 0.5, ce + d2ce * 0.5);

            double d4c1 = GetDeltaC1(c1 + d3c1, c2 + d3c2, c3 + d3c3, h, bolus, continuous);
            double d4c2 = GetDeltaC2(c1 + d3c1, c2 + d3c2);
            double d4c3 = GetDeltaC3(c1 + d3c1, c3 + d3c3);
            double d4ce = GetDeltaCe(c1 + d3c1, ce + d3ce);

            c1 += (d1c1 + 2 * d2c1 + 2 * d3c1 + d4c1) / 6.0 + bolus / h;
            c2 += (d1c2 + 2 * d2c2 + 2 * d3c2 + d4c2) / 6.0;
            c3 += (d1c3 + 2 * d2c3 + 2 * d3c3 + d4c3) / 6.0;
            ce += (d1ce + 2 * d2ce + 2 * d3ce + d4ce) / 6.0;

            RungeKuttaResult result = new RungeKuttaResult
            {
                C1 = c1,
                C2 = c2,
                C3 = c3,
                Ce = ce
            };
            return result;
        }

        /// <summary>
        /// クローンを作成します。
        /// </summary>
        /// <returns>クローン</returns>
        public PharmacokineticModel DeepCopy()
        {
            return this.MemberwiseClone() as PharmacokineticModel;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(K10)}: {K10}, {nameof(K12)}: {K12}, {nameof(K13)}: {K13}, {nameof(K21)}: {K21}, {nameof(K31)}: {K31}, {nameof(Ke0)}: {Ke0}, {nameof(Weight)}: {Weight}, {nameof(V1)}: {V1}, {nameof(V2)}: {V2}, {nameof(V3)}: {V3}, {nameof(CL1)}: {CL1}, {nameof(CL2)}: {CL2}, {nameof(CL3)}: {CL3}";
        }

        public void ConsoleLog()
        {
            string log = $"{nameof(Name)}: {Name}, \r\n" +
                         $"{nameof(K10)}: {K10}, \r\n" +
                         $"{nameof(K12)}: {K12}, \r\n" +
                         $"{nameof(K13)}: {K13}, \r\n" +
                         $"{nameof(K21)}: {K21}, \r\n" +
                         $"{nameof(K31)}: {K31}, \r\n" +
                         $"{nameof(Ke0)}: {Ke0}, \r\n" +
                         $"{nameof(V1)}: {V1}, \r\n" +
                         $"{nameof(V2)}: {V2}, \r\n" +
                         $"{nameof(V3)}: {V3}, \r\n" +
                         $"{nameof(CL1)}: {CL1}, \r\n" +
                         $"{nameof(CL2)}: {CL2}, \r\n" +
                         $"{nameof(CL3)}: {CL3}";

            Console.WriteLine(log);
        }

    }


    public struct RungeKuttaResult
    {
        public double C1 { get; set; }
        public double C2 { get; set; }
        public double C3 { get; set; }
        public double Ce { get; set; }

        public override string ToString()
        {
            return $"{nameof(C1)}: {C1}, {nameof(Ce)}: {Ce}";
        }
    }
}
