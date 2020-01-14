using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whiteboard.Isolated.Ink
{
    /// <summary>
    /// 覆盖模式 - 用于替换EditingModes
    /// </summary>
    public enum CoveredModes
    {
        None,
        SelectByRectangle,
        Ink,
        RubberByPoint,
        RubberByStroke,
        ShapeStamp,
    }
}
