using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Whiteboard.Isolated.Export.Compose;
using Whiteboard.Isolated.Model;

namespace Whiteboard.Isolated.DataAccess
{
    /// <summary>
    /// Service基类
    /// </summary>
    public abstract class BusinessService 
    {
        /// <summary>
        /// 执行会话数据相关操作
        /// </summary>
        /// <param name="action"></param>
        public void UsingUserDb(Action<CanvasDbContext> action)
        {
            string userBusinessId = BusinessDataParts.Singleton?.UserBusinessId;
            if (userBusinessId == null || string.IsNullOrEmpty(userBusinessId))
            {
                throw new ArgumentException("需要指定UserBusinessId");
            }

            using (var context = new CanvasDbContext(userBusinessId))
            {
                action(context);
            }
        }
    }

}
