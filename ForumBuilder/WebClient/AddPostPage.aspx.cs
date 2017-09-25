using ForumBuilder.Common.DataContracts;
using PL.proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebClient
{
    public partial class AddPostPage : System.Web.UI.Page
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
            string userName = Session["userName"].ToString();
            int threadId = ((PostData)Session["thread"]).id;
            String addPost = _pm.addPost(titleBox.Text, contentBox.Text, userName, threadId);
            if (addPost.Equals("comment created"))
            {
                showAlert("post was added successfully");
                Response.Redirect("PostsPage.aspx");
            }
            {
                showAlert(addPost);
            }
        }
        private void showAlert(String content)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "popup", "<script>alert(\"" + content + "\");</script>");
        }

        protected void backButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("PostsPage.aspx");
        }
    }
}