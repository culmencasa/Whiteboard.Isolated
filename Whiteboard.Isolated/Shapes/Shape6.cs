using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 圆角框
    /// </summary>
    public class Shape6 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M32 38H8c-3.4 0-6-2.6-6-6V8c0-3.4 2.6-6 6-6h24c3.4 0 6 2.6 6 6v24C38 35.4 35.4 38 32 38z M8 4  C5.8 4 4 5.8 4 8v24c0 2.2 1.8 4 4 4h24c2.2 0 4-1.8 4-4V8c0-2.2-1.8-4-4-4H8z");
        }
    }
}
