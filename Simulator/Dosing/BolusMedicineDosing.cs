using System;
using static Simulator.Dosing.ValueUnit;

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
        /// ボーラス投与量をすでに返却済みか否か
        /// </summary>
        private bool _isAlreadyReturned = false;

        /// <summary>
        /// 空の投与データを作成します。
        /// </summary>
        public BolusMedicineDosing()
        {
            WeightUnit = WeightUnitEnum.None;
        }

        /// <summary>
        /// 時刻と量を指定してボーラス投与データを作成します。
        /// </summary>
        /// <param name="doseTime">投与時刻</param>
        /// <param name="doseAmount">投与量</param>
        /// <param name="weightUnit">投与量単位</param>
        public BolusMedicineDosing(DateTime doseTime, double doseAmount, WeightUnitEnum weightUnit)
        {
            DoseTime = doseTime;
            DoseAmount = doseAmount;
            WeightUnit = weightUnit;
        }

        /// <summary>
        /// 時刻と量を指定してボーラス投与データを作成します。
        /// </summary>
        /// <param name="doseTime">投与時刻</param>
        /// <param name="weight">投与量</param>
        public BolusMedicineDosing(DateTime doseTime, WeightValueUnit weight)
        : this(doseTime, weight.Value, weight.WeightUnit)
        {
        }


        /// <summary>
        /// 指定時刻の投与量を取得する。
        /// 単位はμgに統一する。
        /// </summary>
        /// <param name="time">この時刻で投与したとして相応しい場合に結果を返す。</param>
        /// <param name="stepSeconds">刻み時間(秒)</param>
        /// <returns>投与量（単位はμg）</returns>
        public double GetDosing(DateTime time, int stepSeconds)
        {
            if (_isAlreadyReturned)
            {
                // ボーラス投与データは1回返すと用済みとなるため
                return 0;
            }

            var spanSecond = (time.Ticks - DoseTime.Ticks) / 1000 / 1000 / 10;
            if (spanSecond >= 0 && spanSecond < stepSeconds)
            {
                _isAlreadyReturned = true;
                double conc = DoseAmount * (double) WeightUnit * GetWeightUnitConvertFactor(WeightUnit, WeightUnitEnum.ug);
                // 濃度がmg/Lと定義されているのでmg基準に合わせて返す
                return conc / GetWeightUnitConvertFactor(WeightUnitEnum.mg, WeightUnit);
            }

            return 0;
        }

        /// <summary>
        /// 初期化処理。
        /// 返却済みフラグをfalseにするのみ。
        /// </summary>
        public void Initialize()
        {
            _isAlreadyReturned = false;
        }


        public override string ToString()
        {
            return $"{DoseTime} {DoseAmount} {WeightUnit.Name()}";
        }
    }
}