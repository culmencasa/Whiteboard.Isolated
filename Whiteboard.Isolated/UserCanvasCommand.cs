using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whiteboard.Isolated
{
    /// <summary>
    /// 用于控件外部与之交互的命令传参
    /// </summary>
    public class UserCanvasCommand
    {
        public int Cmd { get; set; }

        public object Argument { get; set; }
    }
}
