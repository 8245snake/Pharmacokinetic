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
        public ValueUnit.WeightUnitEnum WeightUnit { get; set; }

        /// <summary>
        /// 流速単位（時間）
        /// </summary>
        public ValueUnit.TimeUnitEnum TimeUnit { get; set; }


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
