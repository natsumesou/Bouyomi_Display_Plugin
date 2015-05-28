using FNF.XmlSerializerSetting;
using System;
using System.Drawing;

namespace Plugin_Display_Bouyomi
{
    public class DisplayBouyomiSettings : SettingsBase
    {

        public enum Position
        {
            左上, 右上, 左下, 右下, 中央
        }
        public double FontSize { get; set; } = 60f;
        public string FontColor { get; set; } = "#000000";
        private string _BlurColor;
        public string BlurColor { get {
                if(_BlurColor != null) { return _BlurColor; }
                Color c = ColorTranslator.FromHtml(FontColor);
                // 白に近い色の場合は黒を返す
                if(Math.Abs(c.R - c.G) < 10 && Math.Abs(c.G - c.B) < 10 && Math.Abs(c.R - c.B) < 10 && c.R > Color.White.R/1.5)
                {
                    return ColorTranslator.ToHtml(Color.Black);
                }
                return ColorTranslator.ToHtml(Color.White);
            }
            set { _BlurColor = value; }
        }
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
