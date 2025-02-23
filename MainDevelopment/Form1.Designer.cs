namespace MainDevelopment
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_openFolder = new System.Windows.Forms.Button();
            this.tbox_Input = new System.Windows.Forms.TextBox();
            this.btn_addText = new System.Windows.Forms.Button();
            this.panel_image = new System.Windows.Forms.Panel();
            this.pictureBox_loadImage = new System.Windows.Forms.PictureBox();
            this.button_openImage = new System.Windows.Forms.Button();
            this.numericUpDown_roiL = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_roiT = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown_roiW = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_roiH = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_btnTrim = new System.Windows.Forms.Button();
            this.checkBox_bufPack = new System.Windows.Forms.CheckBox();
            this.button_convert24 = new System.Windows.Forms.Button();
            this.button_showScriptEditor = new System.Windows.Forms.Button();
            this.button_encrypt = new System.Windows.Forms.Button();
            this.button_decrypt = new System.Windows.Forms.Button();
            this.panel_image.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loadImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_roiL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_roiT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_roiW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_roiH)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_openFolder
            // 
            this.btn_openFolder.Location = new System.Drawing.Point(9, 10);
            this.btn_openFolder.Margin = new System.Windows.Forms.Padding(2);
            this.btn_openFolder.Name = "btn_openFolder";
            this.btn_openFolder.Size = new System.Drawing.Size(70, 23);
            this.btn_openFolder.TabIndex = 0;
            this.btn_openFolder.Text = "Open Folder";
            this.btn_openFolder.UseVisualStyleBackColor = true;
            this.btn_openFolder.Click += new System.EventHandler(this.btn_openFolder_Click);
            // 
            // tbox_Input
            // 
            this.tbox_Input.Location = new System.Drawing.Point(9, 38);
            this.tbox_Input.Margin = new System.Windows.Forms.Padding(2);
            this.tbox_Input.Name = "tbox_Input";
            this.tbox_Input.Size = new System.Drawing.Size(157, 22);
            this.tbox_Input.TabIndex = 1;
            // 
            // btn_addText
            // 
            this.btn_addText.Location = new System.Drawing.Point(83, 10);
            this.btn_addText.Margin = new System.Windows.Forms.Padding(2);
            this.btn_addText.Name = "btn_addText";
            this.btn_addText.Size = new System.Drawing.Size(70, 23);
            this.btn_addText.TabIndex = 0;
            this.btn_addText.Text = "Add Text";
            this.btn_addText.UseVisualStyleBackColor = true;
            this.btn_addText.Click += new System.EventHandler(this.btn_addText_Click);
            // 
            // panel_image
            // 
            this.panel_image.AutoScroll = true;
            this.panel_image.Controls.Add(this.pictureBox_loadImage);
            this.panel_image.Location = new System.Drawing.Point(196, 21);
            this.panel_image.Name = "panel_image";
            this.panel_image.Size = new System.Drawing.Size(415, 304);
            this.panel_image.TabIndex = 2;
            // 
            // pictureBox_loadImage
            // 
            this.pictureBox_loadImage.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_loadImage.Name = "pictureBox_loadImage";
            this.pictureBox_loadImage.Size = new System.Drawing.Size(100, 50);
            this.pictureBox_loadImage.TabIndex = 0;
            this.pictureBox_loadImage.TabStop = false;
            // 
            // button_openImage
            // 
            this.button_openImage.Location = new System.Drawing.Point(196, 345);
            this.button_openImage.Name = "button_openImage";
            this.button_openImage.Size = new System.Drawing.Size(75, 23);
            this.button_openImage.TabIndex = 3;
            this.button_openImage.Text = "Open Image";
            this.button_openImage.UseVisualStyleBackColor = true;
            this.button_openImage.Click += new System.EventHandler(this.button_openImage_Click);
            // 
            // numericUpDown_roiL
            // 
            this.numericUpDown_roiL.Location = new System.Drawing.Point(350, 348);
            this.numericUpDown_roiL.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_roiL.Name = "numericUpDown_roiL";
            this.numericUpDown_roiL.Size = new System.Drawing.Size(72, 22);
            this.numericUpDown_roiL.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(320, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Left";
            // 
            // numericUpDown_roiT
            // 
            this.numericUpDown_roiT.Location = new System.Drawing.Point(350, 376);
            this.numericUpDown_roiT.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_roiT.Name = "numericUpDown_roiT";
            this.numericUpDown_roiT.Size = new System.Drawing.Size(72, 22);
            this.numericUpDown_roiT.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(320, 384);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Top";
            // 
            // numericUpDown_roiW
            // 
            this.numericUpDown_roiW.Location = new System.Drawing.Point(468, 348);
            this.numericUpDown_roiW.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_roiW.Name = "numericUpDown_roiW";
            this.numericUpDown_roiW.Size = new System.Drawing.Size(72, 22);
            this.numericUpDown_roiW.TabIndex = 4;
            // 
            // numericUpDown_roiH
            // 
            this.numericUpDown_roiH.Location = new System.Drawing.Point(468, 376);
            this.numericUpDown_roiH.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_roiH.Name = "numericUpDown_roiH";
            this.numericUpDown_roiH.Size = new System.Drawing.Size(72, 22);
            this.numericUpDown_roiH.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(438, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "W";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(441, 384);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "H";
            // 
            // button_btnTrim
            // 
            this.button_btnTrim.Location = new System.Drawing.Point(350, 404);
            this.button_btnTrim.Name = "button_btnTrim";
            this.button_btnTrim.Size = new System.Drawing.Size(75, 23);
            this.button_btnTrim.TabIndex = 6;
            this.button_btnTrim.Text = "Trim";
            this.button_btnTrim.UseVisualStyleBackColor = true;
            this.button_btnTrim.Click += new System.EventHandler(this.button_btnTrim_Click);
            // 
            // checkBox_bufPack
            // 
            this.checkBox_bufPack.AutoSize = true;
            this.checkBox_bufPack.Location = new System.Drawing.Point(431, 410);
            this.checkBox_bufPack.Name = "checkBox_bufPack";
            this.checkBox_bufPack.Size = new System.Drawing.Size(57, 16);
            this.checkBox_bufPack.TabIndex = 7;
            this.checkBox_bufPack.Text = "Packed";
            this.checkBox_bufPack.UseVisualStyleBackColor = true;
            // 
            // button_convert24
            // 
            this.button_convert24.Location = new System.Drawing.Point(196, 373);
            this.button_convert24.Name = "button_convert24";
            this.button_convert24.Size = new System.Drawing.Size(75, 23);
            this.button_convert24.TabIndex = 8;
            this.button_convert24.Text = "Conver 24";
            this.button_convert24.UseVisualStyleBackColor = true;
            this.button_convert24.Click += new System.EventHandler(this.button_convert24_Click);
            // 
            // button_showScriptEditor
            // 
            this.button_showScriptEditor.Location = new System.Drawing.Point(9, 76);
            this.button_showScriptEditor.Name = "button_showScriptEditor";
            this.button_showScriptEditor.Size = new System.Drawing.Size(144, 23);
            this.button_showScriptEditor.TabIndex = 9;
            this.button_showScriptEditor.Text = "Show Script Editor";
            this.button_showScriptEditor.UseVisualStyleBackColor = true;
            this.button_showScriptEditor.Click += new System.EventHandler(this.button_showScriptEditor_Click);
            // 
            // button_encrypt
            // 
            this.button_encrypt.Location = new System.Drawing.Point(12, 141);
            this.button_encrypt.Name = "button_encrypt";
            this.button_encrypt.Size = new System.Drawing.Size(67, 33);
            this.button_encrypt.TabIndex = 1;
            this.button_encrypt.Text = "Entrypt";
            this.button_encrypt.UseVisualStyleBackColor = true;
            this.button_encrypt.Click += new System.EventHandler(this.button_encrypt_Click);
            // 
            // button_decrypt
            // 
            this.button_decrypt.Location = new System.Drawing.Point(83, 141);
            this.button_decrypt.Name = "button_decrypt";
            this.button_decrypt.Size = new System.Drawing.Size(67, 33);
            this.button_decrypt.TabIndex = 1;
            this.button_decrypt.Text = "Decrypt";
            this.button_decrypt.UseVisualStyleBackColor = true;
            this.button_decrypt.Click += new System.EventHandler(this.button_decrypt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 438);
            this.Controls.Add(this.button_decrypt);
            this.Controls.Add(this.button_encrypt);
            this.Controls.Add(this.button_showScriptEditor);
            this.Controls.Add(this.button_convert24);
            this.Controls.Add(this.checkBox_bufPack);
            this.Controls.Add(this.button_btnTrim);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown_roiH);
            this.Controls.Add(this.numericUpDown_roiT);
            this.Controls.Add(this.numericUpDown_roiW);
            this.Controls.Add(this.numericUpDown_roiL);
            this.Controls.Add(this.button_openImage);
            this.Controls.Add(this.panel_image);
            this.Controls.Add(this.tbox_Input);
            this.Controls.Add(this.btn_addText);
            this.Controls.Add(this.btn_openFolder);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel_image.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loadImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_roiL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_roiT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_roiW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_roiH)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_openFolder;
        private System.Windows.Forms.TextBox tbox_Input;
        private System.Windows.Forms.Button btn_addText;
        private System.Windows.Forms.Panel panel_image;
        private System.Windows.Forms.PictureBox pictureBox_loadImage;
        private System.Windows.Forms.Button button_openImage;
        private System.Windows.Forms.NumericUpDown numericUpDown_roiL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_roiT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown_roiW;
        private System.Windows.Forms.NumericUpDown numericUpDown_roiH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_btnTrim;
        private System.Windows.Forms.CheckBox checkBox_bufPack;
        private System.Windows.Forms.Button button_convert24;
        private System.Windows.Forms.Button button_showScriptEditor;
        private System.Windows.Forms.Button button_encrypt;
        private System.Windows.Forms.Button button_decrypt;
    }
}

