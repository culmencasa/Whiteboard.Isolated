using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Ink;

namespace Whiteboard.Isolated.Ink
{
    public class UndoRedo
    {
        public InkCanvasEditingMode LastAction { get; set; }

        public StrokeCollection AddedStrokes { get; set; } = new StrokeCollection();

        public StrokeCollection RemovedStrokes { get; set; } = new StrokeCollection();

        public int ActionCount { get; set; }
    }

    
}
