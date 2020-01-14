using System.Collections.Generic;

namespace Whiteboard.Isolated.Interface
{
    public interface IDrawingBinder
    {
        string ClassLogBusinessId { get; set; }
        int CurrentPageIndex { get; set; }
        int PageCount { get; set; }
        List<IDrawingPage> PageData { get; set; }
    }
}