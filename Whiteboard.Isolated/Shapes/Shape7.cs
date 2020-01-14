using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 圆形
    /// </summary>
    public class Shape7 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M20 37.5c-9.6 0-17.5-7.9-17.5-17.5S10.4 2.5 20 2.5S37.5 10.4 37.5 20S29.6 37.5 20 37.5z M20 4.5  c-8.5 0-15.5 7-15.5 15.5s7 15.5 15.5 15.5s15.5-7 15.5-15.5S28.5 4.5 20 4.5z");
        }
    }
}
