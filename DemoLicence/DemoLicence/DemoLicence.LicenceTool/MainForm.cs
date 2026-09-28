using DemoLicence.Common;
using System.Text;

namespace LicenceTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            this.txtPublicKey.Text=LicenceHelper.GetPublicKey();
            this.txtPrivateKey.Text=LicenceHelper.GetPrivateKey();
        }



        private void btnBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "电脑信息文件|*.key";
            ofd.Multiselect = false;
            ofd.Title = "请选择电脑信息文件";
            ofd.FileName=LicenceHelper.GetDefaultComputerFileName();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.txtSourceFile.Text = ofd.FileName;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(this.txtSourceFile.Text))
                {
                    MessageBox.Show("请先选择电脑信息文件");
                    return;
                }
                if (File.Exists(this.txtSourceFile.Text))
                {
                    //读取电脑文件
                    var info = LicenceHelper.GetComputerInfo(this.txtSourceFile.Text);
                    int days = GetLicenceDays();
                    var keyInfos = new StringBuilder(info);
                    var beginTime = DateTime.Now;
                    var endTime = DateTime.Now.AddDays(days);
                    //keyInfos.AppendLine($"beginTime={beginTime.ToString("yyyy-MM-dd HH:mm:ss")}");
                    keyInfos.AppendLine($"endTime={endTime.ToString("yyyy-MM-dd HH:mm:ss")}");
                    //
                    info = keyInfos.ToString();
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Title = "保存生成的Licence文件";
                    saveFileDialog.FileName = LicenceHelper.GetDefaultRegisterFileName();
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        LicenceHelper.GenerateLicenceKey(info, saveFileDialog.FileName);
                        MessageBox.Show("生成成功");
                    }
                }
                else
                {
                    MessageBox.Show("电脑信息文件不存在！");
                    return;
                }
            }catch(Exception ex)
            {
                string error = $"生成出错：{ex.Message}\r\n{ex.StackTrace}";
                MessageBox.Show(error);
            }
        }

        /// <summary>
        /// 获取有效期天数
        /// </summary>
        /// <returns></returns>
        private int GetLicenceDays()
        {
            int days = 1;
            RadioButton[] rbtns = new RadioButton[] { this.rbtSeven, this.rbtnTen, this.rbtnFifteen, this.rbtnThirty, this.rbtnSixty, this.rbtnSixMonth, this.rbtnNinety, this.rbtnSixMonth, this.rbtnForver };
            foreach (RadioButton rb in rbtns)
            {
                if (rb.Checked)
                {
                    if (!int.TryParse(rb.Tag.ToString(), out days))
                    {
                        days = 0;
                    }
                    break;
                }
            }
            days = days == -1 ? 9999 : days;//永久要转换一下
            return days;
        }
    }
}