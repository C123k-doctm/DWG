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
using Teigha.GraphicsInterface;
using Teigha_Learn_Cad;

namespace Teigha_Learn_Cad
{
    public partial class ManageLayer : KryptonForm
    {
        TeighaBase baseFN;
        public ManageLayer(ref TeighaBase baseF)
        {
            InitializeComponent();
            baseFN = baseF;
        }

        private void ManageLayer_Load(object sender, EventArgs e)
        {
            BindDATAS();
        }

        public void BindDATAS()
        {
            kryDataLayerView.Rows.Clear();
            if (baseFN != null)
            {
                //LinetypeTable tabs = baseFN.database.LinetypeTableId;
                using (var trans = baseFN.database.TransactionManager.StartTransaction())
                {
                    using (LinetypeTable tabs = baseFN.database.LinetypeTableId.GetObject(OpenMode.ForRead, false, true) as LinetypeTable)
                    {
                        KryptonDataGridViewComboBoxColumn boxcol = kryDataLayerView.Columns[kryDataLayerView.Columns.Count - 1] as KryptonDataGridViewComboBoxColumn;
                        foreach (ObjectId item in tabs) 
                        {
                            using (LinetypeTableRecord Reco = item.GetObject(OpenMode.ForRead, false, true) as LinetypeTableRecord)
                            {
                                if (Reco != null)
                                {
                                    boxcol.Items.Add(Reco.Name);

                                }
                            }
                        }
                    }

                    string[] nameList = System.Enum.GetNames(typeof(LineWeight));
                    KryptonDataGridViewComboBoxColumn boxcol2 = kryDataLayerView.Columns[kryDataLayerView.Columns.Count - 2] as KryptonDataGridViewComboBoxColumn;
                    boxcol2.Items.AddRange(nameList);

                    using (LayerTable lineType = (LayerTable)trans.GetObject(baseFN.database.LayerTableId, OpenMode.ForRead, false, true))
                    {
                        int numSum = 0;
                        int IsShow = 0;
                        int i = 0;
                        foreach (ObjectId item in lineType)
                        {
                            using (LayerTableRecord ent = trans.GetObject(item, OpenMode.ForWrite, false, true) as LayerTableRecord)
                            {
                                object[] objitem = new object[8];
                                objitem[0] = ent.Id;
                                objitem[1] = ent.Name;
                                objitem[2] = ent.IsOff ? Properties.Resources.关灯 : Properties.Resources.开灯;
                                objitem[3] = ent.IsFrozen ? Properties.Resources.冻结 : Properties.Resources.太阳;
                                objitem[4] = ent.IsLocked ? Properties.Resources.锁 : Properties.Resources.解锁;
                                objitem[5] = ent.Color.ColorValue.ToString();
                                objitem[6] = ent.LineWeight.ToString();
                                LinetypeTableRecord rec = ent.LinetypeObjectId.GetObject(OpenMode.ForRead, false, true) as LinetypeTableRecord;
                                objitem[7] = rec.Name;
                                kryDataLayerView.Rows.Add(objitem);
                                kryDataLayerView.Rows[i].Cells[kryDataLayerView.Columns.Count-3].Style.BackColor = ent.Color.ColorValue;
                                kryDataLayerView.Rows[i].Cells[kryDataLayerView.Columns.Count - 4].Tag = ent.IsLocked;
                                kryDataLayerView.Rows[i].Cells[kryDataLayerView.Columns.Count - 5].Tag = ent.IsFrozen;
                                kryDataLayerView.Rows[i].Cells[kryDataLayerView.Columns.Count - 6].Tag = ent.IsOff;
                                numSum++;
                                if (!ent.IsHidden)
                                    IsShow++;
                            }
                            i++;
                        }
                        toolStripStatusLabel1.Text = "全部： 显示了 " + IsShow + " 个图层，共 " + numSum + " 个图层";
                    }
                    
                }
            }


        }

        /// <summary>
        /// 错误提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryDataLayerView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        //选中修改图片颜色
        private void kryDataLayerView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.RowIndex != -1)
            {
                if (colorDialogset.ShowDialog() == DialogResult.OK)
                {
                    KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    Colu.Style.BackColor = colorDialogset.Color;
                    updateColor(e.RowIndex, e.ColumnIndex);
                }
            }
            else if (e.ColumnIndex == 4 && e.RowIndex != -1)
            {
                DataGridViewImageCell Colu = (DataGridViewImageCell)kryDataLayerView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if ((bool)Colu.Tag)
                {
                    Colu.Value = Properties.Resources.解锁;
                    Colu.Tag = false;
                }
                else
                {
                    Colu.Value = Properties.Resources.锁;
                    Colu.Tag = true;
                }

                if (!UpDateIsLocked(e.RowIndex, e.ColumnIndex))
                {
                    if ((bool)Colu.Tag)
                    {
                        Colu.Value = Properties.Resources.解锁;
                        Colu.Tag = false;
                    }
                    else
                    {
                        Colu.Value = Properties.Resources.锁;
                        Colu.Tag = true;
                    }
                }

            }
            else if (e.ColumnIndex == 3 && e.RowIndex != -1)
            {
                DataGridViewImageCell Colu = (DataGridViewImageCell)kryDataLayerView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if ((bool)Colu.Tag)
                {
                    Colu.Value = Properties.Resources.太阳;
                    Colu.Tag = false;
                }
                else
                {
                    Colu.Value = Properties.Resources.冻结;
                    Colu.Tag = true;
                }
                if (!UpDateIsFrozen(e.RowIndex, e.ColumnIndex))
                {
                    if ((bool)Colu.Tag)
                    {
                        Colu.Value = Properties.Resources.太阳;
                        Colu.Tag = false;
                    }
                    else
                    {
                        Colu.Value = Properties.Resources.冻结;
                        Colu.Tag = true;
                    }
                }
            }
            else if (e.ColumnIndex == 2 && e.RowIndex != -1)
            {
                DataGridViewImageCell Colu = (DataGridViewImageCell)kryDataLayerView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if ((bool)Colu.Tag)
                {
                    Colu.Value = Properties.Resources.开灯;
                    Colu.Tag = false;
                }
                else
                {
                    Colu.Value = Properties.Resources.关灯;
                    Colu.Tag = true;
                }
                if (!UpDateIsOff(e.RowIndex, e.ColumnIndex))
                {
                    if ((bool)Colu.Tag)
                    {
                        Colu.Value = Properties.Resources.开灯;
                        Colu.Tag = false;
                    }
                    else
                    {
                        Colu.Value = Properties.Resources.关灯;
                        Colu.Tag = true;
                    }
                }
            }
        }

        #region 修改图层 颜色，关闭，冻结，锁定，名称，线宽，线类型
        public void updateColor(int RowIndex, int ColumnIndex)
        {
            using (var trans = baseFN.database.TransactionManager.StartTransaction())
            {
                KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[RowIndex].Cells[0];
                KryptonDataGridViewTextBoxCell Colu2 = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[RowIndex].Cells[ColumnIndex];
                ObjectId Objid = (ObjectId)Colu.Value;
                if (Objid.IsValid)
                {
                    using (LayerTableRecord ent = trans.GetObject(Objid, OpenMode.ForWrite, false, true) as LayerTableRecord)
                    {
                        ent.Color = Teigha.Colors.Color.FromColor(Colu2.Style.BackColor);
                    }
                }
                MainForms Form = (MainForms)this.Owner;
                if (Form.CAD.helperDevice != null)
                {
                    Form.CAD.helperDevice.Update();
                    Form.CAD.helperDevice.Invalidate();
                }
                Form.CAD.Invalidate();
                trans.Commit();
            }
        }

        public bool UpDateIsOff(int RowIndex, int ColumnIndex)
        {
            try
            {
                using (var trans = baseFN.database.TransactionManager.StartTransaction())
                {
                    KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[RowIndex].Cells[0];
                    DataGridViewImageCell Colu2 = (DataGridViewImageCell)kryDataLayerView.Rows[RowIndex].Cells[ColumnIndex];
                    ObjectId Objid = (ObjectId)Colu.Value;
                    if (Objid.IsValid)
                    {
                        using (LayerTableRecord ent = trans.GetObject(Objid, OpenMode.ForWrite, false, true) as LayerTableRecord)
                        {
                            ent.IsOff = (bool)Colu2.Tag;
                        }
                    }
                    trans.Commit();
                }
                MainForms Form = (MainForms)this.Owner;
                if (Form.CAD.helperDevice != null)
                {
                    Form.CAD.helperDevice.Update();
                    Form.CAD.helperDevice.Invalidate();
                }
                Form.CAD.Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateIsFrozen(int RowIndex, int ColumnIndex)
        {
            try
            {
                using (var trans = baseFN.database.TransactionManager.StartTransaction())
                {
                    KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[RowIndex].Cells[0];
                    DataGridViewImageCell Colu2 = (DataGridViewImageCell)kryDataLayerView.Rows[RowIndex].Cells[ColumnIndex];
                    ObjectId Objid = (ObjectId)Colu.Value;
                    if (Objid.IsValid)
                    {
                        using (LayerTableRecord ent = trans.GetObject(Objid, OpenMode.ForWrite, false, true) as LayerTableRecord)
                        {
                            ent.IsFrozen = (bool)Colu2.Tag;
                        }
                    }
                    trans.Commit();
                }
                MainForms Form = (MainForms)this.Owner;
                if (Form.CAD.helperDevice != null)
                {
                    Form.CAD.helperDevice.Update();
                    Form.CAD.helperDevice.Invalidate();
                }
                Form.CAD.Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateIsLocked(int RowIndex, int ColumnIndex)
        {
            try
            {
                using (var trans = baseFN.database.TransactionManager.StartTransaction())
                {
                    KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[RowIndex].Cells[0];
                    DataGridViewImageCell Colu2 = (DataGridViewImageCell)kryDataLayerView.Rows[RowIndex].Cells[ColumnIndex];
                    ObjectId Objid = (ObjectId)Colu.Value;
                    if (Objid.IsValid)
                    {
                        using (LayerTableRecord ent = trans.GetObject(Objid, OpenMode.ForWrite, false, true) as LayerTableRecord)
                        {
                            ent.IsLocked = (bool)Colu2.Tag;
                        }
                    }

                    trans.Commit();
                }
                MainForms Form = (MainForms)this.Owner;
                if (Form.CAD.helperDevice != null)
                {
                    Form.CAD.helperDevice.Update();
                    Form.CAD.helperDevice.Invalidate();
                }
                Form.CAD.Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateLayerName(int RowIndex, int ColumnIndex,string name)
        {
            try
            {
                using (var trans = baseFN.database.TransactionManager.StartTransaction())
                {
                    KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[RowIndex].Cells[0];
                    ObjectId Objid = (ObjectId)Colu.Value;
                    if (Objid.IsValid)
                    {
                        using (LayerTableRecord ent = trans.GetObject(Objid, OpenMode.ForWrite, false, true) as LayerTableRecord)
                        {
                            ent.Name = name;
                        }
                    }
                    trans.Commit();
                }
                MainForms Form = (MainForms)this.Owner;
                if (Form.CAD.helperDevice != null)
                {
                    Form.CAD.helperDevice.Update();
                    Form.CAD.helperDevice.Invalidate();
                }
                Form.CAD.Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateLineWeight(int RowIndex, int ColumnIndex, string LineWeightname)
        {
            try
            {
                using (var trans = baseFN.database.TransactionManager.StartTransaction())
                {
                    KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[RowIndex].Cells[0];
                    ObjectId Objid = (ObjectId)Colu.Value;
                    if (Objid.IsValid)
                    {
                        using (LayerTableRecord ent = trans.GetObject(Objid, OpenMode.ForWrite, false, true) as LayerTableRecord)
                        {
                            ent.LineWeight = (LineWeight)Enum.Parse(typeof(LineWeight), LineWeightname); ;
                        }
                    }
                    trans.Commit();
                }
                MainForms Form = (MainForms)this.Owner;
                if (Form.CAD.helperDevice != null)
                {
                    Form.CAD.helperDevice.Update();
                    Form.CAD.helperDevice.Invalidate();
                }
                Form.CAD.Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateLinetype(int RowIndex, int ColumnIndex, string Linetypetname)
        {
            try
            {
                using (var trans = baseFN.database.TransactionManager.StartTransaction())
                {
                    KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[RowIndex].Cells[0];
                    ObjectId Objid = (ObjectId)Colu.Value;
                    if (Objid.IsValid)
                    {
                        using (LayerTableRecord ent = trans.GetObject(Objid, OpenMode.ForWrite, false, true) as LayerTableRecord)
                        {
                            using (LinetypeTable tabs = baseFN.database.LinetypeTableId.GetObject(OpenMode.ForRead, false, true) as LinetypeTable)
                            {
                                if (tabs.Has(Linetypetname))
                                {
                                    ent.LinetypeObjectId = tabs[Linetypetname];
                                }
                                else
                                {
                                    MessageBox.Show("未找到当前线段类型！");
                                    return false;
                                }
                            }
                        }
                    }
                    trans.Commit();
                }
                MainForms Form = (MainForms)this.Owner;
                if (Form.CAD.helperDevice != null)
                {
                    Form.CAD.helperDevice.Update();
                    Form.CAD.helperDevice.Invalidate();
                }
                Form.CAD.Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool DeleteEntity(DataGridViewSelectedRowCollection rowcell)
        {
            try
            {
                using (var trans = baseFN.database.TransactionManager.StartTransaction())
                {
                    KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)rowcell[0].Cells[0];
                    ObjectId Objid = (ObjectId)Colu.Value;
                    if (Objid.IsValid)
                    {
                        using (LayerTableRecord ent = trans.GetObject(Objid, OpenMode.ForWrite, false, true) as LayerTableRecord)
                        {
                            ent.Erase(true);
                        }
                    }
                    trans.Commit();
                }
                MainForms Form = (MainForms)this.Owner;
                if (Form.CAD.helperDevice != null)
                {
                    Form.CAD.helperDevice.Update();
                    Form.CAD.helperDevice.Invalidate();
                }
                Form.CAD.Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            BindDATAS();
        }

        string OldStr = "";
        
        /// <summary>
        /// 开始编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryDataLayerView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex != -1)
            {
                KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                OldStr = Colu.Value.ToString();
            }
            else if (e.ColumnIndex == 6 && e.RowIndex != -1)
            {
                KryptonDataGridViewComboBoxCell Colu = (KryptonDataGridViewComboBoxCell)kryDataLayerView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                 OldStr = Colu.Value.ToString();
            }
            else if (e.ColumnIndex == 7 && e.RowIndex != -1)
            {
                KryptonDataGridViewComboBoxCell Colu = (KryptonDataGridViewComboBoxCell)kryDataLayerView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                 OldStr = Colu.Value.ToString();
            }
        }

        /// <summary>
        /// 结束编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryDataLayerView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex != -1)
            {
                KryptonDataGridViewTextBoxCell Colu = (KryptonDataGridViewTextBoxCell)kryDataLayerView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string newStr = Colu.Value.ToString();
                if (string.IsNullOrWhiteSpace(newStr))
                {
                    MessageBox.Show("图层名称不能为空！");
                    Colu.Value = OldStr;
                    return;
                }
                if (newStr != OldStr)
                {
                    if (!UpDateLayerName(e.RowIndex, e.ColumnIndex, newStr))
                    {
                        Colu.Value = OldStr;
                    }
                }
            }
            else if(e.ColumnIndex == 6 && e.RowIndex != -1)
            {
                KryptonDataGridViewComboBoxCell Colu = (KryptonDataGridViewComboBoxCell)kryDataLayerView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string newStr = Colu.Value.ToString();
                if (string.IsNullOrWhiteSpace(newStr))
                {
                    MessageBox.Show("请选择线宽！");
                    Colu.Value = OldStr;
                    return;
                }
                if (newStr != OldStr)
                {
                    if (!UpDateLineWeight(e.RowIndex, e.ColumnIndex, newStr))
                    {
                        Colu.Value = OldStr;
                    }
                }
            }
            else if (e.ColumnIndex == 7 && e.RowIndex != -1)
            {
                KryptonDataGridViewComboBoxCell Colu = (KryptonDataGridViewComboBoxCell)kryDataLayerView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string newStr = Colu.Value.ToString();
                if (string.IsNullOrWhiteSpace(newStr))
                {
                    MessageBox.Show("请选择线段类型！");
                    Colu.Value = OldStr;
                    return;
                }
                if (newStr != OldStr)
                {
                    if (!UpDateLinetype(e.RowIndex, e.ColumnIndex, newStr))
                    {
                        Colu.Value = OldStr;
                    }
                }
            }

        }

        /// <summary>
        /// 新建图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            using (var trans = baseFN.database.TransactionManager.StartTransaction())
            {
                using (LayerTable lineType = (LayerTable)trans.GetObject(baseFN.database.LayerTableId, OpenMode.ForWrite, false, true))
                {
                    string name = KryptonInputBox.Show("请输入新建图层的名称！", "添加图层");
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        if (!lineType.Has(name))
                        {
                            LayerTableRecord Layrec = new LayerTableRecord();
                            Layrec.Name = name;
                            lineType.Add(Layrec);
                        }
                        else
                        {
                            MessageBox.Show("当前名称的图层已存在！");
                        }
                    }
                    trans.Commit();
                }
            }
            BindDATAS();
        }

        /// <summary>
        /// 删除选中图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection DAtaSelect = kryDataLayerView.SelectedRows;
            if (DAtaSelect != null && DAtaSelect.Count > 0)
            {
                if (MessageBox.Show("警告，删除后不可恢复,是否确定要删除！","提示",MessageBoxButtons.YesNo)==DialogResult.Yes)
                {
                    DeleteEntity(DAtaSelect);
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的项！");
            }
            BindDATAS();
        }
    }
}
