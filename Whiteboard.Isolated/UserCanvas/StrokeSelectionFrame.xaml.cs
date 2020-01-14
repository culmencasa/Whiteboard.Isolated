using System;
using System.Windows;
using System.Windows.Controls;

namespace Whiteboard.Isolated.UserCanvas
{
    /// <summary>
    /// SelectionFrame.xaml 的交互逻辑
    /// </summary>
    public partial class StrokeSelectionFrame : UserControl
    {
        public StrokeSelectionFrame()
        {
            InitializeComponent();
        }
        

        static Type DependencyType = typeof(StrokeSelectionFrame);

        /// <summary>
        /// 选中的对象
        /// </summary>
        public object PlayObject
        {
            get { return GetValue(PlayObjectProperty); }
            set { SetValue(PlayObjectProperty, value); }
        }

        /// <summary>
        /// 选中的对象装饰器
        /// </summary>
        public object Playground
        {
            get { return GetValue(PlaygroundProperty); }
            set { SetValue(PlaygroundProperty, value); }
        }

        public Point Center
        {
            get;
            set;
        }

        public static readonly DependencyProperty PlaygroundProperty =
            DependencyProperty.Register("Playground",
                typeof(object),
                DependencyType,
                new PropertyMetadata());

        public static readonly DependencyProperty PlayObjectProperty =
            DependencyProperty.Register("PlayObject",
                typeof(object),
                DependencyType,
                new PropertyMetadata());


    }
}
