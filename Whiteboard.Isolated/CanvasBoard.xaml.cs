using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using UniversalSerializerLib3;
using Whiteboard.Isolated.Compose;
using Whiteboard.Isolated.Ink;
using Whiteboard.Isolated.Interface;
using Whiteboard.Isolated.Shapes;
using Whiteboard.Isolated.UserCanvas;

namespace Whiteboard.Isolated
{
    /// <summary>
    /// 画板界面
    /// </summary>
    public partial class CanvasBoard : UserControl, ICanvasBoard
    {
        public event Action<object, IPagePersistence> Shown;


        #region 字段


        private UndoRedoHelper _undoRedoHelper;
        private bool _firstTimeLoad;

        private ToolbarStatus _activedTool = ToolbarStatus.None;

        EZLogger _logger;

        #endregion

        #region 属性

        public IPagePersistence PageSaver { get; set; }

        #endregion

        #region ICanvasBoard成员

        public int CurrentPageIndex
        {
            get
            {
                return this.ucPage.PageIndex;
            }
            set
            {
                this.ucPage.PageIndex = value;
            }
        }

        public int PageCount
        {
            get
            {
                return this.ucPage.PageCount;
            }
            set
            {
                this.ucPage.PageCount = value;
            }
        }
        
        public byte[] ExportXml()
        {
            return CanvasPageDataToByteArray();
        }
        
        public void ImportXml(byte[] data)
        {
            ByteArrayToCanvasPageData(data);
        }

        public void SetToolbarVisibility(bool visibility)
        {
            if (visibility)
            {
                myToolbar.Visibility = Visibility.Visible;
            }
            else
            {
                myToolbar.Visibility = Visibility.Collapsed;
            }
        }

        public void SetPagerVisibility(bool visibility)
        {
            if (visibility)
            {
                myPager.Visibility = Visibility.Visible;
            }
            else
            {
                myPager.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region 构造

        /// <summary>
        /// 构造
        /// </summary>
        public CanvasBoard()
        {
            InitializeComponent();

            // 初始化撤消重做
            _undoRedoHelper = new UndoRedoHelper(myCanvas);

            // 设置白板画笔属性
            var drawingAttributes = new DrawingAttributes
            {
                Color = Color.FromRgb(246, 194, 19),
                Width = 8,
                Height = 8,
                StylusTip = StylusTip.Ellipse
            };
            myCanvas.DefaultDrawingAttributes = drawingAttributes;
            myCanvas.CoveredMode = CoveredModes.None;
            myCanvas.CoveredModeChanged += UserCanvas_CoveredModeChanged;
            myCanvas.MouseDown += UserCanvas_MouseDown;
            

            LostFocus += UserCanvas_LostFocus;
            Loaded += UserCanvas_Loaded;
            Unloaded += UserCanvas_Unloaded;

            //todo:
            //Messenger.Default.Register<UserCanvasCommand>(this, DoExternalJob);
            
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                var catalog = PagePersistenceParts.Compose();

                PageSaver = catalog.PagePersistenceImplment;
                PageSaver.CanvasControl = this;
            }


            _logger = new EZLogger(
                    @"Logs\Canvas_" + DateTime.Now.ToString("yyyyMMdd") + ".log",
                    true,
                    (uint)EZLogger.Level.All);
            _logger.Start();
            if (PageSaver == null)
            {
                _logger.Error("PageSaver未加载.");
            }
        }


        #endregion

        #region UI事件处理

        /// <summary>
        /// 点击白板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HideFloatingPanels();
            if (_activedTool != ToolbarStatus.Ink && _activedTool != ToolbarStatus.Shape)
            {
                SwitchToolbarStatus(ToolbarStatus.Select);
                btnToolSelect.IsChecked = true;
                myCanvas.CoveredMode = CoveredModes.SelectByRectangle;

            } 
        }

        /// <summary>
        /// 白板加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_firstTimeLoad)
            {
                _firstTimeLoad = true;
                if (Shown != null)
                {
                    Shown(this, PageSaver);
                }

            }
        }

        /// <summary>
        /// 卸载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
            //todo:
            //Messenger.Default.Unregister<UserCanvasCommand>(this, DoExternalJob);
        }


        /// <summary>
        /// 白板失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserCanvas_LostFocus(object sender, RoutedEventArgs e)
        {
            //HideFloatingPanels();

            Mouse.Capture(null);
            PageSaver?.SavePage();
        }

        /// <summary>
        /// 白板编辑模态发生改变(内部)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserCanvas_CoveredModeChanged(object sender, ModeChangedEventArgs e)
        {
            if (e.OldValue == CoveredModes.ShapeStamp && e.NewValue == CoveredModes.SelectByRectangle)
            {
                btnToolSelect.IsChecked = true;
            }
        }

        /// <summary>
        /// 工具栏按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolSelect_Click(object sender, EventArgs e)
        {
            var ctrl = sender as IconControl;

            if (ctrl.IconText == "选择")
            {
                #region 选择 
                SwitchToolbarStatus(ToolbarStatus.Select);

                if (ctrl.IsChecked)
                {
                    myCanvas.CoveredMode = CoveredModes.SelectByRectangle;
                }
                else
                {
                    myCanvas.CoveredMode = CoveredModes.None;
                }
                #endregion
            }
            else if (ctrl.IconText == "画笔")
            {
                #region 画笔 
                SwitchToolbarStatus(ToolbarStatus.Ink);
                ArrangeFloatingPanel(ToolbarStatus.Ink);

                myCanvas.CoveredMode = CoveredModes.Ink;
                #endregion
            }
            else if (ctrl.IconText == "橡皮擦")
            {
                #region 橡皮擦

                SwitchToolbarStatus(ToolbarStatus.Rubber);
                ArrangeFloatingPanel(ToolbarStatus.Rubber);

                myCanvas.EraserShape = new RectangleStylusShape(20, 20);
                myCanvas.CoveredMode = CoveredModes.RubberByPoint;
                #endregion
            }
            else if (ctrl.IconText == "撤消")
            {
                #region 撤消
                HideFloatingPanels();
                _undoRedoHelper.Undo();
                #endregion
            }
            else if (ctrl.IconText == "重做" || ctrl.IconText == "恢复")
            {
                #region 重做
                //HideFloatingPanels();
                _undoRedoHelper.Redo();
                #endregion
            }
            else if (ctrl.IconText == "形状")
            {
                #region 形状
                SwitchToolbarStatus(ToolbarStatus.Shape, true);
                myCanvas.CoveredMode = CoveredModes.None;
                ArrangeFloatingPanel(ToolbarStatus.Shape);
                #endregion
            }
            else if (ctrl.IconText == "更多")
            {
                #region 更多                
                SwitchToolbarStatus(ToolbarStatus.More, true);
                myCanvas.CoveredMode = CoveredModes.None;

                ArrangeFloatingPanel(ToolbarStatus.More);

                #endregion
            }
            else
            {
                HideFloatingPanels();
            }

        }

        /// <summary>
        /// 切换画笔大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PenSizeButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            myCanvas.DefaultDrawingAttributes.Width = button.FontSize;
            myCanvas.DefaultDrawingAttributes.Height = button.FontSize;
        }

        /// <summary>
        /// 切换画笔颜色 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PenColorButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            myCanvas.DefaultDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString(button.Background.ToString());
        }

        /// <summary>
        /// 点击形状列表浮窗中的形状
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShapeList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var container = sender as Border;
            if (container != null)
            {
                var shape = container.Child as IReusableShape;

                // 把要拖动的数据临时保存到控件的Tag上
                pnlShapes.Tag = new ShapeDragBag() {
                    MouseDownLocation = Mouse.GetPosition(null),
                    Source = container,
                    ShapeObject = shape };


                #region 点击时默认为形状鼠标拉伸模式, 如果拖动则取消该模式

                // 鼠标按下, 设为形状拉伸模式
                myCanvas.CoveredMode = CoveredModes.ShapeStamp;
                // 清除当前控件相邻的控件选中状态
                var siblings = (container.Parent as WrapPanel).Children;
                foreach (var item in siblings)
                {
                    Selector.SetIsSelected(item as DependencyObject, false);
                }
                Selector.SetIsSelected(container, true);

                // 复制一个形状出来
                var wrapperObj = new ContentControl();
                wrapperObj.Width = 20;
                wrapperObj.Height = 20;
                var shapeObj = shape.Clone() as WiredShape;
                shapeObj.Fill = new SolidColorBrush(myCanvas.DefaultDrawingAttributes.Color);
                shapeObj.Stroke = shapeObj.Fill;
                wrapperObj.Content = shapeObj;
                myCanvas.ShapeWrapper = wrapperObj;

                #endregion
            }
            else
            {

            }
        }
        private void ShapeList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (pnlShapes.Tag != null && e.LeftButton == MouseButtonState.Pressed)
            {
                ShapeDragBag dragBag = pnlShapes.Tag as ShapeDragBag;

                Point mouseMoveLocation = Mouse.GetPosition(null);
                var dragDelta = mouseMoveLocation - dragBag.MouseDownLocation;
                double dragDistance = Math.Abs(dragDelta.Length);
                if (dragDistance >= 20)
                {
                    #region 点击时默认为形状鼠标拉伸模式, 如果拖动则取消该模式

                    var container = sender as Border;
                    if (container != null)
                    {
                        // 清除当前控件相邻的控件选中状态
                        var siblings = (container.Parent as WrapPanel).Children;
                        foreach (var item in siblings)
                        {
                            Selector.SetIsSelected(item as DependencyObject, false);
                        }

                        myCanvas.ShapeWrapper = null;
                    }
                     
                    myCanvas.CoveredMode = CoveredModes.None;

                    #endregion

                    DragDrop.DoDragDrop(dragBag.Source,
                        new DropShapeInfo()
                        {
                            DraggingObject = dragBag.ShapeObject
                        },
                        DragDropEffects.Copy);

                    pnlShapes.Tag = null;
                }
            }
            else if (pnlShapes.Tag != null && e.LeftButton == MouseButtonState.Released)
            {
                pnlShapes.Tag = null;
            }
        }

        /// <summary>
        /// 形状拖放到画布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InkCanvas_Drop(object sender, DragEventArgs e)
        {
            DropShapeInfo info = e.Data.GetData(typeof(DropShapeInfo)) as DropShapeInfo;
            if (info != null)
            {
                var wrapperObj = new ContentControl();

                // 设置位置
                Point position = e.GetPosition(myCanvas);
                //InkCanvas.SetLeft(wrapperObj, position.X);
                //InkCanvas.SetTop(wrapperObj, position.Y);
                var transformGroup = wrapperObj.GetTransformGroup();
                if (transformGroup != null)
                {
                    transformGroup.Children.Add(new TranslateTransform(position.X, position.Y));
                }
                // 设置大小
                wrapperObj.Width = 128;
                wrapperObj.Height = 128;

                // 复制形状
                var shapeObj = info.DraggingObject.Clone() as WiredShape;
                shapeObj.Fill = new SolidColorBrush(myCanvas.DefaultDrawingAttributes.Color);
                shapeObj.Stroke = shapeObj.Fill;
                wrapperObj.Content = shapeObj;
                
                myCanvas.Children.Add(wrapperObj);

                // 设置选中状态
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.myCanvas);
                if (adornerLayer != null)
                {
                    myCanvas.ClearSelectionStatus();
                    ShapeAdorner adorner = new ShapeAdorner(wrapperObj);
                    adornerLayer.Add(adorner);
                } 

                // 隐藏面板
                HideFloatingPanels();

                // 拖完之后把工具栏改为选择模式
                SwitchToolbarStatus(ToolbarStatus.Select, false);
                myCanvas.CoveredMode = CoveredModes.SelectByRectangle;
            }
        }


        /// <summary>
        /// 点击圆形按钮 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearHandle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Button button = sender as Button;
                double left = Canvas.GetLeft(button);
                if (left == 0)
                {
                    button.Tag = e.GetPosition(null);
                }
            }
        }
        /// <summary>
        /// 拖动圆形按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Button button = sender as Button;
                if (!(button.Tag is Point))
                {
                    return;
                }

                Point newPoint = e.GetPosition(null);
                Point initialPoint = (Point)button.Tag;

                
                double delta = newPoint.X - initialPoint.X;
                Console.WriteLine(delta);
                if (delta == 0)
                {
                    return;
                }
                if (delta < 0)
                {
                    delta = 0;
                    Canvas.SetLeft(button, delta);
                }
                else
                {
                    Canvas parent = button.Parent as Canvas; 
                    double boundary = parent.ActualWidth - button.ActualWidth - 3;
                    if (boundary == 0)
                    {
                        // 最右边
                        Canvas.SetLeft(button, boundary);
                        RubberBoundary.Background = new SolidColorBrush(Colors.Orange);
                    }
                    else if (boundary < delta)
                    {
                        Canvas.SetLeft(button, boundary);
                        RubberBoundary.Background = new SolidColorBrush(Colors.Orange);
                        return;
                    }
                    else
                    {
                        Canvas.SetLeft(button, delta);
                        RubberBoundary.Background = new SolidColorBrush(Color.FromRgb(235, 235, 237));
                    }


                    
                }
            }
        }
        /// <summary>
        /// 释放圆形按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearHandle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                Button button = sender as Button;
                Canvas parent = button.Parent as Canvas;

                double boundary = parent.ActualWidth - button.ActualWidth - 3;
                if (Canvas.GetLeft(button) == boundary)
                {
                    ClearCanvas();

                    HideFloatingPanels();
                }
                button.Tag = null;

                RubberBoundary.Background = new SolidColorBrush(Color.FromRgb(235, 235, 237));
                Canvas.SetLeft(button, 0);
            }
        }



        /// <summary>
        /// 点击上一页按钮 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnLastPage_Click(object sender, EventArgs e)
        {
            if (ucPage.PageIndex == 1)
                return;

            // 保存当前页
            //PageSaver?.SavePage();

            //var binder = SaveCurrentPageToDb();

            // 变换页码
            ucPage.PageIndex--;
            if (ucPage.PageIndex < 1)
            {
                ucPage.PageIndex = 1;
            }


            // 加载上一页
            PageSaver?.LoadPage(ucPage.PageIndex);
        }
        /// <summary>
        /// 点击下一页按钮 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnNextPage_Click(object sender, EventArgs e)
        {
            if (ucPage.PageIndex == ucPage.PageCount)
                return;

            // 保存当前页
            //PageSaver?.SavePage();

            // 变换页码
            ucPage.PageIndex++;
            if (ucPage.PageIndex > ucPage.PageCount)
            {
                ucPage.PageIndex = ucPage.PageCount;
            }

            // 加载下一页
            PageSaver?.LoadPage(ucPage.PageIndex);
        }
        /// <summary>
        /// 点击新增页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnAddPage_Click(object sender, EventArgs e)
        {
            // 保存当前页
            //PageSaver?.SavePage();

            // 变换页码
            ucPage.PageCount++;
            if (ucPage.PageCount > ucPage.MaxPageCount)
            {
                ucPage.PageCount = ucPage.MaxPageCount;
            }
            ucPage.PageIndex = ucPage.PageCount;
            PageSaver?.LoadPage(ucPage.PageIndex);
        }

        /// <summary>
        /// 点击放大镜按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnMagnifyGlass_Click(object sender, EventArgs e)
        {
            var win = Window.GetWindow(this);
            Grid rootControl = win.Content as Grid;
            //Canvas container = UIHelper.FindChild<Canvas>(win, "ToolLayer");
            Canvas container = win.FindName("ToolLayer") as Canvas;
            if (container == null)
            {
                container = new Canvas() { Name = "ToolLayer" };
                rootControl.Children.Add(container);
            }

            #region 清理工作
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(rootControl);
            var toRemoveArray = adornerLayer.GetAdorners(rootControl);
            if (toRemoveArray != null)
            {
                for (int x = 0; x < toRemoveArray.Length; x++)
                {
                    adornerLayer.Remove(toRemoveArray[x]);
                }
            }
            container.Children.Clear();
            #endregion

            #region 创建放大镜控件
            MagnifyGlass maginfier = new MagnifyGlass();
            maginfier.Width = 200;
            maginfier.Height = 200;
            maginfier.VisualObj = this;
            // 加到canvas上
            container.Children.Add(maginfier);

            // 设置初始位置
            Canvas.SetLeft(maginfier, 
                (rootControl.ActualWidth - maginfier.Width) / 2);
            Canvas.SetTop(maginfier, 
                (rootControl.ActualHeight - maginfier.Height) / 2);


            // 设置viewbox(25是原来的viewbox 50 / 2)
            Point centerPosition = new Point(
                rootControl.ActualWidth / 2 - 25,
                 rootControl.ActualHeight / 2 - 25);
            maginfier.SetViewbox(centerPosition);



            // 装饰层
            var adorner = new MagnifyGlassAdorner(maginfier);
            adorner.Closing += (a, b) =>
            {
                ShowToolbar();
                this.myCanvas.IsEnabled = true;
                //Messenger.Default.Send(new NotificationMessage<bool>(true, "SideBarVisibility"));
                //win.ShowSideBar();
            };
            adornerLayer.Add(adorner);
            SwitchToolbarStatus(ToolbarStatus.None);
            #endregion

            // 通知外部关闭浮动面板
            //Messenger.Default.Send();
            HideToolbar();

            this.myCanvas.IsEnabled = false;

            //Messenger.Default.Send(new NotificationMessage<bool>(false, "SideBarVisibility"));
            //win.HideSideBar();
        }

        /// <summary>
        /// 点击计时器按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowTimer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TimersWindow timers = new TimersWindow();
            timers.Owner = Window.GetWindow(this);
            timers.ShowDialog();
        }

        #endregion
        
        #region 私有方法

        /// <summary>
        /// 隐藏工具栏上的浮动弹窗
        /// </summary>
        /// <param name="exception"></param>
        private void HideFloatingPanels(ToolbarStatus exception = ToolbarStatus.None)
        {
            if (exception != ToolbarStatus.Ink)
            {
                pnlDrawAtt.Visibility = Visibility.Collapsed;
            }
            if (exception != ToolbarStatus.Rubber)
            {
                pnlRubber.Visibility = Visibility.Collapsed;
            }
            if (exception != ToolbarStatus.Shape)
            {
                pnlShapes.Visibility = Visibility.Collapsed;
            }
            if (exception != ToolbarStatus.More)
            {
                pnlMore.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 获取工具栏对应的弹窗
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        private UIElement GetCorrespondingPanel(ToolbarStatus tool)
        {
            UIElement floatingPanel = null;
            switch (tool)
            {
                case ToolbarStatus.Select:
                    break;
                case ToolbarStatus.Ink:
                    floatingPanel = pnlDrawAtt;
                    break;
                case ToolbarStatus.Rubber:
                    floatingPanel = pnlRubber;
                    break;
                case ToolbarStatus.Shape:
                    floatingPanel = pnlShapes;
                    break;
                case ToolbarStatus.More:
                    floatingPanel = pnlMore;
                    break;
            }
            return floatingPanel;
        }

        /// <summary>
        /// 切换弹窗状态
        /// </summary>
        /// <param name="tool"></param>
        private void SwitchFlaotingPanel(ToolbarStatus tool)
        {
            UIElement floatingPanel = GetCorrespondingPanel(tool);
            if (floatingPanel != null)
            {
                if (floatingPanel.Visibility == Visibility.Visible)
                {
                    floatingPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    floatingPanel.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// 切换工具栏按钮
        /// </summary>
        /// <param name="tool"></param>
        private void SwitchToolbarStatus(ToolbarStatus tool, bool defaultShowPanel=false)
        {
            // 获取工具栏按钮对应的浮动面板
            UIElement floatingPanel = GetCorrespondingPanel(tool);

            // 点击的工具栏按钮与上次点击的按钮不同
            if (_activedTool != tool) 
            {                
                // 先隐藏所有浮动面板
                HideFloatingPanels();

                // 如果点击的目标工具栏按钮有浮动面板, 根据defaultShowPanel参数控制是否显示, 默认不显示
                if (defaultShowPanel && floatingPanel != null)
                {
                    floatingPanel.Visibility = Visibility.Visible;
                }
            }
            else
            {
                // 点击的工具栏按钮与上次点击的按钮相同, 则切换当前工具栏按钮对应的浮动面板
                SwitchFlaotingPanel(tool);
            }

            // 更新当前的工具栏按钮值为新值
            _activedTool = tool;
        }

        /// <summary>
        /// 设置浮动面板的位置
        /// </summary>
        /// <param name="tool"></param>
        private void ArrangeFloatingPanel(ToolbarStatus tool)
        {
            Point toolbarButtonLocation = new Point(0, 0);
            IconControl relativeButton = null;
            FrameworkElement floatingPanel = null;
            switch (tool)
            {
                case ToolbarStatus.Select:
                    break;
                case ToolbarStatus.Ink:
                    floatingPanel = pnlDrawAtt;
                    relativeButton = btnPen;
                    break;
                case ToolbarStatus.Rubber:
                    floatingPanel = pnlRubber;
                    relativeButton = btnRubber;
                    break;
                case ToolbarStatus.Shape:
                    floatingPanel = pnlShapes;
                    relativeButton = btnShape;
                    break;
                case ToolbarStatus.More:
                    floatingPanel = pnlMore;
                    relativeButton = btnMore;
                    break;
                default:
                    return;
            }



            Window topWindow = Window.GetWindow(this);
            toolbarButtonLocation = relativeButton.TranslatePoint(new Point(0, 0), topWindow);

            var left = toolbarButtonLocation.X + relativeButton.ActualWidth / 2 - floatingPanel.Width / 2;
            var bottom = relativeButton.ActualHeight + 10;

            floatingPanel.HorizontalAlignment = HorizontalAlignment.Left;
            floatingPanel.VerticalAlignment = VerticalAlignment.Bottom;
            floatingPanel.Margin = new Thickness(left, 0, 0, bottom);
        }



        /// <summary>
        /// 清理画布
        /// </summary>
        private void ClearCanvas()
        {
            myCanvas.Strokes.Clear();
            myCanvas.Children.Clear();
            
            //issue: 翻页之后, 撤消的记录就没了
            _undoRedoHelper?.Reset();
        }

        /// <summary>
        /// 从指定的页数据重新加载
        /// </summary>
        /// <param name="info"></param>
        private void ReloadCanvas(SmashPageInfo info)
        {
            // 选清空画布
            ClearCanvas();

            if (info != null)
            {
                //注:添加而不是赋值, 不然Strokes的事件丢弃
                myCanvas.Strokes.Add(info.Strokes); 
                foreach (var item in info.Shapes)
                {
                    myCanvas.Children.Add(item);
                }
            }
        }
        
        #region 导入导出, 分页

        /// <summary>
        /// 把Canvas页数据转换为字节数据
        /// </summary>
        /// <returns></returns>
        private byte[] CanvasPageDataToByteArray()
        {
            byte[] result = null;

            // 使用遍历而不使用Linq方法, 否则形状会丢失位置信息
            List<ContentControl> list = new List<ContentControl>();
            foreach (var item in myCanvas.Children)
            {
                if (item is ContentControl  contentControl)
                {
                    contentControl.DataContext = null;
                    list.Add(contentControl);
                }
            }


            SmashPageInfo pageInfo = new SmashPageInfo()
            {
                PageIndex = 0,
                Shapes = list, 
                Strokes = myCanvas.Strokes
            };
            using (MemoryStream ms = new MemoryStream())
            {
                UniversalSerializer ser = new UniversalSerializer(ms);
                ser.Serialize(pageInfo);

                result = ms.ToArray();
            }

            return result;
        }

        /// <summary>
        /// 字节数组转换为Canvas页数据
        /// </summary>
        /// <param name="data"></param>
        public void ByteArrayToCanvasPageData(byte[] data)
        {
            SmashPageInfo pageInfo = null;

            if (data != null)
            {
                // 点击下一页 (this)
                // 从db加载数据 (out)
                // 把数据转成entity 
                // 把entity的属性重新赋给canvas, 刷新控件
                // 
                try
                {
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        UniversalSerializer ser = new UniversalSerializer(ms);
                        pageInfo = ser.Deserialize() as SmashPageInfo;
                    }
                }
                catch (Exception ex)
                {
                    pageInfo = new SmashPageInfo()
                    {
                        PageIndex = 1,
                        Shapes = new List<ContentControl>(),
                        Strokes = new StrokeCollection()
                    };
                }
                ReloadCanvas(pageInfo);
            }
            else
            {
                ClearCanvas();
            }
        }
        


        /// <summary>
        /// 从实体加载, 更新分页控件
        /// </summary>
        /// <param name="binder"></param>
        //private void LoadFromEntity(DrawingBinder binder)
        //{
        //    if (binder != null)
        //    {
        //        ucPage.PageCount = binder.PageCount;
        //        ucPage.PageIndex = binder.CurrentPageIndex;

        //        GotoPage(binder, binder.CurrentPageIndex);
        //    }
        //}


        /// <summary>
        /// 接收并处理外部命令
        /// </summary>
        /// <param name="obj"></param>
        //private void DoExternalJob(UserCanvasCommand obj)
        //{
        //    var parentWindow = Window.GetWindow(this);

        //    // 导入
        //    if (obj.Cmd == 0)
        //    {
        //        // 只导入当前页的
        //        OpenFileDialog fileDialog = new OpenFileDialog();
        //        fileDialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
        //        fileDialog.RestoreDirectory = true;
        //        fileDialog.Filter = "wbb|*.wbb";
        //        if (fileDialog.ShowDialog(parentWindow) == true)
        //        {
        //            DrawingBinder binder = null;
        //            using (var s = new UniversalSerializer(fileDialog.FileName))
        //            {
        //                binder = s.Deserialize() as DrawingBinder;
        //            }

        //            if (binder != null)
        //            {
        //                LoadFromEntity(binder);
        //            }
        //        }

        //    }
        //    // 导出
        //    else if (obj.Cmd == 1)
        //    {
        //        // 先保存到数据库(只保存当前页, 其他页的在翻页时保存), 再从数据库读取所有页的数据, 最后保存到文件
        //        var binder = SaveCurrentPageToDb();
        //        binder.PageData = pageSvc.Load(binder.Id);

        //        SaveFileDialog sfd = new SaveFileDialog();
        //        sfd.RestoreDirectory = true;
        //        sfd.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();

        //        //todo:
        //        string lessonName = "";//ObjectManager.Instance.DataCenter.TeachingService.CurrentClassLog.CustomClassName;
        //        sfd.FileName = $"{lessonName}.wbb";
        //        sfd.Filter = "wbb|*.wbb";
        //        if (sfd.ShowDialog(parentWindow) == true)
        //        {
        //            using (var s = new UniversalSerializer(sfd.FileName))
        //            {
        //                s.Serialize(binder);
        //            }

        //            //DialogHost.Show(new SimpleMessageDialog
        //            //{
        //            //    Message = { Text = "保存成功" }
        //            //}, "RootDialog");
        //        }
        //    }
        //}


        #endregion


        /// <summary>
        /// 隐藏底部工具栏
        /// </summary>
        private void HideToolbar()
        {
            CanvasToolPanel.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 恢复底部工具栏
        /// </summary>
        private void ShowToolbar()
        {
            CanvasToolPanel.Visibility = Visibility.Visible;
        }

        #endregion

        #region 枚举

        /// <summary>
        /// 表示底部工具栏的状态
        /// </summary>
        public enum ToolbarStatus
        {
            None,
            Select,
            Ink,
            Rubber,
            Undo,
            Shape,
            More
        }

        #endregion

        #region 嵌套类

        /// <summary>
        /// 拖动形状到白板上的交换类
        /// </summary>
        class ShapeDragBag
        {
            public Point MouseDownLocation { get; set; }
            public DependencyObject Source { get; set; }
            public IReusableShape ShapeObject { get; set; }
        }

        #endregion


    }
}
