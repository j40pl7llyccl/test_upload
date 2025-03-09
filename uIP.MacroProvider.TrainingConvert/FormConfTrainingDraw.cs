using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics; // ← 重要：呼叫 Python & 讀取輸出
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


// 使用 Newtonsoft.Json 解析 Python 輸出的 JSON（記得先安裝 Newtonsoft.Json 套件）
using Newtonsoft.Json.Linq;

namespace uIP.MacroProvider.TrainingConvert
{
    public partial class FormConfTrainingDraw : Form
    {
        private Process trainingProcess; // 用於保存Python訓練的Process物件

        public FormConfTrainingDraw()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 當使用者按下 [Run] 按鈕時
        /// </summary>
        private void bt_Run_Click(object sender, EventArgs e)
        {
            // 1) 取得使用者在 WinForm UI 中的參數
            string model = cB_Model.Text;
            string dataset = cB_DataSets.Text;
            string weights = cB_Weights.Text;
            string batchSize = tB_BatchSize.Text;
            string epochs = tB_Epochs.Text;

            // 2) 組合成 Python 執行參數 (train.py + 相關參數)
            //   - 以下只是「示例」用的指令與路徑，你需要根據實際路徑做修改
            string pythonFile = @"C:\YourPath\train.py";
            string args = $"\"{pythonFile}\" --data /content/test_ai/data/{dataset} " +
                          $"--cfg /content/test_ai/models/{model} " +
                          $"--weights /content/test_ai/weights/{weights} " +
                          $"--batch-size {batchSize} " +
                          $"--epochs {epochs} " +
                          $"--device cuda:0"; // 或 \"cpu\" 視環境而定

            // 3) 建立 ProcessStartInfo 以執行 Python
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "python",       // ← 需確保系統能從PATH找到python.exe，或使用完整路徑
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            // 4) 建立 Process，設定事件監聽，並開始非同步讀取 stdout & stderr
            trainingProcess = new Process();
            trainingProcess.StartInfo = psi;
            trainingProcess.OutputDataReceived += Process_OutputDataReceived; // 關鍵：逐行讀stdout
            trainingProcess.ErrorDataReceived += Process_ErrorDataReceived;

            try
            {
                trainingProcess.Start();
                trainingProcess.BeginOutputReadLine(); // 開始非同步讀取stdout
                trainingProcess.BeginErrorReadLine();  // 開始非同步讀取stderr
            }
            catch (Exception ex)
            {
                MessageBox.Show($"無法啟動Python: {ex.Message}");
            }
        }

        /// <summary>
        /// 即時讀取 Python stdout，每有一行輸出便會觸發
        /// </summary>
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // e.Data 即一行輸出（含JSON或一般log）
            if (string.IsNullOrEmpty(e.Data))
                return;

            try
            {
                // 嘗試解析JSON
                JObject dataObj = JObject.Parse(e.Data);

                // 取得各項數值（若拿不到則給預設值0）
                int epoch = dataObj["epoch"]?.Value<int>() ?? -1;
                double train_box_loss = dataObj["train_box_loss"]?.Value<double>() ?? 0;
                double train_obj_loss = dataObj["train_obj_loss"]?.Value<double>() ?? 0;
                double train_cls_loss = dataObj["train_cls_loss"]?.Value<double>() ?? 0;
                double val_box_loss = dataObj["val_box_loss"]?.Value<double>() ?? 0;
                double val_obj_loss = dataObj["val_obj_loss"]?.Value<double>() ?? 0;
                double val_cls_loss = dataObj["val_cls_loss"]?.Value<double>() ?? 0;

                // 因為要操作UI控件(Chart)，必須透過Invoke回到UI執行緒
                this.Invoke((MethodInvoker)delegate
                {
                    // 呼叫自訂方法更新圖表
                    UpdateChart(epoch,
                                train_box_loss, train_obj_loss, train_cls_loss,
                                val_box_loss, val_obj_loss, val_cls_loss);
                });
            }
            catch
            {
                // 若解析JSON失敗，代表可能只是一般log訊息，可選擇忽略或紀錄
                Debug.WriteLine($"非JSON輸出: {e.Data}");
            }
        }

        /// <summary>
        /// 讀取 Python stderr（錯誤輸出）時觸發，可印出Log除錯
        /// </summary>
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                // 你可以視需要將錯誤資訊顯示到UI，或寫到日誌檔
                Debug.WriteLine("[stderr] " + e.Data);
            }
        }

        /// <summary>
        /// 將解析到的 Loss 數值分別更新到對應 Chart
        /// </summary>
        private void UpdateChart(int epoch,
                                 double train_box_loss, double train_obj_loss, double train_cls_loss,
                                 double val_box_loss, double val_obj_loss, double val_cls_loss)
        {
            try
            {
                // 顯示於 train_box_loss 專屬圖表
                chart_train_box_loss.Series["Series1"].Points.AddXY(epoch, train_box_loss);
                // 顯示於 train_obj_loss 專屬圖表
                chart_train_obj_loss.Series["Series1"].Points.AddXY(epoch, train_obj_loss);
                // 顯示於 train_cls_loss 專屬圖表
                chart_train_cls_loss.Series["Series1"].Points.AddXY(epoch, train_cls_loss);

                // 顯示於 val_box_loss 專屬圖表
                chart_val_box_loss.Series["Series1"].Points.AddXY(epoch, val_box_loss);
                // 顯示於 val_obj_loss 專屬圖表
                chart_val_obj_loss.Series["Series1"].Points.AddXY(epoch, val_obj_loss);
                // 顯示於 val_cls_loss 專屬圖表
                chart_val_cls_loss.Series["Series1"].Points.AddXY(epoch, val_cls_loss);

                // 刷新圖表
                chart_train_box_loss.Update();
                chart_train_obj_loss.Update();
                chart_train_cls_loss.Update();
                chart_val_box_loss.Update();
                chart_val_obj_loss.Update();
                chart_val_cls_loss.Update();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("更新圖表時發生錯誤: " + ex.Message);
            }
        }

        // 以下為你UI事件，若無特殊處理需求，可以留空即可。
        private void cB_Model_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cB_DataSets_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cB_Weights_SelectedIndexChanged(object sender, EventArgs e) { }
        private void tB_BatchSize_TextChanged(object sender, EventArgs e) { }
        private void tB_Epochs_TextChanged(object sender, EventArgs e) { }
    }
}
