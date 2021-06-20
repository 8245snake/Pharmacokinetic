using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Models;

namespace SimulatorTests.Models
{
    [TestClass()]
    public class PharmacokineticModelTests
    {
        [TestMethod()]
        public void PharmacokineticModelTest1()
        {
            PharmacokineticModel model = new PharmacokineticModel("test", 50.0);
            Assert.AreEqual(model.Name, "test");
            Assert.AreEqual(model.Weight, 50.0);
            Assert.AreEqual(model.K10, 0.0);
            Assert.AreEqual(model.K12, 0.0);
            Assert.AreEqual(model.K13, 0.0);
            Assert.AreEqual(model.K21, 0.0);
            Assert.AreEqual(model.K31, 0.0);
            Assert.AreEqual(model.V1, 0.0);
            Assert.AreEqual(model.V2, 0.0);
            Assert.AreEqual(model.V3, 0.0);
            Assert.AreEqual(model.CL1, 0.0);
            Assert.AreEqual(model.CL2, 0.0);
            Assert.AreEqual(model.CL3, 0.0);
        }

        [TestMethod()]
        public void PharmacokineticModelTest2()
        {
            PharmacokineticModel model = new PharmacokineticModel("test",1,2,3,4,5,6,70);
            Assert.AreEqual(model.Name, "test");
            Assert.AreEqual(model.Weight, 70.0);
            Assert.AreEqual(model.K10, 1.0);
            Assert.AreEqual(model.K12, 2.0);
            Assert.AreEqual(model.K13, 3.0);
            Assert.AreEqual(model.K21, 4.0);
            Assert.AreEqual(model.K31, 5.0);
            Assert.AreEqual(model.V1, 0.0);
            Assert.AreEqual(model.V2, 0.0);
            Assert.AreEqual(model.V3, 0.0);
            Assert.AreEqual(model.CL1, 0.0);
            Assert.AreEqual(model.CL2, 0.0);
            Assert.AreEqual(model.CL3, 0.0);
        }

        [TestMethod()]
        public void PharmacokineticModelTest3()
        {
            PharmacokineticModel model = new PharmacokineticModel("test", 1, 2, 3, 4, 5, 6, 7,70);
            Assert.AreEqual(model.Name, "test");
            Assert.AreEqual(model.Weight, 70.0, 0.00001);
            Assert.AreEqual(model.K10, 1.0, 0.00001);
            Assert.AreEqual(model.K12, 2.0, 0.00001);
            Assert.AreEqual(model.K13, 3.0, 0.00001);
            Assert.AreEqual(model.K21, 4.0, 0.00001);
            Assert.AreEqual(model.K31, 5.0, 0.00001);
            Assert.AreEqual(model.V1, 7 * 70, 0.00001);
            Assert.AreEqual(model.V2, 2 * 7 * 70 / 4, 0.00001);
            Assert.AreEqual(model.V3, 3 * 7 * 70 / 5, 0.00001);
            Assert.AreEqual(model.CL1, 1 * 7 * 70, 0.00001);
            Assert.AreEqual(model.CL2, 2 * 7 * 70, 0.00001);
            Assert.AreEqual(model.CL3, 3 * 7 * 70, 0.00001);

        }

        [TestMethod()]
        public void RungeKuttaCalculationTest1()
        {
            // 正解データを元にテストする
            PharmacokineticModel model = new PharmacokineticModel("Fentanyl_Shafer", 0.083, 0.471, 0.225, 
                0.102, 0.006, 0.114, 0.105, 50);

            var c1 = 0.0;
            var c2 = 0.0;
            var c3 = 0.0;
            var ce = 0.0;

            // 投入直後
            var result = model.RungeKuttaCalculation(c1, c2, c3, ce, 50000, 0, 1);
            Assert.AreEqual(9.523809524, result.C1 / model.V1 / 1000, 0.01);
            Assert.AreEqual(0, result.Ce / model.V1 / 1000, 0.01);
            // 次の1分後
            result = model.RungeKuttaCalculation(result.C1, result.C2, result.C3, result.Ce, 0, 0, 1);
            Assert.AreEqual(4.536687466, result.C1 / model.V1 / 1000, 0.01);
            Assert.AreEqual(0.709213926, result.Ce / model.V1 / 1000, 0.01);
            // 次の1分後
            result = model.RungeKuttaCalculation(result.C1, result.C2, result.C3, result.Ce, 0, 0, 1);
            Assert.AreEqual(2.365159004, result.C1 / model.V1 / 1000, 0.01);
            Assert.AreEqual(0.983718955, result.Ce / model.V1 / 1000, 0.01);

        }

        [TestMethod()]
        public void RungeKuttaCalculationTest2()
        {
            // 正解データを元にテストする
            PharmacokineticModel model = new PharmacokineticModel("Fentanyl_Shafer", 0.083, 0.471, 0.225,
                0.102, 0.006, 0.114, 0.105, 50);

            var c1 = 0.0;
            var c2 = 0.0;
            var c3 = 0.0;
            var ce = 0.0;

            // 投入直後(2000μg/h)
            var result = model.RungeKuttaCalculation(c1, c2, c3, ce, 0, 33333.333, 1);
            Assert.AreEqual(4.424510075, result.C1 / model.V1 / 1000, 0.01);
            Assert.AreEqual(0.277060216, result.Ce / model.V1 / 1000, 0.01);
            // 次の1分後
            result = model.RungeKuttaCalculation(result.C1, result.C2, result.C3, result.Ce, 0, 33333.333, 1);
            Assert.AreEqual(6.611586332, result.C1 / model.V1 / 1000, 0.01);
            Assert.AreEqual(0.858843906, result.Ce / model.V1 / 1000, 0.01);
            // 次の1分後
            result = model.RungeKuttaCalculation(result.C1, result.C2, result.C3, result.Ce, 0, 33333.333, 1);
            Assert.AreEqual(7.821645444, result.C1 / model.V1 / 1000, 0.01);
            Assert.AreEqual(1.551594135, result.Ce / model.V1 / 1000, 0.01);

        }

        [TestMethod()]
        public void DeepCopyTest()
        {
            PharmacokineticModel model = new PharmacokineticModel("Fentanyl_Shafer", 0.083, 0.471, 0.225,
                0.102, 0.006, 0.114, 0.105, 50);

            var clone = model.DeepCopy();
            Assert.AreNotSame(model, clone);

            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                // プロパティの値が同じか
                Assert.AreEqual(info.GetValue(model), info.GetValue(clone));
            }

            foreach (FieldInfo info in model.GetType().GetFields(BindingFlags.Instance))
            {
                // フィールドの値が同じで参照が異なるか
                Assert.AreEqual(info.GetValue(model), info.GetValue(clone));
                Assert.AreNotSame(info.GetValue(model), info.GetValue(clone));
            }

            foreach (FieldInfo info in model.GetType().GetFields(BindingFlags.NonPublic))
            {
                // フィールドの値が同じで参照が異なるか
                Assert.AreEqual(info.GetValue(model), info.GetValue(clone));
                Assert.AreNotSame(info.GetValue(model), info.GetValue(clone));
            }

        }




    }
}