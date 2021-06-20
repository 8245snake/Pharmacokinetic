using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Dosing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Simulator.Dosing.ValueUnit;

namespace Simulator.Dosing.Test
{
    [TestClass()]
    public class VolumeFlowValueUnitTests
    {
        [TestMethod()]
        public void ConvertUnitTest1()
        {
            // 0.1mg/mlを60ml/hで流す
            ConcentrationValueUnit conc = new ConcentrationValueUnit(0.1, WeightUnitEnum.mg, VolumeUnitEnum.mL);
            VolumeFlowValueUnit flow = new VolumeFlowValueUnit(60, VolumeUnitEnum.mL, conc, TimeUnitEnum.hour);
            var conv = flow.ConvertUnit(VolumeUnitEnum.mL, TimeUnitEnum.minute);
            Assert.AreEqual("1ml/min", conv.ToString());
        }

        [TestMethod()]
        public void ConvertTimeUnitTest1()
        {
            var conc = new ConcentrationValueUnit(5, WeightUnitEnum.mg, VolumeUnitEnum.mL);
            var flow = new VolumeFlowValueUnit(2, VolumeUnitEnum.mL, conc, TimeUnitEnum.minute);
            // 濃度5mg/mlを流速2ml/minで1時間投与 → 600mg
            Assert.AreEqual("600mg", flow.ToWeight(1, TimeUnitEnum.hour).ToString());
        }
    }
}