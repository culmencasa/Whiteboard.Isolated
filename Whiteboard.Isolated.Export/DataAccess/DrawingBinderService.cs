using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whiteboard.Isolated.Model;

namespace Whiteboard.Isolated.DataAccess
{
    public class DrawingBinderService : BusinessService
    {
        public bool Exist(string classLogId)
        {
            bool exist = false;
            UsingUserDb((context) =>
            {
                int count = context.DrawingBinders.Count(p => p.ClassLogBusinessId == classLogId);
                exist = count > 0;
            });

            return exist;
        }

        public DrawingBinder Save(DrawingBinder entity)
        {
            DrawingBinder result = null;

            UsingUserDb((context) => {

                DrawingBinder binder = context.DrawingBinders.FirstOrDefault(a => a.ClassLogBusinessId == entity.ClassLogBusinessId);
                if (binder != null)
                {
                    binder.CurrentPageIndex = entity.CurrentPageIndex;
                    binder.PageCount = entity.PageCount;

                    context.SaveChanges();
                    // 返回对象
                    result = binder;
                }
                else
                {
                    context.DrawingBinders.Add(entity);
                    context.SaveChanges();

                    result = entity;
                } 
            });

            return result;
        }

        public DrawingBinder LoadById(int binderId)
        {
            DrawingBinder binder = null;

            UsingUserDb((context) =>
            {
                binder = context.DrawingBinders.FirstOrDefault(a => a.Id == binderId);
            });

            return binder;
        }

        public DrawingBinder LoadByClassLogId(string classLogBusinessId)
        {
            DrawingBinder binder = null;

            UsingUserDb((context) =>
            {
                binder = context.DrawingBinders.FirstOrDefault(a => a.ClassLogBusinessId == classLogBusinessId);
            });

            return binder;
        }


    }
}
