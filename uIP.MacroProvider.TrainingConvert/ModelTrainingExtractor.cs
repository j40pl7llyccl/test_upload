using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using uIP.Lib;
using uIP.Lib.DataCarrier;
using uIP.Lib.Script;

namespace UIPTraining
{
    /// <summary>
    /// ModelTrainingExtractor 作為 UIP 插件，負責：
    /// 1. 根據使用者設定的 ModelPath 與 ConfigPath 呼叫 Python 執行檔進行模型訓練，
    /// 2. 每秒聚合訓練輸出（例如 Loss 指標），並即時更新 UI 上的趨勢圖。
    /// </summary>
    public class ModelTrainingExtractor : UMacroMethodProviderPlugin
    {
        private const string TrainModelMethodName = "TrainModel_Execute";
        private DateTime trainingStartTime;
        private Dictionary<int, List<double>> secondMetrics = new Dictionary<int, List<double>>();
        private string _modelFilePath = string.Empty;
        private string _configFilePath = string.Empty;

        // CancellationTokenSource 以支援取消操作
        private CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        /// 請在 UIP 配置或 Popup 中將圖表控件傳入此屬性
        /// </summary>
        public Chart TrendChart { get; set; }

        public ModelTrainingExtractor() : base()
        {
            m_strInternalGivenName = "ModelTrainingExtractor";
        }

        public override bool Initialize(UDataCarrier[] param)
        {
            // 註冊 TrainModel 方法，當 UIP 調用時會執行 StartModelTraining 方法
            var macro = new UMacro(
                null,
                m_strInternalGivenName,
                TrainModelMethodName,
                StartModelTraining,
                null, // immutable
                null, // variable
                null, // prev
                new UDataCarrierTypeDescription[] {
                    new UDataCarrierTypeDescription(typeof(string), "Training Result Directory")
                } // return value
            );

            m_UserQueryOpenedMethods.Add(macro);
            m_createMacroDoneFromMethod.Add(TrainModelMethodName, MacroShellDoneCall_TrainModel);

            // 設定可供 GET/SET 的參數：ModelPath 與 ConfigPath
            m_MacroControls.Add("ModelPath", new UScriptControlCarrierMacro(
                "ModelPath", true, true, true,
                new UDataCarrierTypeDescription[] { new UDataCarrierTypeDescription(typeof(string), "Model file path") },
                (carrier, macro, ref bool status) => IoctrlGet_FilePath(macro.MethodName, macro, "Model"),
                (carrier, macro, data) => IoctrlSet_FilePath(macro.MethodName, macro, data, "Model")
            ));

            m_MacroControls.Add("ConfigPath", new UScriptControlCarrierMacro(
                "ConfigPath", true, true, true,
                new UDataCarrierTypeDescription[] { new UDataCarrierTypeDescription(typeof(string), "Config file path") },
                (carrier, macro, ref bool status) => IoctrlGet_FilePath(macro.MethodName, macro, "Config"),
                (carrier, macro, data) => IoctrlSet_FilePath(macro.MethodName, macro, data, "Config")
            ));

            // 註冊配置彈出視窗
            m_macroMethodConfigPopup.Add(TrainModelMethodName, PopupConf_TrainModel);

            m_bOpened = true;
            return true;
        }

        private bool IoctrlSet_FilePath(string callMethodName, UMacro instance, UDataCarrier[] data, string type)
        {
            if (instance == null || data == null || data.Length == 0)
                return false;

            if (instance.MutableInitialData == null)
                instance.MutableInitialData = new UDataCarrier(data[0].Data, data[0].Tp);
            else
                instance.MutableInitialData.Data = data[0].Data;

            if (type == "Model")
                _modelFilePath = data[0].ToString();
            else if (type == "Config")
                _configFilePath = data[0].ToString();

            return true;
        }

        private UDataCarrier[] IoctrlGet_FilePath(string callMethodName, UMacro instance, string type)
        {
            return instance == null || instance.MutableInitialData == null
                ? null
                : new UDataCarrier[] { new UDataCarrier(instance.MutableInitialData.Data, instance.MutableInitialData.Tp) };
        }

        private Form PopupConf_TrainModel(string callMethodName, UMacro macroToConf)
        {
            // 這裡假設你已經定義了一個配置視窗 FormConfTrainModel，
            // 該視窗可以讓使用者選擇 Model 與 Config 檔，同時可以選擇或創建一個 Chart 控制項
            return callMethodName == TrainModelMethodName
                ? new FormConfTrainModel() { MacroInstance = macroToConf }
                : null;
        }

        private bool MacroShellDoneCall_TrainModel(string callMethodName, UMacro instance)
        {
            return true;
        }

        /// <summary>
        /// 這是 UIP 呼叫 TrainModel 宏時的入口方法，
        /// 主要檢查 ModelPath 與 ConfigPath 是否設置，然後啟動訓練進程。
        /// </summary>
        private UDataCarrier[] StartModelTraining(
            UMacro MacroInstance,
            UDataCarrier[] PrevPropagationCarrier,
            List<UMacroProduceCarrierResult> historyResultCarriers,
            List<UMacroProduceCarrierPropagation> historyPropagationCarriers,
            List<UMacroProduceCarrierDrawingResult> historyDrawingCarriers,
            List<UScriptHistoryCarrier> historyCarrier,
            ref bool bStatusCode, ref string strStatusMessage,
            ref UDataCarrier[] CurrPropagationCarrier,
            ref UDrawingCarriers CurrDrawingCarriers,
            ref fpUDataCarrierSetResHandler PropagationCarrierHandler,
            ref fpUDataCarrierSetResHandler ResultCarrierHandler)
        {
            if (string.IsNullOrEmpty(_modelFilePath) || string.IsNullOrEmpty(_configFilePath))
            {
                strStatusMessage = "Model or config file path is missing";
                return null;
            }

            if (TrendChart == null)
            {
                strStatusMessage = "Trend Chart is not set. Please configure a Chart control.";
                return null;
            }

            trainingStartTime = DateTime.Now;
            secondMetrics.Clear();
            cts = new CancellationTokenSource();

            // 非同步呼叫訓練進程
            Task.Run(() => RunTrainingProcess(_modelFilePath, _configFilePath, cts.Token));

            bStatusCode = true;
            strStatusMessage = "Training started successfully.";
            return new UDataCarrier[] { new UDataCarrier("Training Started", typeof(string)) };
        }

        /// <summary>
        /// 非同步執行 Python 訓練進程，並根據輸出更新趨勢圖
        /// </summary>
        private async Task RunTrainingProcess(string modelFile, string configFile, CancellationToken token)
        {
            string arguments = $"--model \"{modelFile}\" --config \"{configFile}\"";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            try
            {
                using (Process process = new Process { StartInfo = psi, EnableRaisingEvents = true })
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (token.IsCancellationRequested)
                        {
                            try { process.Kill(); } catch { }
                            return;
                        }

                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            if (TryParseOutput(e.Data, out int epoch, out double metric))
                            {
                                int elapsedSec = (int)(DateTime.Now - trainingStartTime).TotalSeconds;
                                lock (secondMetrics)
                                {
                                    if (!secondMetrics.ContainsKey(elapsedSec))
                                        secondMetrics[elapsedSec] = new List<double>();
                                    secondMetrics[elapsedSec].Add(metric);
                                }
                                AggregateAndUpdateChart(elapsedSec);
                            }
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            Debug.WriteLine("Error: " + e.Data);
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // 使用 WaitForExitAsync（若環境不支援，可參考下方自定義方法）
                    await WaitForExitAsync(process, token);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RunTrainingProcess Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// 使用正則表達式解析 Python 輸出，格式例如："Epoch: 1, Loss: 0.234"
        /// </summary>
        private bool TryParseOutput(string data, out int epoch, out double metric)
        {
            epoch = 0;
            metric = 0;
            try
            {
                var match = Regex.Match(data, @"Epoch:\s*(\d+).*Loss:\s*([\d\.]+)");
                if (match.Success && match.Groups.Count >= 3)
                {
                    epoch = int.Parse(match.Groups[1].Value);
                    metric = double.Parse(match.Groups[2].Value);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse Error: " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 聚合指定秒數內所有數據，計算平均值後更新 UI 上的趨勢圖
        /// </summary>
        private void AggregateAndUpdateChart(int sec)
        {
            List<double> metrics;
            lock (secondMetrics)
            {
                if (!secondMetrics.TryGetValue(sec, out metrics))
                    return;
            }
            double avg = metrics.Average();
            if (TrendChart.InvokeRequired)
            {
                TrendChart.Invoke(new Action(() => UpdateChartPoint(sec, avg)));
            }
            else
            {
                UpdateChartPoint(sec, avg);
            }
        }

        /// <summary>
        /// 檢查 Chart 中是否已有該秒數的資料點，若有則更新否則新增
        /// </summary>
        private void UpdateChartPoint(int sec, double avg)
        {
            var series = TrendChart.Series[0];
            var existingPoint = series.Points.FirstOrDefault(p => (int)p.XValue == sec);
            if (existingPoint != null)
            {
                existingPoint.YValues[0] = avg;
            }
            else
            {
                series.Points.AddXY(sec, avg);
            }
            TrendChart.Invalidate();
        }

        /// <summary>
        /// 等待進程結束的非同步方法（適用於不支援 WaitForExitAsync 的環境）
        /// </summary>
        private Task WaitForExitAsync(Process process, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<object>();
            process.Exited += (s, e) => tcs.TrySetResult(null);
            if (token != CancellationToken.None)
            {
                token.Register(() => tcs.TrySetCanceled());
            }
            return tcs.Task;
        }

        /// <summary>
        /// 外部可調用此方法以取消訓練進程
        /// </summary>
        public void CancelTraining()
        {
            cts?.Cancel();
        }

        public override void Close()
        {
            // 在插件關閉時，可嘗試取消進程與釋放相關資源
            CancelTraining();
            base.Close();
        }
    }
}

