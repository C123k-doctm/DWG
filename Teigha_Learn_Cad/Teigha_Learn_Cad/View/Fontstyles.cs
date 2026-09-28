
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Teigha.DatabaseServices;
using TeighaCad_Bll;
using static ComponentFactory.Krypton.Toolkit.KryptonCheckedListBox;

namespace Teigha_Learn_Cad
{
    public partial class Fontstyles : KryptonForm
    {
        public TeighaBase BasefDB;
        public Fontstyles(ref TeighaBase _aseF)
        {
            InitializeComponent();
            BasefDB = _aseF;
            setcobxlist();
            GetDatabaseFonts();
        }

        private void Fontstyles_Load(object sender, EventArgs e)
        {
        }


        #region 私有方法
        List<KeyValuePair<string, string>> Fontspaths = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// 获取计算机包含字体
        /// </summary>
        public void setcobxlist()
        {
            string folderFullName = System.Environment.GetEnvironmentVariable("windir") + "\\fonts";
            if (Directory.Exists(folderFullName))
            {
                DirectoryInfo TheFolder = new DirectoryInfo(folderFullName);
                foreach (FileInfo NextFile in TheFolder.GetFiles())
                {
                    if (NextFile.Exists)
                    {
                        if (NextFile.Extension.ToLower().Contains("ttf") || NextFile.Extension.ToLower().Contains("ttc"))
                        {
                            Fontspaths.Add(new KeyValuePair<string, string>(NextFile.Name, NextFile.FullName));
                        }
                    }
                }
            }
            else
            {
                KryptonMessageBox.Show("未找到字体文件！");
            }
            kryptonListBox1.DataSource = Fontspaths;
            kryptonListBox1.DisplayMember = "Key";
            kryptonListBox1.ValueMember = "Value";
            if (kryptonListBox1.Items.Count > 0)
            {
                kryptonListBox1.SelectedIndex = 0;
            }
        }


        List<string> StrList = new List<string>();
        /// <summary>
        /// 获取传入文档包含字体
        /// </summary>
        public void GetDatabaseFonts()
        {
            using (var trans = BasefDB.database.TransactionManager.StartTransaction())
            {
                using (TextStyleTable txtstyles = (TextStyleTable)trans.GetObject(BasefDB.database.TextStyleTableId, OpenMode.ForRead))
                {
                    string exCADlow = ConnectionStrings.GetAppConfig("CadExtension");
                    foreach (ObjectId item in txtstyles)
                    {
                        using (TextStyleTableRecord txtse = (TextStyleTableRecord)trans.GetObject(item, OpenMode.ForRead))
                        {
                            if (exCADlow.Contains(Path.GetExtension(txtse.FileName).ToLower()) && Path.GetExtension(txtse.FileName).ToLower() != "")
                            {
                                StrList.Add(txtse.FileName);
                            }
                        }
                    }
                }
            }
            //去重
            StrList = StrList.Distinct().ToList();
            for (int i = 0; i < StrList.Count(); i++)
            {
                KryptonListItem Kitem = new KryptonListItem();
                Kitem.ShortText = StrList[i];
                var listfin = Fontspaths.Where(s1 => s1.Key.ToLower() == StrList[i].ToLower());
                if (listfin.Count() <= 0)
                {
                    Kitem.ImageTransparentColor = Color.Red;
                    Kitem.LongText = "（当前电脑不存在此字体）";
                }
                kryptonCheckedListBox.Items.Add(Kitem);

            }
        }


        #endregion

        private void kryptonCheckBox2_CheckStateChanged(object sender, EventArgs e)
        {
            if (kryptonCheckBox2.Checked)
            {
                kryptonCheckBox1.Checked = false;
                for (int i = 0; i < kryptonCheckedListBox.Items.Count; i++)
                {
                    kryptonCheckedListBox.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < kryptonCheckedListBox.Items.Count; i++)
                {
                    kryptonCheckedListBox.SetItemChecked(i, false);
                }
            }
        }

        private void kryptonCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (kryptonCheckBox1.Checked)
            {
                kryptonCheckBox2.Checked = false;
                for (int i = 0; i < kryptonCheckedListBox.Items.Count; i++)
                {
                    KryptonListItem Kitem = (KryptonListItem)kryptonCheckedListBox.Items[i];
                    if (Kitem.LongText == "（当前电脑不存在此字体）")
                    {
                        kryptonCheckedListBox.SetItemChecked(i, true);
                    }

                }
            }
        }

        public Font Newf = new Font("宋体", 13);
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                //设置高质量插值法    
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度    
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                //消除锯齿  
                //g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PageUnit = GraphicsUnit.Pixel;

                g.DrawString("字体预览效果", Newf, Brushes.Black, 100, 50);
                g.DrawString("1 2 3 4 5 6 7 8 9 ", Newf, Brushes.Black, 100, 80);
                g.DrawString("~！@#￥%……&*（）——+", Newf, Brushes.Black, 100, 110);
                g.Save();
            }
        }

        private void kryptonListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kryptonListBox1.SelectedItem != null)
            {
                KeyValuePair<string, string> kryitem = (KeyValuePair<string, string>)kryptonListBox1.SelectedItem;
                if (File.Exists(kryitem.Value))
                {

                    using (PrivateFontCollection pfc = new PrivateFontCollection())
                    {
                        try
                        {
                            pfc.AddFontFile(kryitem.Value);
                            Newf = new Font(pfc.Families[0], 13);
                            panel1.Invalidate();

                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                        }
                    }

                }
                else
                {
                    kryptonListBox1.Items.Remove(kryptonListBox1.SelectedItem);
                }
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            CheckedItemCollection items = kryptonCheckedListBox.CheckedItems;
            if (items.Count > 0)
            {
                string rep = "";
                if (kryptonListBox1.SelectedItem != null)
                {
                    KeyValuePair<string, string> kryitem = (KeyValuePair<string, string>)kryptonListBox1.SelectedItem;
                    rep = kryitem.Key;
                }
                else
                {
                    KryptonMessageBox.Show("请选择你要替换的字体！");
                    return;
                }


                List<string> ListNew = new List<string>();
                foreach (KryptonListItem item in items)
                    ListNew.Add(item.ShortText);

                try
                {
                    HostApplicationServices.WorkingDatabase = BasefDB.database;

                    if (BasefDB.database != null)
                    {
                        using (var trans = BasefDB.database.TransactionManager.StartTransaction())
                        {
                            BasefDB.database.TileMode = false;
                            using (TextStyleTable txtstyles = (TextStyleTable)trans.GetObject(BasefDB.database.TextStyleTableId, OpenMode.ForWrite))
                            {
                                foreach (ObjectId item in txtstyles)
                                {
                                    TextStyleTableRecord txtse = (TextStyleTableRecord)trans.GetObject(item, OpenMode.ForWrite);
                                    if (ListNew.IndexOf(txtse.FileName) != -1)
                                    {
                                        txtse.BigFontFileName = rep;
                                        txtse.FileName = rep;
                                    }

                                }
                            }
                            BasefDB.database.TileMode = true;
                            trans.Commit();
                        }

                    }
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                KryptonMessageBox.Show("请选择左侧需要被替换的字体！");
            }
        }
    }
}
