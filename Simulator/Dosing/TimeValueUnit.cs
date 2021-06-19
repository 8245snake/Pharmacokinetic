namespace Simulator.Dosing
{
    /// <summary>
    /// 時間を表すクラス
    /// </summary>
    public class TimeValueUnit : ValueUnit
    {

        public TimeUnitEnum TimeUnit { get; set; }

        public int Seconds
        {
            get => (int)Value * (int)GetTimeUnitConvertFactor(this.TimeUnit, TimeUnitEnum.second);
        }

        public TimeValueUnit(double value, TimeUnitEnum timeUnit)
        {
            Value = value;
            TimeUnit = timeUnit;
        }

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