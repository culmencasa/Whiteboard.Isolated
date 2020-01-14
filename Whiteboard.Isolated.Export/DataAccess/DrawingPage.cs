using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whiteboard.Isolated.Interface;

namespace Whiteboard.Isolated.DataAccess
{
    /// <summary>
    /// 表示一页的绘图数据
    /// </summary>
    public class DrawingPage : EntityBase, IDrawingPage
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public int BinderId { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 形状数据
        /// </summary>
        public byte[] Data { get; set; }
    }
}
