using System;

namespace Simulator.Dosing
{
    /// <summary>
    /// 重量を表すクラス
    /// </summary>
    public class WeightValueUnit : ValueUnit
    {

        public WeightUnitEnum WeightUnit { get; set; } = WeightUnitEnum.ug;

        public WeightValueUnit(double value, WeightUnitEnum weightUnit)
        {
            Value = value;
            WeightUnit = weightUnit;
        }

        public WeightValueUnit ConvertUnit(WeightUnitEnum unit)
        {
            double factor = 0.0;
            if (unit == WeightUnitEnum.kg)
            {
                // kgだけ特殊計算（変換先がkgの場合）
                factor = 0.001 / (double)this.WeightUnit;
            }
            else if (this.WeightUnit == WeightUnitEnum.kg)
            {
                // kgだけ特殊計算（変換元がkgの場合）
                factor = (double)unit * 1000;
            }
            else
            {
                factor = (double)unit / (double)this.WeightUnit;
            }

            return new WeightValueUnit(this.Value * factor, unit);
        }

        public override string ToString()
        {
            return $"{this.Value}{this.WeightUnit.Name()}";
        }
    }
}