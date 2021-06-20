using System;

namespace Simulator.Values
{

    /// <summary>
    /// 体積を表すクラス
    /// </summary>
    public class VolumeValueUnit : ValueUnit
    {
        public VolumeUnitEnum VolumeUnit { get; set; } = VolumeUnitEnum.mL;

        /// <summary>
        /// 値と単位で体積データを作成します。
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="volumeUnit">体積単位</param>
        public VolumeValueUnit(double value, VolumeUnitEnum volumeUnit)
        {
            Value = value;
            VolumeUnit = volumeUnit;
        }

        /// <summary>
        /// 割り算
        /// </summary>
        /// <param name="other">割るデータ</param>
        /// <returns>除算結果。体積÷時間の場合は速度が返る。それ以外は例外とする。</returns>
        public override ValueUnit Divide(ValueUnit other)
        {
            if (other is TimeValueUnit)
            {
                // 体積÷時間の場合は速度を出す（濃度は未確定なのでダミーとする）
                var time = other as TimeValueUnit;
                var conc = new ConcentrationValueUnit(1, WeightUnitEnum.None, VolumeUnitEnum.None);
                return new VolumeFlowValueUnit(this.Value / time.Value, this.VolumeUnit, conc, time.TimeUnit);
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        /// <summary>
        /// 体積単位を任意の単位に変換する。
        /// </summary>
        /// <param name="unit">体積単位</param>
        /// <returns>変換されたデータ</returns>
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