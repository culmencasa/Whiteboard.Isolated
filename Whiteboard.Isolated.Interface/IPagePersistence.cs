using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whiteboard.Isolated.Interface
{
    public interface IPagePersistence
    {
        /// <summary>
        /// 一次会话的Id. 这里指一节课
        /// </summary>
        string ClassLogBusinessId { get; set; }
        
        /// <summary>
        /// 控件对象
        /// </summary>
        ICanvasBoard CanvasControl { get; set; }

        /// <summary>
        /// 保存当前页到数据库
        /// </summary>
        void SavePage();

        /// <summary>
        /// 从数据库加载一页的数据
        /// </summary>
        /// <param name="pageIndex">指定页</param>
        void LoadPage(int pageIndex);

        /// <summary>
        /// 加载最后一次页
        /// </summary>
        void LoadLastTimePage();
    }
}
