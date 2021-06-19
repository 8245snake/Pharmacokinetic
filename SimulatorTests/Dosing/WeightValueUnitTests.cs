using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Simulator.Dosing.ValueUnit;

namespace Simulator.Dosing.Test
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

    }
}