using System;
using System.Reflection;
using System.Reflection.Emit;
using static WebTCI.Services.PkModelService;
using Simulator.Dosing;
using Simulator.Values;

namespace WebTCI.Models
{
    public class DosingMethodModel
    {
        public enum DosingTypes
        {
            Bolus,
            Infusion
        }

        public MedicineTypes MedicineType { get; set; }
        public DosingTypes DosingType { get; set; }

        public string UnitName => _Unit.UnitName;

        public ValueUnit Unit => _Unit;

        private ValueUnit _Unit;


        public DosingMethodModel(ValueUnit unit, MedicineTypes medicineType)
        {
            _Unit = unit;
            MedicineType = medicineType;
            if (unit is WeightValueUnit || unit is VolumeValueUnit)
            {
                DosingType = DosingTypes.Bolus;
            }else if (unit is FlowValueUnit)
            {
                DosingType = DosingTypes.Infusion;
            }
        }

        public void OnClick()
        {
            OnDosingMethodClicked(this);
        }

        public delegate void DosingMethodClickedHandler(DosingMethodModel model);
        public event DosingMethodClickedHandler DosingMethodClicked;

        protected virtual void OnDosingMethodClicked(DosingMethodModel model)
        {
            DosingMethodClicked?.Invoke(model);
        }


        public DosingDataModel CreateDosingData(double value, DateTime startTime, DateTime endTime, double individualWeight, ConcentrationValueUnit concentration)
        {
            DosingDataModel data = new DosingDataModel(this.MedicineType);

            switch (DosingType)
            {
                case DosingTypes.Bolus:

                    BolusMedicineDosing bolus = null;
                    switch (Unit)
                    {
                        case VolumeValueUnit volume:
                            volume.Value = value;
                            bolus = new BolusMedicineDosing(startTime, concentration.ToWeight(volume));
                            data.DosingValue = volume;
                            break;
                        case WeightValueUnit weight:
                            weight.Value = value;
                            bolus = new BolusMedicineDosing(startTime, weight);
                            data.DosingValue = weight;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(Unit));
                    }

                    data.Dosing = bolus;

                    break;
                case DosingTypes.Infusion:
                    ContinuousMedicineDosing infusion = null;
                    switch (Unit)
                    {
                        case GammaFlowValueUnit gamma:
                            gamma.Value = value;
                            infusion = new ContinuousMedicineDosing(startTime, endTime, gamma.ToWeightFlow(individualWeight));
                            data.DosingValue = gamma;
                            break;
                        case VolumeFlowValueUnit volumeFlow:
                            volumeFlow.Value = value;
                            volumeFlow.Concentration = concentration;
                            infusion = new ContinuousMedicineDosing(startTime, endTime, volumeFlow.ToWeightFlow());
                            data.DosingValue = volumeFlow;
                            break;
                        case WeightFlowValueUnit weightFlow:
                            weightFlow.Value = value;
                            infusion = new ContinuousMedicineDosing(startTime, endTime, weightFlow);
                            data.DosingValue = weightFlow;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(Unit));
                    }

                    data.Dosing = infusion;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return data;
        }

        public DosingDataModel CreateDosingData(double value, DateTime startTime, double individualWeight, ConcentrationValueUnit concentration)
        {
            return CreateDosingData(value, startTime, DateTime.MaxValue, individualWeight, concentration);
        }
    }
}