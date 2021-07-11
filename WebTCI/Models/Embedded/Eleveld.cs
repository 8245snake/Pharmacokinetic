using Simulator.Factories;
using Simulator.Models;
using static WebTCI.Services.PkModelService;

namespace WebTCI.Models.Embedded
{
    public class Eleveld : PkModel
    {
        public Eleveld(string id, string displayName) : base(id, displayName)
        {
            MedicineType = MedicineTypes.Propofol;
        }

        public override PharmacokineticModel Compile(IndividualModel patient)
        {
            return new EleveldModelFactory(patient).Create("Propofol_Eleveld");
        }
    }
}