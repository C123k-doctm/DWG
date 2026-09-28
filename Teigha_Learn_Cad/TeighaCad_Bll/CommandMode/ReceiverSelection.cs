using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;

namespace TeighaCad_Bll
{
    /// <summary>
    /// 多个实体操作时操作类（包含撤销和执行操作） 具体命令实现了命令接口，定义了动作和接收者之间的绑定关系。
    /// 主要用于实体集合的移动和删除
    /// </summary>
    public class ReceiverSelection : Command
    {

        //start 接收者(Receiver)：接收者知道如何进行必要的动作，实现这个请求。任何类都可以当接收者 
        Database database = null;
        Teigha.GraphicsSystem.LayoutHelperDevice helperDevice;
        //end 

        List<Entity> OldEnts;
        List<Entity> NewEnts;
        ObjectIdCollection Objids;
        ObjectIdCollection Newids;

        public ReceiverSelection(ObjectIdCollection _id, ObjectIdCollection _newid, List<Entity> _oldEnt, List<Entity> _newEnt, ref Database _database, ref Teigha.GraphicsSystem.LayoutHelperDevice _helperDevice)
        {
            Objids = _id;
            Newids = _newid;
            database = _database;
            helperDevice = _helperDevice;
            OldEnts = _oldEnt;
            NewEnts = _newEnt;
        }


        /// <summary>
        /// 执行
        /// </summary>
        public void execute()
        {
            using (var trans = database.TransactionManager.StartTransaction())
            {
                try
                {
                    //添加实体操作
                    if (OldEnts.Count == 0 && NewEnts.Count > 0)
                    {
                        foreach (ObjectId item in Newids)
                        {
                            if (!item.IsNull && item.IsErased)
                                using (Entity ent = (Entity)item.GetObject(OpenMode.ForNotify, true))
                                    ent.Erase(false);
                        }
                    }
                    else if (OldEnts.Count > 0 && NewEnts.Count == 0)
                    {
                        foreach (ObjectId item in Objids)
                        {
                            if (!item.IsNull &&!item.IsErased)
                                using (Entity ent = (Entity)item.GetObject(OpenMode.ForWrite, false))
                                    ent.Erase(true);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Objids.Count; i++)
                        {
                            //实体属性发生修改
                            if (!Objids[i].IsNull && !Objids[i].IsErased)
                                using (Entity ent = (Entity)Objids[i].GetObject(OpenMode.ForWrite, false))
                                {
                                    ent.CopyFrom(NewEnts[i]);
                                }
                        }
                    }
                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Abort();
                    System.Windows.Forms.MessageBox.Show("未能执行操作！" + Environment.NewLine + "错误信息：" + ex.Message);
                }

            }

            if (helperDevice != null)
            {
                helperDevice.Update();
                helperDevice.Invalidate();
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        public void undo()
        {
            using (var trans = database.TransactionManager.StartTransaction())
            {
                try
                {
                    ///添加实体操作
                    if (OldEnts.Count == 0 && NewEnts.Count > 0)
                    {
                        foreach (ObjectId item in Newids)
                        {
                            if (!item.IsNull && !item.IsErased)
                                using (Entity ent = (Entity)item.GetObject(OpenMode.ForWrite, false))
                                    ent.Erase(true);
                        }

                    }
                    //删除实体操作 
                    else if (OldEnts.Count > 0 && NewEnts.Count == 0)
                    {
                        foreach (ObjectId Objid in Objids)
                        {
                            if (!Objid.IsNull && Objid.IsErased)
                                using (Entity ent = (Entity)Objid.GetObject(OpenMode.ForNotify, true))
                                    ent.Erase(false);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Newids.Count; i++)
                        {
                            //实体属性发生修改
                            if (!Newids[i].IsNull && !Newids[i].IsErased)
                                using (Entity ent = (Entity)Newids[i].GetObject(OpenMode.ForWrite, false))
                                {
                                    ent.CopyFrom(OldEnts[i]);
                                }
                        }


                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Abort();
                    System.Windows.Forms.MessageBox.Show("未能执行操作！" + Environment.NewLine + "错误信息：" + ex.Message);
                }

            }

            if (helperDevice != null)
            {
                helperDevice.Update();
                helperDevice.Invalidate();
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void clear()
        {
            foreach (var item in OldEnts)
            {
                if (item != null)
                    item.Dispose();
            }

            foreach (var item in NewEnts)
            {
                if (item != null)
                    item.Dispose();
            }
        }
    }
}
