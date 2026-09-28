using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Teigha.DatabaseServices;
using Teigha.Geometry;

namespace Teigha_Learn_Cad
{
    public partial class EditEntitys : KryptonForm
    {
        public TeighaBase CAD = new TeighaBase();
        public EditEntitys()
        {
            InitializeComponent();
        
        }
        ObjectIdCollection Collection;
        Database DataOwe;
        Teigha.GraphicsSystem.LayoutHelperDevice helperOwe;
        List<Entity> ViewEntity = new List<Entity>();//绘制临时实体
        string BlockReferenceName = string.Empty;
        public void setEditEntitys(ObjectIdCollection _Selects, ref Database _database, ref Teigha.GraphicsSystem.LayoutHelperDevice _helperDevice)
        {
            DataOwe = _database;
            helperOwe = _helperDevice;
            GetLayerToEdit();
            GetLineTypeToEdit();
            GetTextStyleToEdit();

            kryCombolineWidth.Items.Clear();
            kryCombolineWidth.Items.AddRange(System.Enum.GetNames(typeof(LineWeight)));

            kryCombolineWidth.Items.AddRange(System.Enum.GetNames(typeof(LineWeight)));

            Collection = _Selects;
            List<KeyValuePair<ObjectId, string>> ListDB = new List<KeyValuePair<ObjectId, string>>();
            foreach (ObjectId item in _Selects)
            {
                if (!item.IsNull && !item.IsErased)
                    using (Entity ent = (Entity)item.GetObject(OpenMode.ForRead))
                    {
                        if (ListDB.FindIndex(s1 => s1.Key == item) == -1)
                        {
                            ListDB.Add(new KeyValuePair<ObjectId, string>(item, ent.GetType().Name));
                        }
                    }
            }

            kryComboObject.DataSource = ListDB;
            kryComboObject.DisplayMember = "Key";
            kryComboObject.ValueMember = "Value";

            if (_Selects.Count == 0)
            {
                kryptonGroupBox1.Enabled = false ;
                kryGroupText.Enabled = false;
                kryGrouprefBlock.Enabled = false;
            }
            else
            {
                kryptonGroupBox1.Enabled = true;
                kryGroupText.Enabled = true;
                kryGrouprefBlock.Enabled = true;
            }
            
           
        }

        /// <summary>
        /// 清除绘画
        /// </summary>
        public void ClearView()
        {
            if (helperOwe != null)
            {
                using (Teigha.GraphicsSystem.View pView = helperOwe.ActiveView)
                {
                    foreach (Entity item in ViewEntity)
                    {
                        pView.Erase(item);
                    }
                }

                helperOwe.Update();
                helperOwe.Invalidate();
            }
            this.Invalidate();
        }

        private void EditEntitys_Load(object sender, EventArgs e)
        {
            //kryDataGridremark.Columns[0].ReadOnly = true;

        }

        #region 私有方法
        /// <summary>
        /// 编辑时 获取所有图层列表
        /// </summary>
        private void GetLayerToEdit()
        {
            kryComboLayer.Items.Clear();
            using (LayerTable LayTb = (LayerTable)DataOwe.LayerTableId.GetObject(OpenMode.ForRead, false, false))
            {
                foreach (ObjectId item in LayTb)
                {
                    using (LayerTableRecord LayRecTb = (LayerTableRecord)item.GetObject(OpenMode.ForRead, false, false))
                    {
                        kryComboLayer.Items.Add(LayRecTb.Name);
                        
                    }
                }
            }
        }

        /// <summary>
        /// 编辑时 获取线性列表
        /// </summary>
        private void GetLineTypeToEdit()
        {
            kryComboLineType.Items.Clear();
            using (LinetypeTable LineTb = (LinetypeTable)DataOwe.LinetypeTableId.GetObject(OpenMode.ForRead, false, false))
            {
                foreach (ObjectId item in LineTb)
                {
                    using (LinetypeTableRecord LineRecTb = (LinetypeTableRecord)item.GetObject(OpenMode.ForRead, false, false))
                    {
                        kryComboLineType.Items.Add(LineRecTb.Name);
                    }
                }
            }
         
        }

        /// <summary>
        /// 编辑时 获取文本样式列表
        /// </summary>
        private void GetTextStyleToEdit()
        {
            krytextStyle.Items.Clear();
            kryComboreftext.Items.Clear();
            using (TextStyleTable LineTb = (TextStyleTable)DataOwe.TextStyleTableId.GetObject(OpenMode.ForRead, false, false))
            {
                foreach (ObjectId item in LineTb)
                {
                    using (TextStyleTableRecord LineRecTb = (TextStyleTableRecord)item.GetObject(OpenMode.ForRead, false, false))
                    {
                        krytextStyle.Items.Add(LineRecTb.Name);
                        kryComboreftext.Items.Add(LineRecTb.Name);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 切换选中内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryComboObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                {
                    using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForRead, false, false))
                    {


                        #region 常规
                        kryComboLayer.SelectedIndexChanged -= KryComboLayer_SelectedIndexChanged;
                        kryCombolineWidth.SelectedIndexChanged -= kryCombolineWidth_SelectedIndexChanged;
                        kryComboLineType.SelectedIndexChanged -= KryComboLineType_SelectedIndexChanged;
                        kryLayerColor.SelectedColorChanged -= KryLayerColor_SelectedColorChanged;
                        kryEntitColor.SelectedColorChanged -= KryEntitColor_SelectedColorChanged;

                        kryEntname.Text = ent.GetRXClass().Name;
                        kryComboLayer.SelectedItem = ent.Layer;
                        using (LayerTableRecord layrec = (LayerTableRecord)ent.LayerId.GetObject(OpenMode.ForRead, false, false))
                        {
                            kryLayerColor.SelectedColor = layrec.Color.ColorValue;
                        }
                        kryComboLineType.SelectedItem = ent.Linetype;
                        kryLinetypeScaletxt.Text = ent.LinetypeScale.ToString();
                        kryCombolineWidth.SelectedItem = ent.LineWeight.ToString();
                        kryEntitColor.SelectedColor = ent.Color.ColorValue;

                        kryComboLayer.SelectedIndexChanged += KryComboLayer_SelectedIndexChanged;
                        kryLayerColor.SelectedColorChanged += KryLayerColor_SelectedColorChanged;
                        kryComboLineType.SelectedIndexChanged += KryComboLineType_SelectedIndexChanged;
                        kryCombolineWidth.SelectedIndexChanged += kryCombolineWidth_SelectedIndexChanged;
                        kryEntitColor.SelectedColorChanged += KryEntitColor_SelectedColorChanged;

                        #endregion
                        textBox1.Text = null;
                        textBox2.Text = null;
                        textBox3.Text = null;
                        textBox4.Text = null;
                        textBox5.Text = null;
                        textBox6.Text = null;
                        if (ent.GetRXClass().Name == "AcDbText" && CAD != null)
                        {
                            DBText shuchu = ent as DBText;

                            textBox1.Text = "非线束或中间件线束";
                            //kryptonMaskedTextBox1.Text = TeighaBase.Wbjc.Count.ToString();
                            foreach (var i in TeighaBase.Wbjc)
                            {
                                if (i.start.XS == shuchu || i.end.XS == shuchu)
                                {
                                    textBox1.Text = i.start.color.TextString;
                                    textBox2.Text = i.start.XS.TextString;
                                    textBox3.Text = i.start.width.TextString;
                                    textBox4.Text = i.length.ToString();
                                    textBox5.Text = i.jiecha.TextString + "/PIN" + i.start.port.TextString;
                                    textBox6.Text = i.lastjiecha.TextString + "/PIN" + i.end.port.TextString;
                                }
                            }
                        }
                        #region 实体备注
                        try
                        {
                            krDataViewX.Rows.Clear();
                            ResultBuffer resBuf = ent.XData;
                            if (resBuf != null)
                            {
                                TypedValue[] typed = resBuf.AsArray();
                                foreach (TypedValue item in typed)
                                {
                                    if ((DxfCode)item.TypeCode != DxfCode.ExtendedDataRegAppName)
                                        krDataViewX.Rows.Add(new object[] { (DxfCode)item.TypeCode, item.Value });
                                }
                            }
                            //if (ent.GetType().Name == "Line"
                            //  )
                            //{
                            //    using (Line line = (Line)ent)
                            //    {
                            //        string Str = line.StartPoint.ToString().Replace(")", "").Replace("(", "");
                            //        string[] spliteStr = Str.Split(',');
                            //        spliteStr[0] = "X:" + spliteStr[0];
                            //        spliteStr[1] = "Y:" + spliteStr[1];
                            //        spliteStr[2] = "Z:" + spliteStr[2];
                            //        kryptonTextBox1.Text = spliteStr[0] + Environment.NewLine + spliteStr[1] + Environment.NewLine + spliteStr[2];
                            //        kryptonTextBox1.Tag = Str;

                            //        string Str2 = line.EndPoint.ToString().Replace(")", "").Replace("(", "");
                            //        string[] spliteStr2 = Str2.Split(',');
                            //        spliteStr2[0] = "X:" + spliteStr2[0];
                            //        spliteStr2[1] = "Y:" + spliteStr2[1];
                            //        spliteStr2[2] = "Z:" + spliteStr2[2];
                            //        kryptonTextBox2.Text = spliteStr2[0] + Environment.NewLine + spliteStr2[1] + Environment.NewLine + spliteStr2[2];
                            //        kryptonTextBox2.Tag = Str2;
                            //    }

                            //}
                            //else if (ent.GetType().Name == "Arc")
                            //{

                            //    using (Arc line = (Arc)ent)
                            //    {
                            //        string Str = line.StartPoint.ToString().Replace(")", "").Replace("(", "");
                            //        string[] spliteStr = Str.Split(',');
                            //        spliteStr[0] = "X:" + spliteStr[0];
                            //        spliteStr[1] = "Y:" + spliteStr[1];
                            //        spliteStr[2] = "Z:" + spliteStr[2];
                            //        kryptonTextBox1.Text = spliteStr[0] + Environment.NewLine + spliteStr[1] + Environment.NewLine + spliteStr[2];
                            //        kryptonTextBox1.Tag = Str;

                            //        string Str2 = line.EndPoint.ToString().Replace(")", "").Replace("(", "");
                            //        string[] spliteStr2 = Str2.Split(',');
                            //        spliteStr2[0] = "X:" + spliteStr2[0];
                            //        spliteStr2[1] = "Y:" + spliteStr2[1];
                            //        spliteStr2[2] = "Z:" + spliteStr2[2];
                            //        kryptonTextBox2.Text = spliteStr2[0] + Environment.NewLine + spliteStr2[1] + Environment.NewLine + spliteStr2[2];
                            //        kryptonTextBox2.Tag = Str2;
                            //    }
                            //}
                            //else if (  ent.GetType().Name == "Polyline")
                            //{
                            //    using (Polyline line = (Polyline)ent)
                            //    {
                            //        string Str = line.StartPoint.ToString().Replace(")", "").Replace("(", "");
                            //        string[] spliteStr = Str.Split(',');
                            //        spliteStr[0] = "X:" + spliteStr[0];
                            //        spliteStr[1] = "Y:" + spliteStr[1];
                            //        spliteStr[2] = "Z:" + spliteStr[2];
                            //        kryptonTextBox1.Text = spliteStr[0] + Environment.NewLine + spliteStr[1] + Environment.NewLine + spliteStr[2];
                            //        kryptonTextBox1.Tag = Str;

                            //        string Str2 = line.EndPoint.ToString().Replace(")", "").Replace("(", "");
                            //        string[] spliteStr2 = Str2.Split(',');
                            //        spliteStr2[0] = "X:" + spliteStr2[0];
                            //        spliteStr2[1] = "Y:" + spliteStr2[1];
                            //        spliteStr2[2] = "Z:" + spliteStr2[2];
                            //        kryptonTextBox2.Text = spliteStr2[0] + Environment.NewLine + spliteStr2[1] + Environment.NewLine + spliteStr2[2];
                            //        kryptonTextBox2.Tag = Str2;
                            //    }
                            //}
                        }
                        catch { }
                        #endregion

                        #region 文本
                        if (ent.GetType().Name.Contains("Dimension"))
                        {
                            //DimStyleTableRecord
                            kryGroupText.Text = "标注文字";
                            #region 注释类文字
                            kryGroupText.Show();
                            using (Dimension dbtxt = (Dimension)ent)
                            {
                                krytextStyle.SelectedIndexChanged -= KrytextStyle_SelectedIndexChanged;
                                krytxtnode.TextChanged -= Krytxtnode_TextChanged;
                                kryRotation.TextChanged -= KryRotation_TextChanged;

                                krytxtnode.Text = dbtxt.DimensionText;
                                krytextStyle.Enabled = false;
                                kryRotation.Text = (dbtxt.TextRotation * 180 / Math.PI).ToString("F3");
                                kryLostPoint.Text = dbtxt.TextPosition.ToString();

                                kryRotation.TextChanged += KryRotation_TextChanged;
                                krytxtnode.TextChanged += Krytxtnode_TextChanged;
                                krytextStyle.SelectedIndexChanged += KrytextStyle_SelectedIndexChanged;
                            }
                            #endregion
                            kryGrouprefBlock.Hide();
                        }
                        else if (ent.GetType().Name.Contains("DBText"))
                        {
                            krytextStyle.Enabled = true;
                            kryGroupText.Show();
                            kryGrouprefBlock.Hide();
                            using (DBText dbtxt = (DBText)ent)
                            {
                                krytextStyle.SelectedIndexChanged -= KrytextStyle_SelectedIndexChanged;
                                krytxtnode.TextChanged -= Krytxtnode_TextChanged;
                                kryRotation.TextChanged -= KryRotation_TextChanged;

                                kryNumesizes.ValueChanged -= KryNumesizes_ValueChanged;
                                kryNumwidth.ValueChanged -= KryNumwidth_ValueChanged;
                                kryNumheight.ValueChanged -= KryNumheight_ValueChanged;

                                krytxtnode.Text = dbtxt.TextString;
                                krytextStyle.SelectedItem = dbtxt.TextStyleName;
                                kryRotation.Text = (dbtxt.Rotation * 180 / Math.PI).ToString("F3");
                                kryLostPoint.Text = dbtxt.Position.ToString();
                                kryNumwidth.Value = (decimal)dbtxt.WidthFactor;
                                kryNumheight.Value = (decimal)dbtxt.Height;
                                if (!dbtxt.TextStyleId.IsErased && dbtxt.TextStyleId.IsValid)
                                {
                                    TextStyleTableRecord styles = (TextStyleTableRecord)dbtxt.TextStyleId.GetObject(OpenMode.ForRead, false, false);
                                    kryNumesizes.Value = (decimal)styles.TextSize;
                                }

                                kryNumesizes.ValueChanged += KryNumesizes_ValueChanged;
                                kryNumwidth.ValueChanged += KryNumwidth_ValueChanged;
                                kryNumheight.ValueChanged += KryNumheight_ValueChanged;

                                kryRotation.TextChanged += KryRotation_TextChanged;
                                krytxtnode.TextChanged += Krytxtnode_TextChanged;
                                krytextStyle.SelectedIndexChanged += KrytextStyle_SelectedIndexChanged;
                            }

                        }
                        else if (ent.GetType().Name.Contains("MText"))
                        {
                            krytextStyle.Enabled = true;
                            kryGroupText.Show();
                            kryGrouprefBlock.Hide();
                            using (MText dbtxt = (MText)ent)
                            {
                                krytextStyle.SelectedIndexChanged -= KrytextStyle_SelectedIndexChanged;
                                krytxtnode.TextChanged -= Krytxtnode_TextChanged;
                                kryRotation.TextChanged -= KryRotation_TextChanged;

                                kryNumesizes.ValueChanged -= KryNumesizes_ValueChanged;
                                kryNumwidth.ValueChanged -= KryNumwidth_ValueChanged;
                                kryNumheight.ValueChanged -= KryNumheight_ValueChanged;

                                krytxtnode.Text = dbtxt.Contents;
                                krytextStyle.SelectedItem = dbtxt.TextStyleName;
                                kryRotation.Text = (dbtxt.Rotation * 180 / Math.PI).ToString("F3");
                                kryLostPoint.Text = dbtxt.Location.ToString();

                                if (!dbtxt.TextStyleId.IsErased && dbtxt.TextStyleId.IsValid)
                                {
                                    TextStyleTableRecord styles = (TextStyleTableRecord)dbtxt.TextStyleId.GetObject(OpenMode.ForRead, false, false);
                                    kryNumesizes.Value = (decimal)styles.TextSize;
                                }
                                kryNumwidth.Value = (decimal)dbtxt.Width;
                                kryNumheight.Value = (decimal)dbtxt.Height;

                                kryNumesizes.ValueChanged += KryNumesizes_ValueChanged;
                                kryNumwidth.ValueChanged += KryNumwidth_ValueChanged;
                                kryNumheight.ValueChanged += KryNumheight_ValueChanged;

                                kryRotation.TextChanged += KryRotation_TextChanged;
                                krytxtnode.TextChanged += Krytxtnode_TextChanged;
                                krytextStyle.SelectedIndexChanged += KrytextStyle_SelectedIndexChanged;
                            }
                        }
                        else if (ent.GetType().Name.Contains("BlockReference"))
                        {
                            kryGrouprefBlock.Show();
                            kryGroupText.Hide();
                            using (BlockReference dbtxt = (BlockReference)ent)
                            {
                                List<KeyValuePair<ObjectId, string>> ListDB = new List<KeyValuePair<ObjectId, string>>();
                                using (BlockTableRecord blockref =
                                    (BlockTableRecord)dbtxt.BlockTableRecord.GetObject(OpenMode.ForRead, false, false))
                                {
                                    BlockReferenceName = blockref.Name;
                                    foreach (ObjectId item in blockref)
                                    {
                                        if (!item.IsErased && item.IsValid)
                                            using (Entity entitem = (Entity)item.GetObject(OpenMode.ForWrite))
                                            {
                                                if (entitem.GetType().Name == "DBText" && entitem.Visible)
                                                {
                                                    ListDB.Add(new KeyValuePair<ObjectId, string>(item, ent.GetType().Name));
                                                }
                                                else if (entitem.GetType().Name == "MText" && entitem.Visible)
                                                {
                                                    ListDB.Add(new KeyValuePair<ObjectId, string>(item, ent.GetType().Name));
                                                }
                                            }
                                    }

                                    //dbtxt.ConvertToStaticBlock(blockref.Name);
                                }

                                kryComboObjectRef.DataSource = ListDB;
                                kryComboObjectRef.DisplayMember = "Key";
                                kryComboObjectRef.ValueMember = "Value";
                            }

                        }
                        else
                        {
                            kryGrouprefBlock.Hide();
                            kryGroupText.Hide();
                        }
                        #endregion

                       
                    }
                }
               
            }
        }

        #region 常规
        /// <summary>
        /// 实体颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KryEntitColor_SelectedColorChanged(object sender, ColorEventArgs e)
        {
            if (kryEntitColor.SelectedColor != null && kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                ent.Color = Teigha.Colors.Color.FromColor(kryEntitColor.SelectedColor);
                            }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }


                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 图层颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KryLayerColor_SelectedColorChanged(object sender, ColorEventArgs e)
        {
            if (kryComboLayer.SelectedItem != null && kryLayerColor.SelectedColor != null)
            {
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        LayerTable layer = (LayerTable)DataOwe.LayerTableId.GetObject(OpenMode.ForRead, false, false);
                        if (layer.Has(kryComboLayer.SelectedItem.ToString()))
                        {
                            LayerTableRecord layrec = (LayerTableRecord)layer[kryComboLayer.SelectedItem.ToString()].GetObject(OpenMode.ForWrite, false, false);
                            layrec.Color = Teigha.Colors.Color.FromColor(kryLayerColor.SelectedColor);
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }
                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 线形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KryComboLineType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kryComboLineType.SelectedItem != null && kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        using (LinetypeTable Line = (LinetypeTable)DataOwe.LinetypeTableId.GetObject(OpenMode.ForRead, false, false))
                        {
                            if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                                using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                                {
                                    if (ent.Linetype != kryComboLineType.SelectedItem.ToString())
                                    {
                                        if (Line.Has(kryComboLineType.SelectedItem.ToString()))
                                        {
                                            ent.LinetypeId = Line[kryComboLineType.SelectedItem.ToString()];
                                            ent.Linetype = kryComboLineType.SelectedItem.ToString();
                                        }
                                    }
                                }
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }
                   
                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KryComboLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kryComboLayer.SelectedItem != null && kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        using (LayerTable layer = (LayerTable)DataOwe.LayerTableId.GetObject(OpenMode.ForRead, false, false))
                        {
                            if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                                using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                                {
                                    if (ent.Layer != kryComboLayer.SelectedItem.ToString())
                                    {
                                        if (layer.Has(kryComboLayer.SelectedItem.ToString()))
                                        {
                                            ent.LayerId = layer[kryComboLayer.SelectedItem.ToString()];
                                            ent.Layer = kryComboLayer.SelectedItem.ToString();
                                        }
                                    }
                                }
                            if (layer.Has(kryComboLayer.SelectedItem.ToString()))
                            {
                                kryLayerColor.SelectedColorChanged -= KryLayerColor_SelectedColorChanged;
                                using (LayerTableRecord layrec = (LayerTableRecord)layer[kryComboLayer.SelectedItem.ToString()].GetObject(OpenMode.ForRead, false, false))
                                {
                                    kryLayerColor.SelectedColor = layrec.Color.ColorValue;
                                }
                                kryLayerColor.SelectedColorChanged += KryLayerColor_SelectedColorChanged;
                            }
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }
                 
                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 线宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryCombolineWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kryCombolineWidth.SelectedItem != null && kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                ent.LineWeight = (LineWeight)System.Enum.Parse(typeof(LineWeight), kryCombolineWidth.SelectedItem.ToString(), true);
                            }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }

                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }


        #endregion

        #region 文字

        /// <summary>
        /// 旋转角度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KryRotation_TextChanged(object sender, EventArgs e)
        {
            double rotation = 0;
            if (double.TryParse(kryRotation.Text.ToString(), out rotation) && !double.IsNaN(rotation))
            {
                if (kryComboObject.SelectedItem != null)
                {
                    KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                    using (var trans = DataOwe.TransactionManager.StartTransaction())
                    {
                        try
                        {
                            if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                                using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                                {
                                    if (ent.GetType().Name == "DBText")
                                    {
                                        using (DBText Dbtxt = (DBText)ent)
                                        {
                                            Dbtxt.Rotation = rotation * Math.PI / 180;
                                        }
                                    }
                                    else if (ent.GetType().Name == "MText")
                                    {
                                        using (MText Dbtxt = (MText)ent)
                                        {
                                            Dbtxt.Rotation = rotation * Math.PI / 180;
                                        }
                                    }
                                    else if (ent.GetType().Name.Contains("Dimension"))
                                    {
                                        using (Dimension Dbtxt = (Dimension)ent)
                                        {
                                            Dbtxt.TextRotation = rotation * Math.PI / 180;
                                        }
                                    }
                                }
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Abort();
                            MessageBox.Show(ex.Message, "修改失败！");
                        }

                    }
                    if (helperOwe != null)
                    {
                        helperOwe.Update();
                        helperOwe.Invalidate();
                    }
                    this.Invalidate();
                }
            }

        }

        /// <summary>
        /// 文本内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Krytxtnode_TextChanged(object sender, EventArgs e)
        {
            if (kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                if (ent.GetType().Name == "DBText")
                                {
                                    using (DBText Dbtxt = (DBText)ent)
                                    {
                                        Dbtxt.TextString = krytxtnode.Text;
                                    }
                                }
                                else if (ent.GetType().Name == "MText")
                                {
                                    using (MText Dbtxt = (MText)ent)
                                    {
                                        Dbtxt.Contents = krytxtnode.Text;
                                    }
                                }
                                else if (ent.GetType().Name.Contains("Dimension"))
                                {
                                    using (Dimension Dbtxt = (Dimension)ent)
                                    {
                                        Dbtxt.DimensionText = krytxtnode.Text;
                                    }
                                }
                            }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }

                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 文字格式样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KrytextStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (krytextStyle.SelectedItem != null && kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        using (TextStyleTable Line = (TextStyleTable)DataOwe.TextStyleTableId.GetObject(OpenMode.ForRead, false, false))
                        {
                            if (Line.Has(krytextStyle.SelectedItem.ToString()))
                                if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                                    using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                                    {
                                        if (ent.GetType().Name == "DBText")
                                        {
                                            using (DBText Dbtxt = (DBText)ent)
                                            {
                                                Dbtxt.TextStyleId = Line[krytextStyle.SelectedItem.ToString()];
                                            }
                                        }
                                        else if (ent.GetType().Name == "MText")
                                        {
                                            using (MText Dbtxt = (MText)ent)
                                            {
                                                Dbtxt.TextStyleId = Line[krytextStyle.SelectedItem.ToString()];
                                            }
                                        }
                                        else if (ent.GetType().Name.Contains("Dimension"))
                                        {

                                            using (Dimension Dbtxt = (Dimension)ent)
                                            {
                                                Dbtxt.DimensionStyle = Line[krytextStyle.SelectedItem.ToString()];
                                            }
                                        }
                                    }
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }

                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        private void buttonSpecAny1_Click(object sender, EventArgs e)
        {
            krytxtnode.Text = "";
        }

        private void buttonSpecAny2_Click(object sender, EventArgs e)
        {
            kryRotation.Text = "0";
        }

        /// <summary>
        /// 跳转 突出显示该点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSpecAny3_Click(object sender, EventArgs e)
        {
            if (kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                if (ent.GetType().Name == "DBText")
                                {
                                    using (DBText Dbtxt = (DBText)ent)
                                    {
                                        using (Teigha.GraphicsSystem.View pView = helperOwe.ActiveView)
                                        {
                                            DBPoint Dbmine = new DBPoint();
                                            Dbmine.Color = Teigha.Colors.Color.FromColor(Color.Red);
                                            Dbmine.Thickness = 10;
                                            Dbmine.LineWeight = LineWeight.LineWeight200;
                                            Dbmine.Position = Dbtxt.Position;
                                            ViewEntity.Add(Dbmine);
                                            pView.Add(Dbmine);
                                            pView.Dolly(Dbtxt.Position - pView.Position);
                                        }
                                    }
                                }
                                else if (ent.GetType().Name == "MText")
                                {
                                    using (MText Dbtxt = (MText)ent)
                                    {
                                        using (Teigha.GraphicsSystem.View pView = helperOwe.ActiveView)
                                        {
                                            DBPoint Dbmine = new DBPoint();
                                            Dbmine.Color = Teigha.Colors.Color.FromColor(Color.Red);
                                            Dbmine.LineWeight = LineWeight.LineWeight200;
                                            Dbmine.Thickness = 10;
                                            Dbmine.Position = Dbtxt.Location;
                                            ViewEntity.Add(Dbmine);
                                            pView.Add(Dbmine);
                                            pView.Dolly(Dbtxt.Location - pView.Position);
                                        }
                                    }
                                }
                                else if (ent.GetType().Name.Contains("Dimension"))
                                {
                                    using (Dimension Dbtxt = (Dimension)ent)
                                    {
                                        using (Teigha.GraphicsSystem.View pView = helperOwe.ActiveView)
                                        {
                                            DBPoint Dbmine = new DBPoint();
                                            Dbmine.Color = Teigha.Colors.Color.FromColor(Color.Red);
                                            Dbmine.LineWeight = LineWeight.LineWeight200;
                                            Dbmine.Thickness = 10;
                                            Dbmine.Position = Dbtxt.TextPosition;
                                            ViewEntity.Add(Dbmine);
                                            pView.Add(Dbmine);
                                            pView.Dolly(Dbtxt.TextPosition - pView.Position);
                                        }
                                    }
                                }
                            }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }


                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 高度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KryNumheight_ValueChanged(object sender, EventArgs e)
        {
            if (kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                if (ent.GetType().Name == "DBText")
                                {
                                    using (DBText Dbtxt = (DBText)ent)
                                    {
                                        Dbtxt.Height = (double)kryNumheight.Value;
                                    }
                                }
                                else if (ent.GetType().Name == "MText")
                                {
                                    using (MText Dbtxt = (MText)ent)
                                    {
                                        Dbtxt.Height = (double)kryNumheight.Value;
                                    }
                                }
                            }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }

                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KryNumwidth_ValueChanged(object sender, EventArgs e)
        {
            if (kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                if (ent.GetType().Name == "DBText")
                                {
                                    using (DBText Dbtxt = (DBText)ent)
                                    {
                                        Dbtxt.WidthFactor = (double)kryNumwidth.Value;
                                    }
                                }
                                else if (ent.GetType().Name == "MText")
                                {
                                    using (MText Dbtxt = (MText)ent)
                                    {
                                        Dbtxt.Width = (double)kryNumwidth.Value;
                                    }
                                }
                            }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }

                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KryNumesizes_ValueChanged(object sender, EventArgs e)
        {

            if (kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                if (ent.GetType().Name == "DBText")
                                {
                                    using (DBText Dbtxt = (DBText)ent)
                                    {
                                        if (!Dbtxt.TextStyleId.IsErased && Dbtxt.TextStyleId.IsValid)
                                        {
                                            TextStyleTableRecord styles = (TextStyleTableRecord)Dbtxt.TextStyleId.GetObject(OpenMode.ForWrite, false, false);
                                            styles.TextSize = (double)kryNumesizes.Value;
                                        }
                                    }
                                }
                                else if (ent.GetType().Name == "MText")
                                {
                                    using (MText Dbtxt = (MText)ent)
                                    {
                                        if (!Dbtxt.TextStyleId.IsErased && Dbtxt.TextStyleId.IsValid)
                                        {
                                            TextStyleTableRecord styles = (TextStyleTableRecord)Dbtxt.TextStyleId.GetObject(OpenMode.ForWrite, false, false);
                                            styles.TextSize = (double)kryNumesizes.Value;
                                        }
                                    }
                                }
                            }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }

                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        #endregion

        #region 引用块类型
        /// <summary>
        /// 切换引用块中实体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryComboObjectRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kryComboObjectRef.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObjectRef.SelectedItem;
                if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                {
                    using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForRead))
                    {
                        krytextrefText.Text = ent.GetType().Name;
                        if (ent.GetType().Name.Contains("DBText"))
                        {
                           
                            using (DBText Db = (DBText)ent)
                            {
                                kryTextrefnode.TextChanged -= KryTextrefnode_TextChanged;
                                kryComboreftext.SelectedIndexChanged -= KryComboreftext_SelectedIndexChanged;
                                kryTextRotation.TextChanged -= KryTextRotation_TextChanged;
                                kryrefNumericsizae.ValueChanged -= KryrefNumericsizae_ValueChanged;
                                kryNumericrefwidth.ValueChanged -= KryNumericrefwidth_ValueChanged;
                                kryNumericrefheight.ValueChanged -= KryNumericrefheight_ValueChanged;
                                kryColorButref.SelectedColorChanged -= KryEntitColor_SelectedColorChanged1;

                                kryTextrefnode.Text = Db.TextString;
                                kryComboreftext.SelectedItem = Db.TextStyleName;
                                kryTextRotation.Text = (Db.Rotation * 180 / Math.PI).ToString("F3");
                                kryrefTextBox.Text = Db.Position.ToString();
                                kryColorButref.SelectedColor = ent.Color.ColorValue;
                                kryNumericrefwidth.Value = (decimal)Db.WidthFactor;
                                kryNumericrefheight.Value = (decimal)Db.Height;
                                if (!Db.TextStyleId.IsErased && Db.TextStyleId.IsValid)
                                {
                                    TextStyleTableRecord styles = (TextStyleTableRecord)Db.TextStyleId.GetObject(OpenMode.ForRead, false, false);
                                    kryrefNumericsizae.Value = (decimal)styles.TextSize;
                                }

                                kryColorButref.SelectedColorChanged += KryEntitColor_SelectedColorChanged1;
                                kryNumericrefwidth.ValueChanged += KryNumericrefwidth_ValueChanged;
                                kryNumericrefheight.ValueChanged += KryNumericrefheight_ValueChanged;
                                kryrefNumericsizae.ValueChanged += KryrefNumericsizae_ValueChanged;
                                kryTextrefnode.TextChanged += KryTextrefnode_TextChanged;
                                kryComboreftext.SelectedIndexChanged += KryComboreftext_SelectedIndexChanged;
                                kryTextRotation.TextChanged += KryTextRotation_TextChanged;
                            }
                        }
                        else if (ent.GetType().Name.Contains("MText"))
                        {
                            using (MText Db = (MText)ent)
                            {
                                kryTextrefnode.TextChanged -= KryTextrefnode_TextChanged;
                                kryComboreftext.SelectedIndexChanged -= KryComboreftext_SelectedIndexChanged;
                                kryTextRotation.TextChanged -= KryTextRotation_TextChanged;
                                kryrefNumericsizae.ValueChanged -= KryrefNumericsizae_ValueChanged;
                                kryNumericrefwidth.ValueChanged -= KryNumericrefwidth_ValueChanged;
                                kryNumericrefheight.ValueChanged -= KryNumericrefheight_ValueChanged;
                                kryColorButref.SelectedColorChanged -= KryEntitColor_SelectedColorChanged1;

                                kryTextrefnode.Text = Db.Contents;
                                kryComboreftext.SelectedItem = Db.TextStyleName;
                                kryTextRotation.Text = (Db.Rotation * 180 / Math.PI).ToString("F3");
                                kryrefTextBox.Text = Db.Location.ToString();
                                kryColorButref.SelectedColor = ent.Color.ColorValue;
                                kryNumericrefwidth.Value = (decimal)Db.Width;
                                kryNumericrefheight.Value = (decimal)Db.Height;
                                if (!Db.TextStyleId.IsErased && Db.TextStyleId.IsValid)
                                {
                                    TextStyleTableRecord styles = (TextStyleTableRecord)Db.TextStyleId.GetObject(OpenMode.ForRead, false, false);
                                    kryrefNumericsizae.Value = (decimal)styles.TextSize;
                                }

                                kryColorButref.SelectedColorChanged += KryEntitColor_SelectedColorChanged1;
                                kryNumericrefwidth.ValueChanged += KryNumericrefwidth_ValueChanged;
                                kryNumericrefheight.ValueChanged += KryNumericrefheight_ValueChanged;
                                kryrefNumericsizae.ValueChanged += KryrefNumericsizae_ValueChanged;
                                kryTextrefnode.TextChanged += KryTextrefnode_TextChanged;
                                kryComboreftext.SelectedIndexChanged += KryComboreftext_SelectedIndexChanged;
                                kryTextRotation.TextChanged += KryTextRotation_TextChanged;
                            }
                        }
                    }
                }
            }
        }

        private void KryEntitColor_SelectedColorChanged1(object sender, ColorEventArgs e)
        {
            if (kryColorButref.SelectedColor != null && kryComboObjectRef.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObjectRef.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                ent.Color = Teigha.Colors.Color.FromColor(kryColorButref.SelectedColor);
                            }

                        UpdateRefBlock();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }


                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        private void KryNumericrefheight_ValueChanged(object sender, EventArgs e)
        {
            if (kryComboObjectRef.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObjectRef.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                if (ent.GetType().Name == "DBText")
                                {
                                    using (DBText Dbtxt = (DBText)ent)
                                    {
                                        Dbtxt.Height = (double)kryNumericrefheight.Value;
                                    }
                                }
                                else if (ent.GetType().Name == "MText")
                                {
                                    using (MText Dbtxt = (MText)ent)
                                    {
                                        Dbtxt.Height = (double)kryNumericrefheight.Value;
                                    }
                                }
                            }
                        UpdateRefBlock();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }

                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        private void KryNumericrefwidth_ValueChanged(object sender, EventArgs e)
        {
            if (kryComboObjectRef.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObjectRef.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                if (ent.GetType().Name == "DBText")
                                {
                                    using (DBText Dbtxt = (DBText)ent)
                                    {
                                        Dbtxt.WidthFactor = (double)kryNumericrefwidth.Value;
                                    }
                                }
                                else if (ent.GetType().Name == "MText")
                                {
                                    using (MText Dbtxt = (MText)ent)
                                    {
                                        Dbtxt.Width = (double)kryNumericrefwidth.Value;
                                    }
                                }
                            }
                        UpdateRefBlock();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }

                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        //size
        private void KryrefNumericsizae_ValueChanged(object sender, EventArgs e)
        {
            if (kryComboObjectRef.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObjectRef.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                if (ent.GetType().Name == "DBText")
                                {
                                    using (DBText Dbtxt = (DBText)ent)
                                    {
                                        if (!Dbtxt.TextStyleId.IsErased && Dbtxt.TextStyleId.IsValid)
                                        {
                                            TextStyleTableRecord styles = (TextStyleTableRecord)Dbtxt.TextStyleId.GetObject(OpenMode.ForWrite, false, false);
                                            styles.TextSize = (double)kryrefNumericsizae.Value;
                                        }
                                    }
                                }
                                else if (ent.GetType().Name == "MText")
                                {
                                    using (MText Dbtxt = (MText)ent)
                                    {
                                        if (!Dbtxt.TextStyleId.IsErased && Dbtxt.TextStyleId.IsValid)
                                        {
                                            TextStyleTableRecord styles = (TextStyleTableRecord)Dbtxt.TextStyleId.GetObject(OpenMode.ForWrite, false, false);
                                            styles.TextSize = (double)kryrefNumericsizae.Value;
                                        }
                                    }
                                }
                            }
                        UpdateRefBlock();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }

                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        private void KryTextRotation_TextChanged(object sender, EventArgs e)
        {
            double rotation = 0;
            if (double.TryParse(kryTextRotation.Text.ToString(), out rotation) && !double.IsNaN(rotation))
            {
                if (kryComboObjectRef.SelectedItem != null)
                {
                    KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObjectRef.SelectedItem;
                    using (var trans = DataOwe.TransactionManager.StartTransaction())
                    {
                        try
                        {
                            if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                                using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                                {
                                    if (ent.GetType().Name == "DBText")
                                    {
                                        using (DBText Dbtxt = (DBText)ent)
                                        {
                                            Dbtxt.Rotation = rotation * Math.PI / 180;
                                        }
                                    }
                                    else if (ent.GetType().Name == "MText")
                                    {
                                        using (MText Dbtxt = (MText)ent)
                                        {
                                            Dbtxt.Rotation = rotation * Math.PI / 180;
                                        }
                                    }
                                }
                            UpdateRefBlock();
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Abort();
                            MessageBox.Show(ex.Message, "修改失败！");
                        }

                    }
                    if (helperOwe != null)
                    {
                        helperOwe.Update();
                        helperOwe.Invalidate();
                    }
                    this.Invalidate();
                }
            }
        }

        private void KryComboreftext_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kryComboreftext.SelectedItem != null && kryComboObjectRef.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObjectRef.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        using (TextStyleTable Line = (TextStyleTable)DataOwe.TextStyleTableId.GetObject(OpenMode.ForRead, false, false))
                        {
                            if (Line.Has(kryComboreftext.SelectedItem.ToString()))
                                if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                                    using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                                    {
                                        if (ent.GetType().Name == "DBText")
                                        {
                                            using (DBText Dbtxt = (DBText)ent)
                                            {
                                                Dbtxt.TextStyleId = Line[kryComboreftext.SelectedItem.ToString()];
                                            }
                                        }
                                        else if (ent.GetType().Name == "MText")
                                        {
                                            using (MText Dbtxt = (MText)ent)
                                            {
                                                Dbtxt.TextStyleId = Line[kryComboreftext.SelectedItem.ToString()];
                                            }
                                        }
                                    }
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }
                    UpdateRefBlock();
                }
                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }


        private void KryTextrefnode_TextChanged(object sender, EventArgs e)
        {
            if (kryComboObjectRef.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> selectItem = (KeyValuePair<ObjectId, string>)kryComboObjectRef.SelectedItem;
                using (var trans = DataOwe.TransactionManager.StartTransaction())
                {
                    try
                    {
                        if (!selectItem.Key.IsErased && selectItem.Key.IsValid)
                            using (Entity ent = (Entity)selectItem.Key.GetObject(OpenMode.ForWrite, false, false))
                            {
                                
                                if (ent.GetType().Name == "DBText" && ent.Visible)
                                {
                                    using (DBText Dbtxt = (DBText)ent)
                                    {
                                        Dbtxt.TextString = kryTextrefnode.Text;
                                    }
                                }
                                else if (ent.GetType().Name == "MText" && ent.Visible)
                                {
                                    using (MText Dbtxt = (MText)ent)
                                    {
                                        Dbtxt.Contents = kryTextrefnode.Text;
                                    }
                                }
                            }
                        UpdateRefBlock();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        MessageBox.Show(ex.Message, "修改失败！");
                    }

                }
                

                if (helperOwe != null)
                {
                    helperOwe.Update();
                    helperOwe.Invalidate();
                }
                this.Invalidate();
            }
        }

        public void UpdateRefBlock()
        {
           
            if (kryComboObject.SelectedItem != null)
            {
                KeyValuePair<ObjectId, string> select = (KeyValuePair<ObjectId, string>)kryComboObject.SelectedItem;
                if (!select.Key.IsErased && select.Key.IsValid)
                {
                    using (Entity ent = (Entity)select.Key.GetObject(OpenMode.ForWrite, false, false))
                    {
                        BlockReference refblock = (BlockReference)ent;
                        BlockTable blocktab = (BlockTable)DataOwe.BlockTableId.GetObject(OpenMode.ForRead, false, false);
                        refblock.BlockTableRecord = blocktab[BlockReferenceName];
                        refblock.ResetBlock();
                    }
                    //bclok.SwapReferences
                }
            }
        }


        #endregion

        private void buttonSpecAny6_Click(object sender, EventArgs e)
        {
            kryTextrefnode.Text = "";
        }

        private void buttonSpecAny5_Click(object sender, EventArgs e)
        {
            kryTextRotation.Text = "0";
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryComboLineType_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        //private void buttonSpecAny4_Click(object sender, EventArgs e)
        //{
        //    //string[] strpo = kryptonTextBox1.Tag.ToString().Split(',');
        //    //Point3d point = new Point3d(double.Parse(strpo[0]), double.Parse(strpo[1]), double.Parse(strpo[2]));
        //    //using (Teigha.GraphicsSystem.View pView = helperOwe.ActiveView)
        //    //{
        //    //    pView.Dolly(point - pView.Position);
        //    //}
        //}

        //private void buttonSpecAny7_Click(object sender, EventArgs e)
        //{
        //    //string[] strpo = kryptonTextBox2.Tag.ToString().Split(',');
        //    //Point3d point = new Point3d(double.Parse(strpo[0]), double.Parse(strpo[1]), double.Parse(strpo[2]));
        //    //using (Teigha.GraphicsSystem.View pView = helperOwe.ActiveView)
        //    //{
        //    //    pView.Dolly(point - pView.Position);
        //    //}
        //}
    }
}
