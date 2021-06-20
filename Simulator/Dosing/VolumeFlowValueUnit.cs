using System;

namespace Simulator.Dosing
{
    public class VolumeFlowValueUnit : FlowValueUnit
    {

        private VolumeUnitEnum _volumeUnit = VolumeUnitEnum.None;
        private ConcentrationValueUnit _concentration;


        public VolumeUnitEnum VolumeUnit
        {
            get => _volumeUnit;
        }

        public ConcentrationValueUnit Concentration
        {
            get => _concentration;
        }

        /// <summary>
        /// 体積/時間で流速データを作成します。濃度も必要になります。
        /// </summary>
        /// <param name="flow">流速</param>
        /// <param name="volumeUnit">体積単位</param>
        /// <param name="concentration">濃度</param>
        /// <param name="timeUnit">時刻単位</param>
        public VolumeFlowValueUnit(double flow, VolumeUnitEnum volumeUnit, ConcentrationValueUnit concentration, TimeUnitEnum timeUnit)
        {
            Value = flow;
            _volumeUnit = volumeUnit;
            _concentration = concentration;
            TimeUnit = timeUnit;
        }

        public VolumeFlowValueUnit ConvertUnit(VolumeUnitEnum volumeUnit, TimeUnitEnum timeUnit)
        {
            if (this.VolumeUnit == VolumeUnitEnum.None)
            {
                throw new NotImplementedException("体積が未設定です");
            }
            var volume = new VolumeValueUnit(this.Value, this.VolumeUnit).ConvertUnit(volumeUnit);
            var time = new TimeValueUnit(1, this.TimeUnit).ConvertUnit(timeUnit);
            return new VolumeFlowValueUnit(volume.Value / time.Value, volumeUnit, _concentration, timeUnit);
        }

        public override string ToString()
        {
            return $"{this.Value}{this.VolumeUnit.Name()}/{this.TimeUnit.Name()}";
        }

        public override FlowValueUnit ConvertTimeUnit(TimeUnitEnum timeUnit)
        {
            throw new NotImplementedException();
        }

        public override WeightValueUnit ToWeight(double value, TimeUnitEnum timeUnit)
        {
            throw new NotImplementedException();
        }

        public override WeightValueUnit ToWeight(TimeValueUnit time)
        {
            throw new NotImplementedException();
        }
    }
}