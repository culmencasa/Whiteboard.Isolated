using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace Whiteboard.Isolated.UserCanvas
{
    public class StrokeMoveThumb : Thumb
    {
        public StrokeMoveThumb()
        {
            //this.DataContextChanged += StrokeMoveThumb_DataContextChanged;
            //this.DragStarted += StrokeMoveThumb_DragStarted;
            //this.DragDelta += StrokeMoveThumb_DragDelta;
            //this.DragCompleted += StrokeMoveThumb_DragCompleted;

            //this.Loaded += StrokeMoveThumb_Loaded;
        }

        private void StrokeMoveThumb_Loaded(object sender, RoutedEventArgs e)
        {

            host = this.Tag as StrokeSelectionFrame;

        }

        private void StrokeMoveThumb_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            _strokes = e.NewValue as StrokeCollection;
        }

        private double dragOffSetX; 
        private double dragOffSetY;

        StrokeCollection _strokes;
        Rect _strokeBounds = Rect.Empty;

        UserControl host;



        private void StrokeMoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            dragOffSetX = 0;
            dragOffSetY = 0;
            Point curPos = Mouse.GetPosition(null);

            dragOffSetX = curPos.X;
            dragOffSetY = curPos.Y;
        }

        private void StrokeMoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        { 
            Rect bounds = _strokes.GetBounds();


            dragOffSetX += e.HorizontalChange;
            dragOffSetY += e.VerticalChange;
            //lassoStroke.Transform(new Matrix(1, 0, 0, 1, offsetX, offsetY), false);
            //_strokes.Transform(new Matrix(1, 0, 0, 1, offsetX, offsetY), false);
            //_strokes.Transform(new Matrix(1, 0, 0, 1, e.HorizontalChange, e.VerticalChange), false);

            Matrix matrix = new Matrix();
            matrix.TranslatePrepend(e.HorizontalChange, e.VerticalChange);
            _strokes.Transform(matrix, false);

            //_strokes.Transform(new Point(e.HorizontalChange, e.VerticalChange));

            //host.Transform(new Matrix(1, 0, 0, 1, e.HorizontalChange, e.VerticalChange), false);
            //host.InvalidateVisual();
        }
        private void StrokeMoveThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            //_strokes.Transform(new Matrix(1, 0, 0, 1, -dragOffSetX, -dragOffSetY), false);
        }
    }
}
