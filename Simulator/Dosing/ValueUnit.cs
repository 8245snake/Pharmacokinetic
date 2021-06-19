using System;

namespace Simulator.Dosing
{
    public class ValueUnit
    {

        #region const

        public enum WeightUnitEnum
        {
            /// <summary>
            /// キログラム
            /// </summary>
            /// <remarks>SI単位を基準にしてg=1にしたため本当は0.001にしたかったが、Enumなので少数が定義できなかった</remarks>
            kg = -1,
            /// <summary>
            /// グラム
            /// </summary>
            g = 1,
            /// <summary>
            /// ミリグラム
            /// </summary>
            mg = 1000,
            /// <summary>
            /// マイクログラム
            /// </summary>
            ug = 1000 * 1000,
            /// <summary>
            /// ナノグラム
            /// </summary>
            ng = 1000 * 1000 * 1000,
            /// <summary>
            /// なし
            /// </summary>
            None = 0
        }

        public enum VolumeUnitEnum
        {
            /// <summary>
            /// リットル
            /// </summary>
            L = 1,
            /// <summary>
            /// ミリリットル
            /// </summary>
            mL = 1000,
            /// <summary>
            /// なし
            /// </summary>
            None = 0
        }

        public enum TimeUnitEnum
        {
            /// <summary>
            /// 時間
            /// </summary>
            hour = 1,
            /// <summary>
            /// 分
            /// </summary>
            minute = 60,
            /// <summary>
            /// 秒
            /// </summary>
            second = 60 * 60,
            /// <summary>
            /// なし
            /// </summary>
            None = 0
        }

        #endregion

        #region Operator
        public double Value { get; set; } = 0.0;


        public virtual ValueUnit Plus(ValueUnit other)
        {
            return other;
        }

        public virtual ValueUnit Minus(ValueUnit other)
        {
            return other;
        }

        public virtual ValueUnit Multiply(ValueUnit other)
        {
            return other;
        }

        public virtual ValueUnit Divide(ValueUnit other)
        {
            return other;
        }

        public static ValueUnit operator +(ValueUnit a, ValueUnit b)
        {
            return a.Plus(b);
        }

        public static ValueUnit operator -(ValueUnit a, ValueUnit b)
        {
            return a.Minus(b);
        }

        public static ValueUnit operator *(ValueUnit a, ValueUnit b)
        {
            return a.Multiply(b);
        }

        public static ValueUnit operator /(ValueUnit a, ValueUnit b)
        {
            return a.Divide(b);
        }

        #endregion




        #region statucMethod

        /// <summary>
        /// 単位換算するための係数
        /// </summary>
        /// <param name="from">変換元単位</param>
        /// <param name="to">変換先単位</param>
        /// <returns>係数</returns>
        public static double GetWeightUnitConvertFactor(WeightUnitEnum from, WeightUnitEnum to)
        {
            return (double)to / (double)from;
        }

        /// <summary>
        /// 単位換算するための係数
        /// </summary>
        /// <param name="from">変換元単位</param>
        /// <param name="to">変換先単位</param>
        /// <returns>係数</returns>
        public static double GetTimeUnitConvertFactor(TimeUnitEnum from, TimeUnitEnum to)
        {
            return (double)to / (double)from;
        }

        #endregion



    }
}