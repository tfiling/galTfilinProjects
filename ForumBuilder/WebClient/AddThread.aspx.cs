using PL.notificationHost;
using PL.proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebClient
{
    public partial class AddThread : System.Web.UI.Page
    {
        private PostManagerClient _pm;
        private SubForumManagerClient _sm;

        protected void Page_Load(object sender, EventArgs e)
        {
            _sm = new SubForumManagerClient();
            _pm = new PostManagerClient();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            String createTread = _sm.createThread(Title.Text, Content.Text, Session["userName"].ToString(),
                Session["forumName"].ToString(), Session["subFoumName"].ToString());
            if (createTread.Equals("Create tread succeed"))
            {
                showAlert("thread was added successfully");
                Response.Redirect("subForumWebPage.aspx");
            }
            {
                showAlert(createTread);
            }
        }
        private void showAlert(String content)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "popup", "<script>alert(\"" + content + "\");</script>");
        }
        protected void backButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("subForumWebPage.aspx");
        }
    }
}