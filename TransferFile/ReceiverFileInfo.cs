using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferFile
{
    //Get the file information(Name & Size)
    public class ReceiveFileInfo
    {
        public string FileName { get; set; }
        public double FileSize { get; set; }
        private Receiver receiver;

        //Create a Constructor
        public ReceiveFileInfo(Receiver _receiver)
        {
            //set the information
            receiver = _receiver;
        }

        //loading...
        public void LoadData()
        {
            receiver.LoadFile();
        }
    }
}
