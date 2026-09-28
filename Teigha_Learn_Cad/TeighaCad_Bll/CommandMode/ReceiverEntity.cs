using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;

namespace TeighaCad_Bll
{
    /// <summary>
    /// 单个实体操作类（包含撤销和执行操作） 具体命令实现了命令接口，定义了动作和接收者之间的绑定关系。
    /// </summary>
    public class ReceiverEntity : Command
    { 
        //start 接收者(Receiver)：接收者知道如何进行必要的动作，实现这个请求。任何类都可以当接收者 
        Database database = null;
        Teigha.GraphicsSystem.LayoutHelperDevice helperDevice;

        Entity OldEnt;
        Entity NewEnt;
        ObjectId Newid;
        ObjectId Objid;

        public ReceiverEntity(ObjectId _id, ObjectId _newid, Entity _oldEnt, Entity _newEnt, ref Database _database, ref Teigha.GraphicsSystem.LayoutHelperDevice _helperDevice)
        {
            database = _database;
            helperDevice = _helperDevice;
            OldEnt = _oldEnt;
            Newid = _newid;
            NewEnt = _newEnt;
            Objid = _id;
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
                    if (OldEnt == null && NewEnt != null)
                    {
                        if (Newid.IsErased)
                            using (Entity ent = (Entity)Newid.GetObject(OpenMode.ForNotify, true))
                                ent.Erase(false);
                    }
                    //删除实体操作
                    else if (OldEnt != null && NewEnt == null)
                    {
                        if (!Objid.IsErased)
                            using (Entity ent = (Entity)Objid.GetObject(OpenMode.ForWrite, false))
                                ent.Erase(true);
                    }
                    else
                    { 
                        //实体属性发生修改
                        if (!Objid.IsErased)
                            using (Entity ent = (Entity)Objid.GetObject(OpenMode.ForWrite, false))
                            {
                                ent.CopyFrom(NewEnt);
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
                    if (OldEnt == null && NewEnt != null)
                    {
                        if (!Newid.IsErased)
                            using (Entity ent = (Entity)Newid.GetObject(OpenMode.ForWrite, false))
                                ent.Erase(true);
                    }
                    //删除实体操作 
                    else if (OldEnt != null && NewEnt == null)
                    {
                        if (Objid.IsErased)
                            using (Entity ent = (Entity)Objid.GetObject(OpenMode.ForNotify, true))
                                ent.Erase(false);
                    }
                    else
                    {
                        //实体属性发生修改
                        if (!Newid.IsErased)
                            using (Entity ent = (Entity)Newid.GetObject(OpenMode.ForWrite, false))
                            {
                                ent.CopyFrom(OldEnt);
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
            if (OldEnt != null)
                OldEnt.Dispose();
            if (NewEnt != null)
                NewEnt.Dispose();
        }
    }
}
