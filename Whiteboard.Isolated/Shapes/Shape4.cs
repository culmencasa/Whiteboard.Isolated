using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 双箭头
    /// </summary>
    public class Shape4 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M29.4 5.2l5.4 5.4L37 3L29.4 5.2z M31.4 7.2L7.1 31.5l-2-2L3 37l7.5-2.1l-2.1-2.1L32.7 8.5L31.4 7.2z");
        }
    }
}
