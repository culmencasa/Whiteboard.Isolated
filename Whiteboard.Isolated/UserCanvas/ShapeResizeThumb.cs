using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Whiteboard.Isolated.Ink;

namespace Whiteboard.Isolated.UserCanvas
{

    public class ShapeResizeThumb : Thumb
    {
        private Point _transformOrigin;
        private ShapeAdorner _adorner;
        private ContentControl _innerControl;

        public ShapeResizeThumb()
        {
            Loaded += ShapeResizeThumb_Loaded;
            DragStarted += new DragStartedEventHandler(this.ResizeThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        private void ShapeResizeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            _adorner = this.DataContext as ShapeAdorner;
            _innerControl = _adorner.AdornedElement as ContentControl;
            _innerControl.MinHeight = 50;
            _innerControl.MinWidth = 50;
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (this._innerControl != null)
            {
                _transformOrigin = _adorner.Frame.Center;

                //this._transformOrigin = this._innerControl.RenderTransformOrigin;
                //RotateTransform rotateTransform = this._innerControl.RenderTransform as RotateTransform;

                //if (rotateTransform != null)
                //{
                //    this.Angle = rotateTransform.Angle * Math.PI / 180.0;
                //}
                //else
                //{
                //    this.Angle = 0;
                //}
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this._innerControl != null)
            {
                double deltaVertical, deltaHorizontal;



                switch (VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        {
                            deltaVertical = Math.Min(-e.VerticalChange, this._innerControl.ActualHeight - _innerControl.MinHeight);


                            // 设置Left, Top或Margin
                            //Point position = GetPresentPosition(_innerControl);
                            //var newX = position.X- deltaVertical * _transformOrigin.Y * Math.Sin(-Angle);
                            //var newY = position.Y + (_transformOrigin.Y * deltaVertical * (1 - Math.Cos(-Angle)));
                            //SetPosition(newX, newY);

                            // 2019-6-25 设置Transform
                            var newX = -deltaVertical * _transformOrigin.Y * Math.Sin(-Angle);
                            var newY = (_transformOrigin.Y * deltaVertical * (1 - Math.Cos(-Angle)));
                            UpdatePosition(newX, newY);

                            this._innerControl.Height = Math.Abs(_innerControl.Height - deltaVertical);
                            break;
                        }
                    case System.Windows.VerticalAlignment.Top:
                        {
                            deltaVertical = Math.Min(e.VerticalChange, _innerControl.ActualHeight - this._innerControl.MinHeight);

                            //Point position = GetPresentPosition(_innerControl);
                            //var newX = position.X + deltaVertical * Math.Sin(-Angle) - (_transformOrigin.Y * deltaVertical * Math.Sin(-Angle));
                            //var newY = position.Y + deltaVertical * Math.Cos(-Angle) + (_transformOrigin.Y * deltaVertical * (1 - Math.Cos(-Angle)));
                            //SetPosition(newX, newY);

                            // 2019-6-25 
                            var newX = deltaVertical * Math.Sin(-Angle) - (_transformOrigin.Y * deltaVertical * Math.Sin(-Angle));
                            var newY = deltaVertical * Math.Cos(-Angle) + (_transformOrigin.Y * deltaVertical * (1 - Math.Cos(-Angle)));                            
                            UpdatePosition(newX, newY);

                            this._innerControl.Height = Math.Abs(this._innerControl.Height - deltaVertical);
                            break;
                        }
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:
                        {
                            deltaHorizontal = Math.Min(e.HorizontalChange, this._innerControl.ActualWidth - this._innerControl.MinWidth);


                            //var newX = OriginalX + deltaHorizontal * Math.Cos(this.Angle) + (this._transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.Angle)));
                            //var newY = OriginalY + deltaHorizontal * Math.Sin(this.Angle) - this._transformOrigin.X * deltaHorizontal * Math.Sin(this.Angle);
                            //SetPosition(newX, newY);

                            var newX = deltaHorizontal * Math.Cos(Angle) + (this._transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.Angle)));
                            var newY = deltaHorizontal * Math.Sin(Angle) - this._transformOrigin.X * deltaHorizontal * Math.Sin(this.Angle);
                            UpdatePosition(newX, newY);


                            this._innerControl.Width -= deltaHorizontal;
                            break;
                        }
                    case System.Windows.HorizontalAlignment.Right:
                        {
                            deltaHorizontal = Math.Min(-e.HorizontalChange, this._innerControl.ActualWidth - this._innerControl.MinWidth);

                            //var newX = OriginalX + (deltaHorizontal * this._transformOrigin.X * (1 - Math.Cos(this.Angle)));
                            //var newY = OriginalY - this._transformOrigin.X * deltaHorizontal * Math.Sin(this.Angle);
                            //SetPosition(newX, newY);

                            var newX = (deltaHorizontal * this._transformOrigin.X * (1 - Math.Cos(this.Angle)));
                            var newY = - this._transformOrigin.X * deltaHorizontal * Math.Sin(this.Angle);
                            UpdatePosition(newX, newY);

                            this._innerControl.Width -= deltaHorizontal;
                            break;
                        }
                    default:
                        break;
                }
            }

            e.Handled = true;
        }


        [Obsolete]
        private double OriginalX
        {
            get
            {
                if (_innerControl.Parent as SmashInkCanvas != null)
                {
                    return GetPresentPosition(_innerControl).X;

                }
                else if (_innerControl.Parent as InkCanvas != null)
                {
                    return InkCanvas.GetLeft(this._innerControl);
                }
                else if (_innerControl.Parent as Canvas != null)
                {
                    return Canvas.GetLeft(this._innerControl);
                }
                else
                {
                    return this._innerControl.Margin.Left;
                }
            }
        }

        [Obsolete]
        private double OriginalY
        {
            get
            {
                if (_innerControl.Parent as SmashInkCanvas != null)
                {
                    return GetPresentPosition(_innerControl).Y;
                }
                else if (_innerControl.Parent as InkCanvas != null)
                {
                    return InkCanvas.GetTop(this._innerControl);
                }
                else if (_innerControl.Parent as Canvas != null)
                {
                    return Canvas.GetTop(this._innerControl);
                }
                else
                {
                    return this._innerControl.Margin.Top;
                }
            }
        }

        public double Angle
        {
            get
            {
                double angle = 0;
                var tg = _innerControl.GetTransformGroup();
                foreach (var item in tg.Children)
                {
                    if (item is RotateTransform trans)
                    {
                        //angle += trans.Angle * Math.PI / 180.0;
                        angle += trans.Angle;
                    }
                }

                return angle;
            }

        }

        /// <summary>
        /// 获取控件的位置
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private Point GetPresentPosition(UIElement control)
        {
            double x = SmashInkCanvas.GetLeft(control);
            double y = SmashInkCanvas.GetTop(control);

            if (double.IsNaN(x))
            {
                x = 0;
            }
            if (double.IsNaN(y))
            {
                y = 0;
            }

            var tg = _innerControl.GetTransformGroup();
            foreach (var item in tg.Children)
            {
                if (item is TranslateTransform trans)
                {
                    x += trans.X;
                    y += trans.Y;
                }
            }

            return new Point(x, y);
        }

        /// <summary>
        /// 更新控件的位置(Transfrom)
        /// </summary>
        /// <param name="xChange"></param>
        /// <param name="yChange"></param>
        private void UpdatePosition(double xChange, double yChange)
        {
            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = xChange;
            translateTransform.Y = yChange;
            _innerControl.ApplyTransform(translateTransform);
        }

        /// <summary>
        /// 更新控件的位置(Left,Top)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetPosition(double x, double y)
        {
            if (_innerControl.Parent as InkCanvas != null)
            {
                InkCanvas.SetLeft(this._innerControl, x);
                InkCanvas.SetTop(this._innerControl, y);
            }
            else if (_innerControl.Parent as Canvas != null)
            {
                Canvas.SetLeft(this._innerControl, x);
                Canvas.SetTop(this._innerControl, y);
            }
            else
            {
                _innerControl.Margin = new Thickness(
                    x,
                    y,
                    _innerControl.Margin.Right,
                    _innerControl.Margin.Bottom
                );
            }
        }

    }
}
