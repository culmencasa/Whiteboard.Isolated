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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Whiteboard.Isolated
{
    /// <summary>
    /// Timers.xaml 的交互逻辑
    /// </summary>
    public partial class TimersWindow : Window
    {
        public TimersWindow()
        {
            InitializeComponent();
           
        }

        DispatcherTimer coundownTimer;
        DispatcherTimer stopwatchTimer;

        private void RbtnCountdown_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                CheckBbtnCountdownImpl();
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(this.CheckBbtnCountdownImpl), DispatcherPriority.Loaded);
            }
        }

        private void CheckBbtnCountdownImpl()
        {
            this.GridStopwatch.Visibility = Visibility.Collapsed;
            this.GridCountdown.Visibility = Visibility.Visible;
            this.HasAnyStart = this.isCountdownStarted;
        }

        private void RbtnStopwatch_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                CheckStopwatchImpl();
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(this.CheckStopwatchImpl), DispatcherPriority.Loaded);
            }

        }

        private void CheckStopwatchImpl()
        {
            this.GridStopwatch.Visibility = Visibility.Visible;
            this.GridCountdown.Visibility = Visibility.Collapsed;
            this.HasAnyStart = this.isStopwatchStarted;
        }

        private bool isCountdownStarted;
        private bool isStopwatchStarted;

        private bool HasAnyStart
        {
            get { return this.isCountdownStarted || this.isStopwatchStarted; }
            set
            {
                if (!value)
                {
                    this.BtnPause.Visibility = Visibility.Collapsed;
                    this.BtnStart.Visibility = Visibility.Visible;
                }
                else
                {
                    this.BtnPause.Visibility = Visibility.Visible;
                    this.BtnStart.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void BtnStart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.RbtnCountdown.IsChecked == true)
            {
                StartNewCountDownTimer();
            }
            else if (this.RbtnStopwatch.IsChecked == true)
            {
                StartNewStopwatchTimer();
            }
        }

        private void StartNewStopwatchTimer()
        {
            this.isStopwatchStarted = true;
            this.stopwatchTimer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Send, OnStopwatchTick, this.Dispatcher);
            this.stopwatchTimer.Start();
            this.HasAnyStart = true;
        }

        private TimeSpan stopwatchTimeSpan;

        internal TimeSpan StopwatchTimeSpan
        {
            get { return this.stopwatchTimeSpan; }
            set
            {
                this.stopwatchTimeSpan = value;
                string timeString = this.stopwatchTimeSpan.ToString("hhmmss");
                this.LblHours.Content = timeString.Substring(0, 2);
                this.LblMinutes.Content = timeString.Substring(2, 2);
                this.LblSeconds.Content = timeString.Substring(4, 2);
            }
        }

        private TimeSpan countdownTimeSpan;

        internal TimeSpan CountdownTimeSpan
        {
            get { return this.countdownTimeSpan; }
            set
            {
                this.countdownTimeSpan = value;
                string timeString = this.countdownTimeSpan.ToString("mmss");
                this.NumberMinute0.Value = int.Parse(timeString.Substring(0, 1));
                this.NumberMinute1.Value = int.Parse(timeString.Substring(1, 1));
                this.NumberSecond0.Value = int.Parse(timeString.Substring(2, 1));
                this.NumberSecond1.Value = int.Parse(timeString.Substring(3, 1));
            }
        }

        private void OnStopwatchTick(object sender, EventArgs e)
        {
            this.StopwatchTimeSpan = this.stopwatchTimeSpan + TimeSpan.FromSeconds(1);

        }

        private void StartNewCountDownTimer()
        {
            if (this.coundownTimer != null && this.coundownTimer.IsEnabled)
            {
                this.coundownTimer.Stop();
            }
            int seconds = CalcSeconds();
            if (seconds == 0)
            {
                return;
            }
            this.isCountdownStarted = true;
            this.CountdownTimeSpan = TimeSpan.FromSeconds(seconds);
            this.coundownTimer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Send, OnCountdownTick, this.Dispatcher);
            this.coundownTimer.Start();
            this.HasAnyStart = true;
        }

        private int CalcSeconds()
        {
            int minute0 = (int)this.NumberMinute0.Value;
            int minute1 = (int)this.NumberMinute1.Value;
            int second0 = (int)this.NumberSecond0.Value;
            int second1 = (int)this.NumberSecond1.Value;
            int seconds = (minute0 * 10 + minute1) * 60 + second0 * 10 + second1;
            return seconds;
        }


        private void OnCountdownTick(object sender, EventArgs e)
        {
            var newTimeSpan = this.CountdownTimeSpan - TimeSpan.FromSeconds(1);
            if (newTimeSpan >= TimeSpan.Zero)
            {
                this.CountdownTimeSpan = newTimeSpan;
            }
            else
            {
                this.StopCountdownTimer();
            }
        }

        private void BtnPause_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.RbtnCountdown.IsChecked == true)
            {
                StopCountdownTimer();
            }
            else if (this.RbtnStopwatch.IsChecked == true)
            {
                StopStopwatchTimer();
            }
        }

        private void StopStopwatchTimer()
        {
            this.isStopwatchStarted = false;
            if (this.stopwatchTimer != null)
            {
                this.stopwatchTimer.Stop();
            }
            this.HasAnyStart = false;
        }

        private void StopCountdownTimer()
        {
            this.isCountdownStarted = false;
            if (this.coundownTimer != null)
            {
                this.coundownTimer.Stop();
            }
            this.HasAnyStart = false;
        }

        private void BtnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point pt = e.GetPosition(this.Titlebar);
            if (e.LeftButton == MouseButtonState.Pressed && this.Titlebar.InputHitTest(pt) != null)
            {
                this.DragMove();
            }
            base.OnMouseMove(e);
        }

        private void GridReset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.RbtnCountdown.IsChecked == true)
            {
                this.ResetCountdown();
            }
            else if (this.RbtnStopwatch.IsChecked == true)
            {
                this.ResetStopwatch();
            }
        }

        private void ResetStopwatch()
        {
            if (this.stopwatchTimer != null)
            {
                if (this.stopwatchTimer.IsEnabled)
                {
                    this.stopwatchTimer.Stop();
                }
            }
            this.StopwatchTimeSpan = TimeSpan.Zero;
            this.isStopwatchStarted = false;
            this.HasAnyStart = false;
        }

        private void ResetCountdown()
        {
            if (this.coundownTimer != null)
            {
                if (this.coundownTimer.IsEnabled)
                {
                    this.coundownTimer.Stop();
                }
            }
            this.CountdownTimeSpan = TimeSpan.Zero;
            this.isCountdownStarted = false;
            this.HasAnyStart = false;
        }

        private void NumberMinute0_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == 6)
            {
                this.NumberMinute1.Value = 0;
                this.NumberSecond0.Value = 0;
                this.NumberSecond1.Value = 0;   
            }
        }

        private void NumberMinute1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.NumberMinute0.Value == 6 && this.CalcSeconds() > 3600)
            {
                this.NumberMinute0.Value = 6;
                this.NumberMinute1.Value = 0;
                this.NumberSecond1.Value = 0;
                this.NumberSecond0.Value = 0;
            }
        }

        private void NumberSecond0_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.NumberMinute0.Value == 6 && this.CalcSeconds() > 3600)
            {
                this.NumberMinute0.Value = 6;
                this.NumberMinute1.Value = 0;
                this.NumberSecond1.Value = 0;
                this.NumberSecond0.Value = 0;
            }
        }

        private void NumberSecond1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.NumberMinute0.Value == 6 && this.CalcSeconds() > 3600)
            {
                this.NumberMinute0.Value = 6;
                this.NumberMinute1.Value = 0;
                this.NumberSecond1.Value = 0;
                this.NumberSecond0.Value = 0;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
