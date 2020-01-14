using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whiteboard.Isolated.Interface;

namespace Whiteboard.Isolated.Compose
{
    /// <summary>
    /// 页面持久化
    /// </summary>
    class PagePersistenceParts : IPartImportsSatisfiedNotification
    {
        #region 需要被填充的对象

        /// <summary>
        /// IPagePersistence接口的实现对象
        /// </summary>
        [Import(typeof(IPagePersistence))]
        public IPagePersistence PagePersistenceImplment { get; set; }

        #endregion

        #region 字段

        CompositionContainer _container { get; set; }

        #endregion

        #region 构造

        /// <summary>
        /// 实例化
        /// </summary>
        PagePersistenceParts()
        {
            _container = this.ComposeCurrentDir("*Isolated*.dll");

        }

        #endregion

        #region IPartImportsSatisfiedNotification成员

        public void OnImportsSatisfied()
        {
            //IPagePersistence t = _container.GetExportedValue<IPagePersistence>();
        }

        #endregion

        #region 单例实现

        static PagePersistenceParts Singleton = null;
        public static PagePersistenceParts Compose()
        {
            if (Singleton == null)
            {
                Singleton = new PagePersistenceParts();
            }

            return Singleton;
        }

        #endregion
    }
}
