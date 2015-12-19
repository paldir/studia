using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using System.Diagnostics;

namespace WygaśnięcieCiasteczek
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Debug.WriteLine("App_Start");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Debug.WriteLine("Session_Start");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Debug.WriteLine("Session_End");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Debug.WriteLine("App_End");
        }
    }
}