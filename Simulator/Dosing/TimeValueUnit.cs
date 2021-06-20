namespace Simulator.Dosing
{
    /// <summary>
    /// 時間を表すクラス
    /// </summary>
    public class TimeValueUnit : ValueUnit
    {
        /// <summary>
        /// 時間単位
        /// </summary>
        public TimeUnitEnum TimeUnit { get; set; }

        /// <summary>
        /// 秒に変換した値
        /// </summary>
        public int Seconds
        {
            get => (int)Value * (int)GetTimeUnitConvertFactor(this.TimeUnit, TimeUnitEnum.second);
        }

        /// <summary>
        /// 値と単位によって時間データを作成します。
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="timeUnit">時間単位</param>
        public TimeValueUnit(double value, TimeUnitEnum timeUnit)
        {
            Value = value;
            TimeUnit = timeUnit;
        }

        /// <summary>
        /// 時間の単位を任意の単位に変換します。
        /// </summary>
        /// <param name="unit">時間単位</param>
        /// <returns>変換したデータ</returns>
        public TimeValueUnit ConvertUnit(TimeUnitEnum unit)
        {
            if (unit == TimeUnitEnum.None || unit == this.TimeUnit)
            {
                return new TimeValueUnit(this.Value, unit);
            }

            double factor = (double)unit / (double)this.TimeUnit;
            return new TimeValueUnit(this.Value * factor, unit);
        }

        public override string ToString()
        {
            return $"{this.Value}{this.TimeUnit.Name()}";
        }


    }
}