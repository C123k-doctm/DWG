using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Teigha.Geometry;

namespace TeighaCad_Bll
{
    /// <summary>
    /// 视图缩放操作时操作类（包含撤销和执行操作） 具体命令实现了命令接口，定义了动作和接收者之间的绑定关系。
    /// 主要用于视图的缩小和放大
    /// </summary>
    public class ReceiverViews : Command
    {
        //start 接收者(Receiver)：接收者知道如何进行必要的动作，实现这个请求。任何类都可以当接收者 
        Teigha.GraphicsSystem.LayoutHelperDevice helperDevice;
        //end 

        Point3d Position;
        Point MovePoint;
        double Number;

        public ReceiverViews(double _number,Point3d _position, Point _movePoint ,ref Teigha.GraphicsSystem.LayoutHelperDevice _helperDevice)
        {
            Number = _number;
            Position = _position;
            MovePoint = _movePoint;
            helperDevice = _helperDevice;
        }

        public void execute()
        {
            using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
            {
                // camera position in world coordinates 摄像机在世界坐标中的位置
                Point3d pos = pView.Position;
                // TransformBy() returns a transformed copy TransformBy()返回转换后的副本
                pos = pos.TransformBy(pView.WorldToDeviceMatrix);
                int vx = (int)pos.X;
                int vy = (int)pos.Y;
                vx = MovePoint.X - vx;
                vy = MovePoint.Y - vy;
                // we move point of view to the mouse location, to create an illusion of scrolling in/out there
                //我们将视图移动到鼠标位置，以创建一个在那里滚动的错觉
                dolly(pView, -vx, -vy);
                //注意，我们实际上忽略了delta值(符号足以说明问题)
                // note that we essentially ignore delta value (sign is enough for illustrative purposes)
                pView.Zoom(Number);
                dolly(pView, vx, vy);
                //ReceiverView Receive = new ReceiverView(new Point(e.X, e.Y), new Point((int)pos.X,(int)pos.Y), num, ref database, ref helperDevice);
                //InvokerCom.SetCommad(Receive);
            }
            if (helperDevice != null)
            {
                helperDevice.Update();
                helperDevice.Invalidate();
            }
        }

        public void undo()
        {
            using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
            {
                // camera position in world coordinates 摄像机在世界坐标中的位置
                Point3d pos = pView.Position;
                // TransformBy() returns a transformed copy TransformBy()返回转换后的副本
                pos = pos.TransformBy(pView.WorldToDeviceMatrix);
                int vx = (int)pos.X;
                int vy = (int)pos.Y;
                vx = MovePoint.X - vx;
                vy = MovePoint.Y - vy;
                // we move point of view to the mouse location, to create an illusion of scrolling in/out there
                //我们将视图移动到鼠标位置，以创建一个在那里滚动的错觉
                dolly(pView, -vx, -vy);
                //注意，我们实际上忽略了delta值(符号足以说明问题)
                // note that we essentially ignore delta value (sign is enough for illustrative purposes)
                pView.Zoom(1/Number);
                dolly(pView, vx, vy);
                //ReceiverView Receive = new ReceiverView(new Point(e.X, e.Y), new Point((int)pos.X,(int)pos.Y), num, ref database, ref helperDevice);
                //InvokerCom.SetCommad(Receive);
            }
            if (helperDevice != null)
            {
                helperDevice.Update();
                helperDevice.Invalidate();
            }
        }

        private void dolly(Teigha.GraphicsSystem.View pView, int x, int y)
        {
            Vector3d vec = new Vector3d(-x, -y, 0.0);
            vec = vec.TransformBy((pView.ScreenMatrix * pView.ProjectionMatrix).Inverse());
            pView.Dolly(vec);
        }

        public void clear()
        {}
    }
}
