using System;

namespace Simulator.Values
{
    /// <summary>
    /// ガンマ流速を表現するクラス
    /// </summary>
    public class GammaFlowValueUnit : WeightFlowValueUnit
    {

        /// <summary>
        /// 流速、単位でγ流速を作成します。
        /// </summary>
        /// <param name="value">流速</param>
        /// <param name="weightUnit">重量単位</param>
        /// <param name="timeUnit">時間単位</param>
        public GammaFlowValueUnit(double value, WeightUnitEnum weightUnit, TimeUnitEnum timeUnit)
        {
            Value = value;
            WeightUnit = weightUnit;
            TimeUnit = timeUnit;
        }

        /// <summary>
        /// 流速でγ流速を作成します。
        /// </summary>
        /// <param name="flow">流速</param>
        public GammaFlowValueUnit(WeightFlowValueUnit flow)
        : this(flow.Value, flow.WeightUnit, flow.TimeUnit)
        {
        }

        /// <summary>
        /// 重量単位の流速に変換します。
        /// </summary>
        /// <param name="individualWeight">体重</param>
        /// <returns>変換した流速</returns>
        public WeightFlowValueUnit ToWeightFlow(double individualWeight)
        {
            return new WeightFlowValueUnit(Value * individualWeight, WeightUnit, TimeUnit);
        }


        public override string ToString()
        {
            return $"{Value}{WeightUnit.Name()}/kg/{TimeUnit.Name()}";
        }
    }
}