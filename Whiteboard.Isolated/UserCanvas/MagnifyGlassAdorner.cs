using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Whiteboard.Isolated.UserCanvas
{

    public class MagnifyGlassAdorner : Adorner
    {
        #region 构造

        public MagnifyGlassAdorner(UIElement adornedElement) : base(adornedElement)
        {
            SnapsToDevicePixels = true;

            _frame = new MagnifyGlassFrame();
            _frame.Playground = this;

            _toolbar = new MagnifyToolbar();
            _toolbar.HorizontalAlignment = HorizontalAlignment.Center;
            _toolbar.VerticalAlignment = VerticalAlignment.Bottom;
            _toolbar.DimTheLightHandler += _toolbar_DimTheLightHandler;
            _toolbar.CloseHandler += _toolbar_CloseHandler;

            this._visualChildren = new VisualCollection(this);
            this._visualChildren.Add(_frame);
            this._visualChildren.Add(_toolbar);
        }

        #endregion

        #region 工具栏事件处理

        /// <summary>
        /// 关灯
        /// </summary>
        private void _toolbar_DimTheLightHandler(IconControl button)
        {
            var magnifyGlass = AdornedElement as MagnifyGlass;
            var curtain = magnifyGlass.Parent as Canvas;

            // 判断当前是否开灯
            // 默认_lightOut是false(表示开灯)
            if (_lightOut == false)
            {
                _lightOut = true;
                curtain.Tag = curtain.Background;
                curtain.Background = Brushes.Black;

                // 按钮文字改为开灯
                button.IconText = "开灯";
            }
            else
            {
                _lightOut = false;
                curtain.Background = (Brush)curtain.Tag;

                // 按钮文字改为关灯
                button.IconText = "关灯";
            }
        }

        /// <summary>
        /// 关闭放大镜
        /// </summary>
        private void _toolbar_CloseHandler(IconControl button)
        {
            var adornerParent = this.Parent as AdornerLayer;

            var magnifyGlass = AdornedElement as MagnifyGlass;
            var curtain = magnifyGlass.Parent as Canvas;

            // 恢复开灯状态
            curtain.Background = (Brush)curtain.Tag;
            // 删除放大镜
            curtain.Children.Remove(magnifyGlass);
            // 删除Adorner层
            adornerParent.Remove(this);

            Closing?.Invoke(this, EventArgs.Empty);
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
                            (_frame.ActualWidth - _toolbar.Width) / 2 - 5,
                            arrangeBounds.Height + 20,
                            200,
                            80));

            var innerControl = AdornedElement as MagnifyGlass;
            _frame.Center = new Point(innerControl.Width / 2, innerControl.Height / 2);

            return arrangeBounds;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (_visualChildren == null)
                return null;

            return this._visualChildren[index];
        }

        #endregion

        #region 公共方法

        #endregion

        public event EventHandler Closing;

        #region 属性

        public MagnifyGlassFrame Frame { get => _frame; set => _frame = value; }

        #endregion

        #region 字段

        private VisualCollection _visualChildren;
        private MagnifyGlassFrame _frame;
        private MagnifyToolbar _toolbar;
        /// <summary>
        /// 表示放大镜幕布的开关灯状态.
        /// false为开灯, true为关灯. 默认值是false.
        /// </summary>
        private bool _lightOut;


        #endregion
    }
}
