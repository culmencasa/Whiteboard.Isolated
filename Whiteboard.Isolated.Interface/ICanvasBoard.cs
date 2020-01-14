using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whiteboard.Isolated.Interface
{
    /// <summary>
    /// 表示画板(CanvasBoard)的接口
    /// </summary>
    public interface ICanvasBoard
    {
        /// <summary>
        /// 当前页索引(1开始)
        /// </summary>
        int CurrentPageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        int PageCount { get; set; }

        /// <summary>
        /// 序列化画板上的数据
        /// </summary>
        /// <returns></returns>
        byte[] ExportXml();

        /// <summary>
        /// 反序列化数据到画板上
        /// </summary>
        /// <param name="data"></param>
        void ImportXml(byte[] data);

        void SetToolbarVisibility(bool visibility);

        void SetPagerVisibility(bool visibility);

    }
}
