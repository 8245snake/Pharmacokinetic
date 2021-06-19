using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Dosing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Dosing.Test
{
    [TestClass()]
    public class BolusMedicineDosingTests
    {

        [TestMethod()]
        public void GetDosingTest1()
        {
            BolusMedicineDosing dosing = new BolusMedicineDosing()
            {
                DoseTime = new DateTime(2021,6,19,10, 0, 0),
                DoseAmount = 100,
                WeightUnit = ValueUnit.WeightUnitEnum.ng,
            };

            // 投与時間ジャスト→取得成功
            double amount = dosing.GetDosing(new DateTime(2021, 6, 19, 10, 0, 0), 60);
            Assert.AreEqual(100, amount);
            // もう一度読んだら→0が返る
            amount = dosing.GetDosing(new DateTime(2021, 6, 19, 10, 0, 0), 60);
            Assert.AreEqual(0, amount);

        }

        [TestMethod()]
        public void GetDosingTest2()
        {
            BolusMedicineDosing dosing = new BolusMedicineDosing()
            {
                DoseTime = new DateTime(2021, 6, 19, 10, 0, 0),
                DoseAmount = 100,
                WeightUnit = ValueUnit.WeightUnitEnum.ng,
            };

            // 投与より1秒前→0が返る
            double amount = dosing.GetDosing(new DateTime(2021, 6, 19, 9, 59, 59), 60);
            Assert.AreEqual(0, amount);
            // 投与より1秒後→取得成功
            amount = dosing.GetDosing(new DateTime(2021, 6, 19, 10, 00, 1), 60);
            Assert.AreEqual(100, amount);

        }

        [TestMethod()]
        public void GetDosingTest3()
        {
            BolusMedicineDosing dosing = new BolusMedicineDosing()
            {
                DoseTime = new DateTime(2021, 6, 19, 10, 0, 0),
                DoseAmount = 100,
                WeightUnit = ValueUnit.WeightUnitEnum.ng,
            };

            // 刻み時間の最後→取得成功
            double amount = dosing.GetDosing(new DateTime(2021, 6, 19, 10, 00, 59), 60);
            Assert.AreEqual(100, amount);

        }

        [TestMethod()]
        public void GetDosingTest4()
        {
            BolusMedicineDosing dosing = new BolusMedicineDosing()
            {
                DoseTime = new DateTime(2021, 6, 19, 10, 0, 0),
                DoseAmount = 100,
                WeightUnit = ValueUnit.WeightUnitEnum.ng,
            };

            // 次の刻み時間→0が返る
            double amount = dosing.GetDosing(new DateTime(2021, 6, 19, 10, 1, 0), 60);
            Assert.AreEqual(0, amount);

        }

        [TestMethod()]
        public void GetDosingTest5()
        {
            BolusMedicineDosing dosing = new BolusMedicineDosing()
            {
                DoseTime = new DateTime(2021, 6, 19, 10, 0, 0),
                DoseAmount = 100,
                WeightUnit = ValueUnit.WeightUnitEnum.ng,
            };

            // 次の刻み時間→0が返る
            double amount = dosing.GetDosing(new DateTime(2021, 6, 19, 10, 0, 1), 1);
            Assert.AreEqual(0, amount);
            // 投与より1秒前→0が返る
            amount = dosing.GetDosing(new DateTime(2021, 6, 19, 9, 59, 59), 1);
            Assert.AreEqual(0, amount);
            // 投与時間ジャスト→取得成功
            amount = dosing.GetDosing(new DateTime(2021, 6, 19, 10, 0, 0), 1);
            Assert.AreEqual(100, amount);

        }
    }
}