using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Simulator.Dosing.Medicine;

namespace Simulator.Dosing
{
    /// <summary>
    /// 持続投与を表現するクラス
    /// </summary>
    class ContinuousMedicineDosing : IMedicineDosing
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
        /// 刻み時間(秒)
        /// </summary>
        public int StepSeconds { get; set; }

        /// <summary>
        /// 指定時刻の投与量を取得する。
        /// 単位はμgに統一する。
        /// </summary>
        /// <param name="time">この時刻で投与したとして相応しい場合に結果を返す。</param>
        /// <returns>投与量（単位はμg）</returns>
        public double GetDosing(DateTime time)
        {
            // 開始より前か終了より後なら無効（投与直後に濃度上昇しないため開始時刻ちょうどのときもはじく）
            if (time.Ticks <= DoseStartTime.Ticks || time.Ticks > DoseEndTime.Ticks)
            {
                return 0;
            }

            // 刻み時間(秒)ごとの投与量に変換
            double seconds =  GetTimeUnitConvertFactor(TimeUnit, TimeUnitEnum.second);
            double weight = FlowVelocity / seconds * StepSeconds;
            weight *= (double) WeightUnit * GetWeightUnitConvertFactor(WeightUnit, WeightUnitEnum.ug);
            // 濃度がmg/Lと定義されているのでmg基準に合わせる
            weight /= GetWeightUnitConvertFactor(WeightUnitEnum.mg, WeightUnit);
            return weight;
        }

        public void Inittialize()
        {
        }
    }
}
