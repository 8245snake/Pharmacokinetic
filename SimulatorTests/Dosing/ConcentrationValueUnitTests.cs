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
    public class ConcentrationValueUnitTests
    {
        [TestMethod()]
        public void ToStringTest1()
        {
            ConcentrationValueUnit conc = new ConcentrationValueUnit(5, WeightUnitEnum.ug, VolumeUnitEnum.mL);
            Assert.AreEqual(conc.ToString(), "5μg/ml");
        }

        [TestMethod()]
        public void ToStringTest2()
        {
            ConcentrationValueUnit conc = new ConcentrationValueUnit(5, WeightUnitEnum.mg, VolumeUnitEnum.L);
            Assert.AreEqual(conc.ToString(), "5mg/L");
        }

        [TestMethod()]
        public void ConvertUnitTest1()
        {
            ConcentrationValueUnit conc = new ConcentrationValueUnit(5, WeightUnitEnum.ng, VolumeUnitEnum.mL);
            var converted = conc.ConvertUnit(WeightUnitEnum.ug, VolumeUnitEnum.L);
            Assert.AreEqual(converted.ToString(), "5μg/L");
        }

        [TestMethod()]
        public void ConvertUnitTest2()
        {
            ConcentrationValueUnit conc = new ConcentrationValueUnit(5, WeightUnitEnum.ug, VolumeUnitEnum.mL);
            var converted = conc.ConvertUnit(WeightUnitEnum.mg, VolumeUnitEnum.mL);
            Assert.AreEqual(converted.ToString(), "0.005mg/ml");
        }

        [TestMethod()]
        public void ConvertUnitTest3()
        {
            ConcentrationValueUnit conc = new ConcentrationValueUnit(5, WeightUnitEnum.mg, VolumeUnitEnum.mL);
            var converted = conc.ConvertUnit(WeightUnitEnum.ug, VolumeUnitEnum.mL);
            Assert.AreEqual(converted.ToString(), "5000μg/ml");
        }

        [TestMethod()]
        public void ConvertUnitTest4()
        {
            ConcentrationValueUnit conc = new ConcentrationValueUnit(0.005, WeightUnitEnum.mg, VolumeUnitEnum.mL);
            var converted = conc.ConvertUnit(WeightUnitEnum.mg, VolumeUnitEnum.L);
            Assert.AreEqual(converted.ToString(), "5mg/L");
        }

    }
}