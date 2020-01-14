using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Whiteboard.Isolated.Interface;
using Whiteboard.Isolated.Model;

namespace Whiteboard.Isolated.WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            myCanvas.Shown += MyCanvas_Shown;
            this.Closing += MainWindow_Closing;


            _logger = new EZLogger(
                    @"Logs\WPF_" + DateTime.Now.ToString("yyyyMMdd") + ".log",
                    true,
                    (uint)EZLogger.Level.All);
            _logger.Start();

        }

        private void MyCanvas_Shown(object sender, IPagePersistence saver)
        {
            // 应用外部设定
            CanvasBoardSetting setting = App.Setting;
            if (setting.Pager?.PageIndex < 1)
            {
                saver?.LoadLastTimePage();
            }
            else {
                saver?.LoadPage(setting.Pager.PageIndex);
            }

            ICanvasBoard cb = sender as ICanvasBoard;
            cb.SetToolbarVisibility(setting.Toolbar.Visible);
            cb.SetPagerVisibility(setting.Pager.Visible);


            // 定时保存
            SetTimer();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (myCanvas.PageSaver != null)
            {
                myCanvas.PageSaver.SavePage();
                _logger.Info($"退出时保存画板.");
            }
            else
            {
                _logger.Info($"退出时未保存画板.");
            }

            if (aTimer != null)
            {
                aTimer.Stop();
                aTimer.Dispose();
                aTimer = null;
            }
        }

        private void SetTimer()
        {
            if (aTimer == null || !aTimer.Enabled)
            {
                aTimer = new System.Timers.Timer(3000);

                aTimer.Elapsed += (source, e) =>
                {
                    Dispatcher.BeginInvoke((Action)delegate
                    {
                        myCanvas.PageSaver?.SavePage();
                    });
                };
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
        }

        private EZLogger _logger;
        private System.Timers.Timer aTimer;
    }
}
