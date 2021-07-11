using System;

namespace Simulator.Values
{
    /// <summary>
    /// 体積単位での流速を表現するクラス
    /// </summary>
    public class VolumeFlowValueUnit : FlowValueUnit
    {

        private VolumeUnitEnum _volumeUnit = VolumeUnitEnum.None;

        /// <summary>
        /// 体積単位
        /// </summary>
        public VolumeUnitEnum VolumeUnit
        {
            get => _volumeUnit;
        }

        /// <summary>
        /// 濃度
        /// </summary>
        public ConcentrationValueUnit Concentration { get; set; }

        /// <summary>
        /// 体積/時間で流速データを作成します。濃度も必要になります。
        /// </summary>
        /// <param name="flow">流速</param>
        /// <param name="volumeUnit">体積単位</param>
        /// <param name="concentration">濃度</param>
        /// <param name="timeUnit">時刻単位</param>
        public VolumeFlowValueUnit(double flow, VolumeUnitEnum volumeUnit, ConcentrationValueUnit concentration, TimeUnitEnum timeUnit)
        {
            Value = flow;
            _volumeUnit = volumeUnit;
            Concentration = concentration;
            TimeUnit = timeUnit;
        }

        /// <summary>
        /// 体積、時間単位を任意の単位に変換します。
        /// </summary>
        /// <param name="volumeUnit">変換先の体積単位</param>
        /// <param name="timeUnit">変換先の時間単位</param>
        /// <returns>変換された流速データ</returns>
        public VolumeFlowValueUnit ConvertUnit(VolumeUnitEnum volumeUnit, TimeUnitEnum timeUnit)
        {
            if (this.VolumeUnit == VolumeUnitEnum.None)
            {
                throw new NotImplementedException("体積が未設定です");
            }
            var volume = new VolumeValueUnit(this.Value, this.VolumeUnit).ConvertUnit(volumeUnit);
            var time = new TimeValueUnit(1, this.TimeUnit).ConvertUnit(timeUnit);
            return new VolumeFlowValueUnit(volume.Value / time.Value, volumeUnit, Concentration, timeUnit);
        }

        /// <summary>
        /// 時間単位を変換します
        /// </summary>
        /// <param name="timeUnit">時間単位</param>
        /// <returns>変換された流速データ</returns>
        public override FlowValueUnit ConvertTimeUnit(TimeUnitEnum timeUnit)
        {
            TimeValueUnit time = new TimeValueUnit(1, this.TimeUnit).ConvertUnit(timeUnit);
            return new VolumeFlowValueUnit(this.Value / time.Value, this.VolumeUnit, Concentration, timeUnit);
        }

        /// <summary>
        /// 指定した時間分の流量（重量）を取得します。
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="timeUnit">時間単位</param>
        /// <returns>重量<seealso cref="WeightValueUnit"/>。単位は濃度の重量単位と同じ。</returns>
        public override WeightValueUnit ToWeight(double value, TimeUnitEnum timeUnit)
        {
            // 単位時間あたりの体積を出す
            var conv = this.ConvertTimeUnit(timeUnit) as VolumeFlowValueUnit;
            // 単位時間あたりの体積と濃度から重量を出す
            var weight = Concentration.ToWeight(conv.Value, conv.VolumeUnit);
            // 投与時間を掛けて返す
            return new WeightValueUnit(weight.Value * value, weight.WeightUnit);
        }

        /// <summary>
        /// 重量単位に換算した流速を取得します。
        /// </summary>
        /// <returns>流速の<seealso cref="WeightFlowValueUnit"/>データ</returns>
        public WeightFlowValueUnit ToWeightFlow()
        {
            // 単位時間あたりの重量を出して流速に変換
            var weight = this.ToWeight(1, this.TimeUnit);
            return new WeightFlowValueUnit(weight.Value, weight.WeightUnit, this.TimeUnit);
        }

        /// <summary>
        /// 指定した時間分の流量（重量）を取得します。
        /// </summary>
        /// <param name="time">時間</param>
        /// <returns>重量。単位は濃度の重量単位と同じ。</returns>
        public override WeightValueUnit ToWeight(TimeValueUnit time)
        {
            return ToWeight(time.Value, time.TimeUnit);
        }
        public override string ToString()
        {
            return $"{this.Value}{UnitName}";
        }

        public override string UnitName => $"{this.VolumeUnit.Name()}/{this.TimeUnit.Name()}";

    }
}