namespace Whiteboard.Isolated.Interface
{
    public interface IDrawingPage
    {
        int BinderId { get; set; }
        byte[] Data { get; set; }
        int PageIndex { get; set; }
    }
}