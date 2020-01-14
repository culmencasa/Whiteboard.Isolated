using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    public class Shape1 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M3.5 34.9L34.9 3.5l1.6 1.6L5.1 36.5L3.5 34.9z");
        }
    }
}
