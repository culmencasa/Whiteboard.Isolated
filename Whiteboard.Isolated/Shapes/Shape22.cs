using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 书签
    /// </summary>
    public class Shape22 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M5 3v34l8.1-5.1l6.9 4.4l6.9-4.4L35 37V3H5z M32.7 32.9l-5.8-3.7L20 33.6l-6.9-4.4l-5.8 3.7V5.2h25.4V32.9z   M32.7 32.9");
        }
    }
}
