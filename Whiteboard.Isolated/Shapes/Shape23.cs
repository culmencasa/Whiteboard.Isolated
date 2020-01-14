using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 卷轴
    /// </summary>
    public class Shape23 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M31.4 3H9.6C9.5 3 9.4 3 9.3 3.1C8.5 3 7.7 3 7 3.3C5.5 4 4.6 5.5 4.5 7.2c0 0.1 0 0.2 0 0.2    c0 0.1 0 0.1 0 0.2v28.5c0 0.7 0.7 1 1.3 0.8l6.4-2.8c0.9 0.4 1.7 0.9 2.6 1.3l2 1c0.3 0.2 0.7 0.4 1.1 0.5    c0.4 0.1 0.9-0.2 1.2-0.4l2-1.1l2.8-1.5l7.3 2.9c0.5 0.2 1.1-0.3 1.1-0.9V11.2c0.6-0.1 1.2-0.4 1.8-0.8C37 8 35.2 3 31.4 3z     M30.5 34.8l-6.3-2.5c-0.8-0.3-2.2 0.8-2.9 1.1l-3.2 1.7l-2.9-1.5l-2.1-1.1c-0.5-0.2-0.8-0.5-1.4-0.2l-5.5 2.4V7.6    c0-0.1 0-0.2 0-0.2c0-0.1 0-0.1 0-0.2c0.1-2.1 2.5-3.1 4-1.7c1.6 1.5 0.3 4-1.7 4.1c-1.1 0-1.1 1.8 0 1.8c0.2 0 0.4 0 0.6-0.1    c0.1 0 0.2 0.1 0.3 0.1h19.5c0.4 0 0.9 0 1.4 0V34.8z M31.4 9.6H11.9C12.8 8.2 13 6.3 12 4.8h17.2c1.7 0 3.6-0.3 4.5 1.5    C34.4 7.8 33 9.6 31.4 9.6z");
        }
    }
}
