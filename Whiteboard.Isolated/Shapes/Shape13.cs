using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 梯形
    /// </summary>
    public class Shape13 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M37.8 37.5H2.5l4-35h26.1L37.8 37.5z M4.7 35.5h30.7l-4.6-31H8.3L4.7 35.5z");
        }
    }
}
