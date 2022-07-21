using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TransferFile
{
    public class Sender
    {
        private static readonly byte[] WAIT = new byte[] { 0 };
        private static readonly List<Socket> sockets = new List<Socket>();

        //Send Function
        public static void Send(IPAddress iPAddress, int port, string filePath)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sockets.Add(socket);
            var localEndPoint = new IPEndPoint(iPAddress, port);
            try
            {
                socket.Connect(localEndPoint);
            }
            catch (Exception ex)
            {
                throw ex;
                return;
            }

            var fileInfo = new FileInfo(filePath);
            byte[] bytesFileInfo = Encoding.UTF8.GetBytes(fileInfo.Name + "|" + fileInfo.Length);

            socket.Send(bytesFileInfo);//Send Information A bout file Size
            socket.Receive(WAIT);

            int bytesRead;
            byte[] buffer = new byte[332800]; // 325 KB
            try
            {
                using (var networkStream = new NetworkStream(socket))
                using (var binaryWriter = new BinaryWriter(networkStream))
                using (var read = File.OpenRead(filePath))
                {
                    while ((bytesRead = read.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        binaryWriter.Write(buffer, 0, bytesRead);
                        binaryWriter.Flush();//Omit information After write
                    }
                }
            }
            catch { }
            finally
            {
                try
                {
                    if (socket.Connected)
                        socket.Disconnect(false);
                    socket.Close();
                    socket.Dispose();
                    socket = null;
                }
                catch { }
            }
        }

        //private static ListViewItem listViewItem(FileInfo fileInfo)
        //{
        //    var lvi = new ListViewItem();
        //    lvi.Text = fileInfo.Name;
        //    string strSize = String.Format("{0:0.##}", fileInfo.Length / 1024.0) + " KB";
        //    if (fileInfo.Length / 1024.0 > 1024)
        //    {
        //        strSize = String.Format("{0:0.##}", fileInfo.Length / 1024.0 / 1024.0) + " MB";
        //    }
        //    lvi.SubItems.Add(strSize);
        //    lvi.SubItems.Add("Sending...");
        //    lvi.SubItems.Add("");
        //    return lvi;
        //}

        //Close App
        public static void Close()
        {
            foreach (Socket item in sockets)
            {
                if (item != null)
                {
                    try
                    {
                        if (item.Connected)
                            item.Disconnect(false);

                        item.Close();
                        item.Dispose();
                    }
                    catch { }
                }
            }
        }
    }
}
