using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 平等四边形
    /// </summary>
    public class Shape11 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M30.7 34.8H0.3L9.3 5.3h30.4L30.7 34.8z M2.8 32.9h26.6l7.8-25.8H10.6L2.8 32.9z");
        }
    }
}
