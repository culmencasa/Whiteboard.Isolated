using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Whiteboard.Isolated.Compose
{
    static class ObjectExt
    {
        /// <summary>
        /// 组装指定目录下的部件, 默认组装当前工作目录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filePattern"></param>
        /// <returns></returns>
        public static CompositionContainer ComposeCurrentDir<T>(this T obj, string filePattern) where T : class
        {
            DirectoryCatalog catalog = null; 
            if (!string.IsNullOrEmpty(filePattern))
            {
                catalog = new DirectoryCatalog(".", filePattern);
            }
            else
            {
                catalog = new DirectoryCatalog(".");
            }

            CompositionContainer container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(obj);
            }
            catch (CompositionException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return container;
        }

        /// <summary>
        /// 组装程序集的部件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static CompositionContainer ComposeAssembly<T>(this T obj, Assembly assembly) where T : class
        {
            AssemblyCatalog assemblyCat = new AssemblyCatalog(assembly);
            CompositionContainer container = new CompositionContainer(assemblyCat);

            try
            {
                container.ComposeParts(obj);
            }
            catch (CompositionException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return container;
        }
    }
}
