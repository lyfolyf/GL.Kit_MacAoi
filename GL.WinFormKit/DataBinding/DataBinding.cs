namespace System.Windows.Forms
{
    public static class DataBinding
    {
        /// <summary>
        /// RadioButton 绑定 Enum
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataMember">要绑定到的属性名称</param>
        public static Binding RadioButtonBindingEnum(object dataSource, string dataMember)
        {
            Binding binding = new Binding("Checked", dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (sender, e) =>
            {
                e.Value = ((Binding)sender).Control.Tag.Equals(e.Value);
            };
            binding.Parse += (sender, e) =>
            {
                if ((bool)e.Value == true)
                    e.Value = ((Binding)sender).Control.Tag;
            };

            return binding;
        }

        public static void BindEnum<TEnum>(this ComboBox cmb) where TEnum : Enum
        {
            foreach (TEnum level in Enum.GetValues(typeof(TEnum)))
            {
                cmb.Items.Add(level);
            }
        }
    }

}
