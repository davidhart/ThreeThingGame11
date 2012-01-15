namespace UnitEd
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newBtn = new System.Windows.Forms.ToolStripButton();
            this.saveBtn = new System.Windows.Forms.ToolStripButton();
            this.loadBtn = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nameTxtBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.energyCostBox = new System.Windows.Forms.TextBox();
            this.followRngTxtBox = new System.Windows.Forms.TextBox();
            this.atkRangeTxtBox = new System.Windows.Forms.TextBox();
            this.atkDmgTxtBox = new System.Windows.Forms.TextBox();
            this.atkSpdTxtBox = new System.Windows.Forms.TextBox();
            this.movSpdTxtBox = new System.Windows.Forms.TextBox();
            this.hpTxtBox = new System.Windows.Forms.TextBox();
            this.imgAssetTxtBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newBtn,
            this.loadBtn,
            this.saveBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(284, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newBtn
            // 
            this.newBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newBtn.Image = ((System.Drawing.Image)(resources.GetObject("newBtn.Image")));
            this.newBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newBtn.Name = "newBtn";
            this.newBtn.Size = new System.Drawing.Size(23, 22);
            this.newBtn.Text = "New";
            this.newBtn.Click += new System.EventHandler(this.newBtn_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveBtn.Image = ((System.Drawing.Image)(resources.GetObject("saveBtn.Image")));
            this.saveBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(23, 22);
            this.saveBtn.Text = "Save";
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // loadBtn
            // 
            this.loadBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.loadBtn.Image = ((System.Drawing.Image)(resources.GetObject("loadBtn.Image")));
            this.loadBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(23, 22);
            this.loadBtn.Text = "Open";
            this.loadBtn.Click += new System.EventHandler(this.loadBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "HP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Move Speed:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Atk Speed:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Atk Damage:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Atk Range:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-1, 183);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Follow Range:";
            // 
            // nameTxtBox
            // 
            this.nameTxtBox.Location = new System.Drawing.Point(80, 28);
            this.nameTxtBox.Name = "nameTxtBox";
            this.nameTxtBox.Size = new System.Drawing.Size(189, 20);
            this.nameTxtBox.TabIndex = 8;
            this.nameTxtBox.TextChanged += new System.EventHandler(this.nameTxtBox_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(-1, 209);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Energy Cost:";
            // 
            // energyCostBox
            // 
            this.energyCostBox.Location = new System.Drawing.Point(80, 209);
            this.energyCostBox.Name = "energyCostBox";
            this.energyCostBox.Size = new System.Drawing.Size(189, 20);
            this.energyCostBox.TabIndex = 18;
            // 
            // followRngTxtBox
            // 
            this.followRngTxtBox.Location = new System.Drawing.Point(80, 183);
            this.followRngTxtBox.Name = "followRngTxtBox";
            this.followRngTxtBox.Size = new System.Drawing.Size(189, 20);
            this.followRngTxtBox.TabIndex = 14;
            // 
            // atkRangeTxtBox
            // 
            this.atkRangeTxtBox.Location = new System.Drawing.Point(80, 157);
            this.atkRangeTxtBox.Name = "atkRangeTxtBox";
            this.atkRangeTxtBox.Size = new System.Drawing.Size(189, 20);
            this.atkRangeTxtBox.TabIndex = 13;
            // 
            // atkDmgTxtBox
            // 
            this.atkDmgTxtBox.Location = new System.Drawing.Point(80, 131);
            this.atkDmgTxtBox.Name = "atkDmgTxtBox";
            this.atkDmgTxtBox.Size = new System.Drawing.Size(189, 20);
            this.atkDmgTxtBox.TabIndex = 12;
            // 
            // atkSpdTxtBox
            // 
            this.atkSpdTxtBox.Location = new System.Drawing.Point(80, 105);
            this.atkSpdTxtBox.Name = "atkSpdTxtBox";
            this.atkSpdTxtBox.Size = new System.Drawing.Size(189, 20);
            this.atkSpdTxtBox.TabIndex = 11;
            // 
            // movSpdTxtBox
            // 
            this.movSpdTxtBox.Location = new System.Drawing.Point(80, 79);
            this.movSpdTxtBox.Name = "movSpdTxtBox";
            this.movSpdTxtBox.Size = new System.Drawing.Size(189, 20);
            this.movSpdTxtBox.TabIndex = 10;
            // 
            // hpTxtBox
            // 
            this.hpTxtBox.Location = new System.Drawing.Point(80, 54);
            this.hpTxtBox.Name = "hpTxtBox";
            this.hpTxtBox.Size = new System.Drawing.Size(189, 20);
            this.hpTxtBox.TabIndex = 9;
            // 
            // imgAssetTxtBox
            // 
            this.imgAssetTxtBox.Location = new System.Drawing.Point(80, 235);
            this.imgAssetTxtBox.Name = "imgAssetTxtBox";
            this.imgAssetTxtBox.Size = new System.Drawing.Size(189, 20);
            this.imgAssetTxtBox.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(-1, 235);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Image Asset:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 266);
            this.Controls.Add(this.imgAssetTxtBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.energyCostBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.followRngTxtBox);
            this.Controls.Add(this.atkRangeTxtBox);
            this.Controls.Add(this.atkDmgTxtBox);
            this.Controls.Add(this.atkSpdTxtBox);
            this.Controls.Add(this.movSpdTxtBox);
            this.Controls.Add(this.hpTxtBox);
            this.Controls.Add(this.nameTxtBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Volcanis Unit Editor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newBtn;
        private System.Windows.Forms.ToolStripButton loadBtn;
        private System.Windows.Forms.ToolStripButton saveBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox nameTxtBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox energyCostBox;
        private System.Windows.Forms.TextBox followRngTxtBox;
        private System.Windows.Forms.TextBox atkRangeTxtBox;
        private System.Windows.Forms.TextBox atkDmgTxtBox;
        private System.Windows.Forms.TextBox atkSpdTxtBox;
        private System.Windows.Forms.TextBox movSpdTxtBox;
        private System.Windows.Forms.TextBox hpTxtBox;
        private System.Windows.Forms.TextBox imgAssetTxtBox;
        private System.Windows.Forms.Label label9;
    }
}

