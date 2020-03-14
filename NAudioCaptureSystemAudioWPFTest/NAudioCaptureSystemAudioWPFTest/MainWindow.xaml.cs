using NAudio.Wave;
using System;
using System.IO;
using System.Windows;

namespace NAudioCaptureSystemAudioWPFTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        WasapiLoopbackCapture CaptureInstance = null;

        WaveFileWriter RecordedAudioWriter = null;
        private bool recording = false;
        //声明一个路径字符串，作为文件的文件夹
        string audioCaptureDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\Audio Captures\\";
        string outputFilePath;
        public MainWindow()
        {
            InitializeComponent();
            //创建该路径文件夹
           Directory.CreateDirectory(audioCaptureDirectory);
        }

        private void Record_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!recording)
            {
                outputFilePath = audioCaptureDirectory + "Audio"+DateTime.Now.ToString().Replace("/", "-").Replace(":", "-") + ".wav";
                // 实例化
                CaptureInstance = new WasapiLoopbackCapture();
                // 使用给定的配置定义音频编写器实例
                RecordedAudioWriter = new WaveFileWriter(outputFilePath, CaptureInstance.WaveFormat);

                // 捕获器获得音频后，开始将buffer写入指定文件
                CaptureInstance.DataAvailable += (s, a) =>
                {
                    // 将buffer写入文件中
                    RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
                };

                // 录制结束后，释放资源
                CaptureInstance.RecordingStopped += (s, a) =>
                {
                    RecordedAudioWriter.Dispose();
                    RecordedAudioWriter = null;
                    CaptureInstance.Dispose();
                    CaptureInstance = null;
                };

                CaptureInstance.StartRecording();
                Record_btn.Content = "Stop\nRecording";
                recording = true;
            }
            else
            {
                CaptureInstance.StopRecording();

                CaptureInstance.RecordingStopped += (s, a) =>
                {
                    //to do.
                };

                Record_btn.Content = "Start\nRecording";
                recording = false;
                FilePath_tb.Visibility = Visibility.Visible;
                FilePath_tb.Text += "\n"+outputFilePath;
            }
        }
    }
}
