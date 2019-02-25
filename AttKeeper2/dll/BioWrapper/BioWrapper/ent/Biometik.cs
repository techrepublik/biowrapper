using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioWrapper.ent
{
    public class Biometik
    {
        //private static Biometik _biometik;
        ///* 
        //    singleton implementation
        //*/
        //private Biometik() { }

        //public static Biometik GetBiometik()
        //{
        //    return _biometik ?? (_biometik = new Biometik());
        //}

        public string EnrollNo { get; set; }
        public DateTime GetDateTime
        {
            get
            {
                var sTime = String.Format("{0}-{1}-{2} {3}:{4}:{5}", Convert.ToInt16(Year),
                                                                      Convert.ToInt16(Month),
                                                                      Convert.ToInt16(Day),
                                                                      Convert.ToInt16(Hour),
                                                                      Convert.ToInt16(Minute),
                                                                      Convert.ToInt16(Second));
                return Convert.ToDateTime(sTime);
            }
        }

        /*  fingerprint enroll 
            enrollment fields
        */
        public int FingerIndex { get; set; } 
        public int ActionResult { get; set; } 
        public int TemplateLength { get; set; }
        
        /*
            attendance process
            attendance fields
        */
        public int MachineNo { get; set; }
        public int Invalid { get; set; }
        public int AttState { get; set; }
        public int VerifyMethod { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int WorkCode { get; set; }
        public int InOutMode { get; set; }

        /*
            finger feature
        */
        public int Score { get; set; }

        
    }
}
