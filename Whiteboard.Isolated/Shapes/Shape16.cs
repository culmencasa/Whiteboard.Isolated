using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 心形
    /// </summary>
    public class Shape16 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M27.7 4c-3 0-5.8 1.5-7.7 3.9C18.2 5.5 15.4 4 12.3 4C6.9 4 2.5 8.7 2.5 14.6c0 3.5 1.6 5.9 2.9 7.9  c3.7 5.7 13 12.8 13.4 13.1c0.4 0.3 0.8 0.4 1.3 0.4c0.4 0 0.9-0.1 1.3-0.4c0.4-0.3 9.7-7.4 13.4-13.1c1.3-2 2.9-4.4 2.9-7.9  C37.5 8.7 33.1 4 27.7 4L27.7 4z M32.8 21.1C29.3 26.6 20 33.6 20 33.6s-9.3-7.1-12.8-12.5c-1.3-2-2.5-3.9-2.5-6.5  c0-4.5 3.4-8.2 7.7-8.2c3.1 0 5.8 2 7 4.9v0h0c0.1 0.3 0.3 0.5 0.7 0.5c0.3 0 0.6-0.2 0.7-0.5h0c1.2-2.9 3.9-4.9 7-4.9  c4.2 0 7.7 3.7 7.7 8.2C35.3 17.2 34.1 19.1 32.8 21.1L32.8 21.1z M32.8 21.1");
        }
    }
}
