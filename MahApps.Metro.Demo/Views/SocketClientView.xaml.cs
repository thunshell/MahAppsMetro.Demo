using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
    /// SocketClientView.xaml 的互動邏輯
    /// </summary>
    public partial class SocketClientView : UserControl
    {
        public SocketClientView()
        {
            InitializeComponent();
        }



        public bool IsServer
        {
            get { return (bool)GetValue(IsServerProperty); }
            set { SetValue(IsServerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsServer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsServerProperty =
            DependencyProperty.Register("IsServer", typeof(bool), typeof(SocketClientView), new PropertyMetadata(false, new PropertyChangedCallback(IsServerChanged)));

        private static void IsServerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SocketClientView scv = (SocketClientView)d;
            bool isServer = Convert.ToBoolean(e.NewValue);
            scv.btnConnect.Content = isServer ? "开 启" : "连 接";
        }

        /// <summary>
        /// 服务端使用该参数发送接收数据
        /// </summary>
        Socket Client { get; set; }
        /// <summary>
        /// 客户端使用该参数发送接收数据
        /// </summary>
        //Socket Server { get; set; }

        /// <summary>
        /// 客户端连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (IsServer)
                StartServer();
            else
                StartClient();
        }

        /// <summary>
        /// 客户端连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisConnect_Click(object sender, RoutedEventArgs e)
        {
            cc.Disconnect(false);
            //if (IsServer)
            //    cc?.Dispose();
            //else
            //{
            //    Client?.Dispose();
            //    Client = null;
            //}
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = tbMsg.Text;
                if (string.IsNullOrWhiteSpace(msg)) return;
                byte[] bytes = Encoding.UTF8.GetBytes(tbMsg.Text);
                AddMsg(Client.LocalEndPoint.ToString() + NowTimeText, IsServer ? Brushes.DarkRed : Brushes.DarkGreen);
                AddMsg(msg);
                if (IsServer)
                    Client.Send(bytes);
                else
                    Client.Send(bytes);
            }
            catch(Exception ex) { ShowMessageBox(ex); }
        }

        #region Client

        private void StartClient()
        {
            try
            {
                Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //IPAddress iP = IPAddress.Parse(tbIP.Text);
                //IPEndPoint point = new IPEndPoint(iP, int.Parse(tbPort.Text));
                Client.Connect(tbIP.Text, int.Parse(tbPort.Text));
                AddMsg("连接成功。", Brushes.DeepSkyBlue);
                AddMsg($"服务器:{Client.RemoteEndPoint.ToString()}", Brushes.DeepSkyBlue);
                AddMsg($"客户端:{Client.LocalEndPoint.ToString()}", Brushes.DeepSkyBlue);
                Thread thread = new Thread(RecieveServerMsg);
                thread.IsBackground = true;
                thread.Start();
            }
            catch(Exception ex)
            {
                Client.Dispose();
                Client = null;
                ShowMessageBox(ex);
            }
        }

        private void RecieveServerMsg()
        {
            while (true)
            {
                try
                {
                    byte[] bytes = new byte[1024 * 1024];
                    int n = Client.Receive(bytes);
                    if (n <= 0)
                    {
                        throw new Exception("Connection interrupted by received none.");
                    }
                    string msg = Encoding.UTF8.GetString(bytes, 0, n);
                    AddMsg($"{Client.RemoteEndPoint.ToString()}{NowTimeText}", Brushes.DarkRed);
                    AddMsg($"\t{msg}");
                }
                catch (Exception ex)
                {
                    Client?.Dispose();
                    ShowMessageBox(ex);
                    return;
                }
            }
        }
        #endregion

        #region Server

        private void StartServer()
        {
            try
            {
                IPAddress ip = IPAddress.Parse(tbIP.Text);
                IPEndPoint point = new IPEndPoint(ip, int.Parse(tbPort.Text));
                Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Client.Bind(point);
                Client.Listen(10);
                AddMsg($"{NowTimeText}服务已开启", Brushes.DeepSkyBlue);

                Thread thread = new Thread(AcceptInfo);
                thread.IsBackground = true;
                thread.Start(Client);
            }
            catch(Exception ex)
            {
                Client.Dispose();
                Client = null;
                ShowMessageBox(ex);
            }
        }

        Socket cc = null;
        private void AcceptInfo(object o)
        {
            Socket socket = o as Socket;
            while (true)
            {
                Socket client = null;
                try
                {
                    client = socket.Accept();
                    cc = client;
                    string point = client.RemoteEndPoint.ToString();
                    AddMsg($"{NowTimeText}客户端{point}请求连接.", Brushes.DeepSkyBlue);
                    Thread thread = new Thread(RecieveMsg);
                    thread.IsBackground = true;
                    thread.Start(client);
                }
                catch (Exception ex)
                {
                    client?.Dispose();
                    ShowMessageBox(ex);
                }
            }
        }

        private void RecieveMsg(object o)
        {
            Socket client = o as Socket;
            while (true)
            {
                try
                {
                    byte[] bytes = new byte[1024 * 1024];
                    int n = client.Receive(bytes);
                    if (n <= 0)
                    {
                        throw new Exception("Connection interrupted by received none.");
                    }
                    string msg = Encoding.UTF8.GetString(bytes, 0, n);
                    AddMsg(client.RemoteEndPoint.ToString() + NowTimeText, Brushes.DarkGreen);
                    AddMsg(msg);
                }
                catch (Exception ex)
                {
                    client?.Dispose();
                    ShowMessageBox(ex);
                    return;
                }
            }
        }

        #endregion

        void AddMsg(string msg, SolidColorBrush brush = null)
        {
            Dispatcher.Invoke(new Action(() => rtbMsg.Document.Blocks.Add(new Paragraph(new Run(msg) { Foreground = brush == null ? Brushes.Black : brush }) { LineHeight = 10 })));
        }

        public string NowTimeText => DateTime.Now.ToString(" yyyy-MM-dd HH:mm:ss:");

        void ShowMessageBox(Exception ex)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                MetroWindow window = App.Current.MainWindow as MetroWindow;
                MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync(window, "错误", ex.ToString());
            }));
        }
    }
}
