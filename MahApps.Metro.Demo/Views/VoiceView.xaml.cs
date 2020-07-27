using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MahAppsMetro.Demo.Views
{
    /// <summary>
    /// VoiceView.xaml 的互動邏輯
    /// </summary>
    public partial class VoiceView : UserControl
    {
        public VoiceView()
        {
            InitializeComponent();
        }

        private void btnBeep_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                for (int i = 37; i < 32768; i++)
                {
                    Console.Beep(i, 100);
                    this.Dispatcher.Invoke(new Action(() =>  this.tbBeepInt.Text = i.ToString()));
                    Thread.Sleep(100);
                }
            });
        }

        int index = 1;
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction,int uParam,string lpvParam,int fuWinIni);
        private void ChangeWindowTheme(object sender, RoutedEventArgs e)
        {
            //Image image = Image.FromFile("D:\\AAA.jpg");
            //image.Save("D:\\AAA.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            Task.Run(() =>
            {
                do
                {
                    SystemParametersInfo(20, 0, "E:\\YCX\\Sources\\素材\\background\\" + index++.ToString() + ".png", 0x2);
                    if (index > 6)
                        index = 1;
                    Thread.Sleep(100);
                } while (true);
            });
        }
    }
}
