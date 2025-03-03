using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using uIP.Lib;
using uIP.Lib.DataCarrier;
using uIP.Lib.Script;

namespace uIP.MacroProvider.TrainingConvert
{
    public class YoloTrainingPlugin : UMacroMethodProviderPlugin
    {
        // 定義內部使用的 macro 方法名稱
        const string TrainingMethodName = "YoloTraining_Run";

        public YoloTrainingPlugin() : base()
        {
            m_strInternalGivenName = "YoloTrainingPlugin";
        }

        public override bool Initialize(UDataCarrier[] param)
        {
            // 註冊 macro 方法，此處使用三個參數：Model、Config 與 DataSet 路徑
            var macro = new UMacro(
                null,
                m_strInternalGivenName,
                TrainingMethodName,
                RunTrainingProcess,
                null, // 不需 immutable method
                null, // 不需 variable method
                null, // 不需 prev method
                new UDataCarrierTypeDescription[]
                {
                    new UDataCarrierTypeDescription(typeof(string), "Model file path"),
                    new UDataCarrierTypeDescription(typeof(string), "Config file path"),
                    new UDataCarrierTypeDescription(typeof(string), "Dataset path")
                }
            );

            m_UserQueryOpenedMethods.Add(macro);
            m_createMacroDoneFromMethod.Add(TrainingMethodName, MacroShellDoneCall_Training);

            // 可選：若需要彈出配置視窗，可加入如下設定（這裡直接使用已存在的 FormConfTrainingDraw 視窗）
            m_macroMethodConfigPopup.Add(TrainingMethodName, PopupConf_Training);

            m_bOpened = true;
            return true;
        }

        // 彈出配置視窗（可依需求自行設計）
        private Form PopupConf_Training(string callMethodName, UMacro macroToConf)
        {
            // 此處回傳訓練設定畫面，可根據實際需求調整
            return new FormConfTrainingDraw() { MacroInstance = macroToConf };
        }

        private bool MacroShellDoneCall_Training(string callMethodName, UMacro instance)
        {
            // 這裡可以在 macro 結束後做一些清理工作，或傳回成功狀態
            return true;
        }

        /// <summary>
        /// 此方法會由 UIP 框架在 macro 執行時被呼叫，
        /// 並內嵌原先在 button1_Click 裡呼叫 Python 訓練的邏輯。
        /// </summary>
        private UDataCarrier[] RunTrainingProcess(
            UMacro MacroInstance,
            UDataCarrier[] PrevPropagationCarrier, // 前一個 macro 傳入的資料
            List<UMacroProduceCarrierResult> historyResultCarriers,
            List<UMacroProduceCarrierPropagation> historyPropagationCarriers,
            List<UMacroProduceCarrierDrawingResult> historyDrawingCarriers,
            List<UScriptHistoryCarrier> historyCarrier,
            ref bool bStatusCode,
            ref string strStatusMessage,
            ref UDataCarrier[] CurrPropagationCarrier, // 要傳給下一個 macro 的資料
            ref UDrawingCarriers CurrDrawingCarriers,
            ref fpUDataCarrierSetResHandler PropagationCarrierHandler,
            ref fpUDataCarrierSetResHandler ResultCarrierHandler)
        {
            // 取得使用者設定的參數：若在 MutableInitialData 中有設定，則讀取其值，
            // 否則採用預設值
            string modelPath = "model.pt";
            string configPath = "config.yaml";
            string datasetPath = "dataset";

            if (MacroInstance.MutableInitialData is UDataCarrier[] data && data.Length >= 3)
            {
                modelPath = data[0].Data as string ?? modelPath;
                configPath = data[1].Data as string ?? configPath;
                datasetPath = data[2].Data as string ?? datasetPath;
            }

            // 組合 Python 執行所需的命令列引數
            string arguments = $"yolov5.py --model {modelPath} --config {configPath} --dataset {datasetPath}";
            
            try
            {
                // 呼叫內部方法啟動訓練
                StartYoloTraining(arguments);

                // 可根據需求等待訓練完成或直接返回，此處範例為非同步啟動
                CurrPropagationCarrier = UDataCarrier.MakeVariableItemsArray("Training started");

                bStatusCode = true;
                strStatusMessage = "Training process started successfully.";

                // 傳回結果，例如一個訊息字串
                return new UDataCarrier[]
                {
                    new UDataCarrier("Training process started", typeof(string))
                };
            }
            catch (Exception ex)
            {
                bStatusCode = false;
                strStatusMessage = ex.Message;
                return new UDataCarrier[]
                {
                    new UDataCarrier($"Error: {ex.Message}", typeof(string))
                };
            }
        }

        /// <summary>
        /// 呼叫 Python 腳本進行訓練，並利用非同步方式讀取輸出資訊。
        /// 此方法參考先前 button1_Click 中的實作。
        /// </summary>
        /// <param name="arguments">命令列引數</param>
        private void StartYoloTraining(string arguments)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "python.exe", // 若有需要，可改成 Python 絕對路徑
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            Process p = new Process
            {
                StartInfo = psi
            };

            // 監聽標準輸出
            p.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    // 假設輸出格式為 "Epoch:1 loss=0.123 mAP=0.567"
                    if (e.Data.StartsWith("Epoch:"))
                    {
                        // 在此可解析並更新 UIP 畫面或記錄 log，
                        // 例如將輸出內容寫入 UIP 的日誌視窗或進度圖中。
                        Console.WriteLine(e.Data);
                        // TODO: 若有需要更新圖表，請將 e.Data 解析後傳至 UIP 對應元件
                    }
                    else
                    {
                        Console.WriteLine(e.Data);
                    }
                }
            };

            // 監聽錯誤輸出
            p.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine("ERROR: " + e.Data);
                }
            };

            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
        }

        public override void Close()
        {
            // 若需要釋放資源，可在此處進行
            base.Close();
        }
    }
}
