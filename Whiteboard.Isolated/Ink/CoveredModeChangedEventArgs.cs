using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whiteboard.Isolated.Ink
{
    public class ModeChangedEventArgs : EventArgs
    {
        public CoveredModes OldValue { get; set; }

        public CoveredModes NewValue { get; set; }
    }
}
