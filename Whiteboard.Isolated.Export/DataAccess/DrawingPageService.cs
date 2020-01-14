using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whiteboard.Isolated.Model;

namespace Whiteboard.Isolated.DataAccess
{
    public class DrawingPageService : BusinessService
    {
        public void Save(DrawingPage entity)
        {
            UsingUserDb((context) => {
                var page = context.DrawingPages.FirstOrDefault(p => p.BinderId == entity.BinderId && p.PageIndex == entity.PageIndex);

                if (page == null)
                {
                    context.DrawingPages.Add(entity);
                }
                else
                {
                    context.DrawingPages.Remove(page);
                    context.DrawingPages.Add(entity);
                }

                context.SaveChanges();
            });
        }

        public DrawingPage Load(int binderId, int pageIndex)
        {
            DrawingPage result = null;

            UsingUserDb((context) => {

                result = context.DrawingPages.FirstOrDefault(p=> p.BinderId == binderId && p.PageIndex == pageIndex); 
            });

            return result;
        }


        public List<DrawingPage> Load(int binderId)
        {
            List<DrawingPage> result = null;

            UsingUserDb((context) => {

                result = context.DrawingPages.Where(p => p.BinderId == binderId).ToList();
            });

            return result;
        }

    }
}
