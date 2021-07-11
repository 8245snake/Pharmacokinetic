using Simulator.Models;
using static WebTCI.Services.PkModelService;

namespace WebTCI.Models.Embedded
{
    public class Minto : PkModel
    {
        public Minto(string id, string displayName) : base(id, displayName)
        {
            MedicineType = MedicineTypes.Remifentanil;
        }

        public override PharmacokineticModel Compile(IndividualModel patient)
        {
            double LeanBodyMass;
            if (patient.IsMale)
            {
                LeanBodyMass = 1.10 * patient.Weight - 128 * (patient.Weight / patient.Stat / 100) * (patient.Weight / patient.Stat / 100);
            }
            else
            {
                LeanBodyMass = 1.07 * patient.Weight - 148 * (patient.Weight / patient.Stat / 100) * (patient.Weight / patient.Stat / 100);
            }

            var Minto_V1 = 5.1 - 0.0201 * (patient.Age - 40) + 0.072 * (LeanBodyMass - 55);
            var Minto_V2 = 9.82 - 0.0811 * (patient.Age - 40) + 0.108 * (LeanBodyMass - 55);
            var Minto_V3 = 5.42;
            var Minto_CL1 = 2.6 - 0.0162 * (patient.Age - 40) + 0.0191 * (LeanBodyMass - 55);
            var Minto_CL2 = 2.05 - 0.0301 * (patient.Age - 40);
            var Minto_CL3 = 0.076 - 0.00113 * (patient.Age - 40);
            var Minto_KE0 = 0.595 - 0.007 * (patient.Age - 40);

            var k10 = Minto_CL1 / Minto_V1;
            var k12 = Minto_CL2 / Minto_V1;
            var k13 = Minto_CL3 / Minto_V1;
            var k21 = Minto_CL2 / Minto_V2;
            var k31 = Minto_CL3 / Minto_V3;
            var ke0 = Minto_KE0;
            var v1 = Minto_V1;
            var model = new PharmacokineticModel("Remifentanil_Minto", k10, k12, k13, k21, k31, ke0, patient.Weight);
            model.V1 = v1;
            model.V2 = Minto_V2;
            model.V3 = Minto_V3;

            return model;
        }
    }
}