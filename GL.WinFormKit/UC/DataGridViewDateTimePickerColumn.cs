using System.ComponentModel;

namespace System.Windows.Forms
{
    public partial class DataGridViewDateTimePickerColumn : DataGridViewColumn
    {
        public DataGridViewDateTimePickerColumn() : base(new DataGridViewDateTimePickerCell())
        {

        }

        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if (value != null && !(value is DataGridViewDateTimePickerCell))
                {
                    throw new InvalidCastException("必须是一个日历单元格");
                }
                base.CellTemplate = value;
            }
        }

        public override string ToString()
        {
            return $"DataGridViewDateTimePickerColumn {{ Name={Name}, Index={Index} }}";
        }
    }

    public class DataGridViewDateTimePickerCell : DataGridViewTextBoxCell
    {
        static readonly Type defaultFormattedValueType = typeof(string);
        static readonly Type defaultEditType = typeof(DataGridViewDateTimePickerEditingControl);
        static readonly Type defaultValueType = typeof(DateTime);

        /// <summary>
        /// 获取单元格中值的数据类型。
        /// </summary>
        public override Type ValueType
        {
            get { return defaultValueType; }
        }

        /// <summary>
        /// 获取与单元格关联的格式化值的类型
        /// </summary>
        public override Type FormattedValueType
        {
            get { return defaultFormattedValueType; }
        }

        /// <summary>
        /// 获取单元格的寄宿编辑控件的类型。
        /// </summary>
        public override Type EditType
        {
            get { return defaultEditType; }
        }

        /// <summary>
        /// 获取新记录所在行中单元格的默认值
        /// </summary>
        public override object DefaultNewRowValue
        {
            get { return DateTime.Now; }
        }

        public DataGridViewDateTimePickerCell() : base()
        {
            Style.Format = "d";
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            if (DataGridView.EditingControl is DateTimePicker dateTimePicker)
            {
                if (DateTime.TryParse(initialFormattedValue as string, out DateTime dt))
                    dateTimePicker.Value = dt;
                else
                    dateTimePicker.Value = (DateTime)DefaultNewRowValue;
            }
        }

        public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
        {
            return formattedValue;
        }
    }

    class DataGridViewDateTimePickerEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        public DataGridViewDateTimePickerEditingControl() : base()
        {
            Format = DateTimePickerFormat.Short;
        }

        public DataGridView EditingControlDataGridView { get; set; }

        public int EditingControlRowIndex { get; set; }

        public bool EditingControlValueChanged { get; set; }

        public object EditingControlFormattedValue
        {
            get { return GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting); }
            set
            {
                if (value is string newValue)
                {
                    Value = DateTime.Parse(newValue);
                }
            }
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return Value;
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            Font = dataGridViewCellStyle.Font;
            CalendarForeColor = dataGridViewCellStyle.ForeColor;
            CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return false;
            }
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {

        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        protected override void OnValueChanged(EventArgs eventargs)
        {
            EditingControlValueChanged = true;

            EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }

}
