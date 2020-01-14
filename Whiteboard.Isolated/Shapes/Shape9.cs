using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 三角
    /// </summary>
    public class Shape9 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M40.1 36.5H1.5V2.3L40.1 36.5z M3.5 34.5h31.4L3.5 6.7V34.5z");
        }
    }
}
