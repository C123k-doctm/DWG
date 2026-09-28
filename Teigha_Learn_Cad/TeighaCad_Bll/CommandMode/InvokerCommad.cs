using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeighaCad_Bll
{
    /// <summary>
    /// 请求者(Invoker)：持有一个命令对象，有一个行动方法，在某个时间点调用命令对象的 execute() 方法，将请求付诸实行。
    /// </summary>
    public class InvokerCommad
    {
        public Stack<Command> undoStack = new Stack<Command>();
        public Stack<Command> redoStack = new Stack<Command>();

        public delegate void InvokerUpdateStack();

        /// <summary>
        /// 操作发生改变时处理
        /// </summary>
        public event InvokerUpdateStack ChangeStack;
        protected virtual void OnNumChanged()
        {
            if (ChangeStack != null)
            {
                ChangeStack(); /* 事件被触发 */
            }
        }

        public void SetCommad(Command _com)
        {
            undoStack.Push(_com);
            //添加操作时，表示之前执行的操作已经废弃
            foreach (var item in redoStack)
                item.clear();
            redoStack.Clear();
            OnNumChanged();
        }

        /// <summary>
        /// 撤销
        /// </summary>
        public void UndoStack()
        {
            if (undoStack.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("无可撤销动作！");
                return;
            }

            Command com = undoStack.Pop();
            com.undo();
            redoStack.Push(com);
            OnNumChanged();
        }

        /// <summary>
        /// 执行
        /// </summary>
        public void RedoStack()
        {
            if (redoStack.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("无可执行动作！");
                return;
            }

            Command com = redoStack.Pop();
            com.execute();

            undoStack.Push(com);
            OnNumChanged();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            try
            {
                foreach (var item in undoStack)
                {
                    item.clear();
                }
                undoStack.Clear();

                foreach (var item in redoStack)
                {
                    item.clear();
                }
                redoStack.Clear();
            }
            catch { }
        }
    }
}
