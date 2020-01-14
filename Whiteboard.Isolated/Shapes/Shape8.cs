using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 吃豆
    /// </summary>
    public class Shape8 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M20 36.5c-9.1 0-16.5-7.4-16.5-16.5c0-9.1 7.4-16.5 16.5-16.5c0.5 0 1.2 0 1.8 0.1l0.8 0.2v15.7h13.9l0 1  C36.2 29.5 28.9 36.5 20 36.5z M20 5.5C12 5.5 5.5 12 5.5 20S12 34.5 20 34.5c7.5 0 13.6-5.6 14.4-13H20.6v-16  C20.4 5.5 20.2 5.5 20 5.5z");
        }
    }
}
