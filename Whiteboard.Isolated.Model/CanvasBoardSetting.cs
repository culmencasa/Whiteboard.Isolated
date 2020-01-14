using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Whiteboard.Isolated.Model
{
    /// <summary>
    /// 画板相关的外部设定类
    /// </summary>
    [Serializable]
    public class CanvasBoardSetting
    {
        /// <summary>
        /// 显示或隐藏工具栏
        /// </summary>
        public ToolbarSetting Toolbar { get; set; } = new ToolbarSetting();

        /// <summary>
        /// 显示或隐藏分页
        /// </summary>
        public PagerSetting Pager { get; set; } = new PagerSetting();

        /// <summary>
        /// 是否清空数据库
        /// </summary>
        public bool CleanDatabase { get; set; } = false;

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string UserBusinessId { get; set; } = string.Empty;
               
    }

    [Serializable]
    public class ToolbarSetting
    {
        public bool Visible { get; set; } = true;

        public bool Selector { get; set; } = true;

        public bool Pen { get; set; } = true;

        public bool Rubber { get; set; } = true;

        public bool Undo { get; set; } = true;

        public bool Shape { get; set; } = true;

        public MoreTools More { get; set; } = new MoreTools();

        public bool AutoHide { get; set; } = false;
    }

    [Serializable]
    public class MoreTools
    {
        public bool Visible { get; set; } = true;
    }


    [Serializable]
    public class PagerSetting
    {
        public bool Visible { get; set; } = true;

        public int PageIndex { get; set; } = -1;
        
        public bool AutoHide { get; set; } = false;
    }
}
