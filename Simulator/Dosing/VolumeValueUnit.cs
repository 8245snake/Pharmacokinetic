using System.CodeDom;

namespace Simulator.Dosing
{

    /// <summary>
    /// 体積を表すクラス
    /// </summary>
    public class VolumeValueUnit : ValueUnit
    {
        public VolumeUnitEnum VolumeUnit { get; set; } = VolumeUnitEnum.mL;


        public VolumeValueUnit(double value, VolumeUnitEnum volumeUnit)
        {
            Value = value;
            VolumeUnit = volumeUnit;
        }

        public VolumeValueUnit ConvertUnit(VolumeUnitEnum unit)
        {
            if (unit == VolumeUnitEnum.None || unit == this.VolumeUnit)
            {
                return new VolumeValueUnit(this.Value, unit);
            }
            double factor = (double)unit / (double)this.VolumeUnit;
            return new VolumeValueUnit(this.Value * factor, unit);
        }

        public override string ToString()
        {
            return $"{this.Value}{this.VolumeUnit.Name()}";
        }
    }
}