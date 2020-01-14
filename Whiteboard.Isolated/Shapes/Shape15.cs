using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 五角星
    /// </summary>
    public class Shape15 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M37 16l-11.7-1.8L20 3l-5.3 11.2L3 16l8.5 8.7L9.5 37L20 31.2L30.5 37l-2-12.3L37 16z M20 28.7l-7.9 4.4  l1.5-8.9l-6.8-6.9l9.2-1.2l4-8.5l4 8.5l9.2 1.3l-6.7 6.9l1.4 8.9L20 28.7z M20 28.7");
        }
    }
}
