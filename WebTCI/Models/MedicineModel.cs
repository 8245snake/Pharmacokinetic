using System.Collections.Generic;
using static WebTCI.Models.DosingMethodModel;
using static WebTCI.Services.PkModelService;
using Simulator.Values;

namespace WebTCI.Models
{
    public class MedicineModel
    {
        public delegate void ModelSelectionChangedHandler(MedicineModel medicine, PkModel model, bool isChecked);

        public event ModelSelectionChangedHandler ModelSelectionChanged;

        public MedicineTypes MedicineType { get; set; }
        public string MedicineName { get; set; }
        public int DisplayValueMax { get; set; } = 10;

        public ConcentrationValueUnit DisplayUnit { get; set; } =
            new ConcentrationValueUnit(1, ValueUnit.WeightUnitEnum.ng, ValueUnit.VolumeUnitEnum.mL);

        private List<DosingMethodModel> _DosingMethods = new List<DosingMethodModel>();
        private List<PkModel> _PkModels = new List<PkModel>();

        public MedicineModel(MedicineTypes medicineType, string medicineName)
        {
            MedicineType = medicineType;
            MedicineName = medicineName;
        }

        ~MedicineModel()
        {
            foreach (var model in _PkModels)
            {
                model.SelectedChanged -= OnSelectedChanged;
            }

            _PkModels = null;
            _DosingMethods = null;
        }


        public void AddDosingMethod(DosingMethodModel method)
        {
            _DosingMethods.Add(method);
        }

        public IEnumerable<DosingMethodModel> GetDosingMethods(DosingTypes dosingType)
        {
            foreach (var method in _DosingMethods)
            {
                if (method.DosingType == dosingType)
                {
                    yield return method;
                }
            }
        }

        public IEnumerable<DosingMethodModel> GetDosingMethods()
        {
            foreach (var method in _DosingMethods)
            {
                yield return method;
            }
        }

        public IEnumerable<PkModel> GetPkModels()
        {
            return _PkModels;
        }

        public void AddPkModel(PkModel model)
        {
            // モデル選択状態の変化を捕捉する
            model.SelectedChanged += OnSelectedChanged;
            _PkModels.Add(model);
        }

        private void OnSelectedChanged(PkModel model, bool ischecked)
        {
            // どれか１つモデルの選択状態が変化したらイベント発火
            OnModelSelectionChanged(this, model, ischecked);
        }


        protected virtual void OnModelSelectionChanged(MedicineModel medicine, PkModel model, bool ischecked)
        {
            ModelSelectionChanged?.Invoke(medicine, model, ischecked);
        }
    }
}