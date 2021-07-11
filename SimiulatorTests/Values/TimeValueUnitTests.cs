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
    public class TimeValueUnitTests
    {
        [Test()]
        public void SecondTest()
        {
            TimeValueUnit time = new TimeValueUnit(1, TimeUnitEnum.second);
            Assert.AreEqual(1, time.Seconds);

            time = new TimeValueUnit(1, TimeUnitEnum.minute);
            Assert.AreEqual(60, time.Seconds);

            time = new TimeValueUnit(1, TimeUnitEnum.hour);
            Assert.AreEqual(3600, time.Seconds);

        }


        [Test()]
        public void ToStringTest()
        {
            TimeValueUnit time = new TimeValueUnit(1, TimeUnitEnum.hour);
            Assert.AreEqual("1h", time.ToString());
            time = new TimeValueUnit(1, TimeUnitEnum.minute);
            Assert.AreEqual("1min", time.ToString());
            time = new TimeValueUnit(1, TimeUnitEnum.second);
            Assert.AreEqual("1sec", time.ToString());
        }


        [Test()]
        public void ConvertUnitTest1()
        {
            TimeValueUnit time = new TimeValueUnit(1, TimeUnitEnum.hour);
            var conv = time.ConvertUnit(TimeUnitEnum.hour);
            Assert.AreEqual("1h", conv.ToString());
        }


        [Test()]
        public void ConvertUnitTest2()
        {
            TimeValueUnit time = new TimeValueUnit(1, TimeUnitEnum.hour);
            var conv = time.ConvertUnit(TimeUnitEnum.minute);
            Assert.AreEqual("60min", conv.ToString());
        }


        [Test()]
        public void ConvertUnitTest3()
        {
            TimeValueUnit time = new TimeValueUnit(1, TimeUnitEnum.hour);
            var conv = time.ConvertUnit(TimeUnitEnum.second);
            Assert.AreEqual("3600sec", conv.ToString());
        }

        [Test()]
        public void ConvertUnitTest4()
        {
            TimeValueUnit time = new TimeValueUnit(1, TimeUnitEnum.hour);
            var conv = time.ConvertUnit(TimeUnitEnum.None);
            Assert.AreEqual("1", conv.ToString());
        }

        [Test()]
        public void ConvertUnitTest5()
        {
            TimeValueUnit time = new TimeValueUnit(60, TimeUnitEnum.minute);
            var conv = time.ConvertUnit(TimeUnitEnum.hour);
            Assert.AreEqual("1h", conv.ToString());

            conv = time.ConvertUnit(TimeUnitEnum.minute);
            Assert.AreEqual("60min", conv.ToString());

            conv = time.ConvertUnit(TimeUnitEnum.second);
            Assert.AreEqual("3600sec", conv.ToString());
        }

        [Test()]
        public void ConvertUnitTest6()
        {
            TimeValueUnit time = new TimeValueUnit(3600, TimeUnitEnum.second);
            var conv = time.ConvertUnit(TimeUnitEnum.hour);
            Assert.AreEqual("1h", conv.ToString());

            conv = time.ConvertUnit(TimeUnitEnum.minute);
            Assert.AreEqual("60min", conv.ToString());

            conv = time.ConvertUnit(TimeUnitEnum.second);
            Assert.AreEqual("3600sec", conv.ToString());
        }
    }
}