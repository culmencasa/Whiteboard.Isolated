using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;

namespace Whiteboard.Isolated.Ink
{
    public class SmashPageInfo
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 形状
        /// </summary>
        public List<ContentControl> Shapes { get; set; }

        /// <summary>
        /// 线条
        /// </summary>
        public StrokeCollection Strokes { get; set; }
    }
}
