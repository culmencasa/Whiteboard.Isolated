using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Whiteboard.Isolated.UserCanvas
{ 
    public class StrokeRotateThumb : Thumb
    {
        public StrokeRotateThumb()
        {
            Loaded += StrokeRotateThumb_Loaded;
        }

        private void StrokeRotateThumb_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}
