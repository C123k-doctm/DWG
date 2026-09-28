using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Teigha.Geometry;

namespace TeighaCad_Bll
{
    public static  class Function
    {
        #region 获取点与点之间的距离

        /// <summary>
        /// 获取点与点之间的距离
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static double dis(PointF first, PointF second)
        {
            double dx, dy;
            dx = Math.Abs(first.X - second.X);
            dy = Math.Abs(first.Y - second.Y);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double dis(Point3d first, Point3d second)
        {
            return dis(new PointF((float)first.X, (float)first.Y), new PointF((float)second.X, (float)second.Y));
        }

        public static double dis(Point first, Point second)
        {
            return dis(new PointF((float)first.X, (float)first.Y), new PointF((float)second.X, (float)second.Y));
        }
        #endregion

        #region 已知三点坐标，求夹角

        //已知三点坐标，求夹角
        public static double Angle(Point3d cen, Point3d first, Point3d second)
        {
            double M_PI = Math.PI;
            double ma_x = first.X - cen.X;
            double ma_y = first.Y - cen.Y;
            double mb_x = second.X - cen.X;
            double mb_y = second.Y - cen.Y;
            double v1 = (ma_x * mb_x) + (ma_y * mb_y);
            double ma_val = Math.Sqrt(ma_x * ma_x + ma_y * ma_y);
            double mb_val = Math.Sqrt(mb_x * mb_x + mb_y * mb_y);
            double cosM = v1 / (ma_val * mb_val);
            double angleAMB = Math.Acos(cosM) * 180 / M_PI;
            return angleAMB;
        }

        public static double Angle(Point cen, Point first, Point second)
        {
            return Angle(new Point3d(cen.X, cen.Y, 0), new Point3d(first.X, first.Y, 0), new Point3d(second.X, second.Y, 0));
        }

        #endregion
    }
}
