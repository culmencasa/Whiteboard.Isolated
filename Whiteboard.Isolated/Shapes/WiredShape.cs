using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Whiteboard.Isolated.Shapes
{
    public abstract class WiredShape : Shape, IReusableShape
    {
        public WiredShape()
        {
            this.Stretch = System.Windows.Media.Stretch.Fill;
            this.StrokeLineJoin = PenLineJoin.Round;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return GetGeometry();
            }
        }

        public IReusableShape Clone()
        {
            Shape cloneInstance = Activator.CreateInstance(this.GetType()) as Shape;
            // 设置宽高后Fill会失效？
            //cloneInstance.Width = this.Width;
            //cloneInstance.Height = this.Height;
            cloneInstance.Stroke = this.Stroke;
            cloneInstance.StrokeThickness = this.StrokeThickness;
            cloneInstance.Fill = this.Fill;

            return cloneInstance as IReusableShape;
        }

        public abstract Geometry GetGeometry();
    }
}
