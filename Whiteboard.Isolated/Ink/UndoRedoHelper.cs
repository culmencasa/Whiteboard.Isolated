using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;

namespace Whiteboard.Isolated.Ink
{
    public class UndoRedoHelper
    {
        #region 常量

        const int CAPACITY = 50;

        #endregion

        #region 字段

        private Stack<UndoRedo> _undoStacks = new Stack<UndoRedo>(CAPACITY);

        private Stack<UndoRedo> _redoStacks = new Stack<UndoRedo>(CAPACITY);

        private InkCanvas _canvas = null;

        private int _editingOperationCount;

        #endregion

        #region 构造

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="canvas"></param>
        public UndoRedoHelper(InkCanvas canvas)
        {
            _canvas = canvas;
            _canvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
            _canvas.MouseUp += _canvas_MouseUp;
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 画笔改变后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
        {
            if (StopTrackingEvent)
            {
                return;
            }

            // Stack会自动增长容量，所以超出时把底部的排出，效率有点低...
            if (_undoStacks.Count >= CAPACITY)
            {
                _undoStacks.Reverse();
                while (_undoStacks.Count >= CAPACITY)
                {
                    _undoStacks.Pop();
                }
                _undoStacks.Reverse();
            }

            UndoRedo previousItem = HasUndo ? _undoStacks.Peek() : null;
            UndoRedo changingItem = new UndoRedo()
            {
                LastAction = _canvas.EditingMode,
                AddedStrokes = new StrokeCollection(e.Added),
                RemovedStrokes = new StrokeCollection(e.Removed),
                ActionCount = _editingOperationCount
            };

            if (NeedMerge(previousItem, changingItem))
            {
                Merge(previousItem, changingItem);
            }
            else
            {
                _undoStacks.Push(changingItem);
            }

            if (HasRedo)
            {
                _redoStacks.Clear();
            }
        }

        /// <summary>
        /// 鼠标抬起时算一笔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _canvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _editingOperationCount++;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 是否可撤销
        /// </summary>
        public bool HasUndo
        {
            get
            {
                return _undoStacks?.Count > 0;
            }
        }

        /// <summary>
        /// 是否可重做
        /// </summary>
        public bool HasRedo
        {
            get
            {
                return _redoStacks?.Count > 0;
            }
        }

        /// <summary>
        /// 暂停事件响应
        /// </summary>
        public bool StopTrackingEvent { get; private set; }

        #endregion

        #region 公开方法

        /// <summary>
        /// 撤消
        /// </summary>
        public void Undo()
        {
            if (!HasUndo)
                return;

            StopTrackingEvent = true;

            UndoRedo info = _undoStacks.Pop();

            try
            {
                _canvas.Strokes.Remove(info.AddedStrokes);
                _canvas.Strokes.Add(info.RemovedStrokes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            this._redoStacks.Push(info);

            StopTrackingEvent = false;


        }

        /// <summary>
        /// 重做
        /// </summary>
        public void Redo()
        {
            if (!HasRedo)
                return;


            StopTrackingEvent = true;

            UndoRedo info = _redoStacks.Pop();

            try
            {
                _canvas.Strokes.Add(info.AddedStrokes);
                _canvas.Strokes.Remove(info.RemovedStrokes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            _undoStacks.Push(info);

            StopTrackingEvent = false;
        }

        public void Reset()
        {
            _undoStacks.Clear();
            _redoStacks.Clear();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 是否需要合并(只针对按点擦除)
        /// </summary>
        /// <param name="previousItem"></param>
        /// <param name="nextItem"></param>
        /// <returns></returns>
        private bool NeedMerge(UndoRedo previousItem, UndoRedo nextItem)
        {
            if (previousItem == null || nextItem == null ||
                nextItem.LastAction != previousItem.LastAction ||
                nextItem.ActionCount != previousItem.ActionCount)
            {
                return false;
            }

            if (previousItem.LastAction != InkCanvasEditingMode.EraseByPoint)
            {
                return false;
            }
            if (nextItem.LastAction != InkCanvasEditingMode.EraseByPoint)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="previousItem"></param>
        /// <param name="nextItem"></param>
        /// <returns></returns>
        private bool Merge(UndoRedo previousItem, UndoRedo nextItem)
        {

            foreach (Stroke doomed in nextItem.RemovedStrokes)
            {
                if (previousItem.AddedStrokes.Contains(doomed))
                {
                    previousItem.AddedStrokes.Remove(doomed);
                }
                else
                {
                    previousItem.RemovedStrokes.Add(doomed);
                }
            }
            previousItem.AddedStrokes.Add(nextItem.AddedStrokes);

            return true;
        }

        #endregion
    }
}
