using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Whiteboard.Isolated.Shapes;

namespace Whiteboard.Isolated.UserCanvas
{

    public class ShapeRotateThumb : Thumb
    {
        private Point _centerPoint;
        private ContentControl _innerControl;
        private WiredShape _shape;
        private ShapeAdorner _adorner;
        
        public ShapeRotateThumb()
        {
            Loaded += RotateThumb_Loaded;

            DragDelta += new DragDeltaEventHandler(this.RotateThumb_DragDelta);
            DragStarted += new DragStartedEventHandler(this.RotateThumb_DragStarted);
        }

        private void RotateThumb_Loaded(object sender, RoutedEventArgs e)
        {
            _adorner = DataContext as ShapeAdorner;
            _innerControl = _adorner.AdornedElement as ContentControl;
            _shape = _innerControl.Content as WiredShape;
        }

        private void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (this._shape != null)
            {
                //this._innerControl = VisualTreeHelper.GetParent(this._shape) as FrameworkElement;

                if (this._innerControl != null)
                {
                    var width = _innerControl.Width;
                    var height = _innerControl.Height;
                    if (double.IsNaN(width))
                    {
                        width = _innerControl.ActualWidth;
                    }
                    if (double.IsNaN(height))
                    {
                        height = _innerControl.ActualHeight;
                    }

                    this._centerPoint = this._shape.TranslatePoint(new Point(width * 0.5, height * 0.5), this._innerControl);

                    var c = _adorner.Frame.Center;

                    //Console.WriteLine(_centerPoint);
                    //Console.WriteLine(c);
                    //Console.WriteLine("----");
                }
            }
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this._shape != null && this._innerControl != null)
            {
                //Point pos = Mouse.GetPosition(this._innerControl);

                //double deltaX = pos.X - _centerPoint.X;
                //double deltaY = pos.Y - _centerPoint.Y;

                //if (deltaY.Equals(0))
                //{

                //    return;
                //}

                //double tan = deltaX / deltaY;
                //double angle = Math.Atan(tan);


                //angle = angle * 180 / Math.PI;

                //// 如果鼠标过垂直中线, 找到余角
                //if (deltaY > 0)
                //{
                //    angle = 180 - Math.Abs(angle);
                //}

                //if (deltaX < 0)
                //{
                //    angle = -Math.Abs(angle);
                //}
                //else
                //{
                //    angle = Math.Abs(angle);
                //}

                //if (double.IsNaN(angle))
                //{
                //    return;
                //}

                var angle = GetAngle(_centerPoint); 
                
                RotateTransform rotateTransform = new RotateTransform(angle, this._centerPoint.X, _centerPoint.Y);

                //var tgShape = _adorner.GetTransformGroup(_shape);
                //var tgAdorner = _adorner.GetTransformGroup(_adorner);
                //tgShape.Children.Add(rotateTransform);
                //tgAdorner.Children.Add(rotateTransform);

                _shape.RenderTransform = rotateTransform;
                _adorner.Frame.RenderTransform = rotateTransform;


                _lastAngle = angle;
            }
        }

        private double GetAngle(Point lastCenterPoint)
        {
            Point pos = Mouse.GetPosition(_innerControl);

            double deltaX = pos.X - lastCenterPoint.X;
            double deltaY = pos.Y - lastCenterPoint.Y;

            if (deltaY.Equals(0))
            {
                return 0;
            }

            double tan = deltaX / deltaY;
            double angle = Math.Atan(tan);

            angle = angle * 180 / Math.PI;

            if (deltaY > 0)
            {
                angle = 180 - Math.Abs(angle);
            }

            if (deltaX < 0)
            {
                angle = -Math.Abs(angle);
            }
            else
            {
                angle = Math.Abs(angle);
            }

            if (double.IsNaN(angle))
            {
                return 0;
            }

            return angle;
        }


        /// <summary>
        /// 上次旋转的角度
        /// </summary>
        double _lastAngle;
    }
}
