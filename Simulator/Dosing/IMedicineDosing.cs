using System;

namespace Simulator.Dosing
{
    public interface IMedicineDosing
    {
        /// <summary>
        /// 指定時刻の投与量を取得する
        /// </summary>
        /// <param name="time">時刻</param>
        /// <param name="stepSeconds">刻み時間(秒)</param>
        /// <returns>投与量</returns>
        double GetDosing(DateTime time, int stepSeconds);

        /// <summary>
        /// 初期化処理
        /// </summary>
        void Initialize();

    }
}