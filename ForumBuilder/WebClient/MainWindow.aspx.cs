using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebClient.proxies;
using System.ServiceModel;
using ForumBuilder.Common.DataContracts;
using System.Threading;
using ForumBuilder.Common.ClientServiceContracts;

namespace WebClient
{
    public partial class MainWindow : System.Web.UI.Page, IUserNotificationsService
    {
        private List<string> _forumsList;
        private String _choosenForum;
        private ForumManagerClient _fMC;

        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.Sleep(3000);

            _fMC = new ForumManagerClient(new InstanceContext(this));
            _forumsList = _fMC.getForums();
        }

        protected void Btn_ImSuperUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("SuperUserLoginWindow.aspx");
        }

        protected void Btn_Login_Click(object sender, EventArgs e)
        {
            _choosenForum = forum_dropList.SelectedItem.ToString();
            Session["UserName"] = ID.Text;
            if (Password.Text != "" && SessionKeyTextField.Text == "")
            {
                Session["Password"] = Password.Text;
                try
                {
                    _choosenForum  = forum_dropList.SelectedItem.Text;
                }
                catch
                {
                    showAlert("choose a forum");
                    return;
                }
                String sessionKey = "-1";//general login error code
                if (_choosenForum != null)
                {
                    ForumData toSend = _fMC.getForum(_choosenForum);
                    if (CheckBox_Guest.Checked)
                    {
                        Session["forumName"] = _choosenForum;
                        Session["userName"] = "Guest";
                        Session["ForumManagerClient"] = _fMC;
                        Response.Redirect("ForumWindow.aspx");
                    }
                    else if ((sessionKey = _fMC.login(Session["UserName"].ToString(), _choosenForum, Session["Password"].ToString())).Contains(","))
                    {
                        Session["forumName"] = _choosenForum;
                        Session["userName"] = Session["UserName"];
                        Session["ForumManagerClient"] = _fMC;
                        Session["sessionKey"] = sessionKey;
                        Response.Redirect("ForumWindow.aspx");
                    }
                    else
                    {
                        switch (sessionKey)
                        {
                            case "-1":
                                showAlert("login failed");
                                break;

                            case "-2":
                                showAlert("user name \\ password are invalid");
                                break;

                            case "-3":
                                showAlert("you already connected via another client, " +
                                            "please login using your session key");
                                SessionKeyTextField.Visible = true;
                                sessionKeyLabel.Visible = true;
                                Password.Visible = false;
                                passwordLabel.Visible = false;
                                ID.Visible = false;
                                userNameLabel.Visible = false;
                                LogInWithDiffUserButton.Visible = true;
                                SessionKeyTextField.Text = "";

                                break;

                            default:
                                showAlert("login failed");
                                break;
                        }
                    }

                }
                else
                {
                    showAlert("choose a forum");
                }
            }
            else if (SessionKeyTextField.Text != "")
            {
                int insertedSessionKeyByInt = -1;
                String result = "";
                try
                {
                    insertedSessionKeyByInt = Int32.Parse(SessionKeyTextField.Text);
                }
                catch
                {
                    showAlert("invalid session key!, digits only");
                }
                try
                {
                    _choosenForum = forum_dropList.SelectedItem.Text;
                }
                catch
                {
                    showAlert("choose a forum");
                    return;
                }
                if (_choosenForum != null)
                {
                    ForumData toSend = _fMC.getForum(_choosenForum);
                    if (CheckBox_Guest.Checked)
                    {
                        showAlert("please clear the session key field");
                        return;
                    }
                    else if ((result = _fMC.loginBySessionKey(insertedSessionKeyByInt, Session["UserName"].ToString(), _choosenForum)).Contains(","))
                    {
                        Session["forumName"] = _choosenForum;
                        Session["userName"] = Session["UserName"];
                        Session["ForumManagerClient"] = _fMC;
                        Session["sessionKey"] = result;
                        Response.Redirect("ForumWindow.aspx");
                    }
                    else
                    {
                        showAlert(result);
                        return;
                    }

                }
                else
                {
                    showAlert("choose a forum");
                    return;
                }

            }
            else if (CheckBox_Guest.Checked)
            {
                Session["Password"] = Password.Text;
                try
                {
                    _choosenForum = forum_dropList.SelectedItem.Text;
                }
                catch
                {
                    showAlert("choose a forum");
                    return;
                }
                Session["forumName"] = _choosenForum;
                Session["userName"] = "Guest";
                Session["ForumManagerClient"] = _fMC;
                Response.Redirect("ForumWindow.aspx");
            }
            else
            {
                showAlert("please fill the required fields");
                return;
            }
        }

        protected void Btn_signUp_Click(object sender, EventArgs e)
        {
            showAlert("sign up");
        }

        private void showAlert(String content)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "popup", "<script>alert(\"" + content + "\");</script>");
        }
        
        public void applyPostPublishedInForumNotification(String forumName, String subForumName, String publisherName)
        {
            showAlert("new post<br>" + publisherName + " published a post in " + forumName +
                    "'s sub-forum " + subForumName);
        }

        public void applyPostModificationNotification(String forumName, String publisherName, String title)
        {//a stab for web client notification. there was no requirement for this feature so we muted it for the time being
        }

        public void applyPostDelitionNotification(String forumName, String publisherName, bool toSendMessage)
        {//a stab for web client notification. there was no requirement for this feature so we muted it for the time being
        }

        public void sendUserMessage(String senderName, String content)
        {//a stab for web client notification. there was no requirement for this feature so we muted it for the time being
        }

        protected void forum_dropList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_choosenForum = forum_dropList.SelectedItem.ToString();
        }

        protected void forum_dropList_Load(object sender, EventArgs e)
        {
            forum_dropList.ClearSelection();
            forum_dropList.Items.Clear();
            while (_forumsList == null) { Thread.Sleep(20); }
            foreach (String forumName in this._forumsList)
                forum_dropList.Items.Add(forumName);
        }

        protected void LogInWithDiffUserButton_Click(object sender, EventArgs e)
        {
            sessionKeyLabel.Visible = false;
            SessionKeyTextField.Visible = false;
            Password.Visible = true;
            passwordLabel.Visible = true;
            ID.Visible = true;
            userNameLabel.Visible = true;
            LogInWithDiffUserButton.Visible = false;
            Session["sessionKey"] = "";
            SessionKeyTextField.Text = "";
        }
    }
}