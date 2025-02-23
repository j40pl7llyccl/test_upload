using System.Windows.Forms;

namespace uIP.MacroProvider.StreamIO.DividedData
{
    partial class DataSetSplitter
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
            this.lblFolderPath = new System.Windows.Forms.Label();
            this.lblTrain = new System.Windows.Forms.Label();
            this.lblTest = new System.Windows.Forms.Label();
            this.lblVal = new System.Windows.Forms.Label();
            this.numTrain = new System.Windows.Forms.NumericUpDown();
            this.numTest = new System.Windows.Forms.NumericUpDown();
            this.numVal = new System.Windows.Forms.NumericUpDown();
            this.btnSplit = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numTrain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVal)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFolderPath
            // 
            this.lblFolderPath.AutoSize = true;
            this.lblFolderPath.Location = new System.Drawing.Point(34, 56);
            this.lblFolderPath.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblFolderPath.Name = "lblFolderPath";
            this.lblFolderPath.Size = new System.Drawing.Size(136, 24);
            this.lblFolderPath.TabIndex = 0;
            this.lblFolderPath.Text = "資料夾路徑:";
            // 
            // lblTrain
            // 
            this.lblTrain.AutoSize = true;
            this.lblTrain.Location = new System.Drawing.Point(26, 112);
            this.lblTrain.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTrain.Name = "lblTrain";
            this.lblTrain.Size = new System.Drawing.Size(161, 24);
            this.lblTrain.TabIndex = 3;
            this.lblTrain.Text = "Train資料比例:";
            // 
            // lblTest
            // 
            this.lblTest.AutoSize = true;
            this.lblTest.Location = new System.Drawing.Point(26, 170);
            this.lblTest.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(151, 24);
            this.lblTest.TabIndex = 4;
            this.lblTest.Text = "Test資料比例:";
            // 
            // lblVal
            // 
            this.lblVal.AutoSize = true;
            this.lblVal.Location = new System.Drawing.Point(26, 228);
            this.lblVal.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblVal.Name = "lblVal";
            this.lblVal.Size = new System.Drawing.Size(144, 24);
            this.lblVal.TabIndex = 5;
            this.lblVal.Text = "Val資料比例:";
            // 
            // numTrain
            // 
            this.numTrain.Location = new System.Drawing.Point(217, 108);
            this.numTrain.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.numTrain.Name = "numTrain";
            this.numTrain.Size = new System.Drawing.Size(130, 36);
            this.numTrain.TabIndex = 6;
            this.numTrain.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // numTest
            // 
            this.numTest.Location = new System.Drawing.Point(217, 166);
            this.numTest.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.numTest.Name = "numTest";
            this.numTest.Size = new System.Drawing.Size(130, 36);
            this.numTest.TabIndex = 7;
            this.numTest.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numVal
            // 
            this.numVal.Location = new System.Drawing.Point(217, 224);
            this.numVal.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.numVal.Name = "numVal";
            this.numVal.Size = new System.Drawing.Size(130, 36);
            this.numVal.TabIndex = 8;
            this.numVal.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnSplit
            // 
            this.btnSplit.Location = new System.Drawing.Point(26, 300);
            this.btnSplit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(336, 46);
            this.btnSplit.TabIndex = 9;
            this.btnSplit.Text = "開始分配";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(217, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 35);
            this.button1.TabIndex = 10;
            this.button1.Text = "Select";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(217, 53);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(527, 36);
            this.textBox1.TabIndex = 11;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // DataSetSplitter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 378);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSplit);
            this.Controls.Add(this.numVal);
            this.Controls.Add(this.numTest);
            this.Controls.Add(this.numTrain);
            this.Controls.Add(this.lblVal);
            this.Controls.Add(this.lblTest);
            this.Controls.Add(this.lblTrain);
            this.Controls.Add(this.lblFolderPath);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "DataSetSplitter";
            this.Text = "資料集隨機分配";
            ((System.ComponentModel.ISupportInitialize)(this.numTrain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Button button1;
        private TextBox textBox1;

        #endregion

        //private System.Windows.Forms.Button btnSelectFolder;
        //private System.Windows.Forms.TextBox txtFolderPath;

    }
}