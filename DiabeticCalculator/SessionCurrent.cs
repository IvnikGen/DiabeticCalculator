using SqlConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiabeticCalculator
{
    [Serializable]
    public class SessionCurrent
    {
        private const string SESSION_NAME = "Singleton_Current_DiaCalc_User_Session";

        private SessionCurrent()
        {

        }

        public static SessionCurrent Current
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME] == null)
                {
                    HttpContext.Current.Session[SESSION_NAME] = new SessionCurrent();
                }

                return HttpContext.Current.Session[SESSION_NAME] as SessionCurrent;
            }
            set { }
        }

        public List<Products> CurentBreadUnitsList { get; set; }

    }
}