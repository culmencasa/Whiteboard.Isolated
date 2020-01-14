using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 闪电
    /// </summary>
    public class Shape17 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M21.8 8l-0.2 8.6l-0.1 2l2-0.1l7.1-0.3L18.2 32l0.2-8.9l0.1-2l-2 0.1l-7 0.3L21.8 8 M23.8 3L5 23.4L16.5 23  l-0.4 14L35 16.2l-11.5 0.5L23.8 3z M23.8 3");
        }
    }
}
