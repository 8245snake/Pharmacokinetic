using System;
using System.Collections.Generic;
using System.Linq;
using Simulator.Values;
using WebTCI.Models;
using WebTCI.Models.Embedded;
using static Simulator.Values.ValueUnit;
using static WebTCI.Models.DosingMethodModel;

namespace WebTCI.Services
{
    public class PkModelService
    {

        private List<MedicineModel> _MedicineModels;

        #region 薬剤選択変更イベント系
        public enum MedicineTypes
        {
            Propofol,
            Remifentanil,
            Fentanil,
            Rocuronium
        }

        private MedicineTypes _selectedMedicineType;

        public MedicineTypes SelectedMedicineType
        {
            get => _selectedMedicineType;
            set
            {
                _selectedMedicineType = value;
                OnSelectedMedicineChanged(value);
            }
        }

        public MedicineModel SelectedMedicine => _MedicineModels.FirstOrDefault(x => x.MedicineType == SelectedMedicineType);

        public delegate void SelectedMedicineChangedEventHandler(MedicineTypes medicine);
        public event SelectedMedicineChangedEventHandler SelectedMedicineChanged;

        protected virtual void OnSelectedMedicineChanged(MedicineTypes medicine)
        {
            SelectedMedicineChanged?.Invoke(medicine);
        }

        #endregion

        public delegate void ModelSelectionChangedHandler(MedicineModel medicine, PkModel model, bool isChecked);
        public event ModelSelectionChangedHandler ModelSelectionChanged;

        private void OnModelSelectionChanged(MedicineModel medicine, PkModel model, bool ischecked)
        {
            ModelSelectionChanged?.Invoke(medicine, model, ischecked);
        }


        public PkModelService()
        {
            _MedicineModels = GenerateList();
        }


        /// <summary>
        /// 指定した薬剤の投与法を返す
        /// </summary>
        /// <param name="medicineType">薬剤タイプ</param>
        /// <param name="dosingType">投与法タイプ</param>
        /// <returns>投与法</returns>
        public IEnumerable<DosingMethodModel> GetDosingMethods(MedicineTypes medicineType, DosingTypes dosingType)
        {
            foreach (MedicineModel medicine in _MedicineModels.Where(item => item.MedicineType == medicineType))
            {
                foreach (var dosing in medicine.GetDosingMethods(dosingType))
                {
                    yield return dosing;
                }
            }
        }

        public IEnumerable<DosingMethodModel> GetDosingMethods(MedicineTypes medicineType)
        {
            foreach (MedicineModel medicine in _MedicineModels.Where(item => item.MedicineType == medicineType))
            {
                foreach (var dosing in medicine.GetDosingMethods())
                {
                    yield return dosing;
                }
            }
        }

        /// <summary>
        /// 指定した薬剤の薬物動態モデルを返す
        /// </summary>
        /// <param name="medicineType">薬剤タイプ</param>
        /// <returns>薬物動態モデル</returns>
        public IEnumerable<PkModel> GetPkModels(MedicineTypes medicineType)
        {
            foreach (MedicineModel medicine in _MedicineModels.Where(item => item.MedicineType == medicineType))
            {
                foreach (var model in medicine.GetPkModels())
                {
                    yield return model;
                }
            }
        }


        #region 静的メソッド

        /// <summary>
        /// 選択可能な薬剤とインデックスを返す。
        /// 将来的には設定値をパースする。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetMedicineOption()
        {
            yield return new Tuple<int, string>((int) MedicineTypes.Propofol, "プロポフォール");
            yield return new Tuple<int, string>((int) MedicineTypes.Remifentanil, "レミフェンタニル");
            yield return new Tuple<int, string>((int) MedicineTypes.Fentanil, "フェンタニル");
            yield return new Tuple<int, string>((int) MedicineTypes.Rocuronium, "ロクロニウム");
        }

        /// <summary>
        /// 薬剤モデルのリストを返す。
        /// 将来的には設定値をパースする。
        /// </summary>
        /// <returns></returns>
        private List<MedicineModel> GenerateList()
        {
            List<MedicineModel> list = new List<MedicineModel>();
            MedicineModel medicine = new MedicineModel(MedicineTypes.Propofol, "プロポフォール");
            medicine.AddDosingMethod(new DosingMethodModel(new WeightValueUnit(0, WeightUnitEnum.ug), MedicineTypes.Propofol));
            medicine.AddDosingMethod(new DosingMethodModel(new WeightValueUnit(0, WeightUnitEnum.mg), MedicineTypes.Propofol));
            medicine.AddDosingMethod(new DosingMethodModel(new GammaFlowValueUnit(0, WeightUnitEnum.mg, TimeUnitEnum.hour), MedicineTypes.Propofol));
            medicine.AddPkModel(new Marsh("propofol-1", "Marsh"));
            medicine.AddPkModel(new Kataria("propofol-2", "Katarina"));
            medicine.AddPkModel(new Eleveld("propofol-3", "Eleveld"));
            medicine.ModelSelectionChanged += OnModelSelectionChanged;
            list.Add(medicine);

            medicine = new MedicineModel(MedicineTypes.Remifentanil, "レミフェンタニル");
            medicine.AddDosingMethod(new DosingMethodModel(new WeightValueUnit(0, WeightUnitEnum.ug), MedicineTypes.Remifentanil));
            medicine.AddDosingMethod(new DosingMethodModel(new WeightValueUnit(0, WeightUnitEnum.mg), MedicineTypes.Remifentanil));
            medicine.AddDosingMethod(new DosingMethodModel(new GammaFlowValueUnit(0, WeightUnitEnum.mg, TimeUnitEnum.minute), MedicineTypes.Remifentanil));
            medicine.AddPkModel(new Egan("remifentanil-1", "Egan"));
            medicine.AddPkModel(new Minto("remifentanil-2", "Minto"));
            medicine.ModelSelectionChanged += OnModelSelectionChanged;
            list.Add(medicine);

            return list;
        }


        #endregion



    }
}