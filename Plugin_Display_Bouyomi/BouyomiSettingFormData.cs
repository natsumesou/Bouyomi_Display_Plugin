using FNF.XmlSerializerSetting;

namespace Plugin_Display_Bouyomi
{
    public class BouyomiSettingFormData : ISettingFormData
    {
        private DisplayBouyomiSettings _settings;
        public BouyomiSettingPropertyGrid settingPropertyGrid;

        public BouyomiSettingFormData(DisplayBouyomiSettings setting)
        {
            _settings = setting;
            settingPropertyGrid = new BouyomiSettingPropertyGrid(_settings);
        }

        public bool ExpandAll
        {
            get
            {
                return false;
            }
        }

        public SettingsBase Setting
        {
            get
            {
                return _settings;
            }
        }

        public string Title
        {
            get
            {
                return _settings.plugin.Name;
            }
        }
    }
}
