using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Whiteboard.Isolated.UserCanvas
{
    public class MagnifyMoveThumb : Thumb
    {
        MagnifyGlass _innerControl;
        MagnifyGlassAdorner _adorner;
        Point _lastDragPoint = new Point(0, 0);

        public MagnifyMoveThumb()
        {
            Loaded += MagnifyMoveThumb_Loaded;
            DragStarted += ShapeMoveThumb_DragStarted;
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta); 
        }

        private void MagnifyMoveThumb_Loaded(object sender, RoutedEventArgs e)
        {
            _adorner = this.DataContext as MagnifyGlassAdorner;
            _innerControl = _adorner.AdornedElement as MagnifyGlass;
        }

        private void ShapeMoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            _lastDragPoint = Mouse.GetPosition(null);
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_innerControl != null)
            {
                //Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);


                //var tgShape = _innerControl.GetTransformGroup();
                //var tgAdorner = _adorner.GetTransformGroup();


                Point currentPoint = Mouse.GetPosition(null);
                Point dragDelta = new Point(currentPoint.X - _lastDragPoint.X, currentPoint.Y - _lastDragPoint.Y);
                _lastDragPoint = currentPoint;

                if (_innerControl.Parent as Canvas != null)
                {
                    var left = Canvas.GetLeft(_innerControl) + dragDelta.X;
                    var top = Canvas.GetTop(_innerControl) + dragDelta.Y;
                    Canvas.SetLeft(_innerControl, left);
                    Canvas.SetTop(_innerControl, top);

                    //_innerControl.SetPosition(dragDelta);

                    // 以中心点为基准
                    _innerControl.SetViewbox(new Point(
                        left + _innerControl.Width / 2 - 25 + dragDelta.X,
                        top + _innerControl.Height / 2 - 25 + dragDelta.Y));
                }
            }
        }

    }
}
