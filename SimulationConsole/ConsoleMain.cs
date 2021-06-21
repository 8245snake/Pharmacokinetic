using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulator;
using Simulator.Dosing;
using Simulator.Factories;
using Simulator.Models;
using Simulator.Values;
using  static  Simulator.Values.ValueUnit;

namespace SimulationConsole
{
    class ConsoleMain
    {

        static void Main(string[] args)
        {
            //TestBolus();
            //TestContinuous();
            //TestBolusPropofol();
            //TestContinuousPropofol();

            Console.ReadKey();

        }

        private static void TestBolus()
        {
            var time = new DateTime(2021, 6, 12, 12, 30, 0);
            var f = new PharmacokineticModelFactory();
            PharmacokineticModel model = f.Create("1");
            PharmacokineticSimulator sim = new PharmacokineticSimulator()
            {
                DurationdMinutes = 10,
                StepSeconds = 60,
                CalculationStartTime = time
            };

            // 開始時に5μg投与
            sim.BolusDose(time, 5, ValueUnit.WeightUnitEnum.ug);
            // 1分後に2μg投与
            time = time.AddMinutes(1);
            sim.BolusDose(time, 2, ValueUnit.WeightUnitEnum.ug);

            // シミュレーション開始
            foreach (var result in sim.Predict(model))
            {
                Console.WriteLine(result.ToString());
            }

        }

        private static void TestContinuous()
        {
            var time = new DateTime(2021, 6, 12, 12, 30, 0);

            var f = new PharmacokineticModelFactory();
            PharmacokineticModel model = f.Create("1");
            PharmacokineticSimulator sim = new PharmacokineticSimulator()
            {
                DurationdMinutes = 10,
                StepSeconds = 60,
                CalculationStartTime = time
            };

            // 開始時に100μg/h持続投与開始
            sim.ContinuousDose(time, time.AddMinutes(3),  100, ValueUnit.WeightUnitEnum.ug, ValueUnit.TimeUnitEnum.hour);
            // 1分後に300μg/hへ流速変更
            time = time.AddMinutes(3);
            sim.ContinuousDose(time, DateTime.MaxValue, 300, ValueUnit.WeightUnitEnum.ug, ValueUnit.TimeUnitEnum.hour);


            // シミュレーション開始
            foreach (var result in sim.Predict(model))
            {
                Console.WriteLine(result.ToString());
            }

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
            sim.BolusDose(time, 100, ValueUnit.WeightUnitEnum.mg);

            // プロポフォールのモデルでシミュレーション開始
            var f = new PharmacokineticModelFactory();
            PharmacokineticModel model = f.Create("1");
            foreach (var result in sim.Predict(model, ValueUnit.WeightUnitEnum.ng))
            {
                Console.WriteLine(result.ToString());
            }
        }

        private static void TestContinuousPropofol()
        {
            var time = new DateTime(2021, 6, 12, 12, 30, 0);

            PharmacokineticSimulator sim = new PharmacokineticSimulator()
            {
                DurationdMinutes = 10,
                StepSeconds = 30,
                CalculationStartTime = time
            };

            // 開始時に1000μg/h持続投与開始
            sim.ContinuousDose(time, DateTime.MaxValue, 1000, ValueUnit.WeightUnitEnum.ug, ValueUnit.TimeUnitEnum.hour);

            // プロポフォールのモデルでシミュレーション開始
            var f = new PharmacokineticModelFactory();
            PharmacokineticModel model = f.Create("1");
            foreach (var result in sim.Predict(model, ValueUnit.WeightUnitEnum.ug))
            {
                Console.WriteLine(result.ToString());
            }
        }

        private static void TestEleveldModel()
        {
            var time = new DateTime(2021, 6, 12, 12, 30, 0);

            PharmacokineticSimulator simulator = new PharmacokineticSimulator()
            {
                DurationdMinutes = 10,
                StepSeconds = 60,
                CalculationStartTime = time
            };

            // 開始時に100μg投与
            simulator.BolusDose(time, 100, ValueUnit.WeightUnitEnum.ug);

            // モデル作成
            var model = EleveldModelFactory.Create("動脈", 50, 1.5, 40, 2200, true, false);
            model.ConsoleLog();

            foreach (var result in simulator.Predict(model))
            {
                Console.WriteLine(result.ToString());
            }
        }
    }
}
