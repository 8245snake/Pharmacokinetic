﻿using System;
using System.Collections.Generic;
using System.Linq;
using Simulator.Dosing;

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

        /// <summary>
        /// ボーラス投与
        /// </summary>
        /// <param name="time">投与時刻</param>
        /// <param name="amount">投与量</param>
        /// <param name="weightUnit">質量単位</param>
        public void BolusDose(DateTime time, double amount, Medicine.WeightUnitEnum weightUnit)
        {
            BolusMedicineDosing dosing = new BolusMedicineDosing()
            {
                DoseTime = time,
                DoseAmount = amount,
                WeightUnit = weightUnit,
                StepSeconds = this.StepSeconds
            };

            _MedicineDosingList.Add(dosing);
        }

        /// <summary>
        /// 持続投与
        /// </summary>
        /// <param name="start">開始時刻</param>
        /// <param name="end">終了時刻</param>
        /// <param name="flow">流速</param>
        /// <param name="weightUnit">質量単位</param>
        /// <param name="timeUnit">時間単位</param>
        public void ContinuousDose(DateTime start, DateTime end, double flow, Medicine.WeightUnitEnum weightUnit, Medicine.TimeUnitEnum timeUnit)
        {
            ContinuousMedicineDosing dosing = new ContinuousMedicineDosing()
            {
                DoseStartTime = start,
                DoseEndTime = end,
                FlowVelocity = flow,
                WeightUnit = weightUnit,
                TimeUnit = timeUnit,
                StepSeconds = this.StepSeconds
            };

            _MedicineDosingList.Add(dosing);

        }

        /// <summary>
        /// 血中濃度など予測するして逐次返す
        /// </summary>
        /// <param name="predictSource">モデル</param>
        /// <returns>予測結果</returns>
        public IEnumerable<SimulatorResult> Predict(PharmacokineticModel predictSource)
        {
            double h = 60 / (double) StepSeconds;

            // 単位時間あたりの係数に変換する
            PharmacokineticModel model = predictSource.DeepCopy();
            model.K10 /= h;
            model.K12 /= h;
            model.K13 /= h;
            model.K21 /= h;
            model.K31 /= h;
            model.Ke0 /= h;

            //濃度(mg/L)
            double c1 = 0.0;
            double c2 = 0.0;
            double c3 = 0.0;
            double ce = 0.0;

            DateTime targetTime = CalculationStartTime;
            DateTime lastTime = CalculationLastTime;

            while (targetTime < lastTime)
            {
                // その時刻のボーラスと持続の投与量の合計値を出す
                double bolus = _MedicineDosingList.Where(dosing => dosing is BolusMedicineDosing)
                    .Sum(dosing => dosing.GetDosing(targetTime));
                double continuous = _MedicineDosingList.Where(dosing => dosing is ContinuousMedicineDosing)
                    .Sum(dosing => dosing.GetDosing(targetTime));

                var result = model.RungeKuttaCalculation(c1, c2, c3, ce, bolus * h, continuous, h);
                c1 = result.C1;
                c2 = result.C2;
                c3 = result.C3;
                ce = result.Ce;
                // 結果返却
                yield return new SimulatorResult() {
                    C1 = c1 / model.V1 /1000,
                    C2 = c2 / model.V1 / 1000,
                    C3 = c3 / model.V1 / 1000,
                    Ce = ce / model.V1 / 1000, 
                    Bolus = bolus,
                    Continuous = continuous,
                    PlotTime = targetTime};

                // 加算
                targetTime = targetTime.AddSeconds(StepSeconds);

            }

        }

    }

    public struct SimulatorResult
    {

        public DateTime PlotTime { get; set; }
        public double C1 { get; set; }
        public double C2 { get; set; }
        public double C3 { get; set; }
        public double Ce { get; set; }
        public double Bolus { get; set; }
        public double Continuous { get; set; }

        public override string ToString()
        {
            return $"{nameof(PlotTime)}: {PlotTime}, {nameof(C1)}: {C1}, {nameof(Ce)}: {Ce}, {nameof(Bolus)}: {Bolus}, {nameof(Continuous)}: {Continuous}";
        }
    }
}