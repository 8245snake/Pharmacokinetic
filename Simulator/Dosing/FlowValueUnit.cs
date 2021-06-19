using static Simulator.Dosing.ValueUnit;

namespace Simulator.Dosing
{
    /// <summary>
    /// 流速を表すクラス
    /// </summary>
    public class FlowValueUnit : ValueUnit
    {
        private WeightUnitEnum _weightUnit = WeightUnitEnum.None;
        private VolumeUnitEnum _volumeUnit = VolumeUnitEnum.None;
        private ConcentrationValueUnit _concentration;

        public TimeUnitEnum TimeUnit { get; set; }

        public WeightUnitEnum WeightUnit
        {
            get => _weightUnit;
        }

        public VolumeUnitEnum VolumeUnit
        {
            get => _volumeUnit;
        }

        public ConcentrationValueUnit Concentration
        {
            get => _concentration;
        }

        /// <summary>
        /// 重量/時間で流速データを作成します
        /// </summary>
        /// <param name="flow">流速</param>
        /// <param name="weightUnit">重量単位</param>
        /// <param name="timeUnit">時刻単位</param>
        public FlowValueUnit(double flow, WeightUnitEnum weightUnit, TimeUnitEnum timeUnit)
        {
            Value = flow;
            _weightUnit = weightUnit;
            TimeUnit = timeUnit;
        }

        /// <summary>
        /// 体積/時間で流速データを作成します。濃度も必要になります。
        /// </summary>
        /// <param name="flow">流速</param>
        /// <param name="volumeUnit">体積単位</param>
        /// <param name="concentration">濃度</param>
        /// <param name="timeUnit">時刻単位</param>
        public FlowValueUnit(double flow, VolumeUnitEnum volumeUnit, ConcentrationValueUnit concentration, TimeUnitEnum timeUnit)
        {
            Value = flow;
            _volumeUnit = volumeUnit;
            _concentration = concentration;
            TimeUnit = timeUnit;
        }

        public FlowValueUnit ConvertUnit(WeightUnitEnum weightUnit, TimeUnitEnum timeUnit)
        {
            WeightValueUnit weight = new WeightValueUnit(this.Value, this.WeightUnit).ConvertUnit(weightUnit);
            TimeValueUnit time = new TimeValueUnit(1, this.TimeUnit).ConvertUnit(timeUnit);
            return new FlowValueUnit(weight.Value / time.Value, weightUnit, timeUnit);
        }

        public FlowValueUnit ConvertUnit(VolumeUnitEnum volumeUnit, TimeUnitEnum timeUnit)
        {
            VolumeValueUnit volume = new VolumeValueUnit(this.Value, this.VolumeUnit).ConvertUnit(volumeUnit);
            TimeValueUnit time = new TimeValueUnit(1, this.TimeUnit).ConvertUnit(timeUnit);
            return new FlowValueUnit(volume.Value / time.Value, volumeUnit, _concentration, timeUnit);
        }

        public override string ToString()
        {
            if (_volumeUnit == VolumeUnitEnum.None)
            {
                return $"{this.Value}{this.WeightUnit.Name()}/{this.TimeUnit.Name()}";
            }
            else
            {
                return $"{this.Value}{this.VolumeUnit.Name()}/{this.TimeUnit.Name()}";
            }
        }
    }
}