using System;
using Microsoft.CodeAnalysis;
using Simulator.Models;
using static WebTCI.Services.PkModelService;

namespace WebTCI.Models
{
    public class PkModel
    {
        private bool _isSelected = false;
        public MedicineTypes MedicineType { get; set; }

        public string ModelID { get; set; }
        public string DisplayName { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnSelectedChanged(this, value);
            }
        }

        public delegate void SelectedChangedHandler(PkModel model, bool isChecked);

        public event SelectedChangedHandler SelectedChanged;

        protected virtual void OnSelectedChanged(PkModel model, bool ischecked)
        {
            SelectedChanged?.Invoke(model, ischecked);
        }

        public PkModel(string id, string displayName)
        {
            ModelID = id;
            DisplayName = displayName;
        }

        /// <summary>
        /// Marshモデルを作成するテスト関数
        /// 本当は設定とかをパースしたい
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public PharmacokineticModel CompileMarsh(IndividualModel patient)
        {
            return new PharmacokineticModel("Propofol_Marsh", 0.119, 0.114, 0.042, 0.055, 0.003, 0.260, 0.228, patient.Weight);
        }

        public virtual PharmacokineticModel Compile(IndividualModel patient)
        {
            throw new NotImplementedException();
        }
    }
}