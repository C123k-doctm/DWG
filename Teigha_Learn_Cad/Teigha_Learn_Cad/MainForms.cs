using NPOI.SS.Util;
using ComponentFactory.Krypton.Navigator;
using ComponentFactory.Krypton.Toolkit;
using ComponentFactory.Krypton.Ribbon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teigha.DatabaseServices;
using TeighaCad_Bll;
using Teigha.Geometry;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using System.IO;
using Image = System.Drawing.Image;
using System.Threading;
using OdViewExMgd;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;
using Exception = System.Exception;
using DemoLicence;

namespace Teigha_Learn_Cad
{
    
    public partial class MainForms : KryptonForm
    {
        public TeighaBase CAD = null;

        public MainForms()
        {
            InitializeComponent();
            bindblendent();
            QatUndo.Click += QatUndo_Click;
            QatDoit.Click += QatDoit_Click;

            kryptonContextMenuItem10.Click += KryptonContextMenuItem10_Click;
            kryptonContextMenuItem11.Click += KryptonContextMenuItem11_Click;
            kryptonContextMenuItem12.Click += KryptonContextMenuItem12_Click;

            string filePath = "points.txt";

            // 检查文件是否存在
            if (File.Exists(filePath))
            {
                // 文件存在，清空文件内容
                File.WriteAllText(filePath, string.Empty);
                Console.WriteLine("文件已存在，内容已被清空。");
            }
            else
            {
                // 文件不存在，可以选择创建一个空文件
                File.Create(filePath).Dispose();
                Console.WriteLine("文件不存在，已创建一个空文件。");
            }

            Load_the_dwgordxf(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Template\test.dwg", false);
        }

       

        private void MainForms_Load(object sender, EventArgs e)
        {            
            try
            {              
                string info = string.Empty;
                string msg = string.Empty;
                if (LicenceHelper.CheckLicenceKeyIsExists())
                {
                    string keyFile = LicenceHelper.GetDefaultRegisterFileName();
                    info = LicenceHelper.ReadLicenceKey(keyFile);
                }
                else
                {
                    var dialogResult = MessageBox.Show("没有找到默认首选文件，是否手动选择授权文件？", "询问", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Title = "请选择授权文件";
                        openFileDialog.FileName = LicenceHelper.GetDefaultRegisterFileName();
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            var keyFile = openFileDialog.FileName;
                            info = LicenceHelper.ReadLicenceKey(keyFile);
                            //验证成功后，将手动选择的文件复制到程序根目录,且修改为默认名称
                            File.Copy(keyFile, LicenceHelper.GetDefaultRegisterFileName());

                        }
                        else
                        {
                            string computerFile = LicenceHelper.GetComputerInfoAndGenerateFile();
                            if (!string.IsNullOrEmpty(computerFile))
                            {
                                msg = $"您还没有被授权，请将程序根目录下的{computerFile}文件，发送到管理员，获取Licence.";
                            }
                        }
                    }
                    else
                    {
                        string computerFile = LicenceHelper.GetComputerInfoAndGenerateFile();
                        if (!string.IsNullOrEmpty(computerFile))
                        {
                            msg = $"您还没有被授权，请将程序根目录下的{computerFile}文件，发送到管理员，获取Licence.";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(info) && string.IsNullOrEmpty(msg))
                {
                    string[] infos = info.Split(new string[] { "\r\n" }, StringSplitOptions.None);                   
                    if (infos.Length > 0)
                    {
                        var dicInfo = new Dictionary<string, string>();
                        foreach (var info2 in infos)
                        {
                            if (string.IsNullOrEmpty(info2))
                            {
                                continue;
                            }
                            var info2Arr = info2.Split(new string[] { "=" }, StringSplitOptions.None);
                            dicInfo.Add(info2Arr[0], info2Arr[1]);
                        }
                        if (dicInfo.Count > 0)
                        {
                            string localMacAddress = string.Empty;
                            var computerInfo = LicenceHelper.GetComputerInfo();
                            if (computerInfo != null)
                            {
                                localMacAddress = computerInfo["mac"];
                            }
                            //比较本地信息和Licence中的信息是否一致
                            if (localMacAddress == dicInfo["mac"])
                            {
                                var endTime = DateTime.Parse(dicInfo["endTime"]);
                                if (DateTime.Now < endTime)
                                {
                                    //在有效期内，可以使用
                                }
                                else
                                {
                                    msg = $"软件授权使用时间范围：[{endTime}之前]，已过期";
                                }
                            }
                            else
                            {
                                msg = "软件Licence不匹配";
                            }
                        }
                        else
                        {
                            msg = $"软件Licence非法.";
                        }
                    }
                    else
                    {
                        msg = $"软件Licence非法.";
                    }
                }
                if (!string.IsNullOrEmpty(msg))
                {

                    MessageBox.Show(msg);
                    foreach (var control in this.Controls)
                    {
                        (control as Control).Enabled = false;
                    }
                    return;

                }
            }
            catch (Exception ex)
            {
                string error = $"程序异常，请联系管理人员：{ex.Message}\r\n{ex.StackTrace}";
                MessageBox.Show(error);
                foreach (var control in this.Controls)
                {
                    (control as Control).Enabled = false;
                }
            }
        }

        #region 撤销事件
        private void QatDoit_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                CAD.Invoker.RedoStack();
            }
        }

        private void QatUndo_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                CAD.Invoker.UndoStack();
            }
        } 
        #endregion

        #region 控件事件
        //打开
        private void kryGroupButopenfile_Click(object sender, EventArgs e)
        {

            if (DialogResult.OK == openFileDialog.ShowDialog(this))
            {
                Load_the_dwgordxf(openFileDialog.FileName, false);
            }
        }


        private void MainForms_FormClosed(object sender, FormClosedEventArgs e)
        {
            KryptonPageCollection krylist = kryNavigators.Pages;
            foreach (KryptonPage item in krylist)
            {
                TeighaBase basef = SelectDataGridView(item);
                if (basef != null)
                {
                    basef.Invoker.Dispose();
                    if (basef.graphics != null)
                        basef.graphics.Dispose();
                    if (basef.helperDevice != null)
                    {
                        basef.helperDevice.Dispose();
                        basef.helperDevice = null;
                    }
                    if (basef.database != null)
                        basef.database.Dispose();
                }
                
            }
            try
            {
                if (PublicClass.ServiceD != null)
                    PublicClass.ServiceD.Dispose();
            }
            catch { }
        }



        private void kryGroupButCreateDWG_Click(object sender, EventArgs e)
        {
            string tempFile = System.IO.Path.GetTempFileName();
            string path= AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Template\\dwgModel.dwg";
            FileInfo info = new FileInfo(path);
            info.CopyTo(tempFile, true);
            Load_the_dwgordxf(tempFile, false);
        }

        private void kryGroupButFontSet_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                Fontstyles font = new Fontstyles(ref CAD);
                font.Show(this);
            }
        }

        private void kryRibself_adaption_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                CAD.zoom_extents_handler();
            }
        }

        private void kryNavigators_SelectedPageChanged(object sender, EventArgs e)
        {
            TeighaBase basef = SelectDataGridView(kryNavigators.SelectedPage);
            if (basef != null)
            {
                //kryRibself_adaption1.Enabled = true;
                //kryAmplificationbtn.Enabled = true;
                //kryShrinkbtn.Enabled = true;
                krypGroupButRoll.Enabled = true;
                kryGyroscopebtn.Enabled = true;

                kryptonRibbonGroupButton1.Enabled = true;
                kryGroupButFontSet.Enabled = true;

                kryShowLayers.Enabled = true;
                krypGroupCloseSelect.Enabled = true;
                kryGroupButCloseOther.Enabled = true;
                kryGroupButOpenAll.Enabled = true;

                CAD = basef;
                HostApplicationServices.WorkingDatabase = basef.database;
                CAD.basepanel.Focus();
            }
            else
            {
                //kryRibself_adaption1.Enabled = false;
                //kryAmplificationbtn.Enabled = false;
                //kryShrinkbtn.Enabled = false;
                krypGroupButRoll.Enabled = false;
                kryGyroscopebtn.Enabled = false;

                kryptonRibbonGroupButton1.Enabled = false;
                kryGroupButFontSet.Enabled = false;

                kryShowLayers.Enabled = false;
                krypGroupCloseSelect.Enabled = false;
                kryGroupButCloseOther.Enabled = false;
                kryGroupButOpenAll.Enabled = false;

            }
        }

        private void kryAmplificationbtn_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                CAD.Set_zoom(3);
            }
        }

        private void kryShrinkbtn_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                double nums = (double)1 / (double)3;
                CAD.Set_zoom(nums);
            }
        }

        private void kryShowLayers_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                ManageLayer Manlay = new ManageLayer(ref CAD);
                Manlay.Show(this);
            }
        }

        private void krypGroupCloseSelect_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                CAD.LayerHidePitchOn();
            }
        }

        private void kryGroupButCloseOther_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                CAD.LayerHideExceptPitchOn();
            }
        }

        private void kryGroupButOpenAll_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                CAD.LayerShowAll();
            }
        } 
        #endregion

        #region 自定义方法
        /// <summary>
        /// 加载dwg
        /// </summary>
        /// <param name="Path"></param>
        public void Load_the_dwgordxf(string Path, bool Tag)
        {
            KryptonPage page = new KryptonPage();
            page.Name = System.IO.Path.GetFileNameWithoutExtension(Path);
            page.Text = System.IO.Path.GetFileNameWithoutExtension(Path);
            page.TextTitle = System.IO.Path.GetFileNameWithoutExtension(Path);
            page.ToolTipBody = Path;
            page.ToolTipTitle = Path;


            //page.ImageSmall = Resource.note2;
            //page.ImageMedium = Resource.note1;
            //page.ImageLarge = Resource.note;

            ButtonSpecAny bsa = new ButtonSpecAny();
            bsa.Tag = page;
            bsa.Type = PaletteButtonSpecStyle.Close;

            bsa.Click += new EventHandler(OnClosePage);
            page.ButtonSpecs.Add(bsa);

            page.Select();
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);

            SingleCAD Speed = new SingleCAD();
            Speed.TopLevel = false;
            Speed.Tag = Tag;
            Speed.Dock = DockStyle.Fill;
            Speed.LoadPage(Path);
            //Panl.Controls.Add(Speed);
            Speed.FormBorderStyle = FormBorderStyle.None;
            //string name = "[" + System.IO.Path.GetFileNameWithoutExtension(Path) + "]";
            page.Controls.Add(Speed);
            Speed.Show();

            kryNavigators.Pages.Add(page);
            kryNavigators.SelectedPage = page;
            kryNavigators.Invalidate();

            page.Refresh();
        }

        /// <summary>
        /// 关闭page页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosePage(object sender, EventArgs e)
        {
            KryptonPage kar = ((ButtonSpecAny)sender).Tag as KryptonPage;
            if (kar != null)
            {
                TeighaBase basef = SelectDataGridView(kar);
                if (basef != null)
                {
                    basef.Invoker.Dispose();
                    if (basef.graphics != null)
                        basef.graphics.Dispose();
                    if (basef.helperDevice != null)
                        basef.helperDevice.Dispose();
                    if (basef.database != null)
                        basef.database.Dispose();
                    basef.Dispose();
                }
                kar.Dispose();
            }
        }

        /// <summary>
        /// 查找BaseForms;
        /// </summary>
        /// <param name="Conl"></param>
        /// <returns></returns>
        public TeighaBase SelectDataGridView(Control Conl)
        {
            if (Conl == null)
                return null;
            foreach (Control item in Conl.Controls)
            {
                if (item is TeighaBase)
                {
                    return item as TeighaBase;
                }
                else
                    return SelectDataGridView(item);
            }
            return null;
        }


        #endregion

        #region 修改界面配色
        /// <summary>
        /// 修改配色
        /// </summary>
        public void bindblendent()
        {
            kryptonManager1.GlobalPaletteMode = (PaletteModeManager)System.Enum.Parse(typeof(PaletteModeManager), ConnectionStrings.GetAppConfig("Setinterface"), true);
            switch (kryptonManager1.GlobalPaletteMode)
            {
                case PaletteModeManager.ProfessionalSystem:
                    krypGroupblendent.SelectedIndex = 10;
                    break;
                case PaletteModeManager.ProfessionalOffice2003:
                    krypGroupblendent.SelectedIndex = 9;
                    break;
                case PaletteModeManager.Office2007Blue:
                    krypGroupblendent.SelectedIndex = 3;
                    break;
                case PaletteModeManager.Office2007Silver:
                    krypGroupblendent.SelectedIndex = 4;
                    break;
                case PaletteModeManager.Office2007Black:
                    krypGroupblendent.SelectedIndex = 5;
                    break;
                case PaletteModeManager.Office2010Blue:
                    krypGroupblendent.SelectedIndex = 0;
                    break;
                case PaletteModeManager.Office2010Silver:
                    krypGroupblendent.SelectedIndex = 1;
                    break;
                case PaletteModeManager.Office2010Black:
                    krypGroupblendent.SelectedIndex = 2;
                    break;
                case PaletteModeManager.SparkleBlue:
                    krypGroupblendent.SelectedIndex = 6;
                    break;
                case PaletteModeManager.SparkleOrange:
                    krypGroupblendent.SelectedIndex = 7;
                    break;
                case PaletteModeManager.SparklePurple:
                    krypGroupblendent.SelectedIndex = 8;
                    break;
                case PaletteModeManager.Custom:
                    krypGroupblendent.SelectedIndex = 11;
                    break;
                default:
                    break;
            }

            //skinEngine1.SkinFile = @"C:\Users\Administrator\Desktop\teigha_learning_project\Bin\Skins\Calmness.ssk";

        }

        private void krypGroupblendent_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (krypGroupblendent.SelectedIndex)
            {
                case 0:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2010Blue;
                    break;
                case 1:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2010Silver;
                    break;
                case 2:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2010Black;
                    break;
                case 3:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2007Blue;
                    break;
                case 4:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2007Silver;
                    break;
                case 5:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2007Black;
                    break;
                case 6:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.SparkleBlue;
                    break;
                case 7:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.SparkleOrange;
                    break;
                case 8:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.SparklePurple;
                    break;
                case 9:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.ProfessionalOffice2003;
                    break;
                case 10:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.ProfessionalSystem;
                    break;
                case 11:
                    kryptonManager1.GlobalPaletteMode = PaletteModeManager.Custom;
                    break;
                default:
                    break;
            }
            ConnectionStrings.modifyItem("Setinterface", kryptonManager1.GlobalPaletteMode.ToString());

        }

        #endregion

        #region 插入常见实体
        
        private void setStateGBnt(object obj)
        {
            if (CAD != null)
            {
                if (CAD.OperModel != Operations.None)
                {
                    KryptonRibbonGroupButton but = obj as KryptonRibbonGroupButton;
                    if (kryRibGButPen.Checked && kryRibGButPen!=but)
                        kryRibGButPen.Checked = false;
                    if (kryRibGButLine.Checked && kryRibGButLine != but)
                        kryRibGButLine.Checked = false;
                    if (kryRibGButPoliy.Checked && kryRibGButPoliy != but)
                        kryRibGButPoliy.Checked = false;
                    if (kryRibGButHu.Checked && kryRibGButHu != but)
                        kryRibGButHu.Checked = false;
                    if (kryRibGButArc.Checked && kryRibGButArc != but)
                        kryRibGButArc.Checked = false;
                    if (kryRibGButPoint.Checked && kryRibGButPoint != but)
                        kryRibGButPoint.Checked = false;
                }
                CAD.OperModel = Operations.None;
                CAD.basepanel.Cursor = Cursors.Default;
                CAD.DragSelected = false;

            }
        }
        
        private void kryRibGButPen_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (kryRibGButPen.Checked)
                setStateGBnt(sender);
            if (CAD != null)
                CAD.DrawPebs(kryRibGButPen.Checked);
        }

        private void kryRibGButLine_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (kryRibGButLine.Checked)
                setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(kryRibGButLine.Checked, EntityType.Line);
        }

        private void kryRibGButPoliy_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (kryRibGButPoliy.Checked)
                setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(kryRibGButPoliy.Checked, EntityType.Polyline);
        }

        private void kryRibGButHu_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (kryRibGButHu.Checked)
                setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(kryRibGButHu.Checked, EntityType.Arc);
        }

        private void kryRibGButArc_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (kryRibGButArc.Checked)
                setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(kryRibGButArc.Checked, EntityType.Circle);
        }

        private void kryRibButEllipse_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (kryRibButEllipse.Checked)
                setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(kryRibButEllipse.Checked, EntityType.Ellipse);
        }

        private void kryRibGButPoint_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (kryRibGButPoint.Checked)
                setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(kryRibGButPoint.Checked, EntityType.Point);
        }

        private void kryRibGButText_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void kryRibGButMtext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        /// <summary>
        /// 拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryptonRibbonGroupButton4_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (kryptonRibbonGroupButton4.Checked)
                setStateGBnt(sender);
            if (CAD != null)
                CAD.SetmouseTypeOk(kryptonRibbonGroupButton4.Checked);
        }

        private void kryptonRibbonGroupButton6_Click(object sender, EventArgs e)
        {
            if (CAD != null)
                CAD.InsertDwfBy();
        }

        private void kryptonRibbonGroupButton5_Click(object sender, EventArgs e)
        {
            setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertImages();
        }

        private void kryptonRibbonGroupButton7_Click(object sender, EventArgs e)
        {
            setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(true, EntityType.leader);
        }

        private void kryptonRibbonGroupButton8_Click(object sender, EventArgs e)
        {
            setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(true, EntityType.LineDim);
        }

        private void kryptonRibbonGroupButton13_Click(object sender, EventArgs e)
        {
            setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(true, EntityType.ArcLenDim);
        }

        #region 角度值
        //倾斜
        private void KryptonContextMenuItem12_Click(object sender, EventArgs e)
        {
            setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(true, EntityType.AnglerotaDim);
        }

        //直线
        private void KryptonContextMenuItem11_Click(object sender, EventArgs e)
        {
            if (CAD != null)
                CAD.InsertLine(true, EntityType.AngleLengthDim);
        }

        //圆弧
        private void KryptonContextMenuItem10_Click(object sender, EventArgs e)
        {
            setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(true, EntityType.AngleArcDim);
        }
        #endregion

        private void kryptonRibbonGroupButton10_Click(object sender, EventArgs e)
        {
            setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(true, EntityType.ArcDim);
        }

        private void kryptonRibbonGroupButton11_Click(object sender, EventArgs e)
        {
            setStateGBnt(sender);
            if (CAD != null)
                CAD.InsertLine(true, EntityType.Dimension);
        }

        private void kryptonRibbonGroupButton12_Click(object sender, EventArgs e)
        {
            if (CAD != null)
            {
                string StrText = KryptonInputBox.Show("请输入要添加的备注信息，无内容自动取消", "提示");
                if (!string.IsNullOrWhiteSpace(StrText))
                    CAD.AddEntity_Xdata(StrText);
            }
        }





        #endregion

        private void kryptonRibbonGroupButton15_Click(object sender, EventArgs e)
        {
            Image img = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            Graphics g = Graphics.FromImage(img);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size);
            ScreenBody body = new ScreenBody();
            body.BackgroundImage = img;
            body.Show();
        }

        private void excel(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines("tl.txt");

            // 创建Excel对象
            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet("Sheet1");


            // 创建一个样式：单元格背景为浅蓝色并且内容居中显示
            ICellStyle style = workbook.CreateCellStyle();
            style.FillForegroundColor = IndexedColors.LightBlue.Index;
            style.FillPattern = FillPattern.SolidForeground;
            style.VerticalAlignment = VerticalAlignment.Center;  // 垂直居中
            style.Alignment = HorizontalAlignment.Center;  // 水平居中


            //更改列的宽度
            worksheet.SetColumnWidth(1, 5000);
            worksheet.SetColumnWidth(2, 5000);
            worksheet.SetColumnWidth(3, 5000);
            worksheet.SetColumnWidth(4, 5000);
            worksheet.SetColumnWidth(5, 5000);
            worksheet.SetColumnWidth(6, 6000);
            worksheet.SetColumnWidth(7, 6000);


            //创建一个样式：仅单元格内容居中
            ICellStyle style2 = workbook.CreateCellStyle();
            style2.VerticalAlignment = VerticalAlignment.Center;  // 垂直居中
            style2.Alignment = HorizontalAlignment.Center;  // 水平居中



            // 创建Excel表的框架
            IRow row1 = worksheet.CreateRow(0);
            ICell cell1 = row1.CreateCell(0);
            cell1.SetCellValue("序号");
            cell1.CellStyle = style;
            cell1.CellStyle = style;
            ICell cell2 = row1.CreateCell(1);
            cell2.SetCellValue("一级保险");
            cell2.CellStyle = style;
            ICell cell3 = row1.CreateCell(2);
            cell3.SetCellValue("二级级保险");
            cell3.CellStyle = style;
            ICell cell4 = row1.CreateCell(3);
            cell4.SetCellValue("三级保险");
            cell4.CellStyle = style;
            ICell cell5 = row1.CreateCell(4);
            cell5.SetCellValue("四级保险");
            cell5.CellStyle = style;
            ICell cell6 = row1.CreateCell(5);
            cell6.SetCellValue("五级保险");
            cell6.CellStyle = style;
            ICell cell7 = row1.CreateCell(6);
            cell7.SetCellValue("负载");
            cell7.CellStyle = style;
            ICell cell8 = row1.CreateCell(7);
            cell8.SetCellValue("线号、线径及颜色");
            cell8.CellStyle = style;
            ICell cell9 = row1.CreateCell(8);
            cell9.SetCellValue("备注");
            cell9.CellStyle = style;


            int a = 0;
            //将txt内容读取到excel表中
            for (int i = 0; i < lines.Length; i++)
            {

                string[] fields = lines[i].Split('：');
                if (fields[0] == "第1级保险")
                {

                    IRow row = worksheet.CreateRow(a + 1);
                    row.CreateCell(1).SetCellValue(fields[1]);
                    a++;
                }


                //添加负载和线束信息：用负载的前置保险来将负载添加到对应的保险后面
                else if (fields[0] == "前置保险")
                {

                    string[] fieldsfuzai = lines[i - 2].Split('：');
                    string fuzai;
                    fuzai = fieldsfuzai[1];
                    string[] fieldsxianshu = lines[i - 1].Split('：');
                    string xianshu;
                    xianshu = fieldsxianshu[1];
                    string baoxian = fields[1];
                    if (fieldsxianshu[1] == " SPARE（预留）")
                    {
                        fuzai = fieldsxianshu[1];
                        xianshu = null;

                    }
                    for (int iq = 0; iq <= worksheet.LastRowNum; iq++)
                    {
                        var rowq1 = worksheet.GetRow(iq);
                        if (rowq1 != null)
                        {
                            for (int jq = 1; jq < rowq1.LastCellNum; jq++)
                            {
                                var cell = rowq1.GetCell(jq);
                                if (cell != null)
                                {
                                    var cellValue = cell.ToString();
                                    if (cellValue == baoxian)
                                    {
                                        if (rowq1.GetCell(6) == null)
                                        {
                                            var cellq1 = rowq1.CreateCell(6);
                                            cellq1.SetCellValue(fuzai);
                                            var cellq2 = rowq1.CreateCell(7);
                                            cellq2.SetCellValue(xianshu);
                                        }
                                        else
                                        {

                                            int rowIndex = iq;
                                            worksheet.ShiftRows(rowIndex + 1, worksheet.LastRowNum, 1, true, false);
                                            IRow newRowin = worksheet.CreateRow(rowIndex + 1);
                                            ICell cellinf = newRowin.CreateCell(6);
                                            cellinf.SetCellValue(fuzai);
                                            ICell cellinx = newRowin.CreateCell(7);
                                            cellinx.SetCellValue(xianshu);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                //添加2级保险：用2级保险的上一级保险来匹配对应位置
                else if (fields[0] == "上一级保险")
                {
                    string[] fieldserji = lines[i - 1].Split('：');
                    string erji = fieldserji[1];

                    string baoxian = fields[1];
                    //Console.WriteLine(baoxian);
                    for (int iq = 0; iq <= worksheet.LastRowNum; iq++)
                    {
                        var rowq1 = worksheet.GetRow(iq);
                        if (rowq1 != null)
                        {
                            for (int jq = 1; jq < rowq1.LastCellNum; jq++)
                            {
                                var cell = rowq1.GetCell(jq);
                                if (cell != null)
                                {
                                    var cellValue = cell.ToString();
                                    if (cellValue == baoxian)
                                    {

                                        if (rowq1.GetCell(2) == null && rowq1.GetCell(6) == null)
                                        {
                                            var cellq1 = rowq1.CreateCell(2);

                                            cellq1.SetCellValue(erji);
                                        }
                                        else
                                        {
                                            int rowIndex = iq;
                                            worksheet.ShiftRows(rowIndex + 1, worksheet.LastRowNum, 1, true, false);
                                            IRow newRowin = worksheet.CreateRow(rowIndex + 1);
                                            ICell cellinf = newRowin.CreateCell(2);
                                            cellinf.SetCellValue(erji);
                                        }
                                    }
                                }
                            }
                        }

                    }


                }

                else if (fields[0] == "同支路继电器线圈")
                {
                    string[] fieldsfuzai = lines[i - 3].Split('：');
                    string fuzai;
                    fuzai = fieldsfuzai[1];
                    string xianquan;
                    xianquan = fields[1];
                    int xxx = 0;
                    for (int iq = 0; iq <= worksheet.LastRowNum; iq++)
                    {

                        var rowq1 = worksheet.GetRow(iq);
                        if (rowq1 != null)
                        {
                            for (int jq = 1; jq < rowq1.LastCellNum; jq++)
                            {
                                var cell = rowq1.GetCell(jq);
                                if (cell != null)
                                {
                                    var cellValue = cell.ToString();
                                    if (cellValue == fuzai)
                                    {
                                        int rowIndex = iq;
                                        worksheet.ShiftRows(rowIndex + 1, worksheet.LastRowNum, 1, true, false);
                                        IRow newRowin = worksheet.CreateRow(rowIndex + 1);
                                        ICell cellinf = newRowin.CreateCell(6);
                                        cellinf.SetCellValue(xianquan);
                                        xxx = 1;
                                    }
                                }
                            }
                        }
                        if (xxx == 1)
                        {
                            break;
                        }


                    }


                }


            }

            //序号
            for (int i = 0; i < lines.Length; i++)
            {
                var row = worksheet.GetRow(i);
                if (row != null)
                {
                    var cell = row.GetCell(0);
                    if (cell == null)
                    {
                        ICell cell0 = row.CreateCell(0);
                        cell0.SetCellValue(i);

                    }

                }

            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Export To EXCEL";
            sfd.Filter = "xls(*.xlsx)|*.xlsx";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string folderPath = sfd.FileName.ToString();

                MessageBox.Show("您选择的文件保存路径是：" + folderPath);
                FileStream file = new FileStream(folderPath, FileMode.Create, FileAccess.Write);
                workbook.Write(file);
                file.Close();
            }
        }

        private static void DeleteRowsWithSpecificValues(ISheet worksheet)
        {
            for (int rowIndex = worksheet.LastRowNum; rowIndex >= 0; rowIndex--)
            {
                IRow row = worksheet.GetRow(rowIndex);
                if (row != null)
                {
                    ICell cell = row.GetCell(6); // 获取第6列的单元格（索引从0开始，所以是5）
                    if (cell != null && (cell.ToString() == " VCU" || cell.ToString() == " BCM（LOW）" || cell.ToString() == " BCM（HIGH）" || cell.ToString() == " BCM"))
                    {

                        //worksheet.RemoveRow(row);
                        if (rowIndex < worksheet.LastRowNum) // 如果不是最后一行，将下面的行上移
                        {
                            worksheet.ShiftRows(rowIndex + 1, worksheet.LastRowNum, -1);
                        }
                    }
                }
            }
        }


        private void kryptonRibbonGroupButton17_Click(object sender, EventArgs e)
        {
            CAD.fenpei_toTxtToolStripMenuItem_Click(sender, e);
        }

        private void KryptonGroupPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonRibbonGroupButton18_Click(object sender, EventArgs e)
        {
            CAD.toxianshuToolStripMenuItem_Click(sender, e);//线束分析
        }

        static bool IsChineseCharacter(char c)
        {
            // 检查字符是否是汉字
            // 汉字的 Unicode 编码范围为 \u4e00 到 \u9fa5
            return (c >= '\u4e00' && c <= '\u9fa5');
        }
        /// <summary>
        /// 分析表
        /// </summary>
        /// <param name="Path"></param>
        private void Ms_Click(object sender, EventArgs e)
        {
         string[] lines = File.ReadAllLines("text.txt");


            // 创建Excel对象
            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet("Sheet1");


            // 创建一个样式：单元格背景为浅蓝色并且内容居中显示
            ICellStyle style = workbook.CreateCellStyle();
            style.FillForegroundColor = IndexedColors.Green.Index;
            style.FillPattern = FillPattern.SolidForeground;
            style.VerticalAlignment = VerticalAlignment.Center;  // 垂直居中
            style.Alignment = HorizontalAlignment.Center;  // 水平居中
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;


            //更改列的宽度
            worksheet.SetColumnWidth(0, 4000);
            worksheet.SetColumnWidth(1, 8000);
            worksheet.SetColumnWidth(2, 4000);
            worksheet.SetColumnWidth(3, 4000);
            worksheet.SetColumnWidth(4, 4000);
            worksheet.SetColumnWidth(5, 4000);
            worksheet.SetColumnWidth(6, 7000);
            worksheet.SetColumnWidth(7, 7000);
            worksheet.SetColumnWidth(8, 4000);
            worksheet.SetColumnWidth(9, 4000);
            worksheet.SetColumnWidth(10, 4000);
            worksheet.SetColumnWidth(11, 4000);
            worksheet.SetColumnWidth(12, 4000);
            worksheet.SetColumnWidth(13, 4000);
            worksheet.SetColumnWidth(14, 4000);

            //创建一个样式：仅单元格内容居中
            ICellStyle style2 = workbook.CreateCellStyle();
            style2.VerticalAlignment = VerticalAlignment.Center;  // 垂直居中
            style2.Alignment = HorizontalAlignment.Center;  // 水平居中



            ICellStyle style3 = workbook.CreateCellStyle();
            style3.FillForegroundColor = IndexedColors.LightGreen.Index;
            style3.FillPattern = FillPattern.SolidForeground;
            style3.VerticalAlignment = VerticalAlignment.Center;
            style3.Alignment = HorizontalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 30;
            style3.SetFont(font);


            ICellStyle style4 = workbook.CreateCellStyle();
            style4.FillForegroundColor = IndexedColors.SeaGreen.Index;
            style4.FillPattern = FillPattern.SolidForeground;
            style4.VerticalAlignment = VerticalAlignment.Center;  // 垂直居中
            style4.Alignment = HorizontalAlignment.Center;  // 水平居中
            style4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            IFont font2 = workbook.CreateFont();
            font2.FontHeightInPoints = 15;
            style4.SetFont(font2);



            // 创建Excel表的框架

            CellRangeAddress region = new CellRangeAddress(0, 0, 0, 13);
            worksheet.AddMergedRegion(region);
            IRow row0 = worksheet.CreateRow(0);
            row0.HeightInPoints = 30;
            ICell cell = row0.CreateCell(0);
            cell.SetCellValue("线束EMC特性分析表");
            cell.CellStyle = style3;

            CellRangeAddress region2 = new CellRangeAddress(1, 1, 0, 8);
            worksheet.AddMergedRegion(region2);
            CellRangeAddress region3 = new CellRangeAddress(1, 1, 9, 13);
            worksheet.AddMergedRegion(region3);
            IRow row02 = worksheet.CreateRow(1);
            row02.HeightInPoints = 20;
            for (int i = 0; i < 9; i++)
            {
                ICell cell02 = row02.CreateCell(i);
                cell02.SetCellValue("线束基本信息");
                cell02.CellStyle = style4;
            }
            for (int i = 9; i < 14; i++)
            {
                ICell cell03 = row02.CreateCell(i);
                cell03.SetCellValue("线束电气特性");
                cell03.CellStyle = style4;
            }

            IRow row1 = worksheet.CreateRow(2);
            row1.HeightInPoints = 15;
            ICell cell1 = row1.CreateCell(0);
            cell1.SetCellValue("线束所属系统");
            cell1.CellStyle = style;
            cell1.CellStyle = style;
            ICell cell2 = row1.CreateCell(1);
            cell2.SetCellValue("线束名称");
            cell2.CellStyle = style;
            ICell cell3 = row1.CreateCell(2);
            cell3.SetCellValue("线束标号");
            cell3.CellStyle = style;
            ICell cell4 = row1.CreateCell(3);
            cell4.SetCellValue("线径(mm)");
            cell4.CellStyle = style;
            ICell cell5 = row1.CreateCell(4);
            cell5.SetCellValue("颜色");
            cell5.CellStyle = style;
            ICell cell6 = row1.CreateCell(5);
            cell6.SetCellValue("长度(mm)");
            cell6.CellStyle = style;
            ICell cell7 = row1.CreateCell(6);
            cell7.SetCellValue("起点接口");
            cell7.CellStyle = style;
            ICell cell8 = row1.CreateCell(7);
            cell8.SetCellValue("终点接口");
            cell8.CellStyle = style;
            ICell cell9 = row1.CreateCell(8);
            cell9.SetCellValue("线束EMC处理");
            cell9.CellStyle = style;
            ICell cell10 = row1.CreateCell(9);
            cell10.SetCellValue("信号类型");
            cell10.CellStyle = style;
            ICell cell11 = row1.CreateCell(10);
            cell11.SetCellValue("大电流");
            cell11.CellStyle = style;
            ICell cell12 = row1.CreateCell(11);
            cell12.SetCellValue("连接感性负载");
            cell12.CellStyle = style;
            ICell cell13 = row1.CreateCell(12);
            cell13.SetCellValue("数据");
            cell13.CellStyle = style;
            ICell cell14 = row1.CreateCell(13);
            cell14.SetCellValue("PWM");
            cell14.CellStyle = style;

            int b = 0;
            for (int i = 0; i < lines.Length; i++)
            {

                string[] changdu = lines[i].Split('：');

                if (changdu[0] == "线束长度" && i >= 4)
                {
                    string[] biaohao = lines[i - 4].Split('：');
                    string[] sejing = lines[i - 3].Split('：');
                    string[] xiansejing = sejing[1].Split(' ');
                    string xianse = "";
                    string xianjing = "";
                    if (sejing[1] == null)
                    {
                        xianse = null;
                        xianjing = null;
                    }
                    else if (xiansejing.Length == 1)
                    {

                        foreach (char c in xiansejing[0])
                        {
                            if (char.IsLetter(c))
                            {
                                xianse = xiansejing[0];
                                xianjing = null;
                            }
                            else if (char.IsDigit(c))
                            {
                                xianse = null;
                                xianjing = xiansejing[0];
                            }
                        }
                    }
                    else if (xiansejing.Length == 2)
                    {
                        xianse = xiansejing[0];
                        xianjing = xiansejing[1];
                    }

                    // 跳过空行的处理逻辑
                    if (i >= 2 && // 确保有足够的上下文行来检查 i-2 和 i-1
                        (string.IsNullOrEmpty(lines[i - 2].Split('：')[1]) || lines[i - 2].Split('：')[1] == "0") &&
                        (string.IsNullOrEmpty(lines[i - 1].Split('：')[1]) || lines[i - 1].Split('：')[1] == "0"))
                    {
                        // 如果第7列和第8列都为空或为0，跳过这一行
                        continue;
                    }


                    string[] qidian = lines[i - 2].Split('：');
                    string[] jiekou = qidian[1].Split('/');

                    string[] zhongdian = lines[i - 1].Split('：');

                    IRow row = worksheet.CreateRow(b + 3);
                    ICell cell010 = row.CreateCell(1);
                    cell010.SetCellValue(jiekou[0]);
                    cell010.CellStyle = style2;
                    ICell cell051 = row.CreateCell(5);
                    cell051.SetCellValue(changdu[1]);
                    cell051.CellStyle = style2;
                    ICell cell021 = row.CreateCell(2);
                    cell021.SetCellValue(biaohao[1]);
                    cell021.CellStyle = style2;
                    ICell cell040 = row.CreateCell(4);
                    cell040.SetCellValue(xianse);
                    cell040.CellStyle = style2;
                    ICell cell031 = row.CreateCell(3);
                    cell031.SetCellValue(xianjing);
                    cell031.CellStyle = style2;

                    // 判断并填充第10列信号类型
                    ICell cell100 = row.CreateCell(9); // 第10列的索引为9
                    double xianjingValue;
                    if (double.TryParse(xianjing, out xianjingValue))
                    {
                        if (xianjingValue > 0.5)
                        {
                            cell100.SetCellValue("功率线");
                        }
                        else
                        {
                            cell100.SetCellValue("信号线");
                        }
                    }
                    else
                    {
                        cell100.SetCellValue("未知");
                    }
                    cell100.CellStyle = style2;

                    // 判断并填充第11列大电流
                    ICell cell110 = row.CreateCell(10); // 第11列的索引为10
                    if (xianjingValue >= 0.75)
                    {
                        cell110.SetCellValue("是");
                    }
                    else
                    {
                        cell110.SetCellValue("否");
                    }
                    cell110.CellStyle = style2;

                    ICell cell061 = row.CreateCell(6);
                    cell061.SetCellValue(qidian[1]);
                    cell061.CellStyle = style2;
                    ICell cell071 = row.CreateCell(7);
                    //if (zhongdian.Length > 1)
                    
                    cell071.SetCellValue(zhongdian[1]);
                    
                    cell071.CellStyle = style2;
                    b++;
                }

            }


            int lastRowNum = worksheet.LastRowNum;
            // 上一个单元格的值
            string prevValue = null;
            // 开始合并的行号
            int startRow = 3;

            string fileName = Path.GetFileName(openFileDialog.FileName);
            string resultfile = "";
            int index = 0;
            int l = 0;
            for (int i = 0; i < fileName.Length; i++)
            {
                char currentChar = fileName[i];

                if (IsChineseCharacter(currentChar))
                {
                    index = i;
                    break;
                }
                else
                {
                    if (currentChar.ToString() == ".")
                    {
                        l = fileName.Length;
                        break;
                    }
                    l++;
                    resultfile += currentChar;
                }
            }
            if (l != fileName.Length)
            {
                resultfile = "";
                for (int i = index; i < fileName.Length; i++)
                {
                    char currentChar = fileName[i];

                    if (IsChineseCharacter(currentChar))
                    {
                        resultfile += currentChar;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            CellRangeAddress fileregion = new CellRangeAddress(3, worksheet.LastRowNum, 0, 0);
            worksheet.AddMergedRegion(fileregion);
            IRow filerow = worksheet.GetRow(3);
            ICell filecell = filerow.CreateCell(0);
            filecell.SetCellValue(resultfile);
            filecell.CellStyle = style2;

            // 遍历第2列所有单元格;
            for (int i = 3; i <= lastRowNum; i++)
            {
                // 获取当前行
                IRow nowrow = worksheet.GetRow(i);
                // 如果当前行为空，则跳过
                if (nowrow == null)
                {
                    continue;
                }

                // 获取当前单元格
                ICell nowcell = nowrow.GetCell(1);
                // 如果当前单元格为空，则跳过
                if (nowcell == null)
                {
                    continue;
                }

                // 获取当前单元格的值
                string value = nowcell.StringCellValue;
                // 如果当前单元格的值和上一个单元格的值相同，则跳过
                if (value == prevValue)
                {
                    continue;
                }

                // 如果上一个单元格的值不为空，并且开始合并的行号小于i-1，则合并上一个单元格到当前单元格之间的所有单元格
                if (prevValue != null && startRow < i - 1)
                {
                    CellRangeAddress nowregion = new CellRangeAddress(startRow, i - 1, 1, 1);
                    worksheet.AddMergedRegion(nowregion);
                }

                // 更新上一个单元格的值和开始合并的行号
                prevValue = value;
                startRow = i;
            }

            // 如果上一个单元格的值不为空，并且开始合并的行号小于最后一行的行号，则合并上一个单元格到最后一行之间的所有单元格
            if (prevValue != null && startRow < lastRowNum)
            {
                CellRangeAddress region1 = new CellRangeAddress(startRow, lastRowNum, 1, 1);
                worksheet.AddMergedRegion(region1);
            }

            /*
            string fileName = Path.GetFileName(openFileDialog.FileName);
            string resultfile = "";
            int index = -1;
            for (int i = 0; i < fileName.Length; i++)
            {
                char currentChar = fileName[i];

                if (IsChineseCharacter(currentChar))
                {
                    index = i;
                    break;
                }
            }
            for (int i = index; i < fileName.Length; i++)
            {
                char currentChar = fileName[i];

                if (IsChineseCharacter(currentChar))
                {
                    resultfile += currentChar;
                }
                else
                {
                    break;
                }
            }

            CellRangeAddress fileregion = new CellRangeAddress(3, worksheet.LastRowNum, 0, 0);
            worksheet.AddMergedRegion(fileregion);
            IRow filerow = worksheet.GetRow(3);
            ICell filecell = filerow.CreateCell(0);
            filecell.SetCellValue(resultfile);
            filecell.CellStyle = style2;
            */
            /*
            for (int i = 0; i < lines.Length; i++)
            {
                var row = worksheet.GetRow(i);
                if (row != null)
                {
                    var cell = row.GetCell(0);
                    if (cell == null)
                    {
                        ICell cell0 = row.CreateCell(0);
                        cell0.SetCellValue(i);

                    }

                }

            }
            */
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Export To EXCEL";
            sfd.Filter = "xls(*.xls)|*.xls";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string folderPath = sfd.FileName.ToString();

                MessageBox.Show("您选择的文件保存路径是：" + folderPath);
                FileStream file = new FileStream(folderPath, FileMode.Create, FileAccess.Write);
                workbook.Write(file);
                file.Close();
            }
            //    FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    string folderPath = folderBrowserDialog1.SelectedPath+ "\\线束分析.xlsx";

            //    MessageBox.Show("您选择的文件保存路径是：" + folderPath);
            //    FileStream file = new FileStream(folderPath , FileMode.Create, FileAccess.Write);
            //    workbook.Write(file);
            //    file.Close();
            //}


        }

        private void MainForms_Click(object sender, EventArgs e)
        { }

        private void kryptonRibbonGroupButton20_Click(object sender, EventArgs e)
        {
            CAD.toolStripButton11_Click_1(sender, e);
        }

        private void kryptonRibbonGroupButton21_Click(object sender, EventArgs e)
        {
            CAD.teee1(sender, e); 
        }

        private void kryptonRibbonGroupButton22_Click(object sender, EventArgs e)
        {
            CAD.toTxtToolStripMenuItem_Click(sender, e);
        }

        private void 线束提取_Click(object sender, EventArgs e)
        {
            CAD.ReadPointsFromFile(sender, e);
        }

        private void kryptonRibbonGroupButton23_Click(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines("x.txt");
            string xlsFilePath = "./output_fuse_info.xlsx";

            // 创建Excel对象
            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet("Sheet1");

            // 创建一个样式：单元格背景为浅蓝色并且内容居中显示
            ICellStyle style = workbook.CreateCellStyle();
            style.FillForegroundColor = IndexedColors.LightBlue.Index;
            style.FillPattern = FillPattern.SolidForeground;
            style.VerticalAlignment = VerticalAlignment.Center;  // 垂直居中
            style.Alignment = HorizontalAlignment.Center;  // 水平居中

            // 更改列的宽度
            worksheet.SetColumnWidth(0, 5000);
            for (int i = 1; i <= 8; i++)
            {
                worksheet.SetColumnWidth(i, 5000);
            }

            // 创建Excel表的框架
            IRow row1 = worksheet.CreateRow(0);
            string[] headers = new string[] { "序号", "一级保险", "二级保险", "三级保险", "四级保险", "五级保险", "负载", "线号、线径及颜色", "备注" };
            for (int i = 0; i < headers.Length; i++)
            {
                ICell cell = row1.CreateCell(i);
                cell.SetCellValue(headers[i]);
                cell.CellStyle = style;
            }

            // 处理数据
            int rowIndex = 1; // 用于记录Excel的行索引
            foreach (string line in lines)
            {
                if (line.StartsWith("第"))
                {
                    string[] parts = line.Split(new char[] { ' ', '保' }, StringSplitOptions.RemoveEmptyEntries);
                    int level = int.Parse(parts[0].Substring(1, parts[0].Length - 2)); // 解析级别
                    string fuseValue = parts[1]; // 解析保险值

                    // 插入行数据
                    IRow row = worksheet.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(rowIndex); // 序号
                    row.CreateCell(level).SetCellValue(fuseValue); // 根据级别设置保险值
                    row.Cells[level].CellStyle = style;

                    rowIndex++;
                }
                else if (line.StartsWith("负载"))
                {
                    string loadInfo = line.Substring(2).Trim();
                    worksheet.GetRow(rowIndex - 1).CreateCell(6).SetCellValue(loadInfo);
                    worksheet.GetRow(rowIndex - 1).GetCell(6).CellStyle = style;
                }
            }

            // 保存Excel文件
            using (FileStream fs = new FileStream(xlsFilePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }

            Console.WriteLine("Excel 文件已生成在: " + xlsFilePath);
        }

        private void kryRibGButPoliy_Click(object sender, EventArgs e)
        {

        }

        private void kryRibGButLine_Click(object sender, EventArgs e)
        {

        }

        private void kryptonRibbonGroup4_DialogBoxLauncherClick(object sender, EventArgs e)
        {

        }

        private void kryConpolygon_Opening(object sender, CancelEventArgs e)
        {

        }

        private void kryConAngle_Opening(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void kryRibGButPoliy_Click_1(object sender, EventArgs e)
        {

        }

        private void kryptonRibbonGroupButton21_Click_1(object sender, EventArgs e)
        {
            CAD.DeleteLine_BFS(sender, e);//删除主干线
        }

        private void kryptonRibbonGroupButton22_Click_1(object sender, EventArgs e)
        {
            CAD.toolStripButton_DeletePolyline_Click(sender, e);//删除包围盒
        }

        private void kryptonRibbonGroupButton27_Click(object sender, EventArgs e)
        {
            //CAD.GroupAndDrawFullyMergedBoundingBoxes(sender, e);//包围盒
            CAD.OpencvForContour(sender, e);
        }

        private void kryptonRibbonGroupButton28_Click(object sender, EventArgs e)
        {
            //CAD.GetTextInsidePolylines(sender, e);//线束分组
        }

        private void kryptonRibbonGroupButton29_Click(object sender, EventArgs e)
        {
            CAD.ClassifyEntities(sender, e);//接地分析
        }

        private void kryptonRibbonGroupButton30_Click(object sender, EventArgs e)
        {
            
        }

        private void kryRibGButPoliy_Click_2(object sender, EventArgs e)
        {
            
        }

        private void kryptonRibbonGroupButton31_Click(object sender, EventArgs e)
        {
            //CAD.RestoreDeletedCurves(sender, e);//恢复主干线
        }

        private void kryptonRibbonGroupButton28_Click_1(object sender, EventArgs e)
        {
            CAD.toTxtToolStripMenuItem_Click(sender, e);
        }

        private void kryptonRibbonGroupButton30_Click_1(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines("x.txt");

            // 创建Excel对象
            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet("Sheet1");

            // 创建表头样式：背景色为绿色，内容居中显示，并且字体加粗
            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.FillForegroundColor = IndexedColors.LightGreen.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.Alignment = HorizontalAlignment.Center;
            IFont headerFont = workbook.CreateFont();
            headerFont.Boldweight = (short)FontBoldWeight.Bold;
            headerFont.FontHeightInPoints = 16;
            headerStyle.SetFont(headerFont);

            // 创建大标题行并合并单元格
            IRow headerRow = worksheet.CreateRow(0);
            headerRow.HeightInPoints = 40;
            ICell headerCell = headerRow.CreateCell(0);
            headerCell.SetCellValue("电气特性分析");
            headerCell.CellStyle = headerStyle;
            worksheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));

            // 创建列标题行样式
            ICellStyle titleStyle = workbook.CreateCellStyle();
            titleStyle.FillForegroundColor = IndexedColors.SeaGreen.Index;
            titleStyle.FillPattern = FillPattern.SolidForeground;
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.Alignment = HorizontalAlignment.Center;
            titleStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            titleStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            titleStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            titleStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

            // 设置列标题
            IRow row1 = worksheet.CreateRow(1);
            string[] headers = { "序号", "一级保险", "二级保险", "三级保险", "四级保险", "五级保险", "负载", "引脚", "继电器", "线号、线径及颜色", "备注" };
            for (int i = 0; i < headers.Length; i++)
            {
                ICell cell = row1.CreateCell(i);
                cell.SetCellValue(headers[i]);
                cell.CellStyle = titleStyle;
                if (i == 0)
                    worksheet.SetColumnWidth(i, 1000); // 设置初始列宽为8000
                else
                    worksheet.SetColumnWidth(i, 6000); // 设置初始列宽为8000
            }

            // 处理数据，序号居中
            ICellStyle centeredStyle = workbook.CreateCellStyle();
            centeredStyle.VerticalAlignment = VerticalAlignment.Center;
            centeredStyle.Alignment = HorizontalAlignment.Center;
            int rowIndex = 2; // 行索引从2开始（第三行）
            IRow currentRow = null; // 当前行对象
            int num = 1;
            bool flag = false;

            foreach (string line in lines)
            {
                if (line.StartsWith("第"))
                {
                    string[] parts = line.Split(new char[] { '保' }, StringSplitOptions.RemoveEmptyEntries);
                    string levelPart = parts[0].Substring(1, parts[0].IndexOf("级") - 1);
                    int level;
                    if (!int.TryParse(levelPart, out level))
                    {
                        level = -1; // 如果级别解析失败，则设置为未知级别
                    }

                    string fuseValue = parts.Length > 1 ? parts[1].Trim() : "未知";
                    currentRow = worksheet.CreateRow(rowIndex++);
                    ICell numCell = currentRow.CreateCell(0);
                    numCell.SetCellValue(num);
                    numCell.CellStyle = centeredStyle;
                    if (fuseValue.StartsWith("险"))
                    {
                        fuseValue = fuseValue.Substring(1);
                    }
                    if (level <= 8)
                    {
                        ICell valueCell = currentRow.CreateCell(level);
                        valueCell.SetCellValue(fuseValue);
                        valueCell.CellStyle = centeredStyle;
                        if (level == 1)
                        {
                            ICell valueCell2 = currentRow.CreateCell(10);
                            valueCell2.SetCellValue("电源保险");
                            valueCell2.CellStyle = centeredStyle;
                        }
                    }
                    num++;
                    flag = true;
                }
                else if (line.StartsWith("对应") && line.Substring(5).Trim() == "未找到保险")
                {
                    flag = false;
                    /*                    currentRow = worksheet.CreateRow(rowIndex++);
                                        ICell loadCell = currentRow.CreateCell(1);
                                        loadCell.SetCellValue("未找到保险");
                                        loadCell.CellStyle = centeredStyle;*/
                }
                else if (line.StartsWith("负载") && flag)
                {
                    string loadInfo = line.Substring(2).Trim();
                    if (currentRow != null)
                    {
                        ICell loadCell = currentRow.CreateCell(6);
                        loadCell.SetCellValue(loadInfo);
                        loadCell.CellStyle = centeredStyle;
                    }
                }
                else if (line.StartsWith("引脚") && flag)
                {
                    string pin = line.Substring(2).Trim();
                    if (currentRow != null)
                    {
                        ICell pinCell = currentRow.CreateCell(7);
                        pinCell.SetCellValue(pin);
                        pinCell.CellStyle = centeredStyle;
                    }
                }
                else if (line.EndsWith("继电器") && flag)
                {
                    string Relay = line.Trim();
                    if (currentRow != null)
                    {
                        ICell relayCell = currentRow.CreateCell(8);
                        relayCell.SetCellValue(Relay);
                        relayCell.CellStyle = centeredStyle;
                    }
                }
            }

            // 使用SaveFileDialog让用户选择文件保存位置
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Export To EXCEL",
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                AddExtension = true
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(file);
                    MessageBox.Show("文件已保存到：" + sfd.FileName);
                }
            }
        }

        private void kryptonRibbonGroupButton31_Click_1(object sender, EventArgs e)
        {
            CAD.TableProces(sender, e);
        }

        private void kryptonRibbonGroup8_DialogBoxLauncherClick(object sender, EventArgs e)
        {

        }

        private void KryptonGroupPanel_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void kryptonRibbonGroupButton32_Click(object sender, EventArgs e)
        {
            CAD.teee1(sender, e);
        }

        private void MainForms_Load_1(object sender, EventArgs e)
        {

        }
    }
}
