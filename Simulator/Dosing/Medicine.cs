

namespace Simulator.Dosing
{
    public class Medicine
    {
        #region const


        public enum WeightUnitEnum
        {
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
            second = 60 * 60
        }

        #endregion

        public  string Name { get; set; }



    }

}
