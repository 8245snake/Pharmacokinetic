namespace Simulator.Models
{
    /// <summary>
    /// シミュレーション対象の個人を表すクラス
    /// </summary>
    public class IndividualModel
    {
        /// <summary>
        /// 年齢
        /// </summary>
        public double Age { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 身長
        /// </summary>
        public double Stat { get; set; }
        /// <summary>
        /// BMI
        /// </summary>
        public double BMI => Weight / Stat / Stat;
        /// <summary>
        /// 性別（True:男、False:女）
        /// </summary>
        public bool IsMale { get; set; }
        /// <summary>
        /// オピオイド使用有無（True:あり、False:なし）
        /// </summary>
        public bool Opiates { get; set; }
        /// <summary>
        /// 在胎週数
        /// </summary>
        public double PMA { get; set; }

        public IndividualModel(double age, double weight, double stat, bool isMale, bool opiates, double pma)
        {
            Age = age;
            Weight = weight;
            Stat = stat;
            IsMale = isMale;
            Opiates = opiates;
            PMA = pma;
        }

        public override string ToString()
        {
            return $"{nameof(Age)}: {Age}, {nameof(Weight)}: {Weight}, {nameof(Stat)}: {Stat}, {nameof(BMI)}: {BMI}, {nameof(IsMale)}: {IsMale}, {nameof(Opiates)}: {Opiates}, {nameof(PMA)}: {PMA}";
        }
    }
}