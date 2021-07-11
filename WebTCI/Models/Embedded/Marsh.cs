using Simulator.Models;
using static WebTCI.Services.PkModelService;

namespace WebTCI.Models.Embedded
{
    public class Marsh : PkModel
    {
        public Marsh(string id, string displayName) : base(id, displayName)
        {
            MedicineType = MedicineTypes.Propofol;
        }

        public override PharmacokineticModel Compile(IndividualModel patient)
        {
            return new PharmacokineticModel("Propofol_Marsh", 0.119, 0.114, 0.042, 0.055, 0.003, 0.260, 0.228, patient.Weight);
        }
    }
}