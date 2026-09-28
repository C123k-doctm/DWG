using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using Teigha.GraphicsInterface;
using Teigha.GraphicsSystem;

namespace TeighaCad_Bll
{
    public class SRnew : SelectionReactor
    {
        ObjectIdCollection selected;
        ObjectId spaceId;
        public SRnew(ObjectIdCollection s, ObjectId id)
        {
            spaceId = id;
            selected = s;
        }
        public override bool Selected(DrawableDesc pDrawableDesc)
        {
            DrawableDesc pDesc = pDrawableDesc;
            if (pDesc.Parent != null)
            {
                try
                {
                    // we walk up the GS node path to the root container primitive
                    // to avoid e.g. selection of individual lines in a dimension 
                    //我们沿着GS节点路径到达根容器原语
                    //避免例如在一个维度中选择单独的行
                    while (((DrawableDesc)pDesc.Parent).Parent != null)
                        pDesc = (DrawableDesc)pDesc.Parent;
                    if (pDesc.PersistId != IntPtr.Zero && ((DrawableDesc)pDesc.Parent).PersistId == spaceId.OldIdPtr)
                    {
                        ObjectId objid = new ObjectId(pDesc.PersistId);
                        if (selected.IndexOf(objid) == -1)
                        {
                            pDesc.MarkedToSkip = true; // regen abort for selected drawable, to avoid duplicates
                            selected.Add(objid);
                        }
                    }
                    return true;
                }
                catch (System.Exception ex)
                {
                }
            }
            return false;
        }
        // this more informative callback may be used to implement subentities selection
        public override SelectionReactorResult Selected(PathNode pthNode, Teigha.GraphicsInterface.Viewport viewInfo)
        {
            return SelectionReactorResult.NotImplemented;
        }
    };
}
