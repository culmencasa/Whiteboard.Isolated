using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whiteboard.Isolated.DataAccess;
using Whiteboard.Isolated.Export.Compose;
using Whiteboard.Isolated.Interface;

namespace Whiteboard.Isolated.Export
{ 
    [Export(typeof(IPagePersistence))]
    public class PagePersistence : IPagePersistence
    {
        #region IPagePersistencer的成员
        
        /// <summary>
        /// 一次会话的Id. 这里指一节课. 如果没有指定, 默认值Default
        /// </summary>
        public string ClassLogBusinessId { get; set; } = "Default";

        /// <summary>
        /// 画板控件
        /// </summary>
        public ICanvasBoard CanvasControl { get; set; }
        
        /// <summary>
        /// 保存当前页到数据库
        /// </summary>
        /// <returns></returns>
        public void SavePage()
        {
            if (CanvasControl == null)
            {
                throw new NullReferenceException("未能找到关联的ICanvasBoard控件.");
            }

            if (binderSvc == null)
            {
                Console.WriteLine("DrawingBinderService没有实例化");
                return;
            }
            if (pageSvc == null)
            {
                Console.WriteLine("DrawingPageService没有实例化");
                return;
            }


            // 从控件取到要保存的页数据
            byte[] pageData = CanvasControl.ExportXml();
            int pageIndex = CanvasControl.CurrentPageIndex;
            int pageCount = CanvasControl.PageCount;

            // 保存该页
            string classLogId = ClassLogBusinessId;
            var binder = binderSvc.Save(new DrawingBinder()
            {
                ClassLogBusinessId = classLogId,
                CurrentPageIndex = pageIndex,
                PageCount = pageCount
            });
            pageSvc.Save(new DrawingPage()
            {
                BinderId = binder.Id,
                PageIndex = pageIndex,
                Data = pageData
            });

            //return binder;
        }
        
        /// <summary>
        /// 加载指定页的数据
        /// </summary>
        /// <param name="newPageIndex"></param>
        public void LoadPage(int newPageIndex)
        {
            if (CanvasControl == null)
            {
                throw new NullReferenceException("未能找到关联的ICanvasBoard控件.");
            }

            if (binderSvc == null)
            {
                Console.WriteLine("DrawingBinderService没有实例化");
                return;
            }
            if (pageSvc == null)
            {
                Console.WriteLine("DrawingPageService没有实例化");
                return;
            }


            // 更新数据库的PageIndex, 指向新PageIndex, 
            DrawingBinder binder = binderSvc.LoadByClassLogId(this.ClassLogBusinessId);
            if (binder == null)
            {
                binder = binderSvc.Save(new DrawingBinder() {
                    ClassLogBusinessId = ClassLogBusinessId,
                    CurrentPageIndex = newPageIndex,
                    PageCount = newPageIndex
                });
            }
            else
            {
                binder.CurrentPageIndex = newPageIndex;
                if (newPageIndex > binder.PageCount)
                    binder.PageCount = newPageIndex;
                binderSvc.Save(binder);
            }
            CanvasControl.CurrentPageIndex = binder.CurrentPageIndex;
            CanvasControl.PageCount = binder.PageCount;


            // 读取数据库的页数据
            var newPage = pageSvc.Load(binder.Id, newPageIndex);
            // 如果没有, 创建新页
            if (newPage == null)
            {
                newPage = new DrawingPage() { BinderId = binder.Id, Data = null, PageIndex = newPageIndex };
                pageSvc.Save(newPage);

                // 如何加载序列化数据, 由外部接口实现 
                CanvasControl.ImportXml(newPage.Data);
            }
            else
            {
                // 如何加载序列化数据, 由外部接口实现 
                CanvasControl.ImportXml(newPage.Data);
            }

        }

        public void LoadLastTimePage()
        {
            if (CanvasControl == null)
            {
                throw new NullReferenceException("未能找到关联的ICanvasBoard控件.");
            }

            if (binderSvc == null)
            {
                Console.WriteLine("DrawingBinderService没有实例化");
                return;
            }
            if (pageSvc == null)
            {
                Console.WriteLine("DrawingPageService没有实例化");
                return;
            }

            DrawingBinder binder = binderSvc.LoadByClassLogId(this.ClassLogBusinessId);
            if (binder == null)
            {
                binder = binderSvc.Save(new DrawingBinder()
                {
                    ClassLogBusinessId = ClassLogBusinessId,
                    CurrentPageIndex = 1,
                    PageCount = 1
                });
            }

            CanvasControl.CurrentPageIndex = binder.CurrentPageIndex;
            CanvasControl.PageCount = binder.PageCount;


            // 读取数据库的页数据
            var newPage = pageSvc.Load(binder.Id, binder.CurrentPageIndex);
            // 如果没有, 创建新页
            if (newPage == null)
            {
                newPage = new DrawingPage() { BinderId = binder.Id, Data = null, PageIndex = binder.CurrentPageIndex };
                pageSvc.Save(newPage);

                // 如何加载序列化数据, 由外部接口实现 
                CanvasControl.ImportXml(newPage.Data);
            }
            else
            {
                // 如何加载序列化数据, 由外部接口实现 
                CanvasControl.ImportXml(newPage.Data);
            }
        }

        #endregion

        #region 字段

        private DrawingBinderService binderSvc = new DrawingBinderService();

        private DrawingPageService pageSvc = new DrawingPageService();

        #endregion

        #region 构造

        public PagePersistence()
        {
        }

        #endregion


        /// <summary>
        /// 从实体加载, 更新分页控件
        /// </summary>
        /// <param name="binder"></param>
        //private void LoadFromEntity(DrawingBinder binder)
        //{
        //    if (binder != null)
        //    {
        //        //ucPage.PageCount = binder.PageCount;
        //        //ucPage.PageIndex = binder.CurrentPageIndex;

        //        GotoPage(binder, binder.CurrentPageIndex);
        //    }
        //}

    }
}
