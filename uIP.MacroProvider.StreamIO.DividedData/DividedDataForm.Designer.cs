﻿namespace uIP.MacroProvider.StreamIO.DividedData
{
    partial class DividedDataForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            this.Text = "Video cutting tool";
        }


        private void InitializeComponent()
        {
            this.Text = "Divided Dat Form";
            
        }
        #region Windows Form 設計工具產生的程式碼

            /// <summary>
            /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
            /// 這個方法的內容。
            /// </summary>
        private void InitializeComponent()
        {
            this.bt_Auto = new System.Windows.Forms.Button();
            this.bt_Select = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_Next = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bt_Auto
            // 
            this.bt_Auto.Font = new System.Drawing.Font("新細明體", 10F);
            this.bt_Auto.Location = new System.Drawing.Point(76, 79);
            this.bt_Auto.Name = "bt_Auto";
            this.bt_Auto.Size = new System.Drawing.Size(90, 50);
            this.bt_Auto.TabIndex = 0;
            this.bt_Auto.Text = "Auto";
            this.bt_Auto.UseVisualStyleBackColor = true;
            // 
            // bt_Select
            // 
            this.bt_Select.Font = new System.Drawing.Font("新細明體", 10F);
            this.bt_Select.Location = new System.Drawing.Point(355, 79);
            this.bt_Select.Name = "bt_Select";
            this.bt_Select.Size = new System.Drawing.Size(75, 50);
            this.bt_Select.TabIndex = 1;
            this.bt_Select.Text = "Select";
            this.bt_Select.UseVisualStyleBackColor = true;
            this.bt_Select.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 15F);
            this.label1.Location = new System.Drawing.Point(206, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "DataSet Divided";
            // 
            // bt_Next
            // 
            this.bt_Next.Location = new System.Drawing.Point(76, 323);
            this.bt_Next.Name = "bt_Next";
            this.bt_Next.Size = new System.Drawing.Size(354, 32);
            this.bt_Next.TabIndex = 3;
            this.bt_Next.Text = "Next";
            this.bt_Next.UseVisualStyleBackColor = true;
            // 
            // DividedDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bt_Next);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_Select);
            this.Controls.Add(this.bt_Auto);
            this.Name = "DividedDataForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_Auto;
        private System.Windows.Forms.Button bt_Select;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_Next;
    }
}

