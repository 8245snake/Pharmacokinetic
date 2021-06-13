
using System.Data;
using System.Windows.Forms;

namespace SimulationApp
{
    /// <summary>任意の型をコンボボックスと紐付けるためのラッパークラス</summary>
    /// <typeparam name="T">コンボボックスと紐付ける型</typeparam>
    public class GenericComboBoxWrapper<T>
    {
        private ComboBox _ComboBox;
        private DataTable _DataTable;
        private const string DispColName = "disp";
        private const string ValueColName = "value";

        public ComboBox ComboBox
        {
            get => this._ComboBox;
            set => this._ComboBox = value;
        }

        public T SelectedValue
        {
            get => (T)this._ComboBox.SelectedValue;
            set => this._ComboBox.SelectedValue = value;
        }

        public int SelectedIndex
        {
            get => this._ComboBox.SelectedIndex;
            set => this._ComboBox.SelectedIndex = value;
        }

        public GenericComboBoxWrapper(ComboBox combobox)
        {
            this._ComboBox = combobox;
            this._DataTable = new DataTable();
            this._DataTable.Columns.Add(DispColName, typeof(string));
            this._DataTable.Columns.Add(ValueColName, typeof(T));
            this._ComboBox.DisplayMember = DispColName;
            this._ComboBox.ValueMember = ValueColName;
            this._ComboBox.DataSource = this._DataTable;

        }


        /// <summary>要素を追加する</summary>
        /// <param name="dispText">表示されるテキスト</param>
        /// <param name="value">内部の値</param>
        public void Add(string dispText, T value) => this._DataTable.Rows.Add(dispText, value);

        /// <summary>コンボボックスの中身をクリアする</summary>
        public void Clear()
        {
            this._DataTable.Rows.Clear();
            this._ComboBox.Items.Clear();
        }
    }
}
