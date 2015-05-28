using System;
using System.Windows;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;

namespace Plugin_Display_Bouyomi
{
    public partial class DisplayBouyomiWindow : Window
    {
        private Timer hideMessageTimer;
        private Timer showMessageTimer;
        private static double MARGIN = 10f;
        private static double SHOW_INTERVAL = 300f;

        public DisplayBouyomiWindow()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
        }

        /// <summary>
        /// 表示するメッセージを変更する
        /// </summary>
        /// <param name="message"></param>
        /// <param name="setting"></param>
        public void ChangeMessage(string message, DisplayBouyomiSettings setting)
        {
            BouyomiDataContext dataContext = new BouyomiDataContext();
            dataContext.BouyomiMessage = message;
            dataContext.Setting = setting;
            // 前の状態のwindow位置をを引き継ぐ
            if (BouyomiDataContext.IsValidInstance(this.DataContext))
            {
                dataContext.WindowTop = ((BouyomiDataContext)this.DataContext).WindowTop;
                dataContext.WindowLeft = ((BouyomiDataContext)this.DataContext).WindowLeft;
            }
            this.DataContext = dataContext;
            SetHideMessageTimer(setting.Timeout);
        }

        /// <summary>
        /// 一度メッセージを適用してwindowサイズを変えてからメッセージの表示位置を調整する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayBouyomiWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!BouyomiDataContext.IsValidInstance(this.DataContext)) { return; }

            BouyomiDataContext dataContext = new BouyomiDataContext();
            dataContext.BouyomiMessage = ((BouyomiDataContext)this.DataContext).BouyomiMessage;
            dataContext.Setting = ((BouyomiDataContext)this.DataContext).Setting;
            dataContext.WindowTop = CalculateTop();
            dataContext.WindowLeft = CalculateLeft();
            this.DataContext = dataContext;
        }
        
        private double CalculateTop()
        {
            double position = 0;
            double height = SystemParameters.PrimaryScreenHeight;

            switch (((BouyomiDataContext)this.DataContext).Setting.position)
            {
                case DisplayBouyomiSettings.Position.左上:
                case DisplayBouyomiSettings.Position.右上:
                    // default
                    break;
                case DisplayBouyomiSettings.Position.左下:
                case DisplayBouyomiSettings.Position.右下:
                    position = height - this.ActualHeight;
                    break;
                case DisplayBouyomiSettings.Position.中央:
                    position = height/2 - this.ActualHeight/2;
                    break;
            }
            return position;
        }

        private double CalculateLeft()
        {
            double position = MARGIN;
            double width = SystemParameters.PrimaryScreenWidth;

            switch (((BouyomiDataContext)this.DataContext).Setting.position)
            {
                case DisplayBouyomiSettings.Position.左上:
                case DisplayBouyomiSettings.Position.左下:
                    // default
                    break;
                case DisplayBouyomiSettings.Position.右上:
                case DisplayBouyomiSettings.Position.右下:
                    position = width - this.ActualWidth - MARGIN;
                    break;
                case DisplayBouyomiSettings.Position.中央:
                    position = width/2 - this.ActualWidth/2;
                    break;
            }
            return position;
        }

        private void SetHideMessageTimer(double interval)
        {
            if(hideMessageTimer != null && hideMessageTimer.Enabled)
            {
                hideMessageTimer.Stop();
            }
            hideMessageTimer = new Timer();
            hideMessageTimer.Elapsed += HideMessage;
            hideMessageTimer.Interval = interval;
            hideMessageTimer.Start();
        } 

        private void HideMessage(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                this.DataContext = new
                {
                    BouyomiSettingFormData = ""
                };
                this.hideMessageTimer.Stop();
            }));
        }

        private void TextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock block = (TextBlock)FindName("MessageText");
            Brush beforeTextColor = block.Foreground;

            BouyomiDataContext dataContext = new BouyomiDataContext();
            if (BouyomiDataContext.IsValidInstance(this.DataContext))
            {
                dataContext.BouyomiMessage = ((BouyomiDataContext)this.DataContext).BouyomiMessage;
                dataContext.Setting = ((BouyomiDataContext)this.DataContext).Setting;
                dataContext.WindowTop = ((BouyomiDataContext)this.DataContext).WindowTop;
                dataContext.WindowLeft = ((BouyomiDataContext)this.DataContext).WindowLeft;
                dataContext.Setting.FontColor = "Transparent";
                dataContext.Setting.BlurColor = "Transparent";
            }
            this.DataContext = dataContext;

            if (showMessageTimer != null && showMessageTimer.Enabled)
            {
                return;
            }
            showMessageTimer = new Timer();
            showMessageTimer.Elapsed += (sdr, args) => VisibleMessage(sdr, beforeTextColor);
            showMessageTimer.Interval = SHOW_INTERVAL;
            showMessageTimer.Start();
        }

        private void VisibleMessage(object sender, Brush beforeColor)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                BouyomiDataContext dataContext = new BouyomiDataContext();
                if (BouyomiDataContext.IsValidInstance(this.DataContext))
                {
                    dataContext.BouyomiMessage = ((BouyomiDataContext)this.DataContext).BouyomiMessage;
                    dataContext.Setting = ((BouyomiDataContext)this.DataContext).Setting;
                    dataContext.WindowTop = ((BouyomiDataContext)this.DataContext).WindowTop;
                    dataContext.WindowLeft = ((BouyomiDataContext)this.DataContext).WindowLeft;
                    dataContext.Setting.FontColor = beforeColor.ToString();
                    dataContext.Setting.BlurColor = null;
                }
                this.DataContext = dataContext;
                this.showMessageTimer.Stop();
            }));
        }
    }
}
