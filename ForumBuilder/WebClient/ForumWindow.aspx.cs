using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using ForumBuilder.Common.DataContracts;
using WebClient.proxies;
using System.ServiceModel;
using ForumBuilder.Common.ClientServiceContracts;


namespace WebClient
{
    public partial class ForumWindow : System.Web.UI.Page, IUserNotificationsService
    {
        public static int ADD_SUB_FORUM_INDEX = 0;
        private static int PRIVATE_MESSAGES_INDEX = 1;
        private static int SET_PREFERENCES_INDEX = 2;
        private static int SIGNUP_INDEX = 3;
        private static int LOGOUT_INDEX = 4;

        private ForumData _myforum;
        private String _subForumChosen;
        private ForumManagerClient _fMC;
        private string _userName;
        private SuperUserManagerClient _sUMC;
        private string _mySessionKey;

        public Boolean newPostNotification = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            _userName = (String)Session["userName"];
            try
            {
                _mySessionKey = Session["sessionKey"].ToString();
            }
            catch
            {

            }

            _fMC = (ForumManagerClient)Session["ForumManagerClient"];
            _fMC.InnerDuplexChannel.CallbackInstance = new InstanceContext(this);
            String _chosenForum = (String)Session["forumName"];
            this._myforum = _fMC.getForum(_chosenForum);
            lbl_forumName.Text = "ForumName:  " + _chosenForum;
            _sUMC = new SuperUserManagerClient();
            InitializePermissons(_userName);
            foreach(MenuItem item in menu.Items)
            {
                if (item.Text.Equals("Logout"))
                    item.Enabled = true;
            }
            
            
            foreach (string subForum in _myforum.subForums)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                Button btn = new Button();
                btn.Text = subForum;
                btn.Click += new EventHandler(clickOnSubForum);
                cell.Controls.Add(btn);
                row.Cells.Add(cell);
                tbl_subForumList.Rows.Add(row);
            }
        }
        
        private void InitializePermissons(string userName)
        {
            if (userName == null)
                return;
            // a guest
            if (userName.Equals("Guest"))
            {
                menu.Items[ADD_SUB_FORUM_INDEX].Enabled = false;
                menu.Items[SET_PREFERENCES_INDEX].Enabled = false;
            }
            // a member but not an admin
            else if (!_fMC.isAdmin(userName, _myforum.forumName) && !_sUMC.isSuperUser(userName))
            {
                menu.Items[ADD_SUB_FORUM_INDEX].Enabled = false;
                menu.Items[SET_PREFERENCES_INDEX].Enabled = false;
                menu.Items[SIGNUP_INDEX].Enabled = false;
            }
            // an admin
            else if (!_sUMC.isSuperUser(userName))
            {
                menu.Items[SIGNUP_INDEX].Enabled = false;
            }
            //  a super user
            else
            {
                // all open 
            }
        }

        protected void NavigationMenu_MenuItemClick(Object sender, MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "AddSub": 
                {
                    //Response.Redirect("AddNewSubForum.aspx");
                } break;
                case "Set": 
                { 
                    //Response.Redirect("SetPreferences.aspx");
                } break;
                case "SignUP": 
                { 
                    //SignUP(); 
                } break;
                case "menuLogout": 
                { 
                    logout(_userName);
                    Session["userName"] = "";
                    Session["password"] = "";
                    Response.Redirect("MainWindow.aspx");
                } break;
                case "privateMessages": 
                {
                //a stab for web client notification. there was no requirement for this feature 
                    //so we muted it for the time being                
                } break;
            }
        }



        private void logout(String nameLogout)
        {
            // a guest
            if (nameLogout.Equals("Guest"))
            {
                Session["sessionKey"] = "";
                Response.Redirect("MainWindow.aspx");
            }
            // an fourom member
            else
            {
                _fMC.logout(nameLogout, _myforum.forumName, _mySessionKey);
                Session["sessionKey"] = "";
                Response.Redirect("MainWindow.aspx");
            }
        }



        private void SignUP()
        {
            Session["ForumController"] = _fMC;
            Session["forumName"] = _myforum.forumName;
            Response.Redirect("SignUpWindow.aspx");
        }

        protected void clickOnSubForum(Object sender, EventArgs e)
        {  
            Session["subForumName"] = ((Button)sender).Text;
            Response.Redirect("subForumWebPage.aspx");
        }

        public void applyPostPublishedInForumNotification(String forumName, String subForumName, String publisherName)
        {//a stab for web client notification. there was no requirement for this feature so we muted it for the time being
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

        private void showAlert(String content)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "popup", "<script>alert(\"" + content + "\");</script>");
        }

    }
}


/*
    this is a way to create a menu dont delete

   <div id = "nav" >
       < ul >
        < li >< a href="MainWindow.aspx">Main Window</a></li>
        <li><a href = "#" > item2 </ a ></ li >
   
           < li >< a href= "#" > item3 </ a >
   
               < ul >
   
                   < li >< a href= "#" > item3.1</a></li>
                <li><a href = "#" ></ a > item3.2</li>
            </ul>
        </li>
           <li><a>item4</a></li>
       
       </ul>
   </div>
   */