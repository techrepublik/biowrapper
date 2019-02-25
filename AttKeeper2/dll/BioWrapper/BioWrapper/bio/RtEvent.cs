using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BioWrapper.ent;

namespace BioWrapper.bio
{
    
    public class RtEvent : IUtil
    {
        public delegate void OnFingerAction();
        //public delegate void OnVerifyAction();
        //public delegate void OnAttTransactionAction();
        //public delegate void OnFingerFeatureAction();
        //public delegate void OnFingerExAction();
        //public delegate void OnDeleteTemplateAction();
        //public delegate void OnNewUserAction();

        public zkemkeeper.CZKEM axCZKEM1 = new zkemkeeper.CZKEM();

        private readonly Biometik Biometik = null;

        private bool _bIsConnected = false;//the boolean value identifies whether the device is connected
        //private int iMachineNumber = 1; //the serial number of the device.After connecting the device ,this value will be changed.

        public string IpAddress { get; set; }
        public int Port { get; set; }
        public int MachineNo { get; set; }
        public string SerialNo { get; set; }

        public string OnFinger { get; set; }
        public string OnVerify { get; set; }
        public string OnAttTransaction { get; set; }
        public string OnFingerFeature { get; set; }
        public string OnFingerEx { get; set; }
        public string OnDeleteTemplate { get; set; }
        public string OnNewUser { get; set; }


        public event OnFingerAction OnFingerEvent;
        public event OnFingerAction OnVerifyEvent;
        public event OnFingerAction OnAttTransactionEvent;
        public event OnFingerAction OnFingerFeatureEvent;
        public event OnFingerAction OnFingerExEvent;
        public event OnFingerAction OnDeleteTemplateEvent;
        public event OnFingerAction OnNewUserEvent;

        public RtEvent()
        {
            
        }

        public string OnFingerReturn()
        {
            return OnFinger;
        }
        
        public bool ConnectToDevice(MacType macType)
        {
            switch (macType)
            {
                case MacType.Net:
                    if (IpAddress.Trim() == "" || Port == 0) return false;
                    _bIsConnected = axCZKEM1.Connect_Net(IpAddress, Port);
                    break;
                case MacType.Com:
                    break;
                case MacType.Usb:
                    break;
                default:
                    break;
            }
            return _bIsConnected;
        }

        public void DisconnectToDevice()
        {
            axCZKEM1.Disconnect();
        }

        public void ExecuteEvent()
        {
            if (!_bIsConnected)
            {
                DisconnectToDevice();

                this.axCZKEM1.OnFinger -= new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
                this.axCZKEM1.OnVerify -= new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
                this.axCZKEM1.OnAttTransactionEx -=
                    new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
                this.axCZKEM1.OnFingerFeature -=
                    new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
                this.axCZKEM1.OnEnrollFingerEx -=
                    new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(axCZKEM1_OnEnrollFingerEx);
                this.axCZKEM1.OnDeleteTemplate -=
                    new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
                this.axCZKEM1.OnNewUser -= new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);

                _bIsConnected = false;
                return;
            }

            if (_bIsConnected)
            {
                MachineNo = 1; //In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.

                //this.rtTimer.Enabled = true;
                if (axCZKEM1.RegEvent(MachineNo, 65535))
                    //Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                {
                    this.axCZKEM1.OnFinger += new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
                    this.axCZKEM1.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
                    this.axCZKEM1.OnAttTransactionEx +=
                        new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
                    this.axCZKEM1.OnFingerFeature +=
                        new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
                    this.axCZKEM1.OnEnrollFingerEx +=
                        new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(axCZKEM1_OnEnrollFingerEx);
                    this.axCZKEM1.OnDeleteTemplate +=
                        new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
                    this.axCZKEM1.OnNewUser += new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);
                }
            }
        }

        #region events
            //When you place your finger on sensor of the device,this event will be triggered
        public void axCZKEM1_OnFinger()
        {
            OnFinger = string.Empty;
            OnFinger = "OnFinger Has been Triggered";
            Console.WriteLine(OnFinger);
            OnFingerEvent?.Invoke();
        }

        //After you have placed your finger on the sensor(or swipe your card to the device),this event will be triggered.
        //If you passes the verification,the returned value userid will be the user enrollnumber,or else the value will be -1;
        private void axCZKEM1_OnVerify(int iUserID)
        {
            OnVerify = string.Empty;
            OnVerify = ("Verifying...");
            if (iUserID != -1)
            {
                OnVerify = String.Format("OK, the UserID is {0}", iUserID.ToString());
            }
            else
            {
                OnVerify = String.Format("Failed... ");
            }
            Console.WriteLine(OnVerify);
            OnVerifyEvent?.Invoke();
        }

        //If your fingerprint(or your card) passes the verification,this event will be triggered
        private void axCZKEM1_OnAttTransactionEx(string sEnrollNumber, int iIsInValid, int iAttState, int iVerifyMethod, int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond, int iWorkCode)
        {
            var dateTime = iYear.ToString() + "-" + iMonth.ToString() + "-" + iDay.ToString() + " " + iHour.ToString() +
                           ":" + iMinute.ToString() + ":" + iSecond.ToString();

            OnAttTransaction = String.Format("Triggered for User: {0} : {1}", sEnrollNumber, dateTime);
            Console.WriteLine(OnAttTransaction);
            OnAttTransactionEvent?.Invoke();
        }

        //When you have enrolled your finger,this event will be triggered and return the quality of the fingerprint you have enrolled
        private void axCZKEM1_OnFingerFeature(int iScore)
        {
            if (iScore < 0)
            {
                OnFingerFeature="Fingerprint is poor";
            }
            else
            {
                OnFingerFeature="Score:　" + iScore.ToString();
            }
            Console.WriteLine(OnFingerFeature);
            OnFingerFeatureEvent?.Invoke();
        }

        //When you are enrolling your finger,this event will be triggered.
        private void axCZKEM1_OnEnrollFingerEx(string sEnrollNumber, int iFingerIndex, int iActionResult, int iTemplateLength)
        {
            Biometik.EnrollNo = sEnrollNumber;
            Biometik.FingerIndex = iFingerIndex;
            Biometik.TemplateLength = iTemplateLength;
            if (iActionResult == 0)
            {
                OnFingerEx = String.Format("Enroll finger activated for Enroll No: {0}, Index: {1}, " +
                                            "Action: {2}, Template: {3}", Biometik.EnrollNo, Biometik.FingerIndex,
                                            Biometik.ActionResult, Biometik.TemplateLength);
            }
            else
            {
                OnFingerEx = String.Format("RTEvent OnEnrollFigerEx Triggered an Error, actionResult=" + Biometik.ActionResult.ToString());
            }
            Console.WriteLine(OnFingerEx);
            OnFingerExEvent?.Invoke();
        }

        //When you have deleted one one fingerprint template,this event will be triggered.
        private void axCZKEM1_OnDeleteTemplate(int iEnrollNumber, int iFingerIndex)
        {
            Biometik.EnrollNo = iEnrollNumber.ToString();
            Biometik.FingerIndex = iFingerIndex;
            OnDeleteTemplate = String.Format("OnDeleteTemplate Triggered. Enroll No: {0}, Index: {1}", Biometik.EnrollNo, Biometik.FingerIndex);
            Console.WriteLine(OnDeleteTemplate);
            OnDeleteTemplateEvent?.Invoke();
        }

        //When you have enrolled a new user,this event will be triggered.
        private void axCZKEM1_OnNewUser(int iEnrollNumber)
        {
            Biometik.EnrollNo = iEnrollNumber.ToString();
            OnNewUser = String.Format("New User: {0}", Biometik.EnrollNo);
            Console.WriteLine(OnNewUser);
            OnNewUserEvent?.Invoke();
        }

        //After function GetRTLog() is called ,RealTime Events will be triggered. 
        //When you are using these two functions, it will request data from the device forwardly.
        //private void rtTimer_Tick(object sender, EventArgs e)
        //{
        //    if (axCZKEM1.ReadRTLog(iMachineNumber))
        //    {
        //        while (axCZKEM1.GetRTLog(iMachineNumber))
        //        {
        //            ;
        //        }
        //    }
        //}
        #endregion
    }
}
