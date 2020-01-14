using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whiteboard.Isolated.Interface;

namespace Whiteboard.Isolated.DataAccess
{
    /// <summary>
    /// 记录每堂课的白板信息
    /// </summary>
    [Table("DrawingBinders")]
    public class DrawingBinder : EntityBase, IDrawingBinder
    {
        /// <summary>
        /// 上课记录Id
        /// </summary>
        public string ClassLogBusinessId { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int CurrentPageIndex { get; set; }

        [NotMapped]
        public List<IDrawingPage> PageData { get; set; }
    }
}
