using NUnit.Framework;
using Simulator.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Simulator.Values.ValueUnit;

namespace Simulator.Values.Tests
{
    [TestFixture()]
    public class GammaFlowValueUnitTests
    {
        [Test()]
        public void ToStringTest()
        {
            var gamma = new GammaFlowValueUnit(5, WeightUnitEnum.ug, TimeUnitEnum.minute);
            Assert.AreEqual("5μg/kg/min", gamma.ToString());
        }

        [Test()]
        public void ToWeightFlowTest()
        {
            // 1μg/kg/minを50kgで計算したときの流速
            var gamma = new GammaFlowValueUnit(1, WeightUnitEnum.ug, TimeUnitEnum.minute);
            Assert.AreEqual("50μg/min", gamma.ToWeightFlow(50).ToString());
        }
    }
}