using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using Whiteboard.Isolated.Ink;

namespace Whiteboard.Isolated.UserCanvas
{
    public class StrokeResizeThumb : Thumb
    {
        public StrokeResizeThumb()
        {
            this.DataContextChanged += StrokeResizeThumb_DataContextChanged;


            this.Loaded += StrokeResizeThumb_Loaded;
            DragStarted += StrokeResizeThumb_DragStarted;
            DragDelta += StrokeResizeThumb_DragDelta;
        }

        private void StrokeResizeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            _adorner = this.Tag as StrokeAdorner;
            _playground = _adorner.MyCanvas as SmashInkCanvas;
            _playObject = this.DataContext as StrokeCollection;
            _playRect = _playObject.GetBounds();
        }

        private void StrokeResizeThumb_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private void StrokeResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            _lastSize = Mouse.GetPosition(_adorner); 


            //this._transformOrigin = this._playObject.RenderTransformOrigin;
            //TransformGroup transformGroup = this._playObject.Transform as TransformGroup;
            //if (transformGroup != null)
            //{
            //    //this._angle = rotateTransform.Angle * Math.PI / 180.0;
            //}
            //else
            //{
            //    transformGroup = new TransformGroup();
            //    //this._angle = 0;
            //}
        } 
        Point _lastSize = new Point(0, 0);

        int minHeight = 50;
        int minWidth = 50;

        private void StrokeResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        { 
            Point currentPoint = Mouse.GetPosition(_adorner);
            Point newDeltaPoint = new Point(currentPoint.X - _lastSize.X, currentPoint.Y - _lastSize.Y);
            _lastSize = currentPoint;


            var frame = _adorner.StrokeFrame;
            var transformGroup = GetTransformGroup(frame);
            //transformGroup.Children.Add(new ScaleTransform(newDeltaPoint.X, newDeltaPoint.Y, frame.Center.X, frame.Center.Y));


            //_playObject.Transform(new Matrix(
             

            if (frame != null)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        {
                            deltaVertical = Math.Min(-e.VerticalChange, _playRect.Height - minHeight);
                            
                            var newX = OriginalX - deltaVertical * this._transformOrigin.Y * Math.Sin(-this._angle);
                            var newY = OriginalY + (this._transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this._angle)));

                            //SetPosition(newX, newY);

                            frame.Height = Math.Abs(_playRect.Height - deltaVertical);
                            break;
                        }
                    case VerticalAlignment.Top:
                        {
                            deltaVertical = Math.Min(e.VerticalChange, _playRect.Height - minHeight);

                            var newX = OriginalX + deltaVertical * Math.Sin(-this._angle) - (this._transformOrigin.Y * deltaVertical * Math.Sin(-this._angle));
                            var newY = OriginalY + deltaVertical * Math.Cos(-this._angle) + (this._transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this._angle)));

                            //SetPosition(newX, newY);

                            frame.Height = Math.Abs(_playRect.Height - deltaVertical);
                            break;
                        }
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        {
                            deltaHorizontal = Math.Min(e.HorizontalChange, _playRect.Width - minWidth);


                            var newX = OriginalX + deltaHorizontal * Math.Cos(this._angle) + (this._transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this._angle)));
                            var newY = OriginalY + deltaHorizontal * Math.Sin(this._angle) - this._transformOrigin.X * deltaHorizontal * Math.Sin(this._angle);

                            //SetPosition(newX, newY);

                            frame.Width -= deltaHorizontal;
                            break;
                        }
                    case HorizontalAlignment.Right:
                        {
                            deltaHorizontal = Math.Min(-e.HorizontalChange, _playRect.Width - minWidth);

                            var newX = OriginalX + (deltaHorizontal * this._transformOrigin.X * (1 - Math.Cos(this._angle)));
                            var newY = OriginalY - this._transformOrigin.X * deltaHorizontal * Math.Sin(this._angle);

                            //SetPosition(newX, newY);

                            frame.Width -= deltaHorizontal;
                            break;
                        }
                    default:
                        break;
                }


            }

            // 更新
            _playRect = _playObject.GetBounds();


            //transformGroup.Children.Add(new TranslateTransform(_playRect.Width * origin.X,
            //                              _playRect.Height * origin.Y));
            e.Handled = true;
        }

        private double OriginalX
        {
            get
            {
                return _playRect.X;
            }
        }

        private double OriginalY
        {
            get
            {
                return _playRect.Y;
            }
        }



        private TransformGroup GetTransformGroup(UIElement control)
        {
            TransformGroup transformGroup = null;

            // 获取原样式
            Transform renderTransform = control.RenderTransform;
            if (renderTransform == Transform.Identity)
            {
                // 不存在原样式, 创建一个空组
                transformGroup = new TransformGroup();
                control.RenderTransform = transformGroup;
            }
            else
            {
                // 两种情况 : Transform是TransformGroup类型, 或者是其他类型
                if (control.RenderTransform is TransformGroup)
                {
                    transformGroup = control.RenderTransform as TransformGroup;
                }
                else
                {
                    transformGroup = new TransformGroup();
                    transformGroup.Children.Add(renderTransform);

                    control.RenderTransform = transformGroup;
                }
            }

            return transformGroup;
        }


        private double _angle = 0;
        private Point _transformOrigin;
        private StrokeCollection _playObject;
        private StrokeAdorner _adorner;
        private SmashInkCanvas _playground;
        private Rect _playRect;

    }
}
