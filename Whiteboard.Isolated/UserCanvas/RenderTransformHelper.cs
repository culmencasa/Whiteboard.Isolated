using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Whiteboard.Isolated.UserCanvas
{
    public static class RenderTransformHelper
    {

        public static TransformGroup GetTransformGroup(this UIElement uIElement)
        {
            TransformGroup transformGroup = null;

            // 获取原样式
            Transform renderTransform = uIElement.RenderTransform;
            if (renderTransform == Transform.Identity)
            {
                // 不存在原样式, 创建一个空组
                transformGroup = new TransformGroup();
                uIElement.RenderTransform = transformGroup;
            }
            else
            {
                // 两种情况 : Transform是TransformGroup类型, 或者是其他类型
                if (uIElement.RenderTransform is TransformGroup)
                {
                    transformGroup = uIElement.RenderTransform as TransformGroup;
                }
                else
                {
                    transformGroup = new TransformGroup();
                    transformGroup.Children.Add(renderTransform);

                    uIElement.RenderTransform = transformGroup;
                }
            }

            return transformGroup;
        }


        public static void ApplyTransform(this UIElement element, Transform value)
        {
            var tg = GetTransformGroup(element);
            tg.Children.Add(value);
        }

    }
}
