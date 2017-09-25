using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebClient
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int realSession = -1;
                string sessionKey = (string)Session["sessionKey"];
                try
                {
                    realSession = Int32.Parse((sessionKey.Substring(0, sessionKey.IndexOf(","))));
                }
                catch
                {
                    realSession = -1;
                }
                if (realSession > 0)
                {
                    mySessionKey.Text = "your session key is: " + realSession.ToString();
                }
                else
                {
                    try
                    {
                        realSession = Int32.Parse(sessionKey);
                    }
                    catch
                    {
                        realSession = -1;
                    }
                    if (realSession > 0)
                    {
                        mySessionKey.Text = "your session key is: " + realSession.ToString();
                    }
                }
            }
            catch { }
        }
    }
}