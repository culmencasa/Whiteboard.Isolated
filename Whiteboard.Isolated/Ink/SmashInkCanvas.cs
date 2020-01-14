using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Whiteboard.Isolated.Ink;
using Whiteboard.Isolated.Shapes;
using Whiteboard.Isolated.UserCanvas;

namespace Whiteboard.Isolated.Ink
{
    /// <summary>
    /// 自定义画布
    /// </summary>
    public class SmashInkCanvas : InkCanvas
    {
        #region 委托

        public event EventHandler<ModeChangedEventArgs> CoveredModeChanged;

        #endregion

        #region 枚举


        #endregion
        
        #region 字段

        /// <summary>
        /// 鼠标拖动开始点
        /// </summary>
        Point _selectStartPoint = new Point(0, 0);
        /// <summary>
        /// 鼠标拖动结束点
        /// </summary>
        Point _selectEndPoint = new Point(0, 0);

        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        bool _isMouseDown = false;

        /// <summary>
        /// 拖动矩形选择框
        /// </summary>
        bool _isDragingSelectionRectangle = false;
        /// <summary>
        /// 拖动拉伸形状
        /// </summary>
        bool _isDragingShape = false;

        /// <summary>
        /// 当前编辑模式
        /// </summary>
        CoveredModes _coveredMode = CoveredModes.None;

        #endregion
        
        #region 属性

        /// <summary>
        /// 当前编辑模式
        /// </summary>
        public CoveredModes CoveredMode
        {
            get
            {
                return _coveredMode;
            }
            set
            {
                bool isChanged = _coveredMode != value;
                if (isChanged)
                {
                    var oldValue = _coveredMode;
                    _coveredMode = value;

                    UpdateCoveredMode(_coveredMode);
                    CoveredModeChanged?.Invoke(this, new ModeChangedEventArgs()
                    {
                        OldValue = oldValue,
                        NewValue = value
                    });
                }
            }
        }

        /// <summary>
        /// 形状模式下的对象
        /// </summary>
        public ContentControl ShapeWrapper { get; set; }

        /// <summary>
        /// 矩形选框
        /// </summary>
        public Rectangle SelectingIndicator { get; set; } = new Rectangle();

        /// <summary>
        /// 拖动临界值
        /// </summary>
        public double DragThreshold { get; set; } = 20;

        #endregion
        
        #region 构造

        public SmashInkCanvas() : base()
        {
            MouseDown += MyCanvas_MouseDown;
            MouseMove += MyCanvas_MouseMove;
            MouseUp += MyCanvas_MouseUp;
            
            this.LostFocus += SmashInkCanvas_LostFocus;
            this.Loaded += SmashInkCanvas_Loaded;
            CreateSelectingIndicator();


            UpdateCoveredMode(_coveredMode);
        }


        #endregion

        #region 事件处理

        private void SmashInkCanvas_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!HitTestOnAdorner())
            {
                ClearSelectionStatus();
            }
        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right)
            {
                // 只在EditMode为None时选取
                if (this.CoveredMode == CoveredModes.SelectByRectangle)
                {
                    _selectStartPoint = e.GetPosition(this);


                    #region 2019-07-15 点击元素选中

                    Adorner ad = null;
                    var control = this;

                    foreach (ContentControl item in control.Children.OfType<ContentControl>())
                    {
                        if (item.InputHitTest(e.GetPosition(item)) != null)
                        { 
                            ShapeAdorner adorner = new ShapeAdorner(item);
                            ad = adorner;
                            break;
                        }
                    }

                    if (ad == null)
                    {
                        foreach (var stroke in control.Strokes)
                        {
                            if (stroke.HitTest(_selectStartPoint))
                            {
                                ad = new StrokeAdorner(control, new StrokeCollection() { stroke });
                                break;
                            }
                        }
                    }

                    if (ad != null)
                    {
                        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                        if (adornerLayer != null)
                        {
                            ClearSelectionStatus();
                            adornerLayer.Add(ad);
                            return;
                        }
                    }

                    #endregion

                    _isMouseDown = true;

                    Mouse.Capture(this);
                }
                else if (this.CoveredMode == CoveredModes.ShapeStamp)
                {
                    _selectStartPoint = e.GetPosition(this);
                    _isMouseDown = true;

                    Mouse.Capture(this);
                }
                else
                {

                }


                if (!HitTestOnAdorner())
                {
                    ClearSelectionStatus();
                }

            }
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsInSelectingMode && !IsInShapeMode)            
                return;

            if (_selectStartPoint == new Point(0, 0))
            {
                return;
            }

            InkCanvas control = this;

            if (CoveredMode == CoveredModes.SelectByRectangle)
            {
                if (_isDragingSelectionRectangle)
                {
                    _selectEndPoint = e.GetPosition(control);
                    UpdateSelectingIndicatorSize(_selectStartPoint, _selectEndPoint);

                    e.Handled = true;
                }
                else if (_isMouseDown && e.LeftButton == MouseButtonState.Pressed)
                {
                    // 第一次拖动
                    Point thisMouseDownPoint = e.GetPosition(control);
                    Point lastMouseDownPoint = _selectStartPoint;
                    var dragDelta = thisMouseDownPoint - lastMouseDownPoint;
                    double dragDistance = Math.Abs(dragDelta.Length);
                    if (dragDistance > DragThreshold)
                    {
                        _isDragingSelectionRectangle = true;

                        _selectEndPoint = thisMouseDownPoint;
                        UpdateSelectingIndicatorSize(lastMouseDownPoint, thisMouseDownPoint);
                    }

                    e.Handled = true;
                }
            }
            else if (CoveredMode == CoveredModes.ShapeStamp)
            {
                if (_isDragingShape)
                {
                    _selectEndPoint = e.GetPosition(control);
                    UpdateDragShapeSize(_selectStartPoint, _selectEndPoint);
                    e.Handled = true;
                }
                else if (_isMouseDown && e.LeftButton == MouseButtonState.Pressed)
                {
                    Point thisMouseDownPoint = e.GetPosition(control);
                    Point lastMouseDownPoint = _selectStartPoint;
                    var dragDelta = thisMouseDownPoint - lastMouseDownPoint;
                    double dragDistance = Math.Abs(dragDelta.Length);
                    if (dragDistance > DragThreshold)
                    {
                        _isDragingShape = true;

                        _selectEndPoint = thisMouseDownPoint;
                        UpdateDragShapeSize(lastMouseDownPoint, thisMouseDownPoint);
                    }

                    e.Handled = true;
                }
            }

            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    endPoint = e.GetPosition(this);
            //    //if (lassoStroke == null)
            //    //{
            //    //    StylusPointCollection pts = new StylusPointCollection();
            //    //    pts.Add(new StylusPoint(startPoint.X, startPoint.Y));
            //    //    pts.Add(new StylusPoint(endPoint.X, endPoint.Y));
            //    //    lassoStroke = new LassoStroke(pts, drawAttr) { DashStyle = new DashStyle(new double[] { 4, 10 }, 0) };

            //    //    this.ink.Strokes.Add(lassoStroke);
            //    //}
            //    //else
            //    //{
            //    //    this.lassoStroke.StylusPoints.Add(new StylusPoint(endPoint.X, endPoint.Y));
            //    //}

            //    UpdateDragSelectionRect(startPoint, endPoint);

            //}
        }

        private void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            InkCanvas control = this;

            if (CoveredMode == CoveredModes.SelectByRectangle)
            {
                if (_isDragingSelectionRectangle)
                {
                    HideSelectingIndicator();


                    List<Adorner> adorners = new List<Adorner>();
                    Rect selectArea = new Rect(SelectingIndicator.Margin.Left, SelectingIndicator.Margin.Top, SelectingIndicator.Width, SelectingIndicator.Height);

                    foreach (var stroke in control.Strokes)
                    {

                        if (stroke.HitTest(selectArea, 30))
                        {
                            StrokeAdorner adorner = new StrokeAdorner(control, new StrokeCollection() { stroke });
                            adorners.Add(adorner);
                        }

                        // 会返回复制的Stroke
                        //StrokeCollection selectedStrokes = stroke.GetClipResult(selectArea);

                    }

                    foreach (ContentControl item in control.Children)
                    {
                        //double left = InkCanvas.GetLeft(item);
                        //double top = InkCanvas.GetTop(item);

                        Point position = item.TranslatePoint(new Point(0, 0), control);

                        var intersection = Rect.Intersect(selectArea, new Rect(position.X, position.Y, item.Width, item.Height));
                        if (intersection.Width > item.Width / 2 && intersection.Height > item.Height / 2)
                        {
                            ShapeAdorner adorner = new ShapeAdorner(item);
                            adorners.Add(adorner);
                        }
                    }

                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(control);
                    if (adornerLayer != null)
                    {
                        foreach (var ad in adorners)
                        {
                            adornerLayer.Add(ad);
                        }
                    }
                }
            }
            else if (CoveredMode == CoveredModes.ShapeStamp)
            {
                if (_isDragingShape)
                {
                    _isDragingShape = false;

                    // 设置选中状态
                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(control);
                    if (adornerLayer != null)
                    {
                        ShapeAdorner adorner = new ShapeAdorner(ShapeWrapper);
                        adornerLayer.Add(adorner);
                    }


                    // 姚卫星 记录拖动后的位置
                    double x = InkCanvas.GetLeft(ShapeWrapper);
                    double y = InkCanvas.GetTop(ShapeWrapper);
                    var transformGroup = ShapeWrapper.GetTransformGroup();
                    InkCanvas.SetLeft(ShapeWrapper, 0);
                    InkCanvas.SetTop(ShapeWrapper, 0);
                    transformGroup.Children.Add(new TranslateTransform(x, y));


                    var nextNewShape = CloneShapeWrapper(ShapeWrapper.Content as WiredShape);
                    ShapeWrapper = nextNewShape;
                }
            }

            if (Mouse.Captured == this)
            {
                Mouse.Capture(null);
            }
        }


        #endregion

        #region 公开方法

        /// <summary>
        /// 判断是否点击在Adorner上
        /// </summary>
        /// <returns></returns>
        public bool HitTestOnAdorner()
        {
            InkCanvas control = this;
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(control);
            if (adornerLayer != null)
            {
                // 线条
                Adorner[] strokeAdorners = adornerLayer.GetAdorners(control);
                if (strokeAdorners != null)
                {
                    for (int x = 0; x < strokeAdorners.Length; x++)
                    {
                        var c = strokeAdorners[x].InputHitTest(Mouse.GetPosition(this));

                        if (c != null)
                        {
                            return true;
                        } 
                    }
                }

                // 形状 
                foreach (UIElement item in control.Children)
                {
                    Adorner[] shapeAdorners = adornerLayer.GetAdorners(item);
                    if (shapeAdorners != null)
                    {
                        for (int x = 0; x < shapeAdorners.Length; x++)
                        {
                            var c = shapeAdorners[x].InputHitTest(Mouse.GetPosition(item));

                            if (c != null)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;             
        }
        
        /// <summary>
        /// 退出选中状态. 清除所有Adorner
        /// </summary>
        public void ClearSelectionStatus()
        {
            InkCanvas control = this;
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(control);
            if (adornerLayer != null)
            {
                // 清除线条
                Adorner[] toRemoveArray = adornerLayer.GetAdorners(control);
                if (toRemoveArray != null)
                {
                    for (int x = 0; x < toRemoveArray.Length; x++)
                    {
                        adornerLayer.Remove(toRemoveArray[x]);
                    }
                }

                // 清除形状 
                foreach (UIElement item in control.Children)
                {
                    toRemoveArray = adornerLayer.GetAdorners(item);
                    if (toRemoveArray != null)
                    {
                        for (int x = 0; x < toRemoveArray.Length; x++)
                        {
                            adornerLayer.Remove(toRemoveArray[x]);
                        }
                    }
                }
            }
        }

        #endregion

        #region 私有方法

        protected virtual ContentControl CloneShapeWrapper(WiredShape shape)
        {
            var wrapperObj = new ContentControl();

            // 设置大小
            wrapperObj.Width = 20;
            wrapperObj.Height = 20;

            // 复制形状
            var shapeObj = shape.Clone() as WiredShape;
            shapeObj.Fill = new SolidColorBrush(this.DefaultDrawingAttributes.Color);
            shapeObj.Stroke = shapeObj.Fill;
            wrapperObj.Content = shapeObj;


            return wrapperObj;
        }

        /// <summary>
        /// 更新内部的EditingMode
        /// </summary>
        /// <param name="newValue"></param>
        protected virtual void UpdateCoveredMode(CoveredModes newValue)
        {
            if (newValue == CoveredModes.None)
            {
                EditingMode = InkCanvasEditingMode.None;
            }
            else if (newValue == CoveredModes.SelectByRectangle)
            {
                EditingMode = InkCanvasEditingMode.None;
            }
            else if (newValue == CoveredModes.ShapeStamp)
            {
                EditingMode = InkCanvasEditingMode.None;
            }
            else if (newValue == CoveredModes.Ink)
            {
                EditingMode = InkCanvasEditingMode.Ink;
            }
            else if (newValue == CoveredModes.RubberByPoint)
            {
                EditingMode = InkCanvasEditingMode.EraseByPoint;
            }
            else if (newValue == CoveredModes.RubberByStroke)
            {
                EditingMode = InkCanvasEditingMode.EraseByStroke;
            }
            else
            {
                throw new Exception("未实现的模式");
            }
        }



        /// <summary>
        /// 正在选择中
        /// </summary>
        protected bool IsInSelectingMode
        {
            get
            {
                InkCanvas control = this;

                if (CoveredMode == CoveredModes.SelectByRectangle && _isMouseDown)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 是否形状图案拖放模式
        /// </summary>
        protected bool IsInShapeMode
        {
            get
            {
                InkCanvas control = this;

                if (CoveredMode == CoveredModes.ShapeStamp && _isMouseDown)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 隐藏选择框
        /// </summary>
        protected void HideSelectingIndicator()
        {
            _isMouseDown = false;
            _isDragingSelectionRectangle = false;

            _selectStartPoint = new Point(0, 0);
            _selectEndPoint = new Point(0, 0);

            InkCanvas control = this;
            if (control.Children.Contains(SelectingIndicator))
            {
                control.Children.Remove(SelectingIndicator);
                SelectingIndicator.Visibility = Visibility.Collapsed;
            }

        }

        /// <summary>
        /// 创建矩形选框
        /// </summary>
        protected void CreateSelectingIndicator()
        {
            SelectingIndicator = new Rectangle();
            SelectingIndicator.Fill = (Brush)new BrushConverter().ConvertFromString("#55ffffff");
            SelectingIndicator.Stroke = (Brush)new BrushConverter().ConvertFromString("#550000ff");
            SelectingIndicator.StrokeDashArray = new DoubleCollection() { 5 };
            SelectingIndicator.Visibility = Visibility.Collapsed;
            if (!this.Children.Contains(SelectingIndicator))
            {
                this.Children.Add(SelectingIndicator);
            }
        }


        /// <summary>
        /// 更新选框大小
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        protected void UpdateSelectingIndicatorSize(Point pt1, Point pt2)
        {
            double x, y, width, height;

            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }


            SelectingIndicator.Width = width;
            SelectingIndicator.Height = height;

            SelectingIndicator.Margin = new Thickness(x, y, 0, 0);

            if (!this.Children.Contains(SelectingIndicator))
            {
                this.Children.Add(SelectingIndicator);
            }
            SelectingIndicator.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 更新拉动矩形大小
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        protected void UpdateDragShapeSize(Point pt1, Point pt2)
        {
            double x, y, width, height;

            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }

            if (!this.Children.Contains(ShapeWrapper))
            {
                this.Children.Add(ShapeWrapper);
            }

            ShapeWrapper.Width = width;
            ShapeWrapper.Height = height;
             
            InkCanvas.SetLeft(ShapeWrapper, x);
            InkCanvas.SetTop(ShapeWrapper, y); 
        }

        #endregion

        #region 重写的成员
        

        // by 姚卫星
        private ImageBrush imageBrush;
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.imageBrush != null)
            {
                drawingContext.DrawImage(this.imageBrush.ImageSource, new Rect(new Point(0, 0), this.RenderSize));
            }
            base.OnRender(drawingContext);
        }
        private void SmashInkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.InkPresenter.Child is FrameworkElement frameworkElement)
            {
                frameworkElement.SetValue(BackgroundProperty, null);
            }
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == BackgroundProperty)
            {
                if (e.NewValue is ImageBrush imageBrush)
                {
                    this.imageBrush = imageBrush;
                    //this.ApplyBackgroundImageSize();
                }
            }
            base.OnPropertyChanged(e);
        }

        #endregion


    }
}
