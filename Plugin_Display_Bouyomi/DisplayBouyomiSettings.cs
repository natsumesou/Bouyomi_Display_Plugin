using FNF.XmlSerializerSetting;

namespace Plugin_Display_Bouyomi
{
    public class DisplayBouyomiSettings : SettingsBase
    {

        public enum Position
        {
            左上, 右上, 左下, 右下, 中央
        }
        public double FontSize { get; set; } = 60f;
        public string FontColor { get; set; } = "#FF0000";
        public Position position { get; set; } = Position.左上;
        public double DisplayTimeout { get; set; } = 5f;
        public double Timeout { get { return DisplayTimeout * 1000; } set { DisplayTimeout = value; } }

        internal DisplayBouyomiPlugin plugin;

        public DisplayBouyomiSettings()
        {

        }

        public DisplayBouyomiSettings(DisplayBouyomiPlugin plugin)
        {
            this.plugin = plugin;
        }
    }
}
