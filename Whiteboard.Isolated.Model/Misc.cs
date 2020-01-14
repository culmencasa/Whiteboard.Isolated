using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whiteboard.Isolated.Model
{
    public class Misc
    {
        /// <summary>
        /// 获取appSetting值
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public static string GetUserDbName()
        {
            return ConfigurationManager.AppSettings["userdb"];
        }
    }
}
