using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebClient.proxies;

namespace WebClient
{
    public partial class SuperUserLoginWindow : System.Web.UI.Page
    {
        private SuperUserManagerClient _su;

        protected void Page_Load(object sender, EventArgs e)
        {
            _su = new SuperUserManagerClient();
        }


        protected void btnClick_login(object sender, EventArgs e)
        {
            if (_su.login(textBox_userName.Text, textBox_password.Text, textBox_email.Text))
            {
                //the concrete requirements for a web client were for basic user operations. the user functionality is muted for the time being
            }
            else
            {
                showAlert("couldn't log in");
            }
        }

        protected void btnClick_back(object sender, EventArgs e)
        {
            Response.Redirect("MainWindow.aspx");
        }

        private void showAlert(String content)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "popup", "<script>alert(\"" + content + "\");</script>");
        }
    }
}