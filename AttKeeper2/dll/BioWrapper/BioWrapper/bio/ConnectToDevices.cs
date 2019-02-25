using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BioWrapper.bio
{
    public class ConnectToDevices : IUtil
    {
        public static zkemkeeper.CZKEM axCZKEM1 = new zkemkeeper.CZKEM();

        public string IpAddress { get; set; }
        public  int Port { get; set; }
        public  int MachineNo { get; set; }
        public MacType MacType { get; set; }
        public int LastError { get; set; }
        public bool ConnectToDevice()
        {
            var bIsconnected = false;
            switch (MacType)
            {
                case MacType.Net:
                    if (IpAddress.Trim() == "" || Port == 0) return false;
                    bIsconnected = axCZKEM1.Connect_Net(IpAddress, Port);
                    break;
                case MacType.Com:
                    break;
                case MacType.Usb:
                    break;
                default:
                    bIsconnected = false;
                    break;
            }
            return bIsconnected;
        }
        public bool ConnectToDevice(MacType macType)
        {
            var bIsconnected = false;
            switch (macType)
            {
                case MacType.Net:
                    if (IpAddress.Trim() == "" || Port == 0) return false;
                    bIsconnected = axCZKEM1.Connect_Net(IpAddress, Port);
                    break;
                case MacType.Com:
                    break;
                case MacType.Usb:
                    break;
                default:
                    bIsconnected = false;
                    break;
            }
            return bIsconnected;
        }

        public void DisconnectToDevice()
        {
            axCZKEM1.Disconnect();
        }

    }
}
