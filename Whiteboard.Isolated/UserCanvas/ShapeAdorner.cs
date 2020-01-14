using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Whiteboard.Isolated.Shapes;

namespace Whiteboard.Isolated.UserCanvas
{

    public class ShapeAdorner : Adorner
    {
        public ShapeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            //this.IsHitTestVisible = false;

            SnapsToDevicePixels = true;
             
            _frame = new ShapeSelectionFrame();
            _frame.Playground = this;           

            _toolbar = new AdornerToolbar();
            _toolbar.HorizontalAlignment = HorizontalAlignment.Center;
            _toolbar.VerticalAlignment = VerticalAlignment.Bottom;
            _toolbar.PutOnTopHandler += _toolbar_PutOnTopHandler;
            _toolbar.CopyHandler += _toolbar_CopyHandler;
            _toolbar.DeleteHandler += _toolbar_DeleteHandler;


            //strokeBounds = new Rect(mainControl.Width / 2 - toolbar.Width / 2, mainControl.ActualHeight + 20, toolbar.Width, toolbar.Height);
            this._visualChildren = new VisualCollection(this);
            this._visualChildren.Add(_frame);
            this._visualChildren.Add(_toolbar);
        }

        #region 工具栏事件处理

        private void _toolbar_DeleteHandler()
        {
            var adornerParent = this.Parent as AdornerLayer;


            var mainControl = AdornedElement as ContentControl; 
            (mainControl.Parent as InkCanvas).Children.Remove(mainControl);

            adornerParent.Remove(this);
        }

        private void _toolbar_CopyHandler()
        {
            var adornerParent = this.Parent as AdornerLayer;
            var sourceControl = AdornedElement as ContentControl;

            //  创建新形状
            var newControl = new ContentControl();
            // 不设置大小会造成无法选中
            newControl.Width = sourceControl.Width;
            newControl.Height = sourceControl.Height;

            newControl.Content = (sourceControl.Content as WiredShape).Clone();
            // 添加到InkCanvas
            (sourceControl.Parent as InkCanvas).Children.Add(newControl);
            // 设置偏移

            newControl.RenderTransform = sourceControl.RenderTransform.Clone();

            //double sourceLeft = InkCanvas.GetLeft(sourceControl); 
            //double sourceTop = InkCanvas.GetTop(sourceControl);
            //InkCanvas.SetLeft(newControl, sourceLeft - 50);
            //InkCanvas.SetTop(newControl, sourceTop - 50);
            // 添加Adorner
            ShapeAdorner adorner = new ShapeAdorner(newControl);
            adornerParent.Add(adorner);

            // 删除当前的Adorner
            adornerParent.Remove(this); 
        }


        private void _toolbar_PutOnTopHandler()
        {
            var adornerParent = this.Parent as AdornerLayer;

            // 重新添加
            var sourceControl = AdornedElement as ContentControl;
            var inkCanvas = sourceControl.Parent as InkCanvas;
            inkCanvas.Children.Remove(sourceControl);
            inkCanvas.Children.Add(sourceControl);

            // 删除Adorner
            adornerParent.Remove(this);
        }

        #endregion
        
        #region 重写的成员

        protected override int VisualChildrenCount
        {
            get
            {
                if (_visualChildren == null)
                    return 0;
               

                return this._visualChildren.Count;
            }
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            this._frame.Arrange(new Rect(arrangeBounds));
            _toolbar.Arrange(new Rect(
                            (_frame.ActualWidth - _toolbar.Width) / 2,
                            arrangeBounds.Height + 20,
                            300,
                            80));

            var innerControl = AdornedElement as ContentControl;
            _frame.Center = new Point(innerControl.Width / 2, innerControl.Height / 2);
                 
            return arrangeBounds;
        }

        protected override Visual GetVisualChild(int index)
        {
            return this._visualChildren[index];
        }

        #endregion
        

        public ShapeSelectionFrame Frame { get => _frame; set => _frame = value; }


        #region 字段

        VisualCollection _visualChildren;
        ShapeSelectionFrame _frame;
        AdornerToolbar _toolbar;


        #endregion
    }
}
