using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using uIP.Lib;
using uIP.Lib.DataCarrier;
using uIP.Lib.Script;

namespace uIP.MacroProvider.TrainingConvert
{
    public class ModelTrainingExtractor : UMacroMethodProviderPlugin
    {
        private const string TrainModelMethodName = "TrainModel_Execute";
        private Chart trendChart;
        private DateTime trainingStartTime;
        private Dictionary<int, List<double>> secondMetrics = new Dictionary<int, List<double>>();
        private string _modelFilePath = string.Empty;
        private string _configFilePath = string.Empty;

        public ModelTrainingExtractor() : base()
        {
            m_strInternalGivenName = "ModelTrainingExtractor";
        }

        public override bool Initialize(UDataCarrier[] param)
        {
            var macro = new UMacro(
                null,
                m_strInternalGivenName,
                TrainModelMethodName,
                StartModelTraining,
                null, // immutable
                null, // variable
                null, // prev
                new UDataCarrierTypeDescription[]{
                    new UDataCarrierTypeDescription(typeof(string), "Training Result Directory")
                } // return value
            );

            m_UserQueryOpenedMethods.Add(macro);
            m_createMacroDoneFromMethod.Add(TrainModelMethodName, MacroShellDoneCall_TrainModel);

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

            m_macroMethodConfigPopup.Add(TrainModelMethodName, PopupConf_TrainModel);
            m_bOpened = true;
            return true;
        }

        private bool IoctrlSet_FilePath(string callMethodName, UMacro instance, UDataCarrier[] data, string type)
        {
            if (instance == null || data == null || data.Length == 0) return false;
            if (instance.MutableInitialData == null)
                instance.MutableInitialData = new UDataCarrier(data[0].Data, data[0].Tp);
            else
                instance.MutableInitialData.Data = data[0].Data;

            if (type == "Model") _modelFilePath = data[0].ToString();
            if (type == "Config") _configFilePath = data[0].ToString();
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
            return callMethodName == TrainModelMethodName
                ? new FormConfTrainingDraw() { MacroInstance = macroToConf }
                : null;
        }

        private bool MacroShellDoneCall_TrainModel(string callMethodName, UMacro instance)
        {
            return true;
        }

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

            trainingStartTime = DateTime.Now;
            secondMetrics.Clear();

            Task.Run(() => RunTrainingProcess(_modelFilePath, _configFilePath));

            bStatusCode = true;
            strStatusMessage = "Training started successfully.";
            return new UDataCarrier[] { new UDataCarrier("Training Started", typeof(string)) };
        }

        private async Task RunTrainingProcess(string modelFile, string configFile)
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

            using (Process process = new Process { StartInfo = psi, EnableRaisingEvents = true })
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data) && TryParseOutput(e.Data, out int epoch, out double metric))
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
                };

                process.Start();
                process.BeginOutputReadLine();
                await process.WaitForExitAsync();
            }
        }

        private bool TryParseOutput(string data, out int epoch, out double metric)
        {
            epoch = 0;
            metric = 0;
            try
            {
                if (data.Contains("Epoch:"))
                {
                    var parts = data.Split(',');
                    if (parts.Length >= 2)
                    {
                        epoch = int.Parse(parts[0].Split(':')[1].Trim());
                        metric = double.Parse(parts[1].Split(':')[1].Trim());
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private void AggregateAndUpdateChart(int sec)
        {
            lock (secondMetrics)
            {
                if (secondMetrics.TryGetValue(sec, out var metrics))
                {
                    double avg = metrics.Average();
                    trendChart.Invoke((Action)(() =>
                    {
                        trendChart.Series[0].Points.AddXY(sec, avg);
                        trendChart.Invalidate();
                    }));
                }
            }
        }

        public override void Close()
        {
            base.Close();
        }
    }
}
