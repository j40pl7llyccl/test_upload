using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using uIP.Lib;
using uIP.Lib.DataCarrier;
using uIP.Lib.Script;

namespace uIP.MacroProvider.StreamIO.DividedData
{
    /// <summary>
    /// 這個類別繼承自 UMacroMethodProviderPlugin，
    /// 目的是透過 "Macro" (巨集) 機制，對外提供一個可呼叫的功能 (ProcessPath)。
    /// 同時，我們也額外增加了一個 public 方法 (DoProcessPath)，
    /// 讓程式可以直接在 WinForm 內呼叫，而不走 Macro 機制。
    /// </summary>
    public class ProcessPath : UMacroMethodProviderPlugin
    {
        /// <summary>
        /// 用來註冊給 uIP 的 Macro 名稱。
        /// 在本程式碼中會將此名稱對應到 Macro_ProcessPath 方法進行執行。
        /// </summary>
        private const string ProcessPathMethodName = "DatasetDev_ProcessPath";

        /// <summary>
        /// 建構子：在底層 Plugin 中，把 m_strInternalGivenName 設為 "ProcessPath"。
        /// 這僅是提供給內部用的標記。
        /// </summary>
        public ProcessPath() : base()
        {
            m_strInternalGivenName = "ProcessPath";
        }

        /// <summary>
        /// Initialize(...) 會在 uIP 載入此 Plugin 時被呼叫，
        /// 在此把我們要提供的 Macro (DatasetDev_ProcessPath) 加入到 m_UserQueryOpenedMethods。
        /// </summary>
        public override bool Initialize(UDataCarrier[] param)
        {
            // 若已經做過初始化，就不再重複進行
            if (m_bOpened) return true;

            // 在 uIP 裡註冊我們要提供的方法 (Macro)
            // 1) Macro 名稱: ProcessPathMethodName
            // 2) 真正的執行邏輯: Macro_ProcessPath (delegate)
            // 3) 若需要輸出型別描述，可在 new UDataCarrierTypeDescription[] 內定義
            m_UserQueryOpenedMethods.Add(
                new UMacro(
                    null,                       // script class(可為null，表示無需指定)
                    m_strCSharpDefClassName,    // 這個類別在 c# 的完整定義名
                    ProcessPathMethodName,      // Macro 名稱 (之後呼叫需要對應)
                    Macro_ProcessPath,          // 實際執行邏輯 (delegate)
                    null,                       // immutable param
                    null,                       // variable param
                    null,                       // prev data type
                    new UDataCarrierTypeDescription[]
                    {
                        // 如果要讓 Macro 有輸出，可在此定義每個輸出欄位的型別描述
                        // 例如: new UDataCarrierTypeDescription(typeof(string), "Result msg")
                    }
                )
            );

            // 可選：Macro 建立後完成時的回呼
            //      若要在 Macro 被建立後做一些額外設定，可以在此進行
            m_createMacroDoneFromMethod.Add(ProcessPathMethodName, MacroShellDoneCall_ProcessPath);

            // 若需要自己的 UI 設定(彈出Form勾選路徑/選項)，
            // 可實作一個 PopupConf_ProcessPath(...) 並加到 m_macroMethodConfigPopup

            m_bOpened = true;
            return true;
        }

        /// <summary>
        /// 核心：真正執行 "搬移檔案到 train/test/val" 的 Macro 函式。
        /// 這裡會被 uIP 以委派方式呼叫，當外部建立 UMacro(名稱=DatasetDev_ProcessPath)
        /// 並且使用 ExecuteMacroMethod(...) 來執行時，就會進到這個函式。
        /// 
        /// 它的參數代表：
        /// - MacroInstance: 目前要執行的巨集實例
        /// - PrevPropagationCarrier: 前一個 Macro 流傳下來的資料 (若無則可能是 null)
        /// - historyResultCarriers / historyPropagationCarriers / historyDrawingCarriers / historyCarrier: 這些都是 uIP 在流程中遞送的歷史資訊 (視需求可忽略)
        /// - bStatusCode / strStatusMessage: 用來回傳執行成功或失敗的資訊
        /// - CurrPropagationCarrier: 若要傳遞資料給後續 Macro，可放在這裡
        /// - CurrDrawingCarriers / PropagationCarrierHandler / ResultCarrierHandler: uIP 內部繪圖或資料傳遞用 (若無需要可不管)
        /// </summary>
        private UDataCarrier[] Macro_ProcessPath(
            UMacro MacroInstance,
            UDataCarrier[] PrevPropagationCarrier,
            List<UMacroProduceCarrierResult> historyResultCarriers,
            List<UMacroProduceCarrierPropagation> historyPropagationCarriers,
            List<UMacroProduceCarrierDrawingResult> historyDrawingCarriers,
            List<UScriptHistoryCarrier> historyCarrier,
            ref bool bStatusCode,
            ref string strStatusMessage,
            ref UDataCarrier[] CurrPropagationCarrier,
            ref UDrawingCarriers CurrDrawingCarriers,
            ref fpUDataCarrierSetResHandler PropagationCarrierHandler,
            ref fpUDataCarrierSetResHandler ResultCarrierHandler)
        {
            try
            {
                // (1) 從 MacroInstance.MutableInitialData 取參數
                //     預期帶入: [0] 路徑(string path)
                //              [1] 是否Train(bool isTrain)
                //              [2] 是否Test(bool isTest)
                //              [3] 是否Val(bool isVal)
                var dataArr = MacroInstance.MutableInitialData?.Data as UDataCarrier[];
                if (dataArr == null || dataArr.Length < 4)
                {
                    // 如果外部沒塞足夠參數，就先用預設值，
                    // 或可直接標記 bStatusCode = false, 然後 return
                    dataArr = UDataCarrier.MakeVariableItemsArray(
                        @"C:\MyData", // 預設路徑
                        true,         // isTrain = true
                        false,        // isTest  = false
                        false         // isVal   = false
                    ) as UDataCarrier[];
                }

                // 取出4個參數
                string path = UDataCarrier.GetItem(dataArr, 0, "", out _);
                bool isTrain = UDataCarrier.GetItem(dataArr, 1, false, out _);
                bool isTest = UDataCarrier.GetItem(dataArr, 2, false, out _);
                bool isVal = UDataCarrier.GetItem(dataArr, 3, false, out _);

                // (2) 實際檔案搬移邏輯
                //     baseDir 是要搬到的總目錄，可視需要改寫或也帶入參數
                string baseDir = @"D:\DataDivided";
                string trainDir = Path.Combine(baseDir, "train");
                string testDir = Path.Combine(baseDir, "test");
                string valDir = Path.Combine(baseDir, "val");

                // 若目錄不存在就自動建立
                Directory.CreateDirectory(trainDir);
                Directory.CreateDirectory(testDir);
                Directory.CreateDirectory(valDir);

                // 若需包含子資料夾，SearchOption 改成 AllDirectories
                var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                int movedCount = 0;

                foreach (var file in files)
                {
                    // 若同時勾了 Train + Test + Val，會造成 File.Move 執行多次，可能失敗
                    // 如果需要複製到多處，可改用 File.Copy
                    if (isTrain)
                    {
                        string destFile = Path.Combine(trainDir, Path.GetFileName(file));
                        File.Move(file, destFile);
                        movedCount++;
                    }
                    else if (isTest)
                    {
                        string destFile = Path.Combine(testDir, Path.GetFileName(file));
                        File.Move(file, destFile);
                        movedCount++;
                    }
                    else if (isVal)
                    {
                        string destFile = Path.Combine(valDir, Path.GetFileName(file));
                        File.Move(file, destFile);
                        movedCount++;
                    }
                }

                // (3) 結果回報
                bStatusCode = true;
                strStatusMessage = $"成功搬移 {movedCount} 個檔案 (從 {path}) => " +
                                   $"Train={isTrain}, Test={isTest}, Val={isVal}";

                // 若需要把某些結果給後續 Macro，可以放到 CurrPropagationCarrier
                // 這裡簡單把 movedCount 傳出去
                CurrPropagationCarrier = UDataCarrier.MakeOneItemArray(movedCount);

                // return null 表示本 Macro 沒有直接產生 ResultCarrier
                return null;
            }
            catch (Exception ex)
            {
                bStatusCode = false;
                strStatusMessage = $"ProcessPath 發生錯誤: {ex}";
                return null;
            }
        }

        /// <summary>
        /// Macro 建立後的回呼 (非必要)
        /// 可以在 Macro 建立完成後做一些額外初始化或檢查。
        /// 這裡若回傳 false，代表初始化失敗。
        /// </summary>
        private bool MacroShellDoneCall_ProcessPath(string callMethodName, UMacro instance)
        {
            // 在 Macro 建立完成後，如果需要進一步設定或檢查，可寫在這裡。
            // 回傳 true 代表成功。
            return true;
        }

        // ------------------------------------------------------------------------
        // 下方新增一個普通的 public 方法，提供給 WinForm 直接呼叫，不透過 Macro 機制。
        // ------------------------------------------------------------------------
        /// <summary>
        /// 一般的 public 方法，用來在 WinForm (或其他 .NET 程式) 中直接呼叫，
        /// 而不走 Macro 執行流程。可以視情況與 Macro_ProcessPath 共用程式碼。
        /// </summary>
        /// <param name="path">來源資料夾路徑</param>
        /// <param name="isTrain">是否搬到 train 資料夾</param>
        /// <param name="isTest">是否搬到 test 資料夾</param>
        /// <param name="isVal">是否搬到 val 資料夾</param>
        public void DoProcessPath(string path, bool isTrain, bool isTest, bool isVal)
        {
            try
            {
                // 這裡的 baseDir, trainDir, ... 寫法與 Macro_ProcessPath 一致
                string baseDir = @"D:\DataDivided";
                string trainDir = Path.Combine(baseDir, "train");
                string testDir = Path.Combine(baseDir, "test");
                string valDir = Path.Combine(baseDir, "val");

                Directory.CreateDirectory(trainDir);
                Directory.CreateDirectory(testDir);
                Directory.CreateDirectory(valDir);

                var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                int movedCount = 0;

                foreach (var file in files)
                {
                    // 同時勾多個可能會發生 File.Move 重複操作問題
                    if (isTrain)
                    {
                        string destFile = Path.Combine(trainDir, Path.GetFileName(file));
                        File.Move(file, destFile);
                        movedCount++;
                    }
                    else if (isTest)
                    {
                        string destFile = Path.Combine(testDir, Path.GetFileName(file));
                        File.Move(file, destFile);
                        movedCount++;
                    }
                    else if (isVal)
                    {
                        string destFile = Path.Combine(valDir, Path.GetFileName(file));
                        File.Move(file, destFile);
                        movedCount++;
                    }
                }

                MessageBox.Show($"成功搬移 {movedCount} 個檔案 (從 {path})\n" +
                                $"Train={isTrain}, Test={isTest}, Val={isVal}",
                                "ProcessPath - 成功",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"搬移檔案時發生錯誤:\n{ex}",
                                "ProcessPath - 錯誤",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // 若想要讓外界彈出設定 UI (例如選路徑、勾Train/Test/Val)，
        // 可以仿照 uMProvidImageLoader 裡的 PopupConf_OpenImageFile(...) 自行實作
        // 然後在 Initialize(...) 時加到 m_macroMethodConfigPopup 中
        /*
        private Form PopupConf_ProcessPath(string callMethodName, UMacro macroToConf)
        {
            // 自訂表單: 例如 FormConfProcessPath
            var form = new FormConfProcessPath(); 
            form.MacroInstance = macroToConf;
            // 在表單裡用 macroToConf.MutableInitialData 讀/寫路徑 & checkBox 狀態
            return form;
        }
        */
    }
}


