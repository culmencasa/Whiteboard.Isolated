using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 五边形
    /// </summary>
    public class Shape14 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M31 37L8.6 36.8L1.8 15.6L20.1 2.8l18.1 13.2L31 37z M10 34.8L29.6 35l6.2-18.3L20.1 5.2L4.2 16.4L10 34.8z");
        }
    }
}
