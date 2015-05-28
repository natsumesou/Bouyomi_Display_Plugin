using System;
using FNF.BouyomiChanApp;
using FNF.XmlSerializerSetting;

namespace Plugin_Display_Bouyomi
{
    public class DisplayBouyomiPlugin : IPlugin
    {
        public string Name { get; } = "読み上げ表示";
        public string Caption { get; } = "読み上げるテキストをデスクトップ上に表示するプラグイン";
        public string Version { get; } = "1.0.2";

        public DisplayBouyomiWindow window;
        private DisplayBouyomiSettings _settings;
        private BouyomiSettingFormData _bouyomiSettingFormData;

        public void Begin()
        {
            Pub.FormMain.BC.TalkTaskStarted += new EventHandler<FNF.Utility.BouyomiChan.TalkTaskStartedEventArgs>(this.TalkTaskStarted);
            window = new DisplayBouyomiWindow();

            _settings = new DisplayBouyomiSettings(this);
            _bouyomiSettingFormData = new BouyomiSettingFormData(_settings);

            window.Show();
        }

        public void End()
        {
            Pub.FormMain.BC.TalkTaskStarted -= new EventHandler<FNF.Utility.BouyomiChan.TalkTaskStartedEventArgs>(this.TalkTaskStarted);
            window.Close();
        }

        public ISettingFormData SettingFormData
        {
            get
            {
                return _bouyomiSettingFormData;
            }
        }

        private void TalkTaskStarted(object sender, FNF.Utility.BouyomiChan.TalkTaskStartedEventArgs e)
        {
            string message = e.TalkTask.SourceText;
            window.Dispatcher.BeginInvoke(new Action(() => {
                window.ChangeMessage(message, _settings);
            }));
        }
    }
}
