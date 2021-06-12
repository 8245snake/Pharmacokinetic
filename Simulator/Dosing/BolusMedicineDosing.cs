using System;
using static Simulator.Dosing.Medicine;

namespace Simulator.Dosing
{
    /// <summary>
    /// ボーラス投与を表現するクラス
    /// </summary>
    public class BolusMedicineDosing : IMedicineDosing
    {
        /// <summary>
        /// 投与時刻。丸められていない厳密な値が入っている。
        /// </summary>
        public  DateTime DoseTime { get; set; }

        /// <summary>
        /// 投与量
        /// </summary>
        public  double DoseAmount { get; set; }

        /// <summary>
        /// 投与量単位（重量）
        /// </summary>
        public WeightUnitEnum WeightUnit { get; set; }

        /// <summary>
        /// 刻み時間。
        /// </summary>
        public int StepSeconds { get; set; }

        private bool _isAlreadyReturned = false;

        /// <summary>
        /// 指定時刻の投与量を取得する。
        /// 単位はμgに統一する。
        /// </summary>
        /// <param name="time">この時刻で投与したとして相応しい場合に結果を返す。</param>
        /// <returns>投与量（単位はμg）</returns>
        public double GetDosing(DateTime time)
        {
            if (_isAlreadyReturned)
            {
                // ボーラス投与データは1回返すと用済みとなるため
                return 0;
            }

            var spanSecond = (time.Ticks - DoseTime.Ticks) / 1000 / 1000 / 10;
            if (spanSecond >= 0 && spanSecond <= StepSeconds)
            {
                _isAlreadyReturned = true;
                double conc = DoseAmount * (double) WeightUnit * GetWeightUnitConvertFactor(WeightUnit, WeightUnitEnum.ug);
                // 濃度がmg/Lと定義されているのでmg基準に合わせて返す
                return conc / GetWeightUnitConvertFactor(WeightUnitEnum.mg, WeightUnit);
            }

            return 0;
        }

        public void Inittialize()
        {
            _isAlreadyReturned = false;
        }


        public override string ToString()
        {
            return $"{DoseTime} {DoseAmount} {WeightUnit.Name()}";
        }
    }
}