using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Dosing;
using static Simulator.Dosing.ValueUnit;

namespace Simulator.Dosing.Test
{
    [TestClass()]
    public class VolumeValueUnitTests
    {
        [TestMethod()]
        public void ConvertUnitTest1()
        {
            VolumeValueUnit volume = new VolumeValueUnit(5, VolumeUnitEnum.mL);
            var conv = volume.ConvertUnit(VolumeUnitEnum.L);
            Assert.AreEqual(conv.ToString(), "0.005L");
        }

        [TestMethod()]
        public void ConvertUnitTest2()
        {
            VolumeValueUnit volume = new VolumeValueUnit(5, VolumeUnitEnum.L);
            var conv = volume.ConvertUnit(VolumeUnitEnum.mL);
            Assert.AreEqual(conv.ToString(), "5000ml");
        }

        [TestMethod()]
        public void ToStringTest1()
        {
            VolumeValueUnit volume = new VolumeValueUnit(5, VolumeUnitEnum.mL);
            Assert.AreEqual(volume.ToString(), "5ml");
        }

        [TestMethod()]
        public void ToStringTest2()
        {
            VolumeValueUnit volume = new VolumeValueUnit(5, VolumeUnitEnum.L);
            Assert.AreEqual(volume.ToString(), "5L");
        }

        [TestMethod()]
        public void ToStringTest3()
        {
            VolumeValueUnit volume = new VolumeValueUnit(5, VolumeUnitEnum.None);
            Assert.AreEqual(volume.ToString(), "5");
        }
    }
}