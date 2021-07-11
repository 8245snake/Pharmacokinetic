namespace Simulator.Values
{
    /// <summary>
    /// 単位付きの値を表現するクラス
    /// </summary>
    public class ValueUnit
    {

        public virtual string UnitName { get; }

        #region const

        /// <summary>
        /// 重量単位
        /// </summary>
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

        /// <summary>
        /// 体積単位
        /// </summary>
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

        /// <summary>
        /// 時間単位
        /// </summary>
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

        /// <summary>
        /// 値
        /// </summary>
        public double Value { get; set; } = 0.0;

        /// <summary>
        /// 足し算
        /// </summary>
        /// <param name="other">足すデータ</param>
        /// <returns>加算結果</returns>
        public virtual ValueUnit Plus(ValueUnit other)
        {
            return other;
        }

        /// <summary>
        /// 引き算
        /// </summary>
        /// <param name="other">引くデータ</param>
        /// <returns>減算結果</returns>
        public virtual ValueUnit Minus(ValueUnit other)
        {
            return other;
        }

        /// <summary>
        /// 掛け算
        /// </summary>
        /// <param name="other">掛けるデータ</param>
        /// <returns>積算結果</returns>
        public virtual ValueUnit Multiply(ValueUnit other)
        {
            return other;
        }

        /// <summary>
        /// 割り算
        /// </summary>
        /// <param name="other">割るデータ</param>
        /// <returns>除算結果</returns>
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

        /// <summary>
        /// ディープコピー
        /// </summary>
        /// <param name="value">オリジナル</param>
        /// <returns>クローン</returns>
        public virtual ValueUnit DeepCopy(ValueUnit value)
        {
            return (ValueUnit)this.MemberwiseClone();
        }


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