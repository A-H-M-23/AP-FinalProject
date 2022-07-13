using System.Net;
using System.Net.Sockets;

namespace Receiver
{
    public delegate void AcceptedSocketHandler(object sender, CurrentSocket e);

    public class Server //: ISocket
    {
        private Socket _serverSocket = null;
        private int _serverPort = -1;
        private string _serverIp = "";
        private bool _isExecuting = false;

        private event AcceptedSocketHandler _accepteHandler;

        public bool IsExecuting{ get { return _isExecuting; } }
        public int Port { get { return _serverPort; } }
        public string AddressIP { get { return _serverIp; } }
        public Socket TransferSocket { get; private set; }

        public Server(AcceptedSocketHandler handler)
        {
            _accepteHandler = handler;
        }

        public static Task<List<string>> GetAllIP()
        {
            List<string> Temp = new List<string>();
            string HostName = Dns.GetHostName();

            IPHostEntry AllIPs = Dns.GetHostEntry(HostName);

            foreach (var item in AllIPs.AddressList)
                if (item.AddressFamily == AddressFamily.InterNetwork)
                    Temp.Add(item.ToString());

            return Task.FromResult(Temp);
        }

        public void Start(string ip, int port)
        {
            if (IsExecuting)
                return;
            else
            {
                _serverPort = port;
                _serverIp = ip;
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(new IPEndPoint(IPAddress.Parse(_serverIp), _serverPort));
                _serverSocket.Listen(1);
                _isExecuting = true;
                _serverSocket.BeginAccept(CallBack, null);
            }
        }

        public void StartReAccept(AcceptedSocketHandler handler)
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Parse(_serverIp), _serverPort));
            _serverSocket.Listen(1);
            _isExecuting = true;
            _serverSocket.BeginAccept(CallBack, null);
            _accepteHandler = handler;
        }

        private void CallBack(IAsyncResult ar)
        {
            try
            {
                Socket socket = _serverSocket.EndAccept(ar);
                if (socket != null)
                {
                    if (TransferSocket == null || !TransferSocket.Connected)
                    {
                        TransferSocket = socket;
                        _accepteHandler(this, new CurrentSocket(socket));
                        return;
                    }
                }
            }
            catch
            {
                _isExecuting = false;
                Close();
            }
            if (IsExecuting)
                _serverSocket.BeginAccept(CallBack, null);
            else return;
        }

        public void Stop()
        {
            if (!IsExecuting)
                return;

            Close();

            _isExecuting = false;
        }

        public void Close()
        {
            if (TransferSocket != null)
            {
                TransferSocket.Close();
                TransferSocket.Dispose();
            }

            if (_serverSocket != null)
            {
                _serverSocket.Close();
                _serverSocket.Dispose();
            }
        }
    }
}
