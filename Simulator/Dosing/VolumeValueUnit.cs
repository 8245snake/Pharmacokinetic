using System;
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

        public override ValueUnit Multiply(ValueUnit other)
        {
            throw new NotImplementedException();
        }


        public override ValueUnit Divide(ValueUnit other)
        {
            if (other is TimeValueUnit)
            {
                // 体積÷時間の場合は速度を出す（濃度は未確定なのでダミーとする）
                var time = other as TimeValueUnit;
                var conc = new ConcentrationValueUnit(1, WeightUnitEnum.None, VolumeUnitEnum.None);
                return new FlowValueUnit(this.Value / time.Value, this.VolumeUnit, conc, time.TimeUnit);
            }
            else
            {
                throw new NotImplementedException();
            }

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