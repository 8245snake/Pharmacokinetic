using System;
using Simulator.Models;

namespace WebTCI.Services
{
    public class PatientSelectService
    {
        private IndividualModel _selectedPatient = new IndividualModel(21, 90, 1.9, true, false, 21*55);

        public delegate void SelectedPatientChangedEventHandler(IndividualModel patient);
        public event SelectedPatientChangedEventHandler SelectedPatientChanged;

        public IndividualModel SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                _selectedPatient = value;
                OnSelectedPatientChanged(_selectedPatient);
            }
        }


        protected virtual void OnSelectedPatientChanged(IndividualModel patient)
        {
            SelectedPatientChanged?.Invoke(patient);
        }
    }
}