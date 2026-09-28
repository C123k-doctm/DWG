namespace Teigha_Learn_Cad
{
    partial class SingleCAD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleCAD));
            this.kryNavigaOne = new ComponentFactory.Krypton.Navigator.KryptonNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.kryNavigaOne)).BeginInit();
            this.kryNavigaOne.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryNavigaOne
            // 
            this.kryNavigaOne.Bar.BarOrientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Bottom;
            this.kryNavigaOne.Bar.ItemSizing = ComponentFactory.Krypton.Navigator.BarItemSizing.Individual;
            this.kryNavigaOne.Bar.TabBorderStyle = ComponentFactory.Krypton.Toolkit.TabBorderStyle.OneNote;
            this.kryNavigaOne.Bar.TabStyle = ComponentFactory.Krypton.Toolkit.TabStyle.OneNote;
            this.kryNavigaOne.Button.ButtonDisplayLogic = ComponentFactory.Krypton.Navigator.ButtonDisplayLogic.ContextNextPrevious;
            this.kryNavigaOne.Button.CloseButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.kryNavigaOne.Button.ContextButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.ShowEnabled;
            this.kryNavigaOne.Button.NextButtonAction = ComponentFactory.Krypton.Navigator.DirectionButtonAction.SelectPage;
            this.kryNavigaOne.Button.PreviousButtonAction = ComponentFactory.Krypton.Navigator.DirectionButtonAction.SelectPage;
            this.kryNavigaOne.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryNavigaOne.Location = new System.Drawing.Point(0, 0);
            this.kryNavigaOne.Margin = new System.Windows.Forms.Padding(0);
            this.kryNavigaOne.Name = "kryNavigaOne";
            this.kryNavigaOne.Size = new System.Drawing.Size(727, 483);
            this.kryNavigaOne.TabIndex = 1;
            this.kryNavigaOne.Text = "kryptonNavigator1";
            this.kryNavigaOne.SelectedPageChanged += new System.EventHandler(this.kryNavigaOne_SelectedPageChanged);
            this.kryNavigaOne.Selecting += new System.EventHandler<ComponentFactory.Krypton.Navigator.KryptonPageCancelEventArgs>(this.kryNavigaOne_Selecting);
            // 
            // SingleCAD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 483);
            this.Controls.Add(this.kryNavigaOne);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SingleCAD";
            this.Text = "SingleCAD";
            this.Load += new System.EventHandler(this.SingleCAD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryNavigaOne)).EndInit();
            this.kryNavigaOne.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Navigator.KryptonNavigator kryNavigaOne;
    }
}