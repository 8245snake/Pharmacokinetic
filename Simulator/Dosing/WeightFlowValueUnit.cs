using System;

namespace Simulator.Dosing
{
    public class WeightFlowValueUnit : FlowValueUnit
    {

        private WeightUnitEnum _weightUnit = WeightUnitEnum.None;

        public WeightUnitEnum WeightUnit
        {
            get => _weightUnit;
        }

        /// <summary>
        /// 重量/時間で流速データを作成します
        /// </summary>
        /// <param name="flow">流速</param>
        /// <param name="weightUnit">重量単位</param>
        /// <param name="timeUnit">時刻単位</param>
        public WeightFlowValueUnit(double flow, WeightUnitEnum weightUnit, TimeUnitEnum timeUnit)
        {
            Value = flow;
            _weightUnit = weightUnit;
            TimeUnit = timeUnit;
        }


        public WeightFlowValueUnit ConvertUnit(WeightUnitEnum weightUnit, TimeUnitEnum timeUnit)
        {
            if (this.WeightUnit == WeightUnitEnum.None)
            {
                throw new NotImplementedException("重量が未設定です");
            }
            WeightValueUnit weight = new WeightValueUnit(this.Value, this.WeightUnit).ConvertUnit(weightUnit);
            TimeValueUnit time = new TimeValueUnit(1, this.TimeUnit).ConvertUnit(timeUnit);
            return new WeightFlowValueUnit(weight.Value / time.Value, weightUnit, timeUnit);
        }

        public override string ToString()
        {
            return $"{this.Value}{this.WeightUnit.Name()}/{this.TimeUnit.Name()}";
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