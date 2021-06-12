﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulator;
using Simulator.Dosing;
using Simulator = Simulator.PharmacokineticSimulator;
using  static  Simulator.Dosing.Medicine;

namespace SimulationConsole
{
    class Program
    {

        //速度定数(h^-1)
        private const double k10 = 0.083;
        private const double k12 = 0.471;
        private const double k13 = 0.225;
        private const double k21 = 0.102;
        private const double k31 = 0.006;
        private const double ke0 = 0.114;
        //体重
        private const double weight = 50.0;
        //分布容積
        private const double v1 = 0.105;

        static void Main(string[] args)
        {

            var time= new DateTime(2021, 6, 12, 12, 30, 0);

            PharmacokineticModel model = new PharmacokineticModel("ﾌｪﾝﾀ", k10, k12, k13, k21, k31, ke0, v1, weight);
            PharmacokineticSimulator sim = new PharmacokineticSimulator()
            {
                DurationdMinutes = 10,
                StepSeconds = 30,
                CalculationStartTime = time
            };

            sim.BolusDose(time, 10, WeightUnitEnum.ug, VolumeUnitEnum.None);

            foreach (var result in sim.Predict(model))
            {
                Console.WriteLine(result.ToString());
            }

            Console.ReadKey();

        }
    }
}
