using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UI.UserControls;

namespace TransferFile
{
    public class Listen
    {
        private static Socket socket;
        private static bool IsStop;

        public static void Start(int port)
        {
            if (socket != null)
                return;

            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.Listen(100);
            }
            catch (Exception ex)
            {
                Stop();
                throw ex;
            }

            IsStop = false;

            new Thread(() =>
            {
                //var client = socket.Accept();
                //var fileInfo = ReceiverEngine.GetFileInfo(client);
                //fileSend = new UserFileSend();
                //fileSend.Username = fileInfo.FileName;
                //fileSend.Address = System.AppDomain.CurrentDomain.BaseDirectory + @"Downloads\" + fileInfo.FileName;
                //mainPanel = new MainPanel();
                //mainPanel.AddMessageToUi(fileSend);
                while (true)
                {
                    try
                    {
                        var clients = socket.Accept();

                        new Thread(() =>
                        {
                            var fileInfo = ReceiverEngine.GetFileInfo(clients);
                            fileInfo.LoadData();
                        }).Start();
                    }
                    catch (Exception ex)
                    {
                        if (!IsStop)
                            throw ex;
                        break;
                    }
                }
            }).Start();
        }

        //private static ListViewItem listViewItem(ReceiveFileInfo fileInfo)
        //{
        //    var lvi = new ListViewItem();
        //    lvi.Text = fileInfo.FileName;
        //    string strSize = fileInfo.FileSize + " KB";
        //    if (fileInfo.FileSize > 1024)
        //    {
        //        strSize = String.Format("{0:0.##}", fileInfo.FileSize / 1024.0) + " MB";
        //    }
        //    lvi.SubItems.Add(strSize);
        //    lvi.SubItems.Add("Receiving...");
        //    lvi.SubItems.Add("");
        //    return lvi;
        //}

        public static void Stop()
        {
            IsStop = true;
            if (socket != null)
            {
                if (socket.Connected)
                    socket.Disconnect(false);

                socket.Close();
                socket.Dispose();
                socket = null;
            }
        }
    }
}
