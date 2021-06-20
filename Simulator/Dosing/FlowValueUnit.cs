using System;
using static Simulator.Dosing.ValueUnit;

namespace Simulator.Dosing
{
    /// <summary>
    /// 流速を表す抽象クラス。
    /// 時間あたりの重量か体積かは具象クラスで分岐する。
    /// </summary>
    public abstract class FlowValueUnit : ValueUnit
    {

        public TimeUnitEnum TimeUnit { get; set; }

        /// <summary>
        /// 時間単位を変換する
        /// </summary>
        /// <param name="timeUnit">変換後の単位</param>
        /// <returns>単位変換された<see cref="FlowValueUnit"/></returns>
        public abstract FlowValueUnit ConvertTimeUnit(TimeUnitEnum timeUnit);

        /// <summary>
        /// 指定時間で投与された重量を取得する
        /// </summary>
        /// <param name="value">時間</param>
        /// <param name="timeUnit">時間単位</param>
        /// <returns>重量</returns>
        public abstract WeightValueUnit ToWeight(double value, TimeUnitEnum timeUnit);

        /// <summary>
        /// 指定時間で投与された重量を取得する
        /// </summary>
        /// <param name="time">時間</param>
        /// <returns>重量</returns>
        public abstract WeightValueUnit ToWeight(TimeValueUnit time);
    }
}