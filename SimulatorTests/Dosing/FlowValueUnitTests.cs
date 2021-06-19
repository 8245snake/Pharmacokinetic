using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Dosing;
using System;
using static Simulator.Dosing.ValueUnit;

namespace Simulator.Dosing.Test
{
    [TestClass()]
    public class FlowValueUnitTests
    {
        [TestMethod()]
        public void ConvertUnitTest1()
        {
            FlowValueUnit flow = new FlowValueUnit(1000, WeightUnitEnum.ug, TimeUnitEnum.hour);
            var conv = flow.ConvertUnit(WeightUnitEnum.mg, TimeUnitEnum.hour);
            Assert.AreEqual("1mg/h", conv.ToString());
        }

        [TestMethod()]
        public void ConvertUnitTest2()
        {
            FlowValueUnit flow = new FlowValueUnit(60, WeightUnitEnum.mg, TimeUnitEnum.hour);
            var conv = flow.ConvertUnit(WeightUnitEnum.mg, TimeUnitEnum.minute);
            Assert.AreEqual("1mg/min", conv.ToString());
        }

        [TestMethod()]
        public void ConvertUnitTest3()
        {
            FlowValueUnit flow = new FlowValueUnit(360, WeightUnitEnum.mg, TimeUnitEnum.hour);
            var conv = flow.ConvertUnit(WeightUnitEnum.ug, TimeUnitEnum.second);
            Assert.AreEqual("100μg/sec", conv.ToString());
        }

        [TestMethod()]
        public void ConvertUnitTest4()
        {
            // 0.1mg/mlを60ml/hで流す
            ConcentrationValueUnit conc = new ConcentrationValueUnit(0.1, WeightUnitEnum.mg, VolumeUnitEnum.mL);
            FlowValueUnit flow = new FlowValueUnit(60, VolumeUnitEnum.mL, conc, TimeUnitEnum.hour);
            var conv = flow.ConvertUnit(VolumeUnitEnum.mL, TimeUnitEnum.minute);
            Assert.AreEqual("1ml/min", conv.ToString());
        }
    }
}