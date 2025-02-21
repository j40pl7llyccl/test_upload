using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using uIP.Lib;
using uIP.Lib.DataCarrier;
using uIP.Lib.Script;
using uIP.MacroProvider.StreamIO.VideoInToFrame;


namespace VideoFrameExtractor
{
    public class VideoExtractor : UMacroMethodProviderPlugin
    {
        const string VideoToFrameMethodName = "VideoDev_VideosToFrame";

        //使用者設定參數
        private string _videoFolderPath = string.Empty;
        private int _intervalSeconds;


        public enum eCallReturn
        {
            NONE,   // No error
            ERROR   // Error occurred
                    // ...other possible return values...
        }
        public VideoExtractor() : base()
        {
            m_strInternalGivenName = "VideoExtractor";

        }
        public override bool Initialize(UDataCarrier[] param)
        {
            /*--
                follow the example of uIP.MacroProvider.StreamIO.ImageFileLoader
            )--*/
            var macro = new UMacro(null, m_strInternalGivenName, VideoToFrameMethodName, OpenVideoFile,
                                    null, //immutable
                                    null, //variable
                                    null, //prev
                                    new UDataCarrierTypeDescription[]{
                                        new UDataCarrierTypeDescription(typeof(string), "Image file path")
                                    }// return
                        );

            m_UserQueryOpenedMethods.Add(macro);

            m_createMacroDoneFromMethod.Add(VideoToFrameMethodName, MacroShellDoneCall_VideoFile);

            // config variable

            // config the macro GET/SET
            // fill macro get/set
            // - list all available names
            // - if multiple methods use same name, check by UMacro MethodName
            // - if own by one method, not check the MethodName
            //m_MacroControls.Add("LoadingImageDir", new UScriptControlCarrierMacro("LoadingImageDir", true, true, true,
            //    new UDataCarrierTypeDescription[] { new UDataCarrierTypeDescription(typeof(string), "Loading dir") },
            //    IoctrlGet_LoadingDir, IoctrlSet_LoadingDir));

            // config the macro GET/SET
            m_MacroControls.Add("LoadingVideoDir", new UScriptControlCarrierMacro("LoadingVideoDir", true, true, true,
                new UDataCarrierTypeDescription[] { new UDataCarrierTypeDescription(typeof(string), "Loading dir") },
                new fpGetMacroScriptControlCarrier((UScriptControlCarrier carrier, UMacro whichMacro, ref bool bRetStatus) => IoctrlGet_LoadingDir(whichMacro.MethodName, whichMacro)),
                new fpSetMacroScriptControlCarrier((UScriptControlCarrier carrier, UMacro whichMacro, UDataCarrier[] data) => IoctrlSet_LoadingDir(whichMacro.MethodName, whichMacro, data))));

            // popup UI to config
            m_macroMethodConfigPopup.Add(VideoToFrameMethodName, PopupConf_VideoFile);
            m_bOpened = true;
            return true;
        }
        //focus on SET about loading video dir
        private bool IoctrlSet_LoadingDir(string callMethodName, UMacro instance, UDataCarrier[] data)
        {
            if (instance == null || data == null || data.Length == 0) return false;
            if (instance.MutableInitialData == null)
                instance.MutableInitialData = new UDataCarrier(data[0].Data, data[0].Tp);
            else
                instance.MutableInitialData.Data = data[0].Data;
            instance.MutableInitialData.Tp = data[0].Tp;
            return true;
        }

        //focus on GET about loading video dir
        private UDataCarrier[] IoctrlGet_LoadingDir(string callMethodName, UMacro instance)
        {
            if (instance == null || instance.MutableInitialData == null) return null;
            return new UDataCarrier[] { new UDataCarrier(instance.MutableInitialData.Data, instance.MutableInitialData.Tp) };
        }

        //focus on popup UI to config
        private Form PopupConf_VideoFile(string callMethodName, UMacro macroToConf)
        {
            if (callMethodName == VideoToFrameMethodName)
            {
                return new FormConfVideoToFrame() { MacroInstance = macroToConf };
            }
            return null;
        }
        private bool MacroShellDoneCall_VideoFile(string callMethodName, UMacro instance)
        {
            return true;
        }
        private UDataCarrier[] OpenVideoFile(UMacro MacroInstance,
                                      UDataCarrier[] PrevPropagationCarrier, //上一個人傳入的資料
                                      List<UMacroProduceCarrierResult> historyResultCarriers,
                                      List<UMacroProduceCarrierPropagation> historyPropagationCarriers,
                                      List<UMacroProduceCarrierDrawingResult> historyDrawingCarriers,
                                      List<UScriptHistoryCarrier> historyCarrier,
                                  ref bool bStatusCode, ref string strStatusMessage,
                                  ref UDataCarrier[] CurrPropagationCarrier, //下一個傳出去的資料
                                  ref UDrawingCarriers CurrDrawingCarriers,
                                  ref fpUDataCarrierSetResHandler PropagationCarrierHandler,
                                  ref fpUDataCarrierSetResHandler ResultCarrierHandler)
        {
            // 1. 若上一個 Macro 有傳入資料，可嘗試從 PrevPropagationCarrier 取出 intervalSeconds
            if (!UDataCarrier.GetByIndex(PrevPropagationCarrier, 0, "", out var filepath))
            {
                strStatusMessage = "no file path error";
                return null;
            }
            //

            if (PrevPropagationCarrier != null && PrevPropagationCarrier.Length > 0)
            {
                double? fromPrev = UDataCarrier.Get<double?>(PrevPropagationCarrier[0], null);
                if (fromPrev.HasValue)
                {
                    _intervalSeconds = (int)fromPrev.Value; // 或者用 (int)Math.Round(fromPrev.Value);
                    Console.WriteLine($"接收上一個 Macro 輸入的 intervalSeconds = {_intervalSeconds}");
                }
            }
            if (MacroInstance.MutableInitialData == null)
            {
                bStatusCode = false;
                strStatusMessage = "not config init data";
                return null;
            }
            if (!(MacroInstance.MutableInitialData.Data is UDataCarrier[] data) || data == null)
            {
                bStatusCode = false;
                strStatusMessage = "init data invalid";
                return null;
            }
            try
            {
                /*--
                // 2.取得影片路徑
                string videoPath = UDataCarrier.Get<string>(data[0], string.Empty);
                if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
                {
                    bStatusCode = false;
                    strStatusMessage = "Invalid video path";
                    return null;
                }

                // 3.取得影片長度 用到GetVideoDuration()
                int videoDuration = GetVideoDuration(videoPath);
                if (videoDuration <= 0)
                {
                    bStatusCode = false;
                    strStatusMessage = "Failed to get video duration";
                    return null;
                }

                // 4.設定輸出資料夾
                string outputFolder = Path.Combine(Path.GetDirectoryName(videoPath), Path.GetFileNameWithoutExtension(videoPath));
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // 5.擷取影片轉成照片,用到SplitVideoIntoFrames()

                SplitVideoIntoFrames(videoPath, outputFolder, _intervalSeconds, MacroInstance);
                --*/

                // 1.設定輸出資料夾
                string outputFolder = Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileNameWithoutExtension(filepath));
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                //2. 處理ProcessVideos()
                ProcessVideos(filepath, outputFolder, _intervalSeconds, MacroInstance);

                //3.下一個傳送出去的資料
                CurrPropagationCarrier = UDataCarrier.MakeVariableItemsArray(outputFolder);

                //4. 假設成功處理，設置狀態碼和狀態訊息
                bStatusCode = true;
                strStatusMessage = "Success";

                return new UDataCarrier[] {
                    new UDataCarrier(outputFolder, typeof(string))
                };
            }
            catch (Exception e)
            {
                bStatusCode = false;
                strStatusMessage = $"Exception:{e.Message}";
                return null;

            }
        }

        public int GetVideoDuration(string videoPath)
        {
            try
            {
                using (VideoCapture cap = new VideoCapture(videoPath))
                {
                    if (!cap.IsOpened())
                    {
                        Console.WriteLine($"無法打開影片: {videoPath}");
                        return 0;
                    }

                    double fps = cap.Fps;
                    double frameCount = cap.FrameCount;
                    if (fps <= 0)
                    {
                        Console.WriteLine($"影片的{videoPath}fps為0或無效");
                        return 0;
                    }

                    int duration = (int)(frameCount / fps);
                    return duration;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"取得影片長度失敗: {videoPath}，錯誤: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// 每隔 intervalSeconds 秒擷取影片中的一幀，並儲存至 outputFolder 資料夾
        /// </summary>
        public void SplitVideoIntoFrames(string videoPath, string outputFolder, double intervalSeconds, UMacro macro)
        {
            try
            {
                // 若輸出資料夾不存在，則建立
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);

                using (VideoCapture cap = new VideoCapture(videoPath))
                {
                    if (!cap.IsOpened())
                    {
                        Console.WriteLine($"無法打開影片: {videoPath}");
                        return;
                    }

                    double fps = cap.Fps;
                    if (fps <= 0)
                    {
                        Console.WriteLine($"影片 {videoPath} 的 FPS 無效。");
                        return;
                    }

                    if (intervalSeconds <= 0)
                    {
                        Console.WriteLine("切格秒數必須大於0");
                        return;
                    }

                    // 計算「影格間隔數」：若 FPS 為 30 且 intervalSeconds 為 30 秒，則 frameInterval 約為 900
                    int frameInterval = (int)Math.Round(fps * intervalSeconds);
                    frameInterval = Math.Max(frameInterval, 1); // 至少每個影格都檢查一次

                    int currentFramePosition = 0;
                    int photoCount = 0;

                    using (Mat frame = new Mat())
                    {
                        while (true)
                        {
                            // 1. 檢查是否要取消（macro.CancelExec）
                            if (macro != null && macro.CancelExec)
                            {
                                Console.WriteLine("偵測到Script已取消，停止擷取影格");
                                break;
                            }
                            // 設定讀取的影格位置
                            cap.Set(VideoCaptureProperties.PosFrames, currentFramePosition);
                            bool ret = cap.Read(frame);
                            if (!ret || frame.Empty())
                            {
                                // 若讀取失敗或影格為空，則認為影片讀取結束
                                break;
                            }

                            string photoPath = Path.Combine(outputFolder, $"frame_{photoCount}.jpg");
                            bool saveResult = Cv2.ImWrite(photoPath, frame);
                            if (saveResult)
                            {
                                Console.WriteLine($"儲存圖片: {photoPath}");
                                photoCount++;
                            }
                            else
                            {
                                Console.WriteLine($"儲存圖片失敗: {photoPath}");
                            }

                            currentFramePosition += frameInterval;
                            // 若下一個影格位置超過影片總影格數，則跳出迴圈
                            if (currentFramePosition >= cap.FrameCount)
                            {
                                break;
                            }
                        }
                    }

                    Console.WriteLine($"影片 {Path.GetFileName(videoPath)} 總共擷取 {photoCount} 張圖片。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"影片 {videoPath} 切割影格時發生錯誤: {ex.Message}");
            }
        }

        /// <summary>
        /// 處理指定資料夾下的所有影片檔，並將每部影片擷取的影格存到對應的輸出子資料夾中
        /// 在加一個進度條
        /// </summary>
        public void ProcessVideos(string videoFolder, string outputFolder, double intervalSeconds, UMacro macro)
        {
            try
            {
                if (!Directory.Exists(videoFolder))
                {
                    Console.WriteLine($"影片目錄不存在: {videoFolder}");
                    return;
                }

                // 篩選指定副檔名的影片檔案
                var videoFiles = Directory.GetFiles(videoFolder)
                    .Where(f => new[] { ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv" }
                                .Contains(Path.GetExtension(f).ToLower()))
                    .ToArray();

                if (videoFiles.Length == 0)
                {
                    Console.WriteLine("找不到任何符合條件的影片檔。");
                    return;
                }

                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);

                // 進度條相關變數
                int totalFiles = videoFiles.Length;
                int processedFiles = 0;
                object lockObject = new object(); // 用於執行緒安全的鎖

                // 使用平行處理來提高多影片處理效率
                Parallel.ForEach(videoFiles, video =>
                {
                    try
                    {
                        Console.WriteLine($"開始處理影片: {video}");
                        int duration = GetVideoDuration(video); // 調用 GetVideoDuration()
                        Console.WriteLine($"影片 {Path.GetFileName(video)} 總長度: {duration} 秒");

                        // 為每部影片建立以影片檔名命名的子資料夾
                        string videoOutputFolder = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(video));
                        if (!Directory.Exists(videoOutputFolder))
                            Directory.CreateDirectory(videoOutputFolder);

                        SplitVideoIntoFrames(video, videoOutputFolder, intervalSeconds, macro); // 調用 SplitVideoIntoFrames()


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"處理影片 {video} 時發生錯誤: {ex.Message}");
                    }
                });

                Console.WriteLine("\n所有影片處理完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"處理影片資料夾時發生錯誤: {ex.Message}");
            }
        }

        public string UpdateProgressBar(int processed, int total)
        {
            // 計算進度百分比
            double progress = (double)processed / total;
            int progressBarLength = 50;
            int filledLength = (int)(progress * progressBarLength);

            // 繪製進度條
            string progressBar = new string('█', filledLength).PadRight(progressBarLength, ' ');
            Console.Write($"\r進度: [{progressBar}] {progress:P0}");

            string progressBar_Output = $"\r進度: [{progressBar}] {progress:P0}";
            return progressBar_Output;
        }

        public override void Close()
        {
            // 如果有需要關閉其他資源，可在這裡進行
            base.Close();
        }
    }
}




