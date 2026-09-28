namespace Teigha_Learn_Cad
{
    partial class ManageLayer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageLayer));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.kryptonGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.kryDataLayerView = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.Objectid = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.名称 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.开 = new System.Windows.Forms.DataGridViewImageColumn();
            this.冻结 = new System.Windows.Forms.DataGridViewImageColumn();
            this.锁 = new System.Windows.Forms.DataGridViewImageColumn();
            this.颜色 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.线宽 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewComboBoxColumn();
            this.线形 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewComboBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.colorDialogset = new System.Windows.Forms.ColorDialog();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).BeginInit();
            this.kryptonGroup1.Panel.SuspendLayout();
            this.kryptonGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryDataLayerView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.kryptonGroup1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(793, 481);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(793, 528);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(793, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel1.Text = "注释";
            // 
            // kryptonGroup1
            // 
            this.kryptonGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroup1.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroup1.Name = "kryptonGroup1";
            // 
            // kryptonGroup1.Panel
            // 
            this.kryptonGroup1.Panel.Controls.Add(this.kryDataLayerView);
            this.kryptonGroup1.Size = new System.Drawing.Size(793, 481);
            this.kryptonGroup1.TabIndex = 1;
            // 
            // kryDataLayerView
            // 
            this.kryDataLayerView.AllowUserToAddRows = false;
            this.kryDataLayerView.AllowUserToResizeRows = false;
            this.kryDataLayerView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.kryDataLayerView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.kryDataLayerView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Objectid,
            this.名称,
            this.开,
            this.冻结,
            this.锁,
            this.颜色,
            this.线宽,
            this.线形});
            this.kryDataLayerView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryDataLayerView.Location = new System.Drawing.Point(0, 0);
            this.kryDataLayerView.MultiSelect = false;
            this.kryDataLayerView.Name = "kryDataLayerView";
            this.kryDataLayerView.RowHeadersVisible = false;
            this.kryDataLayerView.RowTemplate.Height = 23;
            this.kryDataLayerView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryDataLayerView.Size = new System.Drawing.Size(791, 479);
            this.kryDataLayerView.TabIndex = 0;
            this.kryDataLayerView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.kryDataLayerView_CellBeginEdit);
            this.kryDataLayerView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryDataLayerView_CellClick);
            this.kryDataLayerView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryDataLayerView_CellEndEdit);
            this.kryDataLayerView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.kryDataLayerView_DataError);
            // 
            // Objectid
            // 
            this.Objectid.HeaderText = "Objectid";
            this.Objectid.Name = "Objectid";
            this.Objectid.ReadOnly = true;
            this.Objectid.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Objectid.Visible = false;
            this.Objectid.Width = 100;
            // 
            // 名称
            // 
            this.名称.HeaderText = "名称";
            this.名称.Name = "名称";
            this.名称.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.名称.Width = 113;
            // 
            // 开
            // 
            this.开.HeaderText = "开";
            this.开.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.开.Name = "开";
            // 
            // 冻结
            // 
            this.冻结.HeaderText = "冻结";
            this.冻结.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.冻结.Name = "冻结";
            this.冻结.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // 锁
            // 
            this.锁.HeaderText = "锁";
            this.锁.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.锁.Name = "锁";
            this.锁.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // 颜色
            // 
            this.颜色.HeaderText = "颜色";
            this.颜色.Name = "颜色";
            this.颜色.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.颜色.Width = 113;
            // 
            // 线宽
            // 
            this.线宽.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.线宽.DropDownWidth = 121;
            this.线宽.HeaderText = "线宽";
            this.线宽.Name = "线宽";
            this.线宽.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.线宽.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.线宽.Width = 113;
            // 
            // 线形
            // 
            this.线形.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.线形.DropDownWidth = 121;
            this.线形.HeaderText = "线形";
            this.线形.Name = "线形";
            this.线形.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.线形.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.线形.Width = 113;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(793, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "新增图层";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "删除图层";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "刷新列表";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // ManageLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 528);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ManageLayer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "图层编辑器";
            this.Load += new System.EventHandler(this.ManageLayer_Load);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).EndInit();
            this.kryptonGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).EndInit();
            this.kryptonGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryDataLayerView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView kryDataLayerView;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup1;
        private System.Windows.Forms.ColorDialog colorDialogset;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn Objectid;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn 名称;
        private System.Windows.Forms.DataGridViewImageColumn 开;
        private System.Windows.Forms.DataGridViewImageColumn 冻结;
        private System.Windows.Forms.DataGridViewImageColumn 锁;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn 颜色;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewComboBoxColumn 线宽;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewComboBoxColumn 线形;
    }
}