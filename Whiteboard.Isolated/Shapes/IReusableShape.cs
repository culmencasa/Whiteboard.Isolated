using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Whiteboard.Isolated.Shapes
{
    public interface IReusableShape
    {
        IReusableShape Clone();

        Geometry GetGeometry();
    }
}
