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

namespace Whiteboard.Isolated.UserCanvas
{
    /// <summary>
    /// MagnifyGlassFrame.xaml 的交互逻辑
    /// </summary>
    public partial class MagnifyGlassFrame : UserControl
    {
        public MagnifyGlassFrame()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 选中的对象装饰器
        /// </summary>
        public MagnifyGlassAdorner Playground
        {
            get { return (MagnifyGlassAdorner)GetValue(PlaygroundProperty); }
            set { SetValue(PlaygroundProperty, value); }
        }

        public static readonly DependencyProperty PlaygroundProperty =
            DependencyProperty.Register("Playground",
                typeof(MagnifyGlassAdorner),
                typeof(MagnifyGlassFrame),
                new PropertyMetadata());



        private Point _center;
        public Point Center
        {
            get => _center;
            set => _center = value;
        }

    }
}
