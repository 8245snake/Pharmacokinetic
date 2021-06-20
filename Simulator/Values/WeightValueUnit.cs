using System;

namespace Simulator.Values
{
    /// <summary>
    /// 重量を表すクラス
    /// </summary>
    public class WeightValueUnit : ValueUnit
    {
        /// <summary>
        /// 重量単位
        /// </summary>
        public WeightUnitEnum WeightUnit { get; set; }

        /// <summary>
        /// 値と単位で重量データを作成します。
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="weightUnit">重量単位</param>
        public WeightValueUnit(double value, WeightUnitEnum weightUnit)
        {
            Value = value;
            WeightUnit = weightUnit;
        }

        #region Operation


        /// <summary>
        /// 足し算
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="weightUnit">単位</param>
        /// <returns>足し算した結果</returns>
        public WeightValueUnit Plus(double value, WeightUnitEnum weightUnit)
        {
            WeightValueUnit addend = new WeightValueUnit(value, weightUnit).ConvertUnit(this.WeightUnit);
            addend.Value += this.Value;
            return addend;
        }

        /// <summary>
        /// 足し算
        /// </summary>
        /// <param name="other">重量</param>
        /// <returns>足し算した結果</returns>
        public override ValueUnit Plus(ValueUnit other)
        {
            if (!(other is WeightValueUnit weight))
            {
                return base.Plus(other);
            }

            return Plus(other.Value, weight.WeightUnit);
        }


        /// <summary>
        /// 引き算
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="weightUnit">単位</param>
        /// <returns>引き算した結果</returns>
        public WeightValueUnit Minus(double value, WeightUnitEnum weightUnit)
        {
            // マイナスを掛けて足し算
            return Plus(-1 * value, weightUnit);
        }

        /// <summary>
        /// 引き算
        /// </summary>
        /// <param name="other">重量</param>
        /// <returns>引き算した結果</returns>
        public override ValueUnit Minus(ValueUnit other)
        {
            if (!(other is WeightValueUnit weight))
            {
                return base.Minus(other);
            }
            return Minus(weight.Value, weight.WeightUnit);
        }

        /// <summary>
        /// 割り算
        /// </summary>
        /// <param name="other">割るデータ</param>
        /// <returns>重量÷体積の場合は濃度を返す（<seealso cref="ConcentrationValueUnit"/>）。
        /// 重量÷時間の場合は速度を返す（<seealso cref="WeightFlowValueUnit"/>）。</returns>
        public override ValueUnit Divide(ValueUnit other)
        {
            if (other is VolumeValueUnit)
            {
                // 重量÷体積の場合は濃度を出す
                return new ConcentrationValueUnit(this, other as VolumeValueUnit);
            }else if (other is TimeValueUnit)
            {
                // 重量÷時間の場合は速度を出す
                var time = other as TimeValueUnit;
                return new WeightFlowValueUnit(this.Value / time.Value, this.WeightUnit, time.TimeUnit);
            }
            else
            {
                throw new NotImplementedException("濃度と速度計算以外の除算は未実装です。");
            }

        }

        #endregion

        /// <summary>
        /// 重量単位を任意の単位に変換する。
        /// </summary>
        /// <param name="unit">重量単位</param>
        /// <returns>変換されたデータ</returns>
        public WeightValueUnit ConvertUnit(WeightUnitEnum unit)
        {
            if (unit == WeightUnitEnum.None || unit == this.WeightUnit)
            {
                return new WeightValueUnit(this.Value, unit);
            }

            // 係数
            double factor;

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