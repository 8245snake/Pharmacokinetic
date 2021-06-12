using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulator;
using  static  Simulator.Dosing.Medicine;

namespace SimulationConsole
{
    class ConsoleMain
    {

        static void Main(string[] args)
        {
            TestBolusPropofol();
        }

        private static void TestBolus()
        {
            var time = new DateTime(2021, 6, 12, 12, 30, 0);

            PharmacokineticModel model = PharmacokineticModelFactory.CreateFentanyl1(50);
            PharmacokineticSimulator sim = new PharmacokineticSimulator()
            {
                DurationdMinutes = 10,
                StepSeconds = 60,
                CalculationStartTime = time
            };

            // 開始時に5μg投与
            sim.BolusDose(time, 5, WeightUnitEnum.ug);
            // 1分後に2μg投与
            time = time.AddMinutes(1);
            sim.BolusDose(time, 2, WeightUnitEnum.ug);

            // シミュレーション開始
            foreach (var result in sim.Predict(model))
            {
                Console.WriteLine(result.ToString());
            }

            Console.ReadKey();
        }

        private static void TestContinuous()
        {
            var time = new DateTime(2021, 6, 12, 12, 30, 0);

            PharmacokineticModel model = PharmacokineticModelFactory.CreateFentanyl1(50);
            PharmacokineticSimulator sim = new PharmacokineticSimulator()
            {
                DurationdMinutes = 10,
                StepSeconds = 60,
                CalculationStartTime = time
            };

            // 開始時に100μg/h持続投与開始
            sim.ContinuousDose(time, time.AddMinutes(3),  100, WeightUnitEnum.ug, TimeUnitEnum.hour);
            // 1分後に300μg/hへ流速変更
            time = time.AddMinutes(3);
            sim.ContinuousDose(time, DateTime.MaxValue, 300, WeightUnitEnum.ug, TimeUnitEnum.hour);


            // シミュレーション開始
            foreach (var result in sim.Predict(model))
            {
                Console.WriteLine(result.ToString());
            }

            Console.ReadKey();
        }

        private static void TestBolusPropofol()
        {
            var time = new DateTime(2021, 6, 12, 12, 30, 0);

            PharmacokineticSimulator sim = new PharmacokineticSimulator()
            {
                DurationdMinutes = 10,
                StepSeconds = 60,
                CalculationStartTime = time
            };

            // 開始時に100μg投与
            sim.BolusDose(time, 100, WeightUnitEnum.ug);

            // プロポフォールのモデルでシミュレーション開始
            PharmacokineticModel model = PharmacokineticModelFactory.CreatePropofol(50);
            foreach (var result in sim.Predict(model))
            {
                Console.WriteLine(result.ToString());
            }

            Console.ReadKey();
        }
    }
}
