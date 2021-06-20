using System;

namespace Simulator.Dosing
{
    /// <summary>
    /// 重量単位での流速を表現するクラス
    /// </summary>
    public class WeightFlowValueUnit : FlowValueUnit
    {

        private WeightUnitEnum _weightUnit;

        /// <summary>
        /// 重量単位
        /// </summary>
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

        /// <summary>
        /// 重量と時間単位を任意の単位に変換します。
        /// </summary>
        /// <param name="weightUnit">重量単位</param>
        /// <param name="timeUnit">時間単位</param>
        /// <returns>変換されたデータ</returns>
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

        /// <summary>
        /// 時間単位を任意の単位に変換する。
        /// </summary>
        /// <param name="timeUnit">時間単位</param>
        /// <returns>変換されたデータ</returns>
        public override FlowValueUnit ConvertTimeUnit(TimeUnitEnum timeUnit)
        {
            TimeValueUnit time = new TimeValueUnit(1, this.TimeUnit).ConvertUnit(timeUnit);
            return new WeightFlowValueUnit(this.Value / time.Value, this.WeightUnit, timeUnit);
        }

        /// <summary>
        /// 指定した時間分の流量（重量）を取得します。
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="timeUnit">時間単位</param>
        /// <returns>重量。単位は<see cref="WeightUnit"/>と同じ。</returns>
        public override WeightValueUnit ToWeight(double value, TimeUnitEnum timeUnit)
        {
            // 単位時間あたりの量に変換する
            WeightFlowValueUnit conv = this.ConvertTimeUnit(timeUnit) as WeightFlowValueUnit;
            return new WeightValueUnit(conv.Value * value, conv.WeightUnit);
        }

        /// <summary>
        /// 指定した時間分の流量（重量）を取得します。
        /// </summary>
        /// <param name="time">時間</param>
        /// <returns>重量。単位は<see cref="WeightUnit"/>と同じ。</returns>
        public override WeightValueUnit ToWeight(TimeValueUnit time)
        {
            return ToWeight(time.Value, time.TimeUnit);
        }


        public override string ToString()
        {
            return $"{this.Value}{this.WeightUnit.Name()}/{this.TimeUnit.Name()}";
        }
    }
}