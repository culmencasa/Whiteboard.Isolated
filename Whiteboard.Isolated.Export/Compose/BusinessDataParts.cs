using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Whiteboard.Isolated.DataAccess;

namespace Whiteboard.Isolated.Export.Compose
{
    /// <summary>
    /// 业务
    /// </summary>
    public class BusinessDataParts : IPartImportsSatisfiedNotification
    {
        #region 字段

        #endregion

        #region 被填充的成员

        [Import("UserBusinessId")]
        public string UserBusinessId
        {
            get;
            set;
        }

        #endregion

        #region 静态属性

        public static BusinessDataParts Singleton { get
            {
                if (_singleton == null)
                {
                    _singleton = new BusinessDataParts();
                }
                return _singleton;
            }
        }

        #endregion

        #region 静态字段

        private static BusinessDataParts _singleton = null;
        private static CompositionContainer container = null;

        #endregion

        #region 静态构造

        static BusinessDataParts()
        {
        }

        #endregion

        #region 静态方法

        public static void Compose()
        {
            AssemblyCatalog ownLib = new AssemblyCatalog(typeof(BusinessService).Assembly);
            AssemblyCatalog mainApp = new AssemblyCatalog(Assembly.GetEntryAssembly());

            var catalog = new AggregateCatalog(ownLib, mainApp); 

            container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(Singleton);
        }

        #endregion

        #region IPartImportsSatisfiedNotification

        public void OnImportsSatisfied()
        {
            if (container != null)
            {
                //var t = container.GetExportedValue<string>("UserId");
            }
        }

        #endregion
    }
}
