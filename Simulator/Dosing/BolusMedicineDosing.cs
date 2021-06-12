﻿using System;
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
        /// 投与量単位（体積）
        /// </summary>
        public VolumeUnitEnum VolumeUnit { get; set; }

        /// <summary>
        /// シミュレーション開始時刻。刻み時間の基準となる。
        /// </summary>
        public DateTime CalculationStartTime { get; set; }

        /// <summary>
        /// 刻み時間。
        /// </summary>
        public int StepSeconds { get; set; }

        private bool _IsAlreadyReturned = false;

        /// <summary>
        /// 指定時刻の投与量を取得する。
        /// 単位はμgに統一する。
        /// </summary>
        /// <param name="time">この時刻で投与したとして相応しい場合に結果を返す。</param>
        /// <returns>投与量（単位はμg）</returns>
        public double GetDosing(DateTime time)
        {
            if (_IsAlreadyReturned)
            {
                // ボーラス投与データは1回返すと用済みとなるため
                return 0;
            }

            var spanSecond = (time.Ticks - DoseTime.Ticks) / 1000 / 1000 / 10;
            if (spanSecond >= 0 && spanSecond <= StepSeconds)
            {
                _IsAlreadyReturned = true;
                return DoseAmount * WeightUnit.Factor() / VolumeUnit.Factor();
            }

            return 0;
        }


        public override string ToString()
        {
            return $"{DoseTime} {DoseAmount} {WeightUnit.Name()}/{VolumeUnit.Name()}";
        }
    }
}