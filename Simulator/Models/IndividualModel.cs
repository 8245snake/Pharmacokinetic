using Simulator.Factories;

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

        /// <summary>
        /// 個人パラメータを作成する。
        /// </summary>
        /// <param name="age">年齢(year)</param>
        /// <param name="weight">体重(kg)</param>
        /// <param name="stat">身長(m)</param>
        /// <param name="isMale">True:男、False:女</param>
        /// <param name="hasOpiates">オピオイド有無（True:あり, False:なし）</param>
        /// <param name="pma">在胎週数</param>
        public IndividualModel(double age, double weight, double stat, bool isMale, bool hasOpiates, double pma)
        {
            Age = age;
            Weight = weight;
            Stat = stat;
            IsMale = isMale;
            Opiates = hasOpiates;
            PMA = pma;
        }

        /// <summary>
        /// 個人パラメータからファクトリを指定してモデルを作成する。
        /// </summary>
        /// <typeparam name="T">ファクトリ</typeparam>
        /// <param name="modelName">モデル名</param>
        /// <param name="factory">空のファクトリ。メソッド内で個人パラメータが入る。</param>
        /// <returns>薬物動態モデル</returns>
        public PharmacokineticModel CreatePharmacokineticModel<T>(string modelName, T factory)
            where T : IPharmacokineticFactory
        {
            factory.SetIndivisual(this);
            return factory.Create(modelName);
        }

        public override string ToString()
        {
            return $"{nameof(Age)}: {Age}, {nameof(Weight)}: {Weight}, {nameof(Stat)}: {Stat}, {nameof(BMI)}: {BMI}, {nameof(IsMale)}: {IsMale}, {nameof(Opiates)}: {Opiates}, {nameof(PMA)}: {PMA}";
        }
    }
}