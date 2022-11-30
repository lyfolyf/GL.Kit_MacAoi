using GL.Kit.Extension;
using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Forms
{
    public class EnumCombobox
    {
        readonly ComboBox comboBox;
        readonly Type enumType;
        List<EnumInfo> enumInfos;

        public EnumCombobox(ComboBox comboBox, Type enumType)
        {
            if (!enumType.IsEnum) throw new ArgumentException();

            this.comboBox = comboBox;
            this.enumType = enumType;

            ComboboxBindingEnum();
        }

        void ComboboxBindingEnum()
        {
            enumInfos = new List<EnumInfo>();

            foreach (Enum @enum in Enum.GetValues(enumType))
            {
                EnumInfo enumInfo = new EnumInfo
                {
                    Enum = @enum,
                    Description = @enum.ToDescription()
                };

                enumInfos.Add(enumInfo);
            }

            comboBox.DataSource = enumInfos;
            comboBox.DisplayMember = nameof(EnumInfo.Description);
        }

        public object SelectedItem
        {
            get
            {
                return ((EnumInfo)comboBox.SelectedItem)?.Enum;
            }
            set
            {
                comboBox.SelectedItem = enumInfos.FirstOrDefault(a => a.Enum.Equals(value));
            }
        }

        class EnumInfo
        {
            public Enum Enum { get; set; }

            public string Description { get; set; }
        }
    }
}
