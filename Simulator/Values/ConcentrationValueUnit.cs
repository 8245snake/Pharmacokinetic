namespace Simulator.Values
{
    
    /// <summary>
    /// 濃度を表すクラス
    /// </summary>
    public class ConcentrationValueUnit : ValueUnit
    {
        public WeightUnitEnum WeightUnit { get; set; }
        public VolumeUnitEnum VolumeUnit { get; set; }

        /// <summary>
        /// 値、重量単位、体積単位を指定して濃度を作成します。
        /// </summary>
        /// <param name="value">濃度値</param>
        /// <param name="weightUnit">重量単位</param>
        /// <param name="volumeUnit">体積単位</param>
        public ConcentrationValueUnit(double value, WeightUnitEnum weightUnit, VolumeUnitEnum volumeUnit)
        {
            Value = value;
            WeightUnit = weightUnit;
            VolumeUnit = volumeUnit;
        }

        /// <summary>
        /// 重量と体積により濃度を作成します。
        /// 濃度は割り算されます。
        /// </summary>
        /// <param name="weight">重量</param>
        /// <param name="volume">体積</param>
        public ConcentrationValueUnit(WeightValueUnit weight, VolumeValueUnit volume)
        : this(weight.Value / volume.Value, weight.WeightUnit, volume.VolumeUnit)
        {
        }

        /// <summary>
        /// 重量単位、体積単位を指定した単位に変換します。
        /// </summary>
        /// <param name="weightUnit">重量単位</param>
        /// <param name="volumeUnit">体積単位</param>
        /// <returns>変換後の濃度</returns>
        public ConcentrationValueUnit ConvertUnit(WeightUnitEnum weightUnit, VolumeUnitEnum volumeUnit)
        {
            WeightValueUnit weight = new WeightValueUnit(this.Value, this.WeightUnit).ConvertUnit(weightUnit);
            VolumeValueUnit volume = new VolumeValueUnit(1, this.VolumeUnit).ConvertUnit(volumeUnit);
            return new ConcentrationValueUnit(weight, volume);
        }

        /// <summary>
        /// 体積から重量を計算する
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="volumeUnit">単位</param>
        /// <returns>重量</returns>
        public WeightValueUnit ToWeight(double value, VolumeUnitEnum volumeUnit)
        {
            return ToWeight(new VolumeValueUnit(value, volumeUnit));
        }

        /// <summary>
        /// 体積から重量を計算する
        /// </summary>
        /// <param name="volume">体積</param>
        /// <returns>重量</returns>
        public WeightValueUnit ToWeight(VolumeValueUnit volume)
        {
            var conv = volume.ConvertUnit(this.VolumeUnit);
            return new WeightValueUnit(this.Value * conv.Value, this.WeightUnit);
        }

        public override string ToString()
        {
            return $"{this.Value}{this.WeightUnit.Name()}/{this.VolumeUnit.Name()}";
        }
    }
}