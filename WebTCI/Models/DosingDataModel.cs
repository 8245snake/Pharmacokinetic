using System;
using Simulator.Dosing;
using Simulator.Values;
using static WebTCI.Models.DosingMethodModel;
using static WebTCI.Services.PkModelService;

namespace WebTCI.Models
{
    public class DosingDataModel
    {
        public MedicineTypes MedicineType { get; set; }
        public DosingTypes DosingType { get; set; }
        public ValueUnit DosingValue { get; set; }

        public DateTime DosingTime
        {
            get
            {
                switch (DosingType)
                {
                    case DosingTypes.Bolus:
                        return DosingDataBolus.DoseTime;
                    case DosingTypes.Infusion:
                        return DosingDataInfusion.DoseStartTime;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private BolusMedicineDosing DosingDataBolus;
        private ContinuousMedicineDosing DosingDataInfusion;

        public IMedicineDosing Dosing
        {
            get
            {
                switch (DosingType)
                {
                    case DosingTypes.Bolus:
                        return DosingDataBolus;
                    case DosingTypes.Infusion:
                        return DosingDataInfusion;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (value)
                {
                    case BolusMedicineDosing bolus:
                        DosingDataBolus = bolus;
                        DosingType = DosingTypes.Bolus;
                        break;
                    case ContinuousMedicineDosing infusion:
                        DosingDataInfusion = infusion;
                        DosingType = DosingTypes.Infusion;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        public DosingDataModel(MedicineTypes medicineType)
        {
            MedicineType = medicineType;
        }

        public DosingDataModel(MedicineTypes medicineType, DosingTypes dosingType)
            : this(medicineType)
        {
            DosingType = dosingType;
        }

        public DosingDataModel(MedicineTypes medicineType, IMedicineDosing dosing)
            : this(medicineType)
        {
            Dosing = dosing;
        }

    }
}