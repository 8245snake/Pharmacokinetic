using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Values;
using static Simulator.Values.ValueUnit;

namespace SimulatorTests.Values
{
    [TestClass()]
    public class WeightValueUnitTests
    {
        #region ToString

        [TestMethod()]
        public void ToStringTest1()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.ng);
            Assert.AreEqual(weight.ToString(), "5ng");
        }

        [TestMethod()]
        public void ToStringTest2()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.ug);
            Assert.AreEqual(weight.ToString(), "5μg");
        }

        [TestMethod()]
        public void ToStringTest3()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.mg);
            Assert.AreEqual(weight.ToString(), "5mg");
        }

        [TestMethod()]
        public void ToStringTest4()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.kg);
            Assert.AreEqual(weight.ToString(), "5kg");
        }

        [TestMethod()]
        public void ToStringTest5()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.None);
            Assert.AreEqual(weight.ToString(), "5");
        }

        #endregion

        #region ConvertUnit

        [TestMethod()]
        public void ConvertUnitTest1()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.ng);
            var conv = weight.ConvertUnit(WeightUnitEnum.ng);
            Assert.AreEqual(conv.ToString(), "5ng");
        }

        [TestMethod()]
        public void ConvertUnitTest2()
        {
            WeightValueUnit weight = new WeightValueUnit(5000, WeightUnitEnum.ng);
            var conv = weight.ConvertUnit(WeightUnitEnum.ug);
            Assert.AreEqual(conv.ToString(), "5μg");
        }

        [TestMethod()]
        public void ConvertUnitTest3()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.ug);
            var conv = weight.ConvertUnit(WeightUnitEnum.ng);
            Assert.AreEqual(conv.ToString(), "5000ng");
        }

        [TestMethod()]
        public void ConvertUnitTest4()
        {
            WeightValueUnit weight = new WeightValueUnit(5000, WeightUnitEnum.ug);
            var conv = weight.ConvertUnit(WeightUnitEnum.mg);
            Assert.AreEqual(conv.ToString(), "5mg");
        }

        [TestMethod()]
        public void ConvertUnitTest5()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.mg);
            var conv = weight.ConvertUnit(WeightUnitEnum.ug);
            Assert.AreEqual(conv.ToString(), "5000μg");
        }

        [TestMethod()]
        public void ConvertUnitTest6()
        {
            WeightValueUnit weight = new WeightValueUnit(5000, WeightUnitEnum.mg);
            var conv = weight.ConvertUnit(WeightUnitEnum.g);
            Assert.AreEqual(conv.ToString(), "5g");
        }

        [TestMethod()]
        public void ConvertUnitTest7()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.g);
            var conv = weight.ConvertUnit(WeightUnitEnum.mg);
            Assert.AreEqual(conv.ToString(), "5000mg");
        }

        [TestMethod()]
        public void ConvertUnitTest8()
        {
            WeightValueUnit weight = new WeightValueUnit(5000, WeightUnitEnum.g);
            var conv = weight.ConvertUnit(WeightUnitEnum.kg);
            Assert.AreEqual(conv.ToString(), "5kg");
        }

        [TestMethod()]
        public void ConvertUnitTest9()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.kg);
            var conv = weight.ConvertUnit(WeightUnitEnum.g);
            Assert.AreEqual(conv.ToString(), "5000g");
        }

        #endregion

        [TestMethod()]
        public void PlusTest1()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.kg);
            var conv = weight.Plus(1, WeightUnitEnum.kg);
            Assert.AreEqual("6kg", conv.ToString());

            weight = new WeightValueUnit(5, WeightUnitEnum.kg);
            conv = weight.Plus(100, WeightUnitEnum.g);
            Assert.AreEqual("5.1kg", conv.ToString());

            weight = new WeightValueUnit(100, WeightUnitEnum.g);
            conv = weight.Plus(2, WeightUnitEnum.kg);
            Assert.AreEqual("2100g", conv.ToString());

            weight = new WeightValueUnit(1, WeightUnitEnum.g);
            conv = weight.Plus(400, WeightUnitEnum.mg);
            Assert.AreEqual("1.4g", conv.ToString());

            weight = new WeightValueUnit(100, WeightUnitEnum.mg);
            conv = weight.Plus(400, WeightUnitEnum.mg);
            Assert.AreEqual("500mg", conv.ToString());

            weight = new WeightValueUnit(100, WeightUnitEnum.mg);
            conv = weight.Plus(1, WeightUnitEnum.kg);
            Assert.AreEqual("1000100mg", conv.ToString());

        }

        [TestMethod()]
        public void PlusTest2()
        {
            WeightValueUnit a = new WeightValueUnit(5, WeightUnitEnum.kg);
            WeightValueUnit b = new WeightValueUnit(1, WeightUnitEnum.kg);
            Assert.AreEqual("6kg", (a + b).ToString());

        }

        [TestMethod()]
        public void MinusTest1()
        {
            WeightValueUnit weight = new WeightValueUnit(5, WeightUnitEnum.kg);
            var conv = weight.Minus(1, WeightUnitEnum.kg);
            Assert.AreEqual("4kg", conv.ToString());

            weight = new WeightValueUnit(5, WeightUnitEnum.kg);
            conv = weight.Minus(100, WeightUnitEnum.g);
            Assert.AreEqual("4.9kg", conv.ToString());

            weight = new WeightValueUnit(100, WeightUnitEnum.g);
            conv = weight.Minus(2, WeightUnitEnum.kg);
            Assert.AreEqual("-1900g", conv.ToString());

            weight = new WeightValueUnit(1, WeightUnitEnum.g);
            conv = weight.Minus(400, WeightUnitEnum.mg);
            Assert.AreEqual("0.6g", conv.ToString());

            weight = new WeightValueUnit(400, WeightUnitEnum.mg);
            conv = weight.Minus(100, WeightUnitEnum.mg);
            Assert.AreEqual("300mg", conv.ToString());

            weight = new WeightValueUnit(1, WeightUnitEnum.kg);
            conv = weight.Minus(100, WeightUnitEnum.mg);
            Assert.AreEqual("0.9999kg", conv.ToString());

        }

        [TestMethod()]
        public void MinusTest2()
        {
            WeightValueUnit a = new WeightValueUnit(5, WeightUnitEnum.kg);
            WeightValueUnit b = new WeightValueUnit(1, WeightUnitEnum.kg);
            Assert.AreEqual("4kg", (a - b).ToString());
        }

        [TestMethod()]
        public void DivideTest1()
        {
            WeightValueUnit a = new WeightValueUnit(50, WeightUnitEnum.mg);
            VolumeValueUnit b = new VolumeValueUnit(10, VolumeUnitEnum.mL);
            Assert.AreEqual("5mg/ml", (a / b).ToString());
        }

        [TestMethod()]
        public void DivideTest2()
        {
            WeightValueUnit a = new WeightValueUnit(50, WeightUnitEnum.mg);
            TimeValueUnit b = new TimeValueUnit(5, TimeUnitEnum.hour);
            Assert.AreEqual("10mg/h", (a / b).ToString());
        }
    }
}