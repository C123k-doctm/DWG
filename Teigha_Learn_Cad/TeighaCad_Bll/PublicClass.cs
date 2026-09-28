using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeighaCad_Bll
{
    public static class PublicClass
    {
       public static  Teigha.Runtime.Services ServiceD = new Teigha.Runtime.Services();

    }

    /// <summary>
    /// 操作枚举
    /// </summary>
    public enum Operations
    {
        /// <summary>
        /// 无操作
        /// </summary>
        None = 0,

        /// <summary>
        /// 拖拽
        /// </summary>
        Drag = 1,

        /// <summary>
        /// 旋转
        /// </summary>
        Roll = 2,

        /// <summary>
        /// 选择框
        /// </summary>
        SelectFrom = 3,

        /// <summary>
        /// 移动实体
        /// </summary>
        MoveEntity = 4,

        /// <summary>
        /// 陀螺仪--立体旋转
        /// </summary>
        Gyroscope = 5,

        /// <summary>
        /// 旋转实体
        /// </summary>
        RollEntity = 6,

        /// <summary>
        /// 插入实体
        /// </summary>
        InsertEntity = 7,

        /// <summary>
        /// 画笔
        /// </summary>
        PanelDrawPen = 8,

        //批量删除实体
        DeleteEntity = 9,

    }

    public enum EntityType
    {
        Line = 0,

        Polyline = 1,

        None = 2,

        Arc = 3,

        Circle = 4,

        Point = 5,

        Ellipse = 6,

        Img = 7,

        Table = 8,

        leader = 9,

        LineDim = 10,

        ArcLenDim = 11,

        AngleArcDim = 12,

        AngleLengthDim = 13,

        AnglerotaDim = 14,

        ArcDim=15,

        Dimension=16
    }
}
