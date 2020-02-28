using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialNetworks.FatekCom
{
    public sealed class FatekSingleton
    {
        private FatekSingleton()
        {
            fatekCommunication = new FatekCommunication("COM7", 9600, System.IO.Ports.Parity.Even, 7, System.IO.Ports.StopBits.One);
        }

        public FatekSingleton(string COMNo, int baudRate)
        {

        }

        FatekCommunication fatekCommunication = null;
    }
}
