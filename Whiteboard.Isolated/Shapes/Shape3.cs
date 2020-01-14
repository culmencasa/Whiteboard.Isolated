using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 单箭头
    /// </summary>
    public class Shape3 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M29 5.2l5.9 5.9L37 3L29 5.2z M3 35.6L4.4 37L32.5 8.8l-1.4-1.4L3 35.6z");
        }
    }
}
