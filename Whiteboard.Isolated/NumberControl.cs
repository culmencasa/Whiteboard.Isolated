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
using System.Windows.Controls.Primitives;

namespace Whiteboard.Isolated
{
    [TemplatePart(Name = PART_DecreaseButton,Type=typeof(RepeatButton))]
    [TemplatePart(Name = PART_IncreaseButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_NumberLabel, Type = typeof(Label))]
    public class NumberControl : System.Windows.Controls.Primitives.RangeBase
    {
        public NumberControl()
        {
            this.Maximum = 9;
            this.Minimum = 0;
        }

        static NumberControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberControl), new FrameworkPropertyMetadata(typeof(NumberControl)));
            CommandManager.RegisterClassCommandBinding(typeof(NumberControl), new CommandBinding(Slider.IncreaseSmall, OnIncreaseSmallCommand));
            CommandManager.RegisterClassCommandBinding(typeof(NumberControl), new CommandBinding(Slider.DecreaseSmall, OnDecreaseSmallCommand));
        }

        internal const string PART_IncreaseButton = "PART_IncreaseButton";
        internal const string PART_DecreaseButton = "PART_DecreaseButton";
        internal const string PART_NumberLabel = "PART_NumberLabel";


        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        RepeatButton increaseButton;
        RepeatButton decreaseButton;
        Label label;


        public override void OnApplyTemplate()
        {
            this.increaseButton = this.Template?.FindName(PART_IncreaseButton, this) as RepeatButton;
            this.decreaseButton = this.Template?.FindName(PART_DecreaseButton, this) as RepeatButton;
            this.label = this.Template?.FindName(PART_NumberLabel, this) as Label;
            base.OnApplyTemplate();
        }

        // Using a DependencyProperty as the backing store for IsEditable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool), typeof(NumberControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsEditablePropertyChanged));

        private static void OnIsEditablePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumberControl).handleIsEditablePropertyImpl((bool)e.NewValue);
        }

        private void handleIsEditablePropertyImpl(bool newValue)
        {
            if (this.increaseButton != null)
            {
                this.increaseButton.IsEnabled = newValue;
            }
            if (this.decreaseButton != null)
            {
                this.decreaseButton.IsEnabled = newValue;
            }

        }

        private static void OnDecreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is NumberControl numberControl)
            {
                numberControl.handleDecreaseSmallCommandImpl();
            }
        }

        private void handleDecreaseSmallCommandImpl()
        {
            this.Value -= 1;
        }

        private static void OnIncreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is NumberControl numberControl)
            {
                numberControl.handleIncreaseSmallCommandImpl();
            }
        }

        private void handleIncreaseSmallCommandImpl()
        {
            this.Value += 1;
        }


    }
}
