using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 云
    /// </summary>
    public class Shape21 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M2 24.9c0 3.9 2.5 7.2 5.8 7.8l1.2 0.1h11.4l0.2 0l11.4 0l1.2-0.1c3.3-0.6 5.8-3.9 5.8-7.8  c0-3.4-1.9-6.4-4.6-7.5c-0.1-6.8-5-12.3-10.9-12.3c-4.3 0-8.1 2.9-9.8 7c-0.8-1.1-2.1-1.7-3.4-1.7c-2.5 0-4.6 2.4-4.6 5.3  c0 0.8 0.1 1.5 0.4 2.1C3.5 19.1 2 21.8 2 24.9L2 24.9z M7.7 18.5l-0.6-1.7c-0.1-0.3-0.2-0.7-0.2-1c0-1.4 1.2-3.5 3.1-3.5  c0.7 0 1.4 0.4 1.9 0.9l1.8 1.5l1.5-2.4c1.7-3 4.7-5.1 8-5.1c4.7 0 9.3 5.2 9.3 10.4v1.9l1.6 0.5c1.8 0.7 3.1 2.7 3.1 4.8  c0 2.5-2.2 5.7-4.4 6.1H20.5v0h-0.2v0H9l-0.8-0.1c-2.2-0.4-4.7-3.5-4.7-6C3.6 22.8 5 19.7 7.7 18.5L7.7 18.5z M7.7 18.5");
        }
    }
}
