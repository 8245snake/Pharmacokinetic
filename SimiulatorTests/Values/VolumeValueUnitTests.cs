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
    public class VolumeValueUnitTests
    {
        [Test()]
        public void ConvertUnitTest1()
        {
            VolumeValueUnit volume = new VolumeValueUnit(5, VolumeUnitEnum.mL);
            var conv = volume.ConvertUnit(VolumeUnitEnum.L);
            Assert.AreEqual(conv.ToString(), "0.005L");
        }

        [Test()]
        public void ConvertUnitTest2()
        {
            VolumeValueUnit volume = new VolumeValueUnit(5, VolumeUnitEnum.L);
            var conv = volume.ConvertUnit(VolumeUnitEnum.mL);
            Assert.AreEqual(conv.ToString(), "5000ml");
        }

        [Test()]
        public void ToStringTest1()
        {
            VolumeValueUnit volume = new VolumeValueUnit(5, VolumeUnitEnum.mL);
            Assert.AreEqual(volume.ToString(), "5ml");
        }

        [Test()]
        public void ToStringTest2()
        {
            VolumeValueUnit volume = new VolumeValueUnit(5, VolumeUnitEnum.L);
            Assert.AreEqual(volume.ToString(), "5L");
        }

        [Test()]
        public void ToStringTest3()
        {
            VolumeValueUnit volume = new VolumeValueUnit(5, VolumeUnitEnum.None);
            Assert.AreEqual(volume.ToString(), "5");
        }
    }
}