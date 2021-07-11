using NUnit.Framework;
using Simulator.Dosing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulator.Values;

namespace Simulator.Dosing.Tests
{
    [TestFixture()]
    public class ContinuousMedicineDosingTests
    {
        [Test()]
        public void GetDosingTest1()
        {
            var start = new DateTime(2021, 06, 19, 10, 0, 0);
            var stepSecond = 60;

            ContinuousMedicineDosing dosing = new ContinuousMedicineDosing
            {
                DoseStartTime = start,
                DoseEndTime = start.AddMinutes(30),
                FlowVelocity = 60,
                WeightUnit = ValueUnit.WeightUnitEnum.ng,
                TimeUnit = ValueUnit.TimeUnitEnum.hour
            };

            // 投与時間ジャスト→0が返る
            double amount = dosing.GetDosing(start, stepSecond);
            Assert.AreEqual(0, amount);
            // 1分後→1分ごとの投与量が返る
            amount = dosing.GetDosing(start.AddMinutes(1), stepSecond);
            Assert.AreEqual(1, amount);
            // 投与終了時刻→1分ごとの投与量が返る
            amount = dosing.GetDosing(start.AddMinutes(30), stepSecond);
            Assert.AreEqual(1, amount);
            // 投与終了後→0が返る
            amount = dosing.GetDosing(start.AddMinutes(31), stepSecond);
            Assert.AreEqual(0, amount);
        }

        [Test()]
        public void GetDosingTest2()
        {
            // 秒刻みで投与
            var start = new DateTime(2021, 06, 19, 10, 0, 11);
            // 6秒ごと計算
            var stepSecond = 6;

            ContinuousMedicineDosing dosing = new ContinuousMedicineDosing
            {
                DoseStartTime = start,
                DoseEndTime = start.AddMinutes(30),
                FlowVelocity = 60,
                WeightUnit = ValueUnit.WeightUnitEnum.ng,
                TimeUnit = ValueUnit.TimeUnitEnum.hour
            };

            // 投与時間ジャスト→0が返る
            double amount = dosing.GetDosing(start, stepSecond);
            Assert.AreEqual(0, amount);
            // 1秒後→1分ごとの投与量が返る
            amount = dosing.GetDosing(start.AddSeconds(1), stepSecond);
            Assert.AreEqual(1, amount);
        }

    }
}