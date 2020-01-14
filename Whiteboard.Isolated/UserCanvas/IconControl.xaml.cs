using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace Whiteboard.Isolated.UserCanvas
{
    /// <summary>
    /// IconControl.xaml 的交互逻辑
    /// </summary>
    public partial class IconControl : UserControl
    {
        #region 构造

        public IconControl()
        {
            InitializeComponent();


            // 默认值 
            IconImage = DefaultIcon;
            CornerRadius = new CornerRadius(0);
            Stretch = Stretch.None;
            //UncheckedBackground = new SolidColorBrush(Color.FromRgb(232, 234, 236));
            //CheckedBackground = new SolidColorBrush(Color.FromRgb(68, 211, 182));


            UncheckedBackground = NoBackground;
            CheckedBackground = NoBackground;
            MouseHoverColor = NoBackground;

            UncheckedFontColor = Brushes.Black;
            CheckedFontColor = Brushes.White;

            IconButton.MouseEnter += IconButton_MouseEnter;
            IconButton.MouseLeave += IconButton_MouseLeave;

            // 初始化事件
            Loaded += IconControl_Loaded;
        }


        #endregion

        #region 属性

        /// <summary>
        /// 默认图标
        /// </summary>
        public static ImageSource DefaultIcon
        {
            get;
            set;
        }

        /// <summary>
        /// 空背景色
        /// </summary>
        public Brush NoBackground
        {
            get
            {
                return Brushes.Transparent;
            }
        }

        #endregion        

        #region 依赖属性

        /// <summary>
        /// 鼠标经过背景颜色
        /// </summary>
        public Brush MouseHoverColor
        {
            get { return (Brush)GetValue(MouseHoverColorProperty); }
            set { SetValue(MouseHoverColorProperty, value); }
        }

        /// <summary>
        /// 鼠标经过文字颜色
        /// </summary>
        public Brush MouseHoverFontColor
        {
            get { return (Brush)GetValue(MouseHoverFontColorProperty); }
            set { SetValue(MouseHoverFontColorProperty, value); }
        }


        /// <summary>
        /// 选中的文字颜色
        /// </summary>
        public Brush CheckedFontColor
        {
            get { return (Brush)GetValue(CheckedFontColorProperty); }
            set { SetValue(CheckedFontColorProperty, value); }
        }

        /// <summary>
        /// 未选中的文字颜色
        /// </summary>
        public Brush UncheckedFontColor
        {
            get { return (Brush)GetValue(UncheckedFontColorProperty); }
            set { SetValue(UncheckedFontColorProperty, value); }
        }

        /// <summary>
        /// 图片拉伸
        /// </summary>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        /// <summary>
        /// 点击委托
        /// </summary>
        public EventHandler Click
        {
            get { return (EventHandler)GetValue(ClickEventProperty); }
            set { SetValue(ClickEventProperty, value); }
        }

        /// <summary>
        /// 图标文字
        /// </summary>
        public string IconText
        {
            get
            {

                if (DesignerProperties.GetIsInDesignMode(this))
                {
                    return "图标名字";
                }

                return (string)GetValue(IconTextProperty);

            }
            set { SetValue(IconTextProperty, value); }
        }

        /// <summary>
        /// 图标图像
        /// </summary>
        public ImageSource IconImage
        {
            get { return (ImageSource)GetValue(IconImageProperty); }
            set
            {
                SetValue(IconImageProperty, value);
            }
        }

        /// <summary>
        /// 选中后显示的图像
        /// </summary>
        public ImageSource ActiveIconImage
        {
            get
            {
                return (ImageSource)GetValue(ActiveIconImageProperty);
            }
            set { SetValue(ActiveIconImageProperty, value); }
        }

        /// <summary>
        /// 点击命令
        /// </summary>
        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        /// <summary>
        /// 命令参数
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// 选中背景色
        /// </summary>
        public Brush CheckedBackground
        {
            get { return (Brush)GetValue(CheckedBackgroundProperty); }
            set { SetValue(CheckedBackgroundProperty, value); }
        }

        /// <summary>
        /// 非选中背景色
        /// </summary>
        public Brush UncheckedBackground
        {
            get { return (Brush)GetValue(UncheckedBackgroundProperty); }
            set { SetValue(UncheckedBackgroundProperty, value); }
        }

        /// <summary>
        /// 分组名
        /// </summary>
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        #endregion

        #region 注册依赖属性        

        public static readonly DependencyProperty MouseHoverFontColorProperty =
           DependencyProperty.Register("MouseHoverFontColor",
               typeof(Brush),
               typeof(IconControl),
               new PropertyMetadata());

        public static readonly DependencyProperty CheckedFontColorProperty =
           DependencyProperty.Register("CheckedFontColor",
               typeof(Brush),
               typeof(IconControl),
               new PropertyMetadata());

        public static readonly DependencyProperty UncheckedFontColorProperty =
           DependencyProperty.Register("UncheckedFontColor",
               typeof(Brush),
               typeof(IconControl),
               new PropertyMetadata());

        public static readonly DependencyProperty MouseHoverColorProperty =
           DependencyProperty.Register("MouseHoverColor",
               typeof(Brush),
               typeof(IconControl),
               new PropertyMetadata());


        public static readonly DependencyProperty NoBackgroundProperty =
           DependencyProperty.Register("NoBackground",
               typeof(bool),
               typeof(IconControl),
               new PropertyMetadata());

        public static readonly DependencyProperty IsCheckedProperty =
           DependencyProperty.Register("IsChecked",
               typeof(bool),
               typeof(IconControl),
               new PropertyMetadata());


        public static readonly DependencyProperty StretchProperty =
           DependencyProperty.Register("Stretch",
               typeof(Stretch),
               typeof(IconControl),
               new PropertyMetadata());


        public static readonly DependencyProperty CheckedBackgroundProperty =
           DependencyProperty.Register("CheckedBackground",
               typeof(Brush),
               typeof(IconControl),
               new PropertyMetadata());


        public static readonly DependencyProperty UncheckedBackgroundProperty =
           DependencyProperty.Register("UncheckedBackground",
               typeof(Brush),
               typeof(IconControl),
               new PropertyMetadata());


        public static readonly DependencyProperty CornerRadiusProperty =
           DependencyProperty.Register("CornerRadius",
               typeof(CornerRadius),
               typeof(IconControl),
               new PropertyMetadata());



        public static readonly DependencyProperty GroupNameProperty =
           DependencyProperty.Register("GroupName",
               typeof(string),
               typeof(IconControl),
               new PropertyMetadata());



        public static readonly DependencyProperty IconTextProperty =
           DependencyProperty.Register("IconText",
               typeof(string),
               typeof(IconControl),
               new PropertyMetadata());

        public static readonly DependencyProperty ActiveIconImageProperty =
           DependencyProperty.Register("ActiveIconImage",
               typeof(ImageSource),
               typeof(IconControl),
               new PropertyMetadata());


        public static readonly DependencyProperty IconImageProperty =
           DependencyProperty.Register("IconImage",
               typeof(ImageSource),
               typeof(IconControl),
               new PropertyMetadata());



        public static readonly DependencyProperty ClickCommandProperty =
           DependencyProperty.Register("ClickCommand",
               typeof(ICommand),
               typeof(IconControl),
               new UIPropertyMetadata(null));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(IconControl));

        public static readonly DependencyProperty ClickEventProperty =
           DependencyProperty.Register("Click",
               typeof(EventHandler),
               typeof(IconControl));

        #endregion

        #region 事件处理

        // 组件加载完成后
        private void IconControl_Loaded(object sender, RoutedEventArgs e)
        {
            //workaround
            UpdateCheckState();

        }
        
        // 点击事件
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        // 选中改变状态
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateCheckState();
        }

        // 取消改变状态
        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateCheckState();

        }

        // 鼠标悬浮
        private void IconButton_MouseEnter(object sender, MouseEventArgs e)
        {
            RadioButton button = sender as RadioButton; 
            Label buttonText = button.Template.FindName("innerText", button) as Label;
            Border buttonBorder = button.Template.FindName("backCover", button) as Border;
            
            if (MouseHoverColor != null && MouseHoverColor != NoBackground)
            {  
                buttonText.Foreground = MouseHoverFontColor != null ? MouseHoverFontColor : Brushes.White;
                buttonBorder.Background = MouseHoverColor;
            }
            else
            {
                if (button.IsChecked.HasValue && button.IsChecked.Value)
                {
                    buttonText.Foreground = CheckedFontColor;
                    buttonBorder.Background = CheckedBackground;
                }
                else
                {
                    buttonText.Foreground = UncheckedFontColor;
                    buttonBorder.Background = UncheckedBackground;
                }
            }
        }

        // 鼠标离开　
        private void IconButton_MouseLeave(object sender, MouseEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            Label buttonText = button.Template.FindName("innerText", button) as Label;
            Border buttonBorder = button.Template.FindName("backCover", button) as Border; 
            if (button.IsChecked.HasValue && button.IsChecked.Value)
            {
                buttonText.Foreground = CheckedFontColor;
                buttonBorder.Background = CheckedBackground;
            }
            else
            {
                buttonText.Foreground = UncheckedFontColor;
                buttonBorder.Background = UncheckedBackground;
            }
        }

        #endregion

        #region 私有方法

        private void UpdateCheckState()
        {
            if (!this.IsLoaded)
                return;

            RadioButton button = IconButton;
            ControlTemplate machineLabelTemplate = (ControlTemplate)this.FindResource("IconButtonTemplate");
            button.Template = machineLabelTemplate;
            button.ApplyTemplate(); 
            //if (button.ApplyTemplate())
            {
                if (button.IsChecked.HasValue && button.IsChecked.Value)
                {
                    //var root = button.Template.LoadContent() as FrameworkElement;
                    Image buttonIcon = button.Template.FindName("innerIcon", button) as Image;
                    Label buttonText = button.Template.FindName("innerText", button) as Label;
                    Border buttonBorder = button.Template.FindName("backCover", button) as Border;
                    
                    if (ActiveIconImage != null)
                    {
                        buttonIcon.Source = ActiveIconImage;
                    }

                    if (CheckedBackground != null && CheckedBackground != NoBackground)
                    {
                        buttonBorder.Background = CheckedBackground;
                    }

                    if (CheckedFontColor != null)
                    {
                        buttonText.Foreground = CheckedFontColor;
                    }

                }
                else
                {
                    Image buttonIcon = button.Template.FindName("innerIcon", button) as Image;
                    Label buttonText = button.Template.FindName("innerText", button) as Label;
                    Border buttonBorder = button.Template.FindName("backCover", button) as Border;

                    buttonIcon.Source = IconImage;

                    if (UncheckedBackground != null && UncheckedBackground != NoBackground)
                    {
                        buttonText.Foreground = Brushes.Black;
                        buttonBorder.Background = UncheckedBackground;
                    }

                    if (UncheckedFontColor != null)
                    {
                        buttonText.Foreground = UncheckedFontColor;
                    } 
                }
            }
        }



        #endregion
    }
}
