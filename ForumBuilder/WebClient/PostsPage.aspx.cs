using ForumBuilder.Common.DataContracts;
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
    public partial class PostsPage : System.Web.UI.Page
    {
        private PostManagerClient _pm;
        private ForumManagerClient _fm;
        private PostData _thread;
        private string _forumName;
        private string _subForumName;

        protected void Page_Load(object sender, EventArgs e)
        {
            _fm = new ForumManagerClient(new InstanceContext(new ClientNotificationHost()));
            _pm = new PostManagerClient();
            while (Session["thread"] == null)
            {

            }
            if (Session["userName"].ToString().Equals("Guest"))
            {
                Button1.Visible = false;
            }
            _forumName = Session["forumName"].ToString();
            _subForumName = Session["subForumName"].ToString();
            _thread = (PostData)Session["thread"];
            forumNameLabel.Text = _forumName;
            subForumNameLabel.Text = _subForumName;
            List<PostData> posts = _pm.getAllPosts(_forumName, _subForumName);
            List<PostData> postsToDesplay = new List<PostData>();
            postsToDesplay.Add(_thread);
            foreach (PostData post in posts)
            {
                if (post.parentId == _thread.id)
                {
                    postsToDesplay.Add(post);
                }
            }
            int num = 1;
            foreach (PostData post in postsToDesplay)
            {
                TableRow tRow1 = new TableRow();
                TableRow tRow2 = new TableRow();
                TableRow tRow3 = new TableRow();
                //1 field
                TableCell tCell1 = new TableCell();
                Label lb1 = new Label();
                //lb1.Text = "#" + num;
                //lb1.Width = 35;
                tCell1.Controls.Add(lb1);
                //tRow1.Cells.Add(tCell1);
                //field 2
                //TableCell tCell2 = new TableCell();
                Label lb2 = new Label();
                lb2.Text = post.title;
                lb2.Height = 35;
                lb2.Style.Add("font-size", "20px");                
                lb2.Style.Add("font - weight","bold");
                tCell1.Controls.Add(lb2);
                tRow1.Cells.Add(tCell1);
                //field 3
                TableCell tCell3 = new TableCell();
                Label lb3 = new Label();
                lb3.Text = post.writerUserName;
                lb3.Width = 120;
                tCell3.Controls.Add(lb3);
                //tRow2.Cells.Add(tCell3);
                //field 4
                //TableCell tCell4 = new TableCell();
                Label lb4 = new Label();
                lb4.Text = post.timePublished.ToString();
                tCell3.Controls.Add(lb4);
                tRow2.Cells.Add(tCell3);
                //field 5
                TableCell tCell5 = new TableCell();
                TextBox lb5 = new TextBox();
                lb5.Enabled = false;
                lb5.Text = post.content.ToString();
                lb5.Height = 80;
                lb5.Width = 300;
                tRow3.HorizontalAlign = HorizontalAlign.Left;
                lb5.BackColor = System.Drawing.Color.White;
                tCell5.Controls.Add(lb5);
                tRow3.Cells.Add(tCell5);
                //double doubleHeight = tRow2.Height.Value * 4;
                //int height = Convert.ToInt32(doubleHeight);
                tRow3.Height = 80;
                tRow3.Width = 300;
                //add rows
                PostsTable.Rows.Add(tRow1);
                PostsTable.Rows.Add(tRow2);
                PostsTable.Rows.Add(tRow3);
                num++;
            }
            PostsTable.BorderWidth = 3;
            PostsTable.BorderStyle = BorderStyle.Ridge;
            PostsTable.BackColor = System.Drawing.Color.White;
            PostsTable.ForeColor = System.Drawing.Color.Black;
            PostsTable.GridLines = GridLines.Both;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddPostPage.aspx");
        }

        protected void backButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("subForumWebPage.aspx");
        }
    }
}