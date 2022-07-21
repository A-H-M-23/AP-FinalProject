using System.Net.Sockets;
using System.Text;

namespace TransferFile
{
    public class ReceiverEngine
    {
        private static readonly byte[] RESUME = new byte[] { 0 };

        public static ReceiveFileInfo GetFileInfo(Socket socket)
        {
            byte[] buffer = new byte[512];//Consider A buffer
            int bytesRead = socket.Receive(buffer, SocketFlags.None);
            var fileInfo = Encoding.UTF8.GetString(buffer, 0, bytesRead).Split('|');//Encode information with UTF8
            socket.Send(RESUME);
            return new ReceiveFileInfo(new Receiver(socket, fileInfo[0]))
            {
                //Set the Name & Size of file
                FileName = fileInfo[0],
                FileSize = Double.Parse(String.Format("{0:0.##}", long.Parse(fileInfo[1]) / 1024.0))
            };
        }
    }
}
