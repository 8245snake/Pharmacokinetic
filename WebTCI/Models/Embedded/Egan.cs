using Simulator.Models;
using static WebTCI.Services.PkModelService;

namespace WebTCI.Models.Embedded
{
    public class Egan : PkModel
    {
        public Egan(string id, string displayName) : base(id, displayName)
        {
            MedicineType = MedicineTypes.Remifentanil;
        }

        public override PharmacokineticModel Compile(IndividualModel patient)
        {
            var k10 = 0.396;
            var k12 = 0.323;
            var k13 = 0.022;
            var k21 = 0.147;
            var k31 = 0.016;
            var ke0 = 0.433;
            var v1 = 0.089873418;
            return new PharmacokineticModel("Remifentanil_Egan", k10, k12, k13, k21, k31, ke0, v1, patient.Weight);
        }
    }
}