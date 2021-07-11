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
    public class WeightFlowValueUnitTests
    {
        [Test()]
        public void ConvertUnitTest1()
        {
            WeightFlowValueUnit flow = new WeightFlowValueUnit(1000, WeightUnitEnum.ug, TimeUnitEnum.hour);
            var conv = flow.ConvertUnit(WeightUnitEnum.mg, TimeUnitEnum.hour);
            Assert.AreEqual("1mg/h", conv.ToString());
        }

        [Test()]
        public void ConvertUnitTest2()
        {
            WeightFlowValueUnit flow = new WeightFlowValueUnit(60, WeightUnitEnum.mg, TimeUnitEnum.hour);
            var conv = flow.ConvertUnit(WeightUnitEnum.mg, TimeUnitEnum.minute);
            Assert.AreEqual("1mg/min", conv.ToString());
        }

        [Test()]
        public void ConvertUnitTest3()
        {
            WeightFlowValueUnit flow = new WeightFlowValueUnit(360, WeightUnitEnum.mg, TimeUnitEnum.hour);
            var conv = flow.ConvertUnit(WeightUnitEnum.ug, TimeUnitEnum.second);
            Assert.AreEqual("100μg/sec", conv.ToString());
        }

        [Test()]
        public void ConvertTimeUnitTest1()
        {
            WeightFlowValueUnit flow = new WeightFlowValueUnit(60, WeightUnitEnum.mg, TimeUnitEnum.hour);
            var conv = flow.ConvertTimeUnit(TimeUnitEnum.minute);
            Assert.AreEqual("1mg/min", conv.ToString());
        }


        [Test()]
        public void ToWeightTest1()
        {
            WeightFlowValueUnit flow = new WeightFlowValueUnit(60, WeightUnitEnum.mg, TimeUnitEnum.hour);
            var weight = flow.ToWeight(1, TimeUnitEnum.hour);
            Assert.AreEqual("60mg", weight.ToString());

            weight = flow.ToWeight(1, TimeUnitEnum.minute);
            Assert.AreEqual("1mg", weight.ToString());

            flow = new WeightFlowValueUnit(3600, WeightUnitEnum.mg, TimeUnitEnum.hour);
            weight = flow.ToWeight(10, TimeUnitEnum.second);
            Assert.AreEqual("10mg", weight.ToString());
        }

        [Test()]
        public void ToWeightTest2()
        {
            WeightFlowValueUnit flow = new WeightFlowValueUnit(60, WeightUnitEnum.mg, TimeUnitEnum.hour);
            var weight = flow.ToWeight(new TimeValueUnit(1, TimeUnitEnum.hour));
            Assert.AreEqual("60mg", weight.ToString());
        }
    }
}