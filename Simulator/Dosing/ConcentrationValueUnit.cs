namespace Simulator.Dosing
{
    
    /// <summary>
    /// 濃度を表すクラス
    /// </summary>
    public class ConcentrationValueUnit : ValueUnit
    {
        public WeightUnitEnum WeightUnit { get; set; }
        public VolumeUnitEnum VolumeUnit { get; set; }

        public ConcentrationValueUnit(double value, WeightUnitEnum weightUnit, VolumeUnitEnum volumeUnit)
        {
            Value = value;
            WeightUnit = weightUnit;
            VolumeUnit = volumeUnit;
        }

        public ConcentrationValueUnit(WeightValueUnit weight, VolumeValueUnit volume)
        : this(weight.Value / volume.Value, weight.WeightUnit, volume.VolumeUnit)
        {
        }

        public ConcentrationValueUnit ConvertUnit(WeightUnitEnum weightUnit, VolumeUnitEnum volumeUnit)
        {
            WeightValueUnit weight = new WeightValueUnit(this.Value, this.WeightUnit).ConvertUnit(weightUnit);
            VolumeValueUnit volume = new VolumeValueUnit(1, this.VolumeUnit).ConvertUnit(volumeUnit);
            return new ConcentrationValueUnit(weight, volume);
        }

        public override string ToString()
        {
            return $"{this.Value}{this.WeightUnit.Name()}/{this.VolumeUnit.Name()}";
        }
    }
}