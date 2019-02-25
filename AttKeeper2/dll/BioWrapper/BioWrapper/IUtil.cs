using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BioWrapper
{
    public enum MacType
    {
        Net, 
        Usb,
        Com,
    };

    interface IUtil
    {
        string IpAddress { get; set; }
        int Port { get; set; }
        int MachineNo { get; set; }

        bool ConnectToDevice(MacType macType);
        void DisconnectToDevice();
    }
}
