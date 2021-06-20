using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Values;

namespace SimulatorTests.Values
{
    [TestClass()]
    public class GammaFlowValueUnitTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            var gamma = new GammaFlowValueUnit(5, ValueUnit.WeightUnitEnum.ug, ValueUnit.TimeUnitEnum.minute);
            Assert.AreEqual("5μg/kg/min", gamma.ToString());
        }

        [TestMethod()]
        public void ToWeightFlowTest()
        {
            // 1μg/kg/minを50kgで計算したときの流速
            var gamma = new GammaFlowValueUnit(1, ValueUnit.WeightUnitEnum.ug, ValueUnit.TimeUnitEnum.minute);
            Assert.AreEqual("50μg/min", gamma.ToWeightFlow(50).ToString());
        }
    }
}