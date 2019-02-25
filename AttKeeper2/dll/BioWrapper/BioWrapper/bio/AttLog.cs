using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioWrapper.bio;
using BioWrapper.ent;

namespace BioWrapper
{
    public class AttLog
    {
        public zkemkeeper.CZKEM axCZKEM1 = new zkemkeeper.CZKEM();

        private bool _bConnected = false;

        string _sdwEnrollNumber = "";
        int _idwTMachineNumber = 0;
        int _idwEMachineNumber = 0;
        int _idwVerifyMode = 0;
        int _idwInOutMode = 0;
        int _idwYear = 0;
        int _idwMonth = 0;
        int _idwDay = 0;
        int _idwHour = 0;
        int _idwMinute = 0;
        int _idwSecond = 0;
        int _idwWorkcode = 0;

        int _idwErrorCode = 0;

        private ConnectToDevices _con = null;

        public bool ConnectNet(MacType macType, string ipAddress, int port, int machineNo)
        {
            _con = new ConnectToDevices
            {
                MacType = MacType.Net,
                IpAddress = ipAddress,
                Port = port,
                MachineNo = machineNo
            };

            _bConnected = axCZKEM1.Connect_Net(ipAddress, port);
            if (!_bConnected)
                axCZKEM1.GetLastError(_idwErrorCode);

            return _bConnected;
        }

        /************************************
            get all record/log/attendance method
        ************************************/
        public List<Biometik> GetAttLogAll()
        {
            var lBio = new List<Biometik>();
            

            if (!_bConnected) return lBio;
            //disable the device
            axCZKEM1.EnableDevice(_con.MachineNo, false);

            if (axCZKEM1.ReadGeneralLogData(_con.MachineNo))
            {
                while (axCZKEM1.SSR_GetGeneralLogData(_con.MachineNo, out _sdwEnrollNumber, out _idwVerifyMode,
                    out _idwInOutMode, out _idwYear, out _idwMonth, out _idwDay, out _idwHour, out _idwMinute, out _idwSecond, ref _idwWorkcode))//get records from the memory
                {
                    var bio = new Biometik
                    {
                        MachineNo = _con.MachineNo,
                        EnrollNo = _sdwEnrollNumber,
                        VerifyMethod = _idwVerifyMode,
                        InOutMode = _idwInOutMode,
                        Year = _idwYear,
                        Month = _idwMonth,
                        Day = _idwDay,
                        Hour = _idwHour,
                        Minute = _idwMinute,
                        Second = _idwSecond,
                        WorkCode = _idwWorkcode
                    };
                    lBio.Add(bio);
                }
            }
            else
            { 
                axCZKEM1.GetLastError(ref _idwErrorCode);
            }
            //enable device
            axCZKEM1.EnableDevice(_con.MachineNo, true);
            return lBio;
        }

        /************************************
            clear all record/log/attendance method
        ************************************/
        public bool ClearAttLogAll()
        {
            if (!_bConnected) return false;
            var bClear = false;
            //disable the device
            axCZKEM1.EnableDevice(_con.MachineNo, false);

            if (axCZKEM1.ClearGLog(_con.MachineNo))
            {
                axCZKEM1.RefreshData(_con.MachineNo);//the data in the device should be refreshed
                bClear = true;
            }
            else
            {
                axCZKEM1.GetLastError(ref _idwErrorCode);
            }
            //enable device
            axCZKEM1.EnableDevice(_con.MachineNo, true);
            return bClear;
        }

        /************************************
            count all record/log/attendance method
        ************************************/
        public int GetLogCountAll()
        {
            if (!_bConnected) return -1;
            var iCount = 0;

            axCZKEM1.EnableDevice(_con.MachineNo, false);

            if (axCZKEM1.GetDeviceStatus(_con.MachineNo, 6, ref iCount))
            {
                Console.WriteLine(String.Format("Record Counted {0}", iCount));
            }
            else
            {
                axCZKEM1.GetLastError(ref _idwErrorCode);
            }
            //enable device
            axCZKEM1.EnableDevice(_con.MachineNo, true);
            return iCount;
        }
    }
}
