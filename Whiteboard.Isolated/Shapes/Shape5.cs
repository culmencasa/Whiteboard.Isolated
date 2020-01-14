using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    /// <summary>
    /// 方框
    /// </summary>
    public class Shape5 : WiredShape
    {
        public override Geometry GetGeometry()
        {
            return Geometry.Parse("M38 38H2V2h36V38z M4 36h32V4H4V36z");
        }
    }
}
