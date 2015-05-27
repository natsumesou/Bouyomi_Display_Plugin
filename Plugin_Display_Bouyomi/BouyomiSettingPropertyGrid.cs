using FNF.XmlSerializerSetting;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Plugin_Display_Bouyomi
{
    public class BouyomiSettingPropertyGrid : ISettingPropertyGrid
    {
        DisplayBouyomiSettings _settings;
        public BouyomiSettingPropertyGrid(DisplayBouyomiSettings setting)
        {
            _settings = setting;
        }

        public string GetName()
        {
            return "表示設定";
        }

        [Category("基本設定")]
        [DisplayName("フォントサイズ")]
        [Description("テキストのフォントサイズ(数字)")]
        public double fontSize { get { return _settings.FontSize; } set { _settings.FontSize = value; } }

        [Category("基本設定")]
        [DisplayName("フォントカラー")]
        [Description("テキストのフォントカラー")]
        [TypeConverter(typeof(ColorTypeConverter))]
        [Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
        public string fontColor {
            get {
                Color color = ColorTranslator.FromHtml(_settings.FontColor);
                return new CustomColor(color).ToString();
            }
            set {
                Color color = new CustomColor(value).ToColor();
                _settings.FontColor = ColorTranslator.ToHtml(color);
            }
        }

        [Category("基本設定")]
        [DisplayName("テキスト表示位置")]
        [Description("テキストの表示位置を選択")]
        public DisplayBouyomiSettings.Position position { get { return _settings.position; } set { _settings.position = value; } }

        [Category("基本設定")]
        [DisplayName("テキスト表示時間")]
        [Description("テキストが表示されてから消えるまでの時間を設定(秒)")]
        public double seconds { get { return _settings.DisplayTimeout; } set { _settings.Timeout = value; } }
    }

    /// <summary>
    /// 色を扱う構造体
    /// </summary>
    public struct CustomColor
    {
        public int r, g, b, a;

        public CustomColor(Color color) : this()
        {
            this.r = color.R;
            this.g = color.G;
            this.b = color.B;
            this.a = color.A;
        }

        /// <summary>
        /// フォーマットはToString参照
        /// </summary>
        /// <param name="color"></param>
        public CustomColor(string color) : this()
        {
            String[] v = color.Split(new char[] { ' ' });
            this.r = int.Parse(v[0]);
            this.g = int.Parse(v[1]);
            this.b = int.Parse(v[2]);
            this.a = int.Parse(v[3]);
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", r, g, b, a);
        }

        internal Color ToColor()
        {
            return Color.FromArgb(a, r, g, b);
        }

    }

    /// <summary>
    /// フォントカラーの文字列コンバータ
    /// </summary>
    class ColorTypeConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String))
            {
                CustomColor c;
                if (value == null)
                {
                    c = new CustomColor();
                }
                else
                {
                    c = new CustomColor((String)value);
                }
                return c.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is String)
            {
                return value;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }


    /// <summary>
    /// フォントカラーのカスタムエディア
    /// </summary>
    class ColorTypeEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            CustomColor c;
            if (e.Value == null)
            {
                c = new CustomColor();
            } else
            {
                c = new CustomColor((String)e.Value);

            }

            Color color = c.ToColor();
            e.Graphics.FillRectangle(new SolidBrush(color), e.Bounds);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ColorDialog d = new ColorDialog();
            d.ShowDialog();
            return new CustomColor(d.Color).ToString();
        }
    }
}
