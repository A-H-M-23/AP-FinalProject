using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferFile
{
    public class ReceiveFileInfo
    {
        public string FileName { get; set; }
        public double FileSize { get; set; }
        private Receiver receiver;

        public ReceiveFileInfo(Receiver _receiver)
        {
            receiver = _receiver;
        }

        public void LoadData()
        {
            receiver.LoadFile();
        }
    }
}
