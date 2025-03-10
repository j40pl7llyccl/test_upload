using System;
using System.Diagnostics; // ← 用於呼叫 Python & 讀取輸出
using System.Windows.Forms;
using System.Threading.Tasks;

// 使用 Newtonsoft.Json 解析 Python 輸出的 JSON (記得先安裝 Newtonsoft.Json 套件)
using Newtonsoft.Json.Linq;

namespace uIP.MacroProvider.TrainConvert
{
    public partial class FormConfTrainDraw : Form
    {
        private Process trainingProcess; // 用於保存 Python 訓練的 Process 物件

        /// <summary>
        /// 建構子：呼叫 InitializeComponent()，但不包含控制項的手動初始化
        /// 都假設寫在 .Designer.cs 檔裡面
        /// </summary>
        public FormConfTrainDraw()
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
            Console.WriteLine("model: " + model);
            Console.WriteLine("dataset: " + dataset);
            Console.WriteLine("weights: " + weights);
            Console.WriteLine("batchSize: " + batchSize);
            Console.WriteLine("epochs: " + epochs);
            if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(dataset) || string.IsNullOrEmpty(weights) || string.IsNullOrEmpty(batchSize) || string.IsNullOrEmpty(epochs))
            {
                MessageBox.Show("請確認所有參數皆已輸入！");
                return;
            }

            // 2)確認是否有安裝相關套件

            string requirementsFile = @"C:\Users\MIP4070\Desktop\yolov5-master\requirements.txt"; //C: \Users\MIP4070\Desktop\yolov5 - master\requirements.txt

            // 組成指令： python -m pip install -r "requirements.txt"
            ProcessStartInfo py_insatll = new ProcessStartInfo()
            {
                FileName = "python",
                Arguments = $"-m pip install -r \"{requirementsFile}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            Process installProcess = new Process() { StartInfo = py_insatll };

            installProcess.OutputDataReceived += (s, pipArgs) =>
            {
                if (!string.IsNullOrEmpty(pipArgs.Data))
                    Console.WriteLine("[PIP 輸出]: " + pipArgs.Data);
            };

            installProcess.ErrorDataReceived += (s, pipErrArgs) =>
            {
                if (!string.IsNullOrEmpty(pipErrArgs.Data))
                    Console.WriteLine("[PIP 錯誤]: " + pipErrArgs.Data);
            };

            try
            {
                installProcess.Start();
                installProcess.BeginOutputReadLine();
                installProcess.BeginErrorReadLine();
                installProcess.WaitForExit(); // 等待完成
                MessageBox.Show("套件安裝完成!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"無法執行 pip install: {ex.Message}");
            }
            /*------------------------------------------------------------------------------------------------------------------------*/
            // 3) 組合成 Python 執行參數 (train.py + 相關參數)
            string pythonFile = @"C:\Users\MIP4070\Desktop\yolov5-master\train.py";
            string args = $"\"{pythonFile}\" --data C:\\Users\\MIP4070\\Desktop\\yolov5-master\\data\\{dataset} " +
                          $"--cfg  C:\\Users\\MIP4070\\Desktop\\yolov5-master\\models\\{model} " +
                          $"--weights  C:\\Users\\MIP4070\\Desktop\\yolov5-master\\weights\\{weights} " +
                          $"--batch-size {batchSize} " +
                          $"--epochs {epochs} " +
                          $"--device cuda:0"; 

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "python",
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            trainingProcess = new Process { StartInfo = psi };
            trainingProcess.OutputDataReceived += Process_OutputDataReceived; 

            trainingProcess.ErrorDataReceived += Process_ErrorDataReceived;  

            trainingProcess.Exited += (procSender, evtArgs) =>
            {
                MessageBox.Show("Python 訓練已執行完畢！", "通知", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            trainingProcess.EnableRaisingEvents = true;

            try
            {
                trainingProcess.Start();
                trainingProcess.BeginOutputReadLine();
                trainingProcess.BeginErrorReadLine();

                // 使用非同步等待，避免UI凍結
                Task.Run(() =>
                {
                    trainingProcess.WaitForExit();
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show("Python 訓練程序已完成！", "完成提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                });
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
            if (string.IsNullOrEmpty(e.Data))
                return;

            string line = e.Data.Trim();
            Debug.WriteLine($"e.Data Type: {e.Data.GetType().Name}, Value: {e.Data}");

            // 1**如果是 JSON 格式才解析**
            if (line.StartsWith("{") && line.EndsWith("}"))
            {
                try
                {
                    JObject dataObj = JObject.Parse(line);
                    int epoch = dataObj["epoch"]?.Value<int>() ?? -1;
                    double train_box_loss = dataObj["train_box_loss"]?.Value<double>() ?? 0;
                    double train_obj_loss = dataObj["train_obj_loss"]?.Value<double>() ?? 0;
                    double train_cls_loss = dataObj["train_cls_loss"]?.Value<double>() ?? 0;
                    double val_box_loss = dataObj["val_box_loss"]?.Value<double>() ?? 0;
                    double val_obj_loss = dataObj["val_obj_loss"]?.Value<double>() ?? 0;
                    double val_cls_loss = dataObj["val_cls_loss"]?.Value<double>() ?? 0;

                    this.Invoke((MethodInvoker)delegate
                    {
                        textBoxOutput.AppendText(line + Environment.NewLine);
                        UpdateChart(epoch, train_box_loss, train_obj_loss, train_cls_loss, val_box_loss, val_obj_loss, val_cls_loss);
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"JSON 解析失敗: {line}, 錯誤: {ex.Message}");
                }
            }
            // 2️⃣ **解析非 JSON 的數據行**
            else if (line.Contains("train/box_loss") || line.Contains("val/box_loss"))
            {
                try
                {
                    // **手動解析 train/box_loss, val/box_loss 數值**
                    var parts = line.Split(':'); // 分割 ":" 前後
                    if (parts.Length == 2)
                    {
                        var values = parts[1].Trim().Split(' '); // 取出數值
                        if (values.Length >= 3)
                        {
                            int epoch = int.Parse(values[0]); 
                            double train_box_loss = double.Parse(values[1]);
                            double train_obj_loss = double.Parse(values[2]);
                            double train_cls_loss = double.Parse(values[3]);
                            double val_box_loss = double.Parse(values[4]);
                            double val_obj_loss = double.Parse(values[5]);
                            double val_cls_loss = double.Parse(values[6]);

                            this.Invoke((MethodInvoker)delegate
                            {
                                textBoxOutput.AppendText($"[Parsed] {line}" + Environment.NewLine);
                                UpdateChart(-1, train_box_loss, train_obj_loss, train_cls_loss, val_box_loss, val_obj_loss, val_cls_loss);
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"數據解析失敗: {line}, 錯誤: {ex.Message}");
                }
            }
            else
            {
                // **一般日誌輸出**
                this.Invoke((MethodInvoker)delegate
                {
                    textBoxOutput.AppendText("[LOG] " + line + Environment.NewLine);
                });
            }
        }


        /// <summary>
        /// 讀取 Python stderr（錯誤輸出）時觸發，可印出 Log 以供除錯
        /// </summary>
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                
                this.Invoke(new Action(() =>
                {
                    textBoxOutput.AppendText(e.Data + Environment.NewLine);
                }));

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
                chart_train_box_loss.Series["train/box_loss"].Points.AddXY(epoch, train_box_loss);

                // train_obj_loss
                chart_train_obj_loss.Series["train/obj_loss"].Points.AddXY(epoch, train_obj_loss);

                // train_cls_loss
                chart_train_cls_loss.Series["train/cls_loss"].Points.AddXY(epoch, train_cls_loss);

                // val_box_loss
                chart_val_box_loss.Series["val/box_loss"].Points.AddXY(epoch, val_box_loss);

                // val_obj_loss
                chart_val_obj_loss.Series["val/obj_loss"].Points.AddXY(epoch, val_obj_loss);

                // val_cls_loss
                chart_val_cls_loss.Series["val/cls_loss"].Points.AddXY(epoch, val_cls_loss);

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

