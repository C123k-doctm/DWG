using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha;
using Teigha.DatabaseServices;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using Teigha.GraphicsInterface;
using Teigha.Geometry;

namespace TeighaCad_Bll
{
    public class DrawableImp : Teigha.GraphicsInterface.Drawable
    {
        public DrawableImp()
        {
        }

        protected override void SubViewportDraw(ViewportDraw vd)
        {
        }

        protected override int SubSetAttributes(DrawableTraits traits)
        {
            return 0;
        }

        protected override int SubViewportDrawLogicalFlags(ViewportDraw vd)
        {
            return 0;
        }

        protected override bool SubWorldDraw(WorldDraw wd)
        {
            return false;
        }

        public override bool IsPersistent
        {
            get
            {
                return false;
            }
        }

        public override ObjectId Id
        {
            get
            {
                return new ObjectId();
            }
        }
    }

    public class RectFram : DrawableImp
    {
        public Point3dCollection m_pts;
        public RectFram()
        {
            m_pts = new Point3dCollection();
            m_pts.Add(new Point3d());
            m_pts.Add(new Point3d());
            m_pts.Add(new Point3d());
            m_pts.Add(new Point3d());
        }

        public RectFram(Point3d pt)
        {
            m_pts = new Point3dCollection();
            m_pts.Add(pt);
            m_pts.Add(pt);
            m_pts.Add(pt);
            m_pts.Add(pt);
        }

        protected override void SubViewportDraw(ViewportDraw vd)
        {
            Teigha.GraphicsInterface.Viewport vp = vd.Viewport;
            Matrix3d mat = vp.WorldToEyeTransform;
            Point3d p0 = mat * m_pts[0];
            Point3d p2 = mat * m_pts[2];
            m_pts[1] = new Point3d(p0.X, p2.Y, p2.Z);
            m_pts[3] = new Point3d(p2.X, p0.Y, p2.Z);

            mat = vp.EyeToWorldTransform;
            m_pts[1] = m_pts[1].TransformBy(mat);
            m_pts[3] = m_pts[3].TransformBy(mat);
            vd.SubEntityTraits.FillType = FillType.FillNever;
            vd.Geometry.Polygon(m_pts);
            vp = null;
        }

        public void setValue(Point3d pt)
        {
            m_pts[2] = pt;
        }
    }
}
