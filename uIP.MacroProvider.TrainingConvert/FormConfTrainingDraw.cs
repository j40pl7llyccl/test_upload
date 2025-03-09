using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics; // ← 用於呼叫 Python & 讀取輸出
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

// 使用 Newtonsoft.Json 解析 Python 輸出的 JSON (記得先安裝 Newtonsoft.Json 套件)
using Newtonsoft.Json.Linq;

namespace uIP.MacroProvider.TrainingConvert
{
    public partial class FormConfTrainingDraw : Form
    {
        private Process trainingProcess; // 用於保存 Python 訓練的 Process 物件

        /// <summary>
        /// 建構子：呼叫 InitializeComponent()，但不包含控制項的手動初始化
        /// 都假設寫在 .Designer.cs 檔裡面
        /// </summary>
        public FormConfTrainingDraw()
        {
            InitializeComponent();
        }

        /// <summary>
        /// [Run] 按鈕事件：啟動 Python 並開始讀取訓練過程
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
            //   - 以下只是範例，實務上路徑與參數需依照你的實際需求或專案規劃更改
            string pythonFile = @"C:\YourPath\train.py";
            string args = $"\"{pythonFile}\" --data /content/test_ai/data/{dataset} " +
                          $"--cfg /content/test_ai/models/{model} " +
                          $"--weights /content/test_ai/weights/{weights} " +
                          $"--batch-size {batchSize} " +
                          $"--epochs {epochs} " +
                          $"--device cuda:0"; // 或 "cpu" 視你的執行環境而定

            // 3) 建立 ProcessStartInfo 以執行 Python
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "python",       // 如果系統 PATH 裡已包含 python.exe，可直接用 "python"
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            // 4) 建立 Process，設定事件監聽，並開始非同步讀取 stdout & stderr
            trainingProcess = new Process();
            trainingProcess.StartInfo = psi;
            trainingProcess.OutputDataReceived += Process_OutputDataReceived; // 監聽逐行 stdout
            trainingProcess.ErrorDataReceived += Process_ErrorDataReceived;   // 監聽逐行 stderr

            try
            {
                trainingProcess.Start();
                trainingProcess.BeginOutputReadLine(); // 開始非同步讀取 stdout
                trainingProcess.BeginErrorReadLine();  // 開始非同步讀取 stderr
            }
            catch (Exception ex)
            {
                MessageBox.Show($"無法啟動 Python: {ex.Message}");
            }
        }

        /// <summary>
        /// 即時讀取 Python stdout，每有一行輸出便會觸發
        /// </summary>
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // e.Data 即一行輸出（含 JSON 或一般 log）
            if (string.IsNullOrEmpty(e.Data))
                return;

            try
            {
                // 嘗試以 JSON 解析
                JObject dataObj = JObject.Parse(e.Data);

                // 取得各項數值（若拿不到就給預設值0）
                int epoch = dataObj["epoch"]?.Value<int>() ?? -1;
                double train_box_loss = dataObj["train_box_loss"]?.Value<double>() ?? 0;
                double train_obj_loss = dataObj["train_obj_loss"]?.Value<double>() ?? 0;
                double train_cls_loss = dataObj["train_cls_loss"]?.Value<double>() ?? 0;
                double val_box_loss = dataObj["val_box_loss"]?.Value<double>() ?? 0;
                double val_obj_loss = dataObj["val_obj_loss"]?.Value<double>() ?? 0;
                double val_cls_loss = dataObj["val_cls_loss"]?.Value<double>() ?? 0;

                // 因為要更新 UI 控件 (Chart)，必須透過 Invoke 回到 UI 執行緒
                this.Invoke((MethodInvoker)delegate
                {
                    // 自訂方法：將數值更新到對應的 Chart
                    UpdateChart(epoch,
                                train_box_loss, train_obj_loss, train_cls_loss,
                                val_box_loss, val_obj_loss, val_cls_loss);
                });
            }
            catch
            {
                // 若解析 JSON 失敗，可能是一般 log 訊息，可視需求做處理或忽略
                Debug.WriteLine($"[非 JSON] {e.Data}");
            }
        }

        /// <summary>
        /// 讀取 Python stderr（錯誤輸出）時觸發，可印出 Log 以供除錯
        /// </summary>
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                // 視需求可顯示到 UI 或記錄到檔案
                Debug.WriteLine("[stderr] " + e.Data);
            }
        }

        /// <summary>
        /// 根據 Python 輸出的結果，更新 6 張 Chart 的折線
        /// </summary>
        private void UpdateChart(int epoch,
                                 double train_box_loss, double train_obj_loss, double train_cls_loss,
                                 double val_box_loss, double val_obj_loss, double val_cls_loss)
        {
            try
            {
                // 加到 train_box_loss 圖表
                chart_train_box_loss.Series["Series1"].Points.AddXY(epoch, train_box_loss);

                // train_obj_loss
                chart_train_obj_loss.Series["Series1"].Points.AddXY(epoch, train_obj_loss);

                // train_cls_loss
                chart_train_cls_loss.Series["Series1"].Points.AddXY(epoch, train_cls_loss);

                // val_box_loss
                chart_val_box_loss.Series["Series1"].Points.AddXY(epoch, val_box_loss);

                // val_obj_loss
                chart_val_obj_loss.Series["Series1"].Points.AddXY(epoch, val_obj_loss);

                // val_cls_loss
                chart_val_cls_loss.Series["Series1"].Points.AddXY(epoch, val_cls_loss);

                // 可視需要自行做 RecalculateAxesScale() 或 Update()
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

        // 以下是控制項的事件，若無特殊處理需求，可保持空白
        private void cB_Model_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cB_DataSets_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cB_Weights_SelectedIndexChanged(object sender, EventArgs e) { }
        private void tB_BatchSize_TextChanged(object sender, EventArgs e) { }
        private void tB_Epochs_TextChanged(object sender, EventArgs e) { }
    }
}
