using System;
using System.Collections.Generic;
using System.Globalization;
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
using Whiteboard.Isolated.Ink;

namespace Whiteboard.Isolated.UserCanvas
{
    /// <summary>
    /// MagnifyGlass.xaml 的交互逻辑
    /// </summary>
    public partial class MagnifyGlass : UserControl
    {
        public MagnifyGlass()
        {
            InitializeComponent();


        }

        public Rect ViewPort { get
            {

                return vb.Viewport;
            }
            set
            {
                vb.Viewport = value;
            }
        }


        public void SetPosition(Point pos)
        {
            var left = Canvas.GetLeft(magnifierCanvas) + pos.X;
            var top = Canvas.GetTop(magnifierCanvas) + pos.Y;
            Canvas.SetLeft(magnifierCanvas, left);
            Canvas.SetTop(magnifierCanvas, top);

            SetViewbox(new Point(
                        left + magnifierCanvas.Width / 2 - pos.X,
                        top + magnifierCanvas.Height / 2 - pos.Y));
        }

        public void SetViewbox(Point pos)
        {
            var parent = this.Parent as Canvas;
            var rootGrid = parent.Parent as Grid;

            var target = this.VisualObj as SmashInkCanvas;

            // 换算相对位置(不包含margin)
            pos = rootGrid.TranslatePoint(pos, target);

            Rect viewBox = vb.Viewbox;             
            viewBox.X = pos.X;
            viewBox.Y = pos.Y;
            vb.Viewbox = viewBox;
        }


        public Visual VisualObj
        {
            get { return (Visual)GetValue(VisualObjProperty); }
            set { SetValue(VisualObjProperty, value); }
        }

        public static readonly DependencyProperty VisualObjProperty =
            DependencyProperty.Register("VisualObj",
                typeof(Visual),
                typeof(MagnifyGlass),
                new PropertyMetadata());
    }
}
