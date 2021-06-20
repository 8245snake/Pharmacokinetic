using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulator.Values;
using static Simulator.Values.ValueUnit;

namespace Simulator.Dosing
{
    /// <summary>
    /// 持続投与を表現するクラス
    /// </summary>
    public class ContinuousMedicineDosing : IMedicineDosing
    {
        /// <summary>
        /// 投与開始時刻。丸められていない厳密な値が入っている。
        /// </summary>
        public DateTime DoseStartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 投与終了時刻。丸められていない厳密な値が入っている。
        /// </summary>
        public DateTime DoseEndTime { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// 流速
        /// </summary>
        public double FlowVelocity { get; set; }

        /// <summary>
        /// 流速単位（重量）
        /// </summary>
        public WeightUnitEnum WeightUnit { get; set; }

        /// <summary>
        /// 流速単位（時間）
        /// </summary>
        public TimeUnitEnum TimeUnit { get; set; }

        /// <summary>
        /// 値を指定せずに持続投与データを作成します。
        /// </summary>
        public ContinuousMedicineDosing()
        {
            WeightUnit = WeightUnitEnum.None;
            TimeUnit = TimeUnitEnum.None;
        }

        /// <summary>
        /// 流速と単位を指定して持続投与データを作成します。
        /// 開始時刻は現在時刻、終了は無限遠になります。
        /// </summary>
        /// <param name="flowVelocity">流速</param>
        /// <param name="weightUnit">流速の重量単位</param>
        /// <param name="timeUnit">流速の時間単位</param>
        public ContinuousMedicineDosing(double flowVelocity, WeightUnitEnum weightUnit, TimeUnitEnum timeUnit)
        {
            FlowVelocity = flowVelocity;
            WeightUnit = weightUnit;
            TimeUnit = timeUnit;
        }

        /// <summary>
        /// 流速を指定して持続投与データを作成します。
        /// 開始時刻は現在時刻、終了は無限遠になります。
        /// </summary>
        /// <param name="flow">流速</param>
        public ContinuousMedicineDosing(WeightFlowValueUnit flow)
            : this(flow.Value, flow.WeightUnit, flow.TimeUnit)
        {
        }

        /// <summary>
        /// 流速と単位と開始時刻を指定して持続投与データを作成します。
        /// 終了時刻は無限遠になります。
        /// </summary>
        /// <param name="start">開始時刻</param>
        /// <param name="flowVelocity">流速</param>
        /// <param name="weightUnit">流速の重量単位</param>
        /// <param name="timeUnit">流速の時間単位</param>
        public ContinuousMedicineDosing(DateTime start, double flowVelocity, WeightUnitEnum weightUnit, TimeUnitEnum timeUnit)
            : this(flowVelocity, weightUnit, timeUnit)
        {
            DoseStartTime = start;
            DoseEndTime = DateTime.MaxValue;
        }

        /// <summary>
        /// 流速と単位と開始時刻を指定して持続投与データを作成します。
        /// 終了時刻は無限遠になります。
        /// </summary>
        /// <param name="start">開始時刻</param>
        /// <param name="flow">流速</param>
        public ContinuousMedicineDosing(DateTime start, WeightFlowValueUnit flow)
            : this(start, flow.Value, flow.WeightUnit, flow.TimeUnit)
        {
        }

        /// <summary>
        /// 流速と単位と開始・終了時刻を指定して持続投与データを作成します。
        /// </summary>
        /// <param name="start">開始時刻</param>
        /// <param name="end">終了時刻</param>
        /// <param name="flowVelocity">流速</param>
        /// <param name="weightUnit">流速の重量単位</param>
        /// <param name="timeUnit">流速の時間単位</param>
        public ContinuousMedicineDosing(DateTime start, DateTime end, double flowVelocity, WeightUnitEnum weightUnit, TimeUnitEnum timeUnit)
            : this(flowVelocity, weightUnit, timeUnit)
        {
            DoseStartTime = start;
            DoseEndTime = end;
        }

        /// <summary>
        /// 流速と単位と開始・終了時刻を指定して持続投与データを作成します。
        /// </summary>
        /// <param name="start">開始時刻</param>
        /// <param name="end">終了時刻</param>
        /// <param name="flow">流速</param>
        public ContinuousMedicineDosing(DateTime start, DateTime end, WeightFlowValueUnit flow)
            : this(start, end, flow.Value, flow.WeightUnit, flow.TimeUnit)
        {
        }



        /// <summary>
        /// 指定時刻の投与量を取得する。
        /// 単位はngに統一する。
        /// </summary>
        /// <param name="time">この時刻で投与したとして相応しい場合に結果を返す。</param>
        /// <param name="stepSeconds">刻み時間(秒)</param>
        /// <returns>投与量（単位はng）</returns>
        public double GetDosing(DateTime time, int stepSeconds)
        {
            // 開始より前か終了より後なら無効（投与直後に濃度上昇しないため開始時刻ちょうどのときもはじく）
            if (time.Ticks <= DoseStartTime.Ticks || time.Ticks > DoseEndTime.Ticks)
            {
                return 0;
            }
            
            var flow = new WeightFlowValueUnit(this.FlowVelocity, this.WeightUnit, this.TimeUnit);
            // 1分ごとの量を返す
            return flow.ToWeight(1, TimeUnitEnum.minute).ConvertUnit(WeightUnitEnum.ng).Value;
        }

        /// <summary>
        /// 初期化処理。
        /// とくに何もしない。
        /// </summary>
        public void Initialize()
        {
        }
    }
}
