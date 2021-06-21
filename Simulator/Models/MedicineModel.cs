using Simulator.Values;

namespace Simulator.Models
{
    /// <summary>
    /// 薬剤のモデル
    /// </summary>
    public class MedicineModel
    {
        /// <summary>
        /// 薬剤名
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 薬剤規格重量
        /// </summary>
        public WeightValueUnit ProductWeight { get; set; }
        /// <summary>
        /// 薬剤規格容量
        /// </summary>
        public VolumeValueUnit ProductVolume { get; set; }
        /// <summary>
        /// 薬剤カテゴリ
        /// </summary>
        public string MedicineCategory { get; set; }
        /// <summary>
        /// 薬剤濃度
        /// </summary>
        public ConcentrationValueUnit Concentration { get; set; }



    }

}
