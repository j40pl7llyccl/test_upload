namespace uIP.MacroProvider.TrainingConvert
{
    partial class FormConfTrainingDraw
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cB_Model = new System.Windows.Forms.ComboBox();
            this.cB_DataSets = new System.Windows.Forms.ComboBox();
            this.chart_train_box_loss = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.bt_Run = new System.Windows.Forms.Button();
            this.Weights = new System.Windows.Forms.Label();
            this.cB_Weights = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tB_BatchSize = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tB_Epochs = new System.Windows.Forms.TextBox();
            this.chart_train_obj_loss = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_train_cls_loss = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_val_box_loss = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_val_obj_loss = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_val_cls_loss = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart_train_box_loss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_train_obj_loss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_train_cls_loss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_val_box_loss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_val_obj_loss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_val_cls_loss)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(416, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 43);
            this.label1.TabIndex = 0;
            this.label1.Text = "Training Model";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(68, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Model:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(845, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 32);
            this.label4.TabIndex = 3;
            this.label4.Text = "DataSets:";
            // 
            // cB_Model
            // 
            this.cB_Model.FormattingEnabled = true;
            this.cB_Model.Location = new System.Drawing.Point(203, 124);
            this.cB_Model.Name = "cB_Model";
            this.cB_Model.Size = new System.Drawing.Size(121, 32);
            this.cB_Model.TabIndex = 4;
            this.cB_Model.SelectedIndexChanged += new System.EventHandler(this.cB_Model_SelectedIndexChanged);
            // 
            // cB_DataSets
            // 
            this.cB_DataSets.FormattingEnabled = true;
            this.cB_DataSets.Location = new System.Drawing.Point(995, 128);
            this.cB_DataSets.Name = "cB_DataSets";
            this.cB_DataSets.Size = new System.Drawing.Size(121, 32);
            this.cB_DataSets.TabIndex = 6;
            this.cB_DataSets.SelectedIndexChanged += new System.EventHandler(this.cB_DataSets_SelectedIndexChanged);
            // 
            // chart_train_box_loss
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_train_box_loss.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_train_box_loss.Legends.Add(legend1);
            this.chart_train_box_loss.Location = new System.Drawing.Point(60, 361);
            this.chart_train_box_loss.Name = "chart_train_box_loss";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_train_box_loss.Series.Add(series1);
            this.chart_train_box_loss.Size = new System.Drawing.Size(464, 233);
            this.chart_train_box_loss.TabIndex = 7;
            this.chart_train_box_loss.Text = "chart_train_box_loss";
            // 
            // bt_Run
            // 
            this.bt_Run.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.bt_Run.Location = new System.Drawing.Point(1246, 253);
            this.bt_Run.Name = "bt_Run";
            this.bt_Run.Size = new System.Drawing.Size(94, 49);
            this.bt_Run.TabIndex = 8;
            this.bt_Run.Text = "Run";
            this.bt_Run.UseVisualStyleBackColor = true;
            this.bt_Run.Click += new System.EventHandler(this.bt_Run_Click);
            // 
            // Weights
            // 
            this.Weights.AutoSize = true;
            this.Weights.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Weights.Location = new System.Drawing.Point(68, 241);
            this.Weights.Name = "Weights";
            this.Weights.Size = new System.Drawing.Size(121, 32);
            this.Weights.TabIndex = 9;
            this.Weights.Text = "Weights:";
            // 
            // cB_Weights
            // 
            this.cB_Weights.FormattingEnabled = true;
            this.cB_Weights.Location = new System.Drawing.Point(203, 250);
            this.cB_Weights.Name = "cB_Weights";
            this.cB_Weights.Size = new System.Drawing.Size(121, 32);
            this.cB_Weights.TabIndex = 10;
            this.cB_Weights.SelectedIndexChanged += new System.EventHandler(this.cB_Weights_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F);
            this.label5.Location = new System.Drawing.Point(396, 250);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 32);
            this.label5.TabIndex = 11;
            this.label5.Text = "Batch Size:";
            // 
            // tB_BatchSize
            // 
            this.tB_BatchSize.Location = new System.Drawing.Point(568, 246);
            this.tB_BatchSize.Name = "tB_BatchSize";
            this.tB_BatchSize.Size = new System.Drawing.Size(121, 36);
            this.tB_BatchSize.TabIndex = 12;
            this.tB_BatchSize.TextChanged += new System.EventHandler(this.tB_BatchSize_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 12F);
            this.label6.Location = new System.Drawing.Point(851, 261);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 32);
            this.label6.TabIndex = 14;
            this.label6.Text = "Epochs:";
            // 
            // tB_Epochs
            // 
            this.tB_Epochs.Location = new System.Drawing.Point(995, 261);
            this.tB_Epochs.Name = "tB_Epochs";
            this.tB_Epochs.Size = new System.Drawing.Size(121, 36);
            this.tB_Epochs.TabIndex = 15;
            this.tB_Epochs.TextChanged += new System.EventHandler(this.tB_Epochs_TextChanged);
            // 
            // chart_train_obj_loss
            // 
            chartArea2.Name = "ChartArea1";
            this.chart_train_obj_loss.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart_train_obj_loss.Legends.Add(legend2);
            this.chart_train_obj_loss.Location = new System.Drawing.Point(697, 361);
            this.chart_train_obj_loss.Name = "chart_train_obj_loss";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart_train_obj_loss.Series.Add(series2);
            this.chart_train_obj_loss.Size = new System.Drawing.Size(464, 233);
            this.chart_train_obj_loss.TabIndex = 16;
            this.chart_train_obj_loss.Text = "chart_train_obj_loss";
            // 
            // chart_train_cls_loss
            // 
            chartArea3.Name = "ChartArea1";
            this.chart_train_cls_loss.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chart_train_cls_loss.Legends.Add(legend3);
            this.chart_train_cls_loss.Location = new System.Drawing.Point(1293, 361);
            this.chart_train_cls_loss.Name = "chart_train_cls_loss";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart_train_cls_loss.Series.Add(series3);
            this.chart_train_cls_loss.Size = new System.Drawing.Size(464, 233);
            this.chart_train_cls_loss.TabIndex = 17;
            this.chart_train_cls_loss.Text = "chart_train_cls_loss";
            // 
            // chart_val_box_loss
            // 
            chartArea4.Name = "ChartArea1";
            this.chart_val_box_loss.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart_val_box_loss.Legends.Add(legend4);
            this.chart_val_box_loss.Location = new System.Drawing.Point(60, 669);
            this.chart_val_box_loss.Name = "chart_val_box_loss";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chart_val_box_loss.Series.Add(series4);
            this.chart_val_box_loss.Size = new System.Drawing.Size(464, 233);
            this.chart_val_box_loss.TabIndex = 18;
            this.chart_val_box_loss.Text = "chart_val_box_loss";
            // 
            // chart_val_obj_loss
            // 
            chartArea5.Name = "ChartArea1";
            this.chart_val_obj_loss.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chart_val_obj_loss.Legends.Add(legend5);
            this.chart_val_obj_loss.Location = new System.Drawing.Point(697, 669);
            this.chart_val_obj_loss.Name = "chart_val_obj_loss";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chart_val_obj_loss.Series.Add(series5);
            this.chart_val_obj_loss.Size = new System.Drawing.Size(464, 233);
            this.chart_val_obj_loss.TabIndex = 19;
            this.chart_val_obj_loss.Text = "chart_val_obj_loss";
            // 
            // chart_val_cls_loss
            // 
            chartArea6.Name = "ChartArea1";
            this.chart_val_cls_loss.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chart_val_cls_loss.Legends.Add(legend6);
            this.chart_val_cls_loss.Location = new System.Drawing.Point(1293, 689);
            this.chart_val_cls_loss.Name = "chart_val_cls_loss";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.chart_val_cls_loss.Series.Add(series6);
            this.chart_val_cls_loss.Size = new System.Drawing.Size(464, 233);
            this.chart_val_cls_loss.TabIndex = 20;
            this.chart_val_cls_loss.Text = "chart_val_cls_loss";
            // 
            // FormConfTrainingDraw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1858, 954);
            this.Controls.Add(this.chart_val_cls_loss);
            this.Controls.Add(this.chart_val_obj_loss);
            this.Controls.Add(this.chart_val_box_loss);
            this.Controls.Add(this.chart_train_cls_loss);
            this.Controls.Add(this.chart_train_obj_loss);
            this.Controls.Add(this.tB_Epochs);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tB_BatchSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cB_Weights);
            this.Controls.Add(this.Weights);
            this.Controls.Add(this.bt_Run);
            this.Controls.Add(this.chart_train_box_loss);
            this.Controls.Add(this.cB_DataSets);
            this.Controls.Add(this.cB_Model);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormConfTrainingDraw";
            this.Text = "FormConfTrainingDraw";
            ((System.ComponentModel.ISupportInitialize)(this.chart_train_box_loss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_train_obj_loss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_train_cls_loss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_val_box_loss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_val_obj_loss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_val_cls_loss)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cB_Model;
        private System.Windows.Forms.ComboBox cB_DataSets;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_train_box_loss;
        private System.Windows.Forms.Button bt_Run;
        private System.Windows.Forms.Label Weights;
        private System.Windows.Forms.ComboBox cB_Weights;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tB_BatchSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tB_Epochs;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_train_obj_loss;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_train_cls_loss;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_val_box_loss;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_val_obj_loss;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_val_cls_loss;
    }
}
