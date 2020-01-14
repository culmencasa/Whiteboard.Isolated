using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Whiteboard.Isolated.UserCanvas
{
    public class StrokeAdorner : Adorner
    {

        // The surrounding boarder.
        Path outline = null;

        VisualCollection _visualChildren;

        /// <summary>
        /// 线条外框控件
        /// </summary>
        private StrokeSelectionFrame _ucStrokesFrame;

        /// <summary>
        /// 工具栏控件
        /// </summary>
        private AdornerToolbar _ucStrokeToolbar;



        Point _centerOfStrokes;

        /// <summary>
        /// 上次旋转的角度
        /// </summary>
        double _lastAngle;

        RotateTransform rotation;

        // The bounds of the Strokes;
        Rect strokeBounds = Rect.Empty;

        InkCanvas _myCanvas = null;

        /// <summary>
        /// 上一次鼠标拖动时的坐标
        /// </summary>
        Point _lastDragPoint = new Point(0, 0);




        public StrokeAdorner(UIElement adornedElement, StrokeCollection strokes)
            : base(adornedElement)
        {
            MyCanvas = adornedElement as InkCanvas;

            //GeneralTransform inkCanvasToSelectionAdorner = _myCanvas.TransformToDescendant(this);
            //Point pointOnSelectionAdorner = inkCanvasToSelectionAdorner.Transform(pointOnInkCanvas);


            StrokeFrame = new StrokeSelectionFrame();
            StrokeFrame.Playground = this;
            StrokeFrame.PlayObject = strokes;

            // 移动把柄
            StrokeFrame.MoveThumb.DragStarted += MoveThumb_DragStarted;
            StrokeFrame.MoveThumb.DragDelta += MoveThumb_DragDelta;
            StrokeFrame.MoveThumb.DragCompleted += MoveThumb_DragCompleted;

            // 旋转把柄
            StrokeFrame.RotateThumb.DragDelta += RotateThumb_DragDelta;
            StrokeFrame.RotateThumb.DragCompleted += RotateThumb_DragCompleted;
            // 拉伸把柄


            StrokeToolbar = new AdornerToolbar();
            StrokeToolbar.HorizontalAlignment = HorizontalAlignment.Center;
            StrokeToolbar.VerticalAlignment = VerticalAlignment.Bottom;
            StrokeToolbar.PutOnTopHandler += _ucStrokeToolbar_PutOnTopHandler;
            StrokeToolbar.CopyHandler += _ucStrokeToolbar_CopyHandler;
            StrokeToolbar.DeleteHandler += _ucStrokeToolbar_DeleteHandler;


            this._visualChildren = new VisualCollection(this);
            this._visualChildren.Add(StrokeFrame);
            this._visualChildren.Add(StrokeToolbar);


            AdornedStrokes = strokes;
            strokeBounds = AdornedStrokes.GetBounds();

        }

        private void _ucStrokeToolbar_DeleteHandler()
        {
            if (AdornedStrokes != null)
            {
                foreach (var stroke in this.AdornedStrokes)
                {
                    if (MyCanvas.Strokes.Contains(stroke))
                    {
                        MyCanvas.Strokes.Remove(stroke);
                    }
                }
            }

            (this.Parent as AdornerLayer).Remove(this);
        }

        private void _ucStrokeToolbar_CopyHandler()
        {
            var copy = this.AdornedStrokes.Clone();
            MyCanvas.Strokes.Add(copy);
            //copy.Transform(new Matrix(1, 0, 0, 1, -50, -50), false);

            var parentLayer = this.Parent as AdornerLayer;

            StrokeAdorner adorner = new StrokeAdorner(MyCanvas, new StrokeCollection() { copy });
            parentLayer.Add(adorner);


            parentLayer.Remove(this);
        }

        private void _ucStrokeToolbar_PutOnTopHandler()
        {
            var copy = this.AdornedStrokes.Clone();
            MyCanvas.Strokes.Remove(AdornedStrokes);
            MyCanvas.Strokes.Add(copy);

            var parentLayer = this.Parent as AdornerLayer;
            (this.Parent as AdornerLayer).Remove(this);

            StrokeAdorner adorner = new StrokeAdorner(MyCanvas, new StrokeCollection() { copy });
            parentLayer.Add(adorner);

        }

        #region 旋转

        private double GetAngle(Point lastCenterPoint)
        {
            Point pos = Mouse.GetPosition(this);

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

        
        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double angle = GetAngle(_centerOfStrokes);

            Point frameCenter = StrokeFrame.Center;
            double angle2 = angle; 
             

            // 旋转线条
            Matrix mat = new Matrix();
            mat.RotateAt(angle - _lastAngle, _centerOfStrokes.X, _centerOfStrokes.Y);
            AdornedStrokes.Transform(mat, true);//old value true


            // 旋转外框
            TransformGroup transformGroup = GetTransformGroup(StrokeFrame);
            RotateTransform rotation2 = new RotateTransform(angle2 - _lastAngle, frameCenter.X, frameCenter.Y);             
            transformGroup.Children.Add(rotation2);
             

            // 保存上一次旋转的角度
            _lastAngle = angle;

        }

        private void RotateThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (rotation == null)
            {
                return;
            }

            //this.InvalidateArrange();
        }


        #endregion


        public TransformGroup GetTransformGroup(UIElement control)
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


        #region 移动

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {

            //GeneralTransform inkCanvasToSelectionAdorner = _myCanvas.TransformToDescendant(this);
            //Point pointOnSelectionAdorner = inkCanvasToSelectionAdorner.Transform(pointOnInkCanvas);


            //
            // compute our transform, but first remove our artificial border
            //
            //Matrix matrix = MapRectToRect(newRect, previousRect);

            // e变量中的Delta值不准 , 改用鼠标的点计算 
            _lastDragPoint = Mouse.GetPosition(MyCanvas);

        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            Point currentPoint = Mouse.GetPosition(MyCanvas);
            Point newDeltaPoint = new Point(currentPoint.X - _lastDragPoint.X, currentPoint.Y - _lastDragPoint.Y);
            _lastDragPoint = currentPoint;

            Matrix matrix = new Matrix();
            matrix.Translate(newDeltaPoint.X, newDeltaPoint.Y);
            AdornedStrokes.Transform(matrix, false);


            var group = this.StrokeFrame.RenderTransform as TransformGroup;
            if (group == null)
            {
                group = new TransformGroup();
            }
            group.Children.Add(new MatrixTransform(matrix));
            //group.Children.Add(new MatrixTransform(1, 0, 0, 1, differPoint.X, differPoint.Y));
            this.StrokeFrame.RenderTransform = group;
            //this.frame.InvalidateVisual();

            var toolGroup = StrokeToolbar.RenderTransform as TransformGroup;
            if (toolGroup == null)
            {
                toolGroup = new TransformGroup();
            }
            toolGroup.Children.Add(new MatrixTransform(matrix));
            StrokeToolbar.RenderTransform = toolGroup;


            //TransformGroup t = new TransformGroup();

            //Point origin = this.frame.RenderTransformOrigin;
            //bool hasOrigin = (origin.X != 0d || origin.Y != 0d);
            //if (hasOrigin)
            //    t.Children.Add(new TranslateTransform(-(strokeBounds.Width * origin.X), -(strokeBounds.Height * origin.Y)));

            //this.frame.RenderTransform = t;




            //TransformGroup transform = new TransformGroup();
            //transform.Children.Add(new TranslateTransform(-size.Width / 2.0, -size.Height / 2.0));
            //transform.Children.Add(new RotateTransform(GetAngle(child)));
            //transform.Children.Add(new TranslateTransform(position.X, position.Y));


            //    this.RenderTransform = transform;


            //var toolRegion = new Rect(
            //                strokeBounds.X + (handleRect.Width - toolbar.Width) / 2,
            //                strokeBounds.Y + handleRect.Height + 20,
            //                300,
            //                80);
            //toolbar.Arrange(toolRegion);
            //toolbar.RenderTransform = new MatrixTransform(matrix);

            //Rect elementBounds = new Rect(0, 0, size.Width, size.Height);   // Rect in element space
            //elementBounds = elementToCanvas.TransformBounds(elementBounds); // Rect in Canvas space

            //// Now apply the matrix to the element bounds
            //Rect newBounds = Rect.Transform(elementBounds, transform);
        }
        
        private void MoveThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            strokeBounds = AdornedStrokes.GetBounds();
            _centerOfStrokes = new Point(strokeBounds.X + strokeBounds.Width / 2,
                               strokeBounds.Y + strokeBounds.Height / 2);

            Point _centerOfFrame = this.StrokeFrame.Center;
            _centerOfFrame.Offset(e.HorizontalChange, e.VerticalChange);
            this.StrokeFrame.Center = _centerOfFrame;
        }
        
        #endregion


        /// <summary>
        /// Utility for computing a transformation that maps one rect to another
        /// </summary>
        /// <param name="target">Destination Rectangle</param>
        /// <param name="source">Source Rectangle</param>
        /// <returns>Transform that maps source rectangle to destination rectangle</returns>
        private static Matrix MapRectToRect(Rect target, Rect source)
        {
            if (source.IsEmpty)
                throw new ArgumentOutOfRangeException("source");

            /*
            In the horizontal direction:

                    M11*source.left  + Dx = target.left
                    M11*source.right + Dx = target.right

            Subtracting the equations yields:

                    M11*(source.right - source.left) = target.right - target.left
            hence:
                    M11 = (target.right - target.left) / (source.right - source.left)

            Having computed M11, solve the first equation for Dx:

                    Dx = target.left - M11*source.left
            */
            double m11 = target.Width / source.Width;
            double dx = target.Left - m11 * source.Left;

            // Now do the same thing in the vertical direction
            double m22 = target.Height / source.Height;
            double dy = target.Top - m22 * source.Top;
            /*
            We are looking for the transformation that takes one upright rectangle to
            another.  This involves neither rotation nor shear, so:
            */
            return new Matrix(m11, 0, 0, m22, dx, dy);
        }





        /// <summary>
        /// Draw the rotation handle and the outline of
        /// the element.
        /// </summary>
        /// <param name="finalSize">The final area within the 
        /// parent that this element should use to arrange 
        /// itself and its children.</param>
        /// <returns>The actual size used. </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (strokeBounds.IsEmpty)
            {
                return finalSize;
            }

            _centerOfStrokes = new Point(strokeBounds.X + strokeBounds.Width / 2,
                               strokeBounds.Y + strokeBounds.Height / 2);

            StrokeFrame.Center = new Point(StrokeFrame.ActualWidth / 2, StrokeFrame.ActualHeight / 2);

            // 线条外框矩形
            Rect handleRect = new Rect(strokeBounds.X,
                                  strokeBounds.Y,
                                  strokeBounds.Width, strokeBounds.Height);


            // 是否旋转外框
            if (rotation != null)
            {
                handleRect.Transform(rotation.Value);
            }
                       
            // 外框
            this.StrokeFrame.Arrange(handleRect);

            // 工具栏
            var toolRegion = new Rect(
                            strokeBounds.X + (handleRect.Width - StrokeToolbar.Width) / 2,
                            strokeBounds.Y + handleRect.Height + 20,
                            300,
                            80);
            StrokeToolbar.Arrange(toolRegion);


            return finalSize;
        }
















        /// <summary>
        /// Rotates the rectangle representing the
        /// strokes' bounds as the user drags the
        /// Thumb.
        /// </summary>
        void rotateHandle_DragDelta(object sender, DragDeltaEventArgs e)
        {
            // Find the angle of which to rotate the shape.  Use the right
            // triangle that uses the center and the mouse's position 
            // as vertices for the hypotenuse.

            Point pos = Mouse.GetPosition(this);

            double deltaX = pos.X - _centerOfStrokes.X;
            double deltaY = pos.Y - _centerOfStrokes.Y;

            if (deltaY.Equals(0))
            {

                return;
            }

            double tan = deltaX / deltaY;
            double angle = Math.Atan(tan);

            // Convert to degrees.
            angle = angle * 180 / Math.PI;

            // If the mouse crosses the vertical center, 
            // find the complementary angle.
            if (deltaY > 0)
            {
                angle = 180 - Math.Abs(angle);
            }

            // Rotate left if the mouse moves left and right
            // if the mouse moves right.
            if (deltaX < 0)
            {
                angle = -Math.Abs(angle);
            }
            else
            {
                angle = Math.Abs(angle);
            }

            if (Double.IsNaN(angle))
            {
                return;
            }

            // Apply the rotation to the strokes' outline.
            rotation = new RotateTransform(angle, _centerOfStrokes.X, _centerOfStrokes.Y);
            outline.RenderTransform = rotation;
        }

        /// <summary>
        /// Rotates the strokes to the same angle as outline.
        /// </summary>
        void rotateHandle_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (rotation == null)
            {
                return;
            }

            // Rotate the strokes to match the new angle.
            Matrix mat = new Matrix();
            mat.RotateAt(rotation.Angle - _lastAngle, _centerOfStrokes.X, _centerOfStrokes.Y);
            AdornedStrokes.Transform(mat, true);

            // Save the angle of the last rotation.
            _lastAngle = rotation.Angle;

            // Redraw rotateHandle.
            this.InvalidateArrange();
        }

        /// <summary>
        /// Gets the strokes of the adorned element 
        /// (in this case, an InkPresenter).
        /// </summary>
        public StrokeCollection AdornedStrokes
        {
            get;
            set;
        }

        // Override the VisualChildrenCount and 
        // GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount
        {
            get { return _visualChildren.Count; }
        }

        /// <summary>
        /// 画板
        /// </summary>
        public InkCanvas MyCanvas { get => _myCanvas; private set => _myCanvas = value; }
        /// <summary>
        /// 外框
        /// </summary>
        public StrokeSelectionFrame StrokeFrame { get => _ucStrokesFrame; private set => _ucStrokesFrame = value; }
        /// <summary>
        /// 工具栏
        /// </summary>
        public AdornerToolbar StrokeToolbar { get => _ucStrokeToolbar; private set => _ucStrokeToolbar = value; }

        protected override Visual GetVisualChild(int index)
        {
            return _visualChildren[index];
        }
    }
}
