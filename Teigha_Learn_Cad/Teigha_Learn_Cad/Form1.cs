using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Teigha_Learn_Cad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        public class gpipei
        {
            public string daundian;
            public string jiechajian;
        }
        
        //List<gpipei> lpipei = new List<gpipei>();

        private void button1_Click(object sender, EventArgs e)
        {
            gpipei p = new gpipei();
            ////string selected = listBox1.GetItemText(listBox1.SelectedValue);
            // // MessageBox.Show(selected);
            // object item = checkedListBox1.SelectedItem;
            // string selected = checkedListBox1.GetItemText(checkedListBox1.SelectedItem);
            //MessageBox.Show(selected);
            int num = 1;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                //gpipei p=new gpipei();
                if (checkedListBox1.GetItemChecked(i))
                {
                    //MessageBox.Show(checkedListBox1.GetItemText(checkedListBox1.Items[i]));
                    string pipeizhi = checkedListBox1.GetItemText(checkedListBox1.Items[i]);
                    if (num==1)
                    {
                        p.daundian = pipeizhi;
                    }
                    else if(num == 2)
                    {
                        p.jiechajian = pipeizhi;
                    }
                    num++;
                    
                }
                //string pipeizhi = checkedListBox1.GetItemText(checkedListBox1.Items[i]);           
            }
            TeighaBase.lpipei.Add(p);
            for (int j = checkedListBox1.CheckedItems.Count - 1; j > -1; j--)
            {
                checkedListBox1.Items.RemoveAt(checkedListBox1.CheckedIndices[j]);
            }
            //MessageBox.Show(p.daundian);
            //MessageBox.Show(p.jiechajian);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (gpipei gp in TeighaBase.lpipei)
            {
                MessageBox.Show(gp.daundian);
                MessageBox.Show(gp.jiechajian);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (TeighaBase.lpipei.Count == 0) // 首先检查List是否为空
            {
                MessageBox.Show("没有选中的内容，无法回退！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                gpipei p;
                p = TeighaBase.lpipei[TeighaBase.lpipei.Count - 1];
                int itemCount = checkedListBox1.Items.Count;
                checkedListBox1.Items.Insert(0, p.daundian);
                checkedListBox1.Items.Add(p.jiechajian);
            }

            if (TeighaBase.lpipei.Count > 0) // 首先检查List是否为空
            {
                TeighaBase.lpipei.RemoveAt(TeighaBase.lpipei.Count - 1);
            }
        }
    }
}
