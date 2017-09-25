using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using ForumBuilder.Common.DataContracts;
using System.Threading;
using ForumBuilder.Common.ClientServiceContracts;
using WebClient.proxies;
using PL.notificationHost;

namespace WebClient
{
    public partial class subForumWebPage : System.Web.UI.Page
    {
        private PostManagerClient _pm;
        private ForumManagerClient _fm;
        private string _forumName;
        private string _subForumName;
        private PostData[] _postByNumArray;

        protected void Page_Load(object sender, EventArgs e)
        {
            _fm = new ForumManagerClient(new InstanceContext(new ClientNotificationHost()));
            _pm = new PostManagerClient();
            while (Session["forumName"] == null)
            {

            }
            if (Session["userName"].ToString().Equals("Guest"))
            {
                addThreadButton.Visible = false;
            }
            _forumName = Session["forumName"].ToString();
            _subForumName = Session["subForumName"].ToString();
            forumNameLabel.Text = _forumName;
            subForumNameLabel.Text = _subForumName;
            List<PostData> posts = _pm.getAllPosts(_forumName, _subForumName);
            _postByNumArray = new PostData[posts.Count()];
            int num = 0;
            foreach (PostData post in posts)
            {
                if (post.parentId == -1)
                {
                    TableRow tRow = new TableRow();

                    //1 field
                    TableCell tCell1 = new TableCell();
                    Button lb1 = new Button();
                    lb1.Click += new EventHandler(clickOnSubForum);
                    lb1.Text = "#" + num + 1;
                    lb1.BackColor = System.Drawing.Color.White;
                    tCell1.Controls.Add(lb1);
                    tRow.Cells.Add(tCell1);
                    //field 2
                    TableCell tCell2 = new TableCell();
                    Button lb2 = new Button();
                    lb2.Click += new EventHandler(clickOnSubForum);
                    lb2.Text = post.title;
                    lb1.BackColor = System.Drawing.Color.White;
                    tCell2.Controls.Add(lb2);
                    tRow.Cells.Add(tCell2);
                    //field 3
                    TableCell tCell3 = new TableCell();
                    Button lb3 = new Button();
                    lb3.Click += new EventHandler(clickOnSubForum);
                    lb3.Text = post.writerUserName;
                    lb1.BackColor = System.Drawing.Color.White;
                    tCell3.Controls.Add(lb3);
                    tRow.Cells.Add(tCell3);
                    //field 4
                    TableCell tCell4 = new TableCell();
                    Button lb4 = new Button();
                    lb4.Click += new EventHandler(clickOnSubForum);
                    lb4.Text = post.timePublished.ToString();
                    lb1.BackColor = System.Drawing.Color.White;
                    tCell4.Controls.Add(lb4);
                    tRow.Cells.Add(tCell4);
                    //add row
                    ThreadTable.Rows.Add(tRow);
                    _postByNumArray[num] = post;
                    num++;
                }
            }
            ThreadTable.BorderWidth = 3;
            ThreadTable.BorderStyle = BorderStyle.Ridge;
            ThreadTable.BackColor = System.Drawing.Color.White;
            ThreadTable.ForeColor = System.Drawing.Color.Black;
            ThreadTable.GridLines = GridLines.Both;
            ThreadTable.Width = 300;

        }
        protected void clickOnSubForum(Object sender, EventArgs e)
        {
            int num = 0;
            foreach (TableRow row in ThreadTable.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    foreach (var singleControl in cell.Controls)
                    {
                        Button button = singleControl as Button;
                        if (button.Equals(sender))
                        {
                            PostData thread = _postByNumArray[num];
                            Session["thread"] = thread;
                            Response.Redirect("PostsPage.aspx");
                        }
                    }
                }
                num++;
            }
        }
        private void showAlert(String content)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "popup", "<script>alert(\"" + content + "\");</script>");
        }

        protected void addThreadButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddThreadPage.aspx");
        }

        protected void backButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForumWindow.aspx");
        }
    }
}