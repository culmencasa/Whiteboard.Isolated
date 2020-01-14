using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace Whiteboard.Isolated.UserCanvas
{
    public class ShapeMoveThumb : Thumb
    {
        ContentControl _innerControl;
        ShapeAdorner _adorner;
        Point _lastDragPoint = new Point(0, 0);


        public ShapeMoveThumb()
        {
            Loaded += ShapeMoveThumb_Loaded;
            DragStarted += ShapeMoveThumb_DragStarted;
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
            DragCompleted += MoveThumb_DragCompleted;
        }

        private void ShapeMoveThumb_Loaded(object sender, RoutedEventArgs e)
        {
            _adorner = this.DataContext as ShapeAdorner;
            _innerControl = _adorner.AdornedElement as ContentControl;
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


                var tgShape = _innerControl.GetTransformGroup();
                var tgAdorner = _adorner.GetTransformGroup();


                Point currentPoint = Mouse.GetPosition(null);
                Point dragDelta = new Point(currentPoint.X - _lastDragPoint.X, currentPoint.Y - _lastDragPoint.Y);
                _lastDragPoint = currentPoint;

                var myCanvas = _innerControl.Parent as InkCanvas;
                
                //double nx = Math.Min(Math.Max(InkCanvas.GetLeft(_innerControl) + dragDelta.X, 0), myCanvas.Width - _innerControl.Width);
                //double ny = Math.Min(Math.Max(InkCanvas.GetTop(_innerControl) + dragDelta.Y, 0), myCanvas.Height - _innerControl.Height);

                //_lastDragPoint = new Point(nx, ny);



                tgShape.Children.Add(new TranslateTransform(dragDelta.X, dragDelta.Y));
                tgAdorner.Children.Add(new TranslateTransform(dragDelta.X, dragDelta.Y));

                //if (_innerControl.Parent as Canvas != null)
                //{
                //    left = Canvas.GetLeft(_innerControl) + dragDelta.X;
                //    top = Canvas.GetTop(_innerControl) + dragDelta.Y;
                //    Canvas.SetLeft(_innerControl, left);
                //    Canvas.SetTop(_innerControl, top);
                //}
                //else if (_innerControl.Parent as InkCanvas != null)
                //{
                //    left = InkCanvas.GetLeft(_innerControl) + dragDelta.X;
                //    top = InkCanvas.GetTop(_innerControl) + dragDelta.Y;
                //    InkCanvas.SetLeft(_innerControl, left);
                //    InkCanvas.SetTop(_innerControl, top);

                //}
                //else
                //{
                //    left = _innerControl.Margin.Left + dragDelta.X;
                //    top = _innerControl.Margin.Top + dragDelta.Y;
                //    _innerControl.Margin = new Thickness(
                //        left,
                //        top,
                //        _innerControl.Margin.Right,
                //        _innerControl.Margin.Bottom
                //    );
                //}
            }
        }

        private void MoveThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var parent = _innerControl.Parent as UIElement;
            var location = _innerControl.TranslatePoint(new Point(0, 0), parent);
                       

            var _centerOfStrokes = new Point(location.X + _innerControl.Width / 2,
                               location.Y + _innerControl.Height / 2);

            //var _centerOfStrokes2 = new Point(left + _innerControl.Width / 2,
            //                   top + _innerControl.Height / 2);

            CenterPointEventArgs args = new CenterPointEventArgs(UpdateCenterPointEvent, this);
            args.Center = _centerOfStrokes;
            RaiseEvent(args);

        }
        
        //double left = 0;
        //double top = 0;


        #region 路由事件

        public static readonly RoutedEvent UpdateCenterPointEvent = EventManager.RegisterRoutedEvent(
            "UpdateCenterPoint", 
            RoutingStrategy.Direct, 
            typeof(EventHandler<CenterPointEventArgs>), 
            typeof(ShapeMoveThumb));

        public event EventHandler<CenterPointEventArgs> UpdateCenterPoint
        {
            add { AddHandler(UpdateCenterPointEvent, value); }
            remove { RemoveHandler(UpdateCenterPointEvent, value); }
        }

        public class CenterPointEventArgs : RoutedEventArgs
        {
            public Point Center
            {
                get;
                set;
            }

            public CenterPointEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
            { 
            }
        }

        #endregion

    }
}
