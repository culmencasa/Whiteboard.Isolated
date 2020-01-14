using System.Windows;
using System.Windows.Controls;

namespace Whiteboard.Isolated.UserCanvas
{
    /// <summary>
    /// SelectionFrame.xaml 的交互逻辑
    /// </summary>
    public partial class ShapeSelectionFrame : UserControl
    {
        public ShapeSelectionFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选中的对象装饰器
        /// </summary>
        public ShapeAdorner Playground
        {
            get { return (ShapeAdorner)GetValue(PlaygroundProperty); }
            set { SetValue(PlaygroundProperty, value); }
        }

        public static readonly DependencyProperty PlaygroundProperty =
            DependencyProperty.Register("Playground",
                typeof(ShapeAdorner),
                typeof(ShapeSelectionFrame),
                new PropertyMetadata());

        private Point _center;
        public Point Center
        {
            get => _center;
            set => _center = value;
        }

        private void MoveThumb_UpdateCenterPoint_1(object sender, ShapeMoveThumb.CenterPointEventArgs e)
        {
            Center = e.Center;
        }
    }
}
