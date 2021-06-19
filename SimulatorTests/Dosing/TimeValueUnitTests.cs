using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Dosing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Dosing.Test
{
    [TestClass()]
    public class TimeValueUnitTests
    {
        [TestMethod()]
        public void SecondTest()
        {
            TimeValueUnit time = new TimeValueUnit(1, ValueUnit.TimeUnitEnum.second);
            Assert.AreEqual(1, time.Seconds);

            time = new TimeValueUnit(1, ValueUnit.TimeUnitEnum.minute);
            Assert.AreEqual(60, time.Seconds);

            time = new TimeValueUnit(1, ValueUnit.TimeUnitEnum.hour);
            Assert.AreEqual(3600, time.Seconds);

        }


        [TestMethod()]
        public void ToStringTest()
        {
            TimeValueUnit time = new TimeValueUnit(1, ValueUnit.TimeUnitEnum.hour);
            Assert.AreEqual("1h", time.ToString());
            time = new TimeValueUnit(1, ValueUnit.TimeUnitEnum.minute);
            Assert.AreEqual("1min", time.ToString());
            time = new TimeValueUnit(1, ValueUnit.TimeUnitEnum.second);
            Assert.AreEqual("1sec", time.ToString());
        }


        [TestMethod()]
        public void ConvertUnitTest1()
        {
            TimeValueUnit time = new TimeValueUnit(1, ValueUnit.TimeUnitEnum.hour);
            var conv = time.ConvertUnit(ValueUnit.TimeUnitEnum.hour);
            Assert.AreEqual("1h", conv.ToString());
        }


        [TestMethod()]
        public void ConvertUnitTest2()
        {
            TimeValueUnit time = new TimeValueUnit(1, ValueUnit.TimeUnitEnum.hour);
            var conv = time.ConvertUnit(ValueUnit.TimeUnitEnum.minute);
            Assert.AreEqual("60min", conv.ToString());
        }


        [TestMethod()]
        public void ConvertUnitTest3()
        {
            TimeValueUnit time = new TimeValueUnit(1, ValueUnit.TimeUnitEnum.hour);
            var conv = time.ConvertUnit(ValueUnit.TimeUnitEnum.second);
            Assert.AreEqual("3600sec", conv.ToString());
        }

        [TestMethod()]
        public void ConvertUnitTest4()
        {
            TimeValueUnit time = new TimeValueUnit(1, ValueUnit.TimeUnitEnum.hour);
            var conv = time.ConvertUnit(ValueUnit.TimeUnitEnum.None);
            Assert.AreEqual("1", conv.ToString());
        }

        [TestMethod()]
        public void ConvertUnitTest5()
        {
            TimeValueUnit time = new TimeValueUnit(60, ValueUnit.TimeUnitEnum.minute);
            var conv = time.ConvertUnit(ValueUnit.TimeUnitEnum.hour);
            Assert.AreEqual("1h", conv.ToString());

            conv = time.ConvertUnit(ValueUnit.TimeUnitEnum.minute);
            Assert.AreEqual("60min", conv.ToString());

            conv = time.ConvertUnit(ValueUnit.TimeUnitEnum.second);
            Assert.AreEqual("3600sec", conv.ToString());
        }

        [TestMethod()]
        public void ConvertUnitTest6()
        {
            TimeValueUnit time = new TimeValueUnit(3600, ValueUnit.TimeUnitEnum.second);
            var conv = time.ConvertUnit(ValueUnit.TimeUnitEnum.hour);
            Assert.AreEqual("1h", conv.ToString());

            conv = time.ConvertUnit(ValueUnit.TimeUnitEnum.minute);
            Assert.AreEqual("60min", conv.ToString());

            conv = time.ConvertUnit(ValueUnit.TimeUnitEnum.second);
            Assert.AreEqual("3600sec", conv.ToString());
        }
    }
}