using Simulator.Models;
using static WebTCI.Services.PkModelService;

namespace WebTCI.Models.Embedded
{
    public class Kataria : PkModel
    {
        public Kataria(string id, string displayName) : base(id, displayName)
        {
            MedicineType = MedicineTypes.Propofol;
        }

        public override PharmacokineticModel Compile(IndividualModel patient)
        {
            var k10 = 0.035 / 0.41;
            var k12 = 0.077 / 0.41;
            var k13 = 0.026 / 0.41;
            var k21 = patient.Weight * 0.077 / (patient.Weight * 0.78 + 3.1 * patient.Age - 15.5);
            var k31 = 0.026 / 6.9;
            var ke0 = 0.260;
            var v1 = 0.41;
            return new PharmacokineticModel("Propofol_Kataria", k10, k12, k13, k21, k31, ke0, v1, patient.Weight);
        }
    }
}