using System.Net;
using System.Net.Sockets;

namespace Sender
{
    public delegate void ConnectedHandler(object sender, string error);
    public delegate void DisconnectedHandler(object sender);
    public class Client //: ISocket
    {
        private readonly Socket _clientSocket;
        private EndPoint _endPoint;
        private bool _isExecuting = false;

        private event ConnectedHandler _connectedHandler;
        private event DisconnectedHandler _disconnectedHandler;

        public Socket ClientTransferSocket { get { return _clientSocket; } }
        public bool IsExecuting { get { return _isExecuting; } }

        public Client(DisconnectedHandler disconnectedHandler)
        {
            _disconnectedHandler = disconnectedHandler;
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string serverIp, int serverPort, ConnectedHandler connectedHandler)
        {
            if (IsExecuting)
                return;
            else
            {
                _connectedHandler = connectedHandler;
                _isExecuting = true;
                _clientSocket.BeginConnect(serverIp, serverPort, ConnectedCallBack, null);
            }
        }

        private void ConnectedCallBack(IAsyncResult ar)
        {
            string error = "";
            try
            {
                _clientSocket.EndConnect(ar);
                _endPoint = _clientSocket.RemoteEndPoint;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            _connectedHandler(this, error);
        }

        public void Close()
        {
            if (!IsExecuting)
                return;
            else
            {
                _clientSocket.Close();
                _isExecuting = false;
                _disconnectedHandler(this);
            }
        }
    }
}
