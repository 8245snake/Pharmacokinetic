using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Simulator.Dosing;
using Simulator.Models;
using Simulator.Values;
using static Simulator.Values.ValueUnit;


namespace Simulator
{
    public class PharmacokineticSimulator
    {
        /// <summary>
        /// 計算開始時刻
        /// </summary>
        public DateTime CalculationStartTime { get; set; } = DateTime.Now;
        
        /// <summary>
        /// 計算時間範囲（分）
        /// </summary>
        public int DurationdMinutes { get; set; } = 60 * 4;

        /// <summary>
        /// 計算終了時刻
        /// </summary>
        public DateTime CalculationLastTime
        {
            get => CalculationStartTime.AddMinutes(DurationdMinutes);
        }

        /// <summary>
        /// 何秒刻みで計算するか
        /// </summary>
        public int StepSeconds { get; set; } = 60;

        /// <summary>
        /// 投与データ
        /// </summary>
        private List<IMedicineDosing> _MedicineDosingList = new List<IMedicineDosing>();

        public PharmacokineticSimulator(){}

        /// <summary>
        /// シミュレーターを作成します
        /// </summary>
        /// <param name="start">計算開始時刻</param>
        /// <param name="step">計算間隔(秒)</param>
        /// <param name="duration">計算時間範囲（分）</param>
        public PharmacokineticSimulator(DateTime start, int step, int duration)
        {
            DurationdMinutes = duration;
            StepSeconds = step;
            CalculationStartTime = start;
        }


        /// <summary>
        /// ボーラス投与
        /// </summary>
        /// <param name="time">投与時刻</param>
        /// <param name="amount">投与量</param>
        /// <param name="weightUnit">質量単位</param>
        /// <param name="offsetSecond">ピークに達するまでの遅延時間（秒）。拡散時間を表現するため。
        /// 計算間隔より精度を高めることはできないので必ず<see cref="StepSeconds"/>の倍数を指定すること。</param>
        public void BolusDose(DateTime time, double amount, WeightUnitEnum weightUnit, int offsetSecond = 0)
        {
            IMedicineDosing dosing;

            if ((offsetSecond > 0) && (offsetSecond % StepSeconds == 0))
            {
                // 拡散時間ありのボーラスはoffsetSecond秒の持続を入れる
                WeightFlowValueUnit flow = new WeightFlowValueUnit(amount / offsetSecond, weightUnit, TimeUnitEnum.second);
                DateTime end = time.AddSeconds(offsetSecond);
                dosing = new ContinuousMedicineDosing(time, end, flow);
            }
            else
            {
                // 普通のボーラス投与
                dosing = new BolusMedicineDosing(time, amount, weightUnit);
            }

            _MedicineDosingList.Add(dosing);

        }

        /// <summary>
        /// ボーラス投与
        /// </summary>
        /// <param name="time">投与時刻</param>
        /// <param name="weight">投与量</param>
        /// <param name="offsetSecond">ピークに達するまでの遅延時間（秒）。拡散時間を表現するため。
        /// 計算間隔より精度を高めることはできないので必ず<see cref="StepSeconds"/>の倍数を指定すること。</param>
        public void BolusDose(DateTime time, WeightValueUnit weight, int offsetSecond = 0)
        {
            BolusDose(time, weight.Value, weight.WeightUnit, offsetSecond);
        }

        /// <summary>
        /// 持続投与
        /// </summary>
        /// <param name="start">開始時刻</param>
        /// <param name="end">終了時刻</param>
        /// <param name="flow">流速</param>
        /// <param name="weightUnit">質量単位</param>
        /// <param name="timeUnit">時間単位</param>
        public void ContinuousDose(DateTime start, DateTime end, double flow, WeightUnitEnum weightUnit, TimeUnitEnum timeUnit)
        {
            ContinuousMedicineDosing dosing = new ContinuousMedicineDosing()
            {
                DoseStartTime = start,
                DoseEndTime = end,
                FlowVelocity = flow,
                WeightUnit = weightUnit,
                TimeUnit = timeUnit,
            };

            _MedicineDosingList.Add(dosing);
        }

        public void AddDosing(IMedicineDosing dosing)
        {
            _MedicineDosingList.Add(dosing);
        }

        /// <summary>
        /// 持続投与
        /// </summary>
        /// <param name="start">開始時刻</param>
        /// <param name="end">終了時刻</param>
        /// <param name="flow">流速</param>
        public void ContinuousDose(DateTime start, DateTime end, WeightFlowValueUnit flow)
        {
            ContinuousDose(start, end, flow.Value, flow.WeightUnit, flow.TimeUnit);
        }

        /// <summary>
        /// 血中濃度など予測して逐次返す
        /// </summary>
        /// <param name="predictSource">モデル</param>
        /// <param name="weightUnit">取得する濃度の重量単位</param>
        /// <para>重量の単位変換する場合は指定する（デフォルトはμg）</para>
        /// <returns>予測結果</returns>
        public IEnumerable<SimulatorResult> Predict(PharmacokineticModel predictSource, WeightUnitEnum weightUnit = WeightUnitEnum.ug)
        {
            lock (_MedicineDosingList)
            {
                // 初期化
                foreach (var item in _MedicineDosingList)
                {
                    item.Initialize();
                }

                // 刻み時間（min^-1）
                double h = 60 / (double)StepSeconds;

                // 単位時間あたりの係数に変換する
                PharmacokineticModel model = predictSource.DivideByStep(h);

                //濃度(mg/L)
                double c1 = 0.0;
                double c2 = 0.0;
                double c3 = 0.0;
                double ce = 0.0;

                DateTime targetTime = CalculationStartTime;

                while (targetTime < CalculationLastTime)
                {
                    // その時刻のボーラスと持続の投与量の合計値を出す
                    double bolus = _MedicineDosingList.Where(dosing => dosing is BolusMedicineDosing)
                        .Sum(dosing => dosing.GetDosing(targetTime, this.StepSeconds));
                    double continuous = _MedicineDosingList.Where(dosing => dosing is ContinuousMedicineDosing)
                        .Sum(dosing => dosing.GetDosing(targetTime, this.StepSeconds));

                    var result = model.RungeKuttaCalculation(c1, c2, c3, ce, bolus * h, continuous, h);
                    c1 = result.C1;
                    c2 = result.C2;
                    c3 = result.C3;
                    ce = result.Ce;

                    // 結果返却(計算結果の単位がng/mlなので所望の単位に変換して返す)
                    var factor = GetWeightUnitConvertFactor(WeightUnitEnum.ng, weightUnit);
                    yield return new SimulatorResult()
                    {
                        C1 = c1 / model.V1 / 1000 * factor,
                        C2 = c2 / model.V1 / 1000 * factor,
                        C3 = c3 / model.V1 / 1000 * factor,
                        Ce = ce / model.V1 / 1000 * factor,
                        Bolus = bolus,
                        Continuous = continuous,
                        PlotTime = targetTime,
                        WeightUnit = weightUnit,
                        VolumeUnit = VolumeUnitEnum.mL
                    };

                    // 加算
                    targetTime = targetTime.AddSeconds(StepSeconds);

                }
            }

        }

        public async Task<SimulatorResult[]> PredictAsync(PharmacokineticModel model)
        {
            return await Task.Factory.StartNew(() => Predict(model).ToArray());
        }

    }

    /// <summary>
    /// シミュレーション結果格納用構造体
    /// </summary>
    public struct SimulatorResult
    {
        /// <summary>
        /// 時刻
        /// </summary>
        public DateTime PlotTime { get; set; }

        /// <summary>
        /// 血中濃度（ng/ml）
        /// </summary>
        public double C1 { get; set; }

        /// <summary>
        /// C2濃度（ng/ml）
        /// </summary>
        public double C2 { get; set; }

        /// <summary>
        /// C3濃度（ng/ml）
        /// </summary>
        public double C3 { get; set; }

        /// <summary>
        /// 効果部位濃度（ng/ml）
        /// </summary>
        public double Ce { get; set; }

        /// <summary>
        /// ボーラス投与量（ng）
        /// </summary>
        public double Bolus { get; set; }

        /// <summary>
        /// 重量単位
        /// </summary>
        public WeightUnitEnum WeightUnit { get; set; }

        /// <summary>
        /// 容量単位
        /// </summary>
        public VolumeUnitEnum VolumeUnit { get; set; }

        /// <summary>
        /// 持続投与量（ng/min）
        /// </summary>
        public double Continuous { get; set; }

        public override string ToString()
        {
            return $"{nameof(PlotTime)}: {PlotTime}, {nameof(C1)}: {C1}, {nameof(Ce)}: {Ce}, {nameof(Bolus)}: {Bolus}, {nameof(Continuous)}: {Continuous}";
        }
    }
}