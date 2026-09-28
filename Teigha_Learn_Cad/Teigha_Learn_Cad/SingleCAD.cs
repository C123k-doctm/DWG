
using ComponentFactory.Krypton.Navigator;
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Teigha.DatabaseServices;
using Teigha.Geometry;

namespace Teigha_Learn_Cad
{
    public partial class SingleCAD : KryptonForm
    {
        public SingleCAD()
        {
            InitializeComponent();
        }
        public KryptonPage BasePage;
        public TeighaBase DBForm;
        public string Dwgpath = "";

        private void SingleCAD_Load(object sender, EventArgs e)
        {

        }

        public void LoadPage(string _Path)
        {
            //清空
            kryNavigaOne.Pages.Clear();
            DBForm = new TeighaBase();
            DBForm.TopLevel = false;
            DBForm.Dock = DockStyle.Fill;
            DBForm.Activate();
            //DBForm.ContextMenuStrip = contextMenu;
            DBForm.FormBorderStyle = FormBorderStyle.None;
            DBForm.file_open_handlerNotBar(_Path);
            Dwgpath = _Path;
            try
            {
                if (DBForm.database != null)
                {
                    HostApplicationServices.WorkingDatabase = DBForm.database;
                    LayoutManager Layout = DBForm.lm;
                    if (DBForm.database.LayoutDictionaryId != null && DBForm.database.LayoutDictionaryId.IsValid)
                    {
                        using (DBDictionary layoutDict = (DBDictionary)DBForm.database.LayoutDictionaryId.GetObject(OpenMode.ForRead))
                        {
                            foreach (DBDictionaryEntry dicEntry in layoutDict)
                            {
                                KryptonPage page = new KryptonPage();
                                page.Name = dicEntry.Key;
                                page.Text = dicEntry.Key;
                                page.TextTitle = dicEntry.Key;
                                page.ToolTipBody = _Path;
                                page.ToolTipTitle = dicEntry.Key;
                                page.Select();
                                //Document pages cannot be docked or auto hidden
                                page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
                                kryNavigaOne.Pages.Add(page);
                                if (dicEntry.Key == Layout.CurrentLayout)
                                {
                                    page.Controls.Add(DBForm);
                                    DBForm.Show();
                                    BasePage = page;
                                    kryNavigaOne.SelectedPage = page;

                                }

                            }
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                KryptonMessageBox.Show("打开失败" + Environment.NewLine + "错误原因：" + ex.Message);
            }

        }

        private void kryNavigaOne_SelectedPageChanged(object sender, EventArgs e)
        {
            KryptonPage page = kryNavigaOne.SelectedPage;
            if (page != null && BasePage != null && page.Name != BasePage.Name)
            {
                HostApplicationServices.WorkingDatabase = DBForm.database;
                LayoutManager Layout = DBForm.lm;
                Layout.CurrentLayout = page.Name;
                BasePage.Controls.Remove(DBForm);
                page.Controls.Add(DBForm);
                BasePage = page;
              
                DBForm.basepanel.Focus();
            }
        }

        #region 公共方法

        #endregion

        private void kryNavigaOne_Selecting(object sender, KryptonPageCancelEventArgs e)
        {

        }

        #region 布设信息
        
        #endregion



    }
}
