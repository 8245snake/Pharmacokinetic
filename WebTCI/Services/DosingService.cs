
using System;
using System.Collections.Generic;
using System.ComponentModel;
using WebTCI.Models;
using static WebTCI.Models.DosingMethodModel;
using static WebTCI.Services.PkModelService;

namespace WebTCI.Services
{
    public class DosingService
    {

        public delegate void DosingDataChangedHandler(DosingService sender);
        public event DosingDataChangedHandler DosingDataChanged;

        public delegate void DosingInputWindowCalledHandler(DosingTypes dosingType, DosingMethodModel defaultDosingMethod = null, DosingDataModel defaultDosingData = null);
        public event DosingInputWindowCalledHandler DosingInputWindowCalled;

        // すべての薬剤の投与データを保持している
        private List<DosingDataModel> _Dosings = new List<DosingDataModel>();


        public IEnumerable<DosingDataModel> GetDosingModels(MedicineTypes medicine)
        {
            foreach (var dosing in _Dosings)
            {
                if (dosing.MedicineType == medicine)
                {
                    yield return dosing;
                }
            }
        }

        public void AddDosing(DosingDataModel dosing)
        {
            _Dosings.Add(dosing);
            OnDosingDataChanged();
        }

        public void RemoveDosing(DosingDataModel dosing)
        {
            _Dosings.Remove(dosing);
            OnDosingDataChanged();
        }


        protected virtual void OnDosingDataChanged()
        {
            DosingDataChanged?.Invoke(this);
        }

        public void ShowDosingInputWindow(DosingTypes dosingType, DosingMethodModel defaultDosingMethod = null, DosingDataModel defaultDosingData = null)
        {
            DosingInputWindowCalled?.Invoke(dosingType, defaultDosingMethod, defaultDosingData);
        }
    }



}