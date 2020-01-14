using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 虚线
    /// </summary>
    public class Shape2 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M35.1 3.5l-5.7 5.7l1.4 1.4l5.7-5.7L35.1 3.5z M20.7 17.8l1.4 1.4l5.7-5.7l-1.4-1.4L20.7 17.8z M12.1 26.5    l1.4 1.4l5.7-5.7l-1.4-1.4L12.1 26.5z M3.5 35.1l1.4 1.4l5.7-5.7l-1.4-1.4L3.5 35.1z");
        }
    }
}
