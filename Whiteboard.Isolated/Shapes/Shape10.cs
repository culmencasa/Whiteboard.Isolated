using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 等边三角
    /// </summary>
    public class Shape10 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M38.7 36.5H1.3L20 2.4L38.7 36.5z M4.7 34.5h30.6L20 6.6L4.7 34.5z");
        }
    }
}
