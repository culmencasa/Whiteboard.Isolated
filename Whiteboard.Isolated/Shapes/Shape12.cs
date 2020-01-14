using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 菱形
    /// </summary>
    public class Shape12 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M20 37.9L2.1 20L20 2.1L37.9 20L20 37.9z M4.9 20L20 35.1L35.1 20L20 4.9L4.9 20z");
        }
    }
}
