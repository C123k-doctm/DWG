using DemoLicence.Common;

namespace LicenceTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {

                string info = string.Empty;
                string msg = string.Empty;
                //初始化加载
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
                    string[] infos = info.Split("\r\n");
                    if (infos.Length > 0)
                    {
                        var dicInfo = new Dictionary<string, string>();
                        foreach (var info2 in infos)
                        {
                            if (string.IsNullOrEmpty(info2))
                            {
                                continue;
                            }
                            var info2Arr = info2.Split("=");
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
    }
}