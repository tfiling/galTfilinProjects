using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ForumBuilder.Common.DataContracts;
using System.ServiceModel;
using PL.notificationHost;
using PL.proxies;

namespace PL
{
    /// <summary>
    /// Interaction logic for ForumWindow.xaml
    /// </summary>
    /// 

    public partial class ForumWindow : Window
    {

        private ForumData _myforum;
        private String _subForumChosen;
        private ForumManagerClient _fMC;
        private string _userName;
        private SuperUserManagerClient _sUMC;
        private String _sessionKey;
        private ClientNotificationHost _cnh;

        public ForumWindow(ForumData forum, string userName ,ClientNotificationHost cnh, String sessionKey)
        {
            InitializeComponent();
            _cnh = cnh;
            _myforum = forum;
            _fMC = new ForumManagerClient(new InstanceContext(_cnh));
            _userName = userName;
            if (_userName != "Guest")
            {
                _sessionKey = sessionKey;
                session.Content = "Session key:  " + _sessionKey.Substring(0,_sessionKey.IndexOf(","));
                sessionMenu.Header = "Session key: " + _sessionKey.Substring(0, _sessionKey.IndexOf(","));
            }
            if (_userName == "Guest")
            {
                sessionMenu.Header = "Session key: " + 0;
            }
            ForumName.Content = "ForumName:  " + _myforum.forumName;
            UsrName.Content = "UserName:  " + userName;
            UsrMenu.Header = "UserName: " + userName;
            _sUMC = new SuperUserManagerClient();
            InitializePermissons(userName);
            //initializing the subForumListBox
            if (_myforum != null)
            {
                foreach (string subForum in _myforum.subForums)
                {
                    subForumsListBox.Items.Add(subForum);
                }
            } 
        }

        private void InitializePermissons(string userName)
        {
            // a guest
            if (userName.Equals("Guest"))
            {
                AddSub.IsEnabled = false;
                Set.IsEnabled = false;
                privateMessages.IsEnabled = false;
                addFriend.IsEnabled = false;
                View.IsEnabled = false;
            }
            // a member but not an admin
            else if (!_fMC.isAdmin(userName, _myforum.forumName) && !_sUMC.isSuperUser(userName))
            {
                AddSub.IsEnabled = false;
                Set.IsEnabled = false;
                Sign.IsEnabled = false;
                View.IsEnabled = false;
            }
            // an admin
            else if (!_sUMC.isSuperUser(userName))
            {
                Sign.IsEnabled = false;
            }
            //  a super user
            else
            {
                // all open 
            }
        }

        private void MenuItem_Forums(object sender, RoutedEventArgs e)
        {
            usersComboBox.Visibility = Visibility.Collapsed;
            MenuItem menuItem = e.Source as MenuItem;
            switch (menuItem.Name)
            {
                case "AddSub": { addNewSubForum(); } break;
                case "Set": { setPreferences(); } break;
                case "Sign": { SignUP(); } break;
                case "menuLogout": { logout(_userName); } break;
                case "privateMessages": { privateMessages_Click(sender, e); } break;
            }
        }

        private void MenuItem_View(object sender, RoutedEventArgs e)
        {
            usersComboBox.Visibility = Visibility.Collapsed;
            MenuItem menuItem = e.Source as MenuItem;
            switch (menuItem.Name)
            {
                case "viewReports": { showReport(); } break;
                case "viewUserPosts": { userPostsView(); } break;
            }
        }

        private void userPostsView()
        {
            reportListBox.Items.Clear();
            mainGrid.Visibility = System.Windows.Visibility.Collapsed;
            MyDialog.Visibility = System.Windows.Visibility.Collapsed;
            setPreferencesWin.Visibility = System.Windows.Visibility.Collapsed;
            AddSubForum.Visibility = System.Windows.Visibility.Collapsed;
            viewGrid.Visibility = System.Windows.Visibility.Visible;
            usersComboBox.Visibility = Visibility.Visible;

        }

        private void showReport()
        {
            reportListBox.Items.Clear();
            mainGrid.Visibility = System.Windows.Visibility.Collapsed;
            MyDialog.Visibility = System.Windows.Visibility.Collapsed;
            setPreferencesWin.Visibility = System.Windows.Visibility.Collapsed;
            AddSubForum.Visibility = System.Windows.Visibility.Collapsed;
            viewGrid.Visibility = System.Windows.Visibility.Visible;
            List<String> moderaorsReports = _fMC.getAdminReport(_userName, _myforum.forumName);
            numOfPosts.Text = "Number of Posts :  " + (_fMC.getAdminReportNumOfPOst(_userName, _myforum.forumName)).ToString();
            //reportListBox.Items.Clear();
      
            foreach (string report in moderaorsReports)
            {
                //Console.WriteLine(report);
                int lengthOfAddedPosts = 12;
                int indexOfAddedPosts = report.IndexOf("added posts:");
                string beforeposts = report.Substring(0, indexOfAddedPosts+ lengthOfAddedPosts);
                string remainingReport = report.Substring(indexOfAddedPosts + lengthOfAddedPosts);
                int indexOfPostTitle = remainingReport.IndexOf("post title:");
                int indexOfPostContent = remainingReport.IndexOf("post content:");
                reportListBox.Items.Add(beforeposts);
                while (indexOfPostTitle >= 0)
                {
                    Expander exp = new Expander();
                    exp.Header = remainingReport.Substring(indexOfPostTitle, indexOfPostContent);
                    remainingReport = remainingReport.Substring(indexOfPostContent);
                    indexOfPostTitle = remainingReport.IndexOf("post title:");
                    indexOfPostContent = remainingReport.IndexOf("post content:");
                    if (indexOfPostTitle == -1)
                        indexOfPostTitle = remainingReport.Length; 
                    exp.Content = remainingReport.Substring(indexOfPostContent, indexOfPostTitle);
                    remainingReport = remainingReport.Substring(indexOfPostTitle);
                    indexOfPostTitle = remainingReport.IndexOf("post title:");
                    indexOfPostContent = remainingReport.IndexOf("post content:");
                    reportListBox.Items.Add(exp);
                }
            }
        }

        private void logout(String nameLogout)
        {
            MainWindow mw = new MainWindow();
            // a guest
            if (nameLogout.Equals("Guest"))
            {
                mw.Show();
                this.Close();
            }
            // an fourom member
            else
            {
                _fMC.logout(nameLogout, _myforum.forumName,_sessionKey);
                mw.Show();
                this.Close();
            }
        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get SelectedItems from DataGrid.
            var grid = sender as DataGrid;
            var selected = grid.SelectedItems;
            _subForumChosen = selected.ToString();
            SubForumWindow sfw = new SubForumWindow(_myforum.forumName, _subForumChosen, _userName, _sessionKey, _cnh);
            sfw.ShowDialog();
        }

        private void addNewSubForum()
        {
            mainGrid.Visibility = System.Windows.Visibility.Collapsed;
            MyDialog.Visibility = System.Windows.Visibility.Collapsed;
            viewGrid.Visibility = System.Windows.Visibility.Collapsed;
            setPreferencesWin.Visibility = System.Windows.Visibility.Collapsed;
            ComboBoxItem newFirstItem = new ComboBoxItem();
            newFirstItem.Content = "UnLimited";
            comboBoxDuration.Items.Add(newFirstItem);
            for (int i = 1; i < 31; i++)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = i;
                comboBoxDuration.Items.Add(newItem);
            }
            AddSubForum.Visibility = System.Windows.Visibility.Visible;
            backButton.Visibility = System.Windows.Visibility.Visible;

        }

        private void setPreferences()
        {
            mainGrid.Visibility = System.Windows.Visibility.Collapsed;
            MyDialog.Visibility = System.Windows.Visibility.Collapsed;
            AddSubForum.Visibility = System.Windows.Visibility.Collapsed;
            viewGrid.Visibility = System.Windows.Visibility.Collapsed;
            setPreferencesWin.Visibility = System.Windows.Visibility.Visible;
            backButton.Visibility = System.Windows.Visibility.Visible;
            qIdentifying.IsChecked = _myforum.forumPolicy.isQuestionIdentifying;
            deleteMessages.IsChecked = _myforum.forumPolicy.deletePostByModerator;
            Capital.IsChecked = _myforum.forumPolicy.hasCapitalInPassword;
            Number.IsChecked = _myforum.forumPolicy.hasNumberInPassword;
            ForumDescToSet.Text = _myforum.description;
            ForumPolicyToSet.Text = _myforum.forumPolicy.policy;
            PassCombo.Items.Add(_myforum.forumPolicy.timeToPassExpiration);
            PassCombo.SelectedItem = _myforum.forumPolicy.timeToPassExpiration;
            TimeCombo.Items.Add(_myforum.forumPolicy.seniorityInForum);
            TimeCombo.SelectedItem = _myforum.forumPolicy.seniorityInForum;
            NumberCombo.Items.Add(_myforum.forumPolicy.minNumOfModerator);
            NumberCombo.SelectedItem = _myforum.forumPolicy.minNumOfModerator;
            LengthCombo.Items.Add(_myforum.forumPolicy.minLengthOfPassword);
            LengthCombo.SelectedItem = _myforum.forumPolicy.minLengthOfPassword;
        }

        private void SignUP()
        {
            SignUpWindow sU = new SignUpWindow(_fMC, _myforum.forumName);
            sU.Show();
            this.Close();
        }

        private void descChoose(object sender, RoutedEventArgs e)
        {
            bool toChange = descCheck.IsChecked.Value;
            if (toChange) { ForumDescToSet.IsEnabled = true; }
            else { ForumDescToSet.IsEnabled = false; }
        }

        private void policyChoose(object sender, RoutedEventArgs e)
        {
            bool toChange = policyCheck.IsChecked.Value;
            if (toChange) { ForumPolicyToSet.IsEnabled = true; }
            else { ForumPolicyToSet.IsEnabled = false; }
        }

        private void btn_SetForumPref(object sender, RoutedEventArgs e)
        {
            MyDialog.Visibility = System.Windows.Visibility.Visible;
            MyDialog.Focusable = true;
        }

        private void btn_toSetPref(object sender, RoutedEventArgs e)
        {
            String temp = "";
            var btn = sender as Button;
            if (btn.Name.Equals("yesBtn"))
            {
                temp = yesBtn.Content.ToString();
            }
            else { temp = noBtn.Content.ToString(); }
            setPref(temp);
        }

        private void setPref(String isDone)
        {
            MyDialog.Focusable = false;
            MyDialog.Visibility = System.Windows.Visibility.Collapsed;

            if (isDone.Equals("Yes"))
            {
                bool toChange = descCheck.IsChecked.Value;
                if (toChange)
                {
                    _myforum.description = ForumDescToSet.Text;
                }
                toChange = policyCheck.IsChecked.Value;
                if (toChange)
                {
                    _myforum.forumPolicy.policy = ForumPolicyToSet.Text;
                }
                toChange = (qIdentifying.IsChecked.Value != _myforum.forumPolicy.isQuestionIdentifying);
                if (toChange)
                {
                    _myforum.forumPolicy.isQuestionIdentifying = qIdentifying.IsChecked.Value;
                }
                toChange = (deleteMessages.IsChecked.Value != _myforum.forumPolicy.deletePostByModerator);
                if (toChange)
                {
                    _myforum.forumPolicy.deletePostByModerator = deleteMessages.IsChecked.Value;
                }
                if (!PassCombo.Text.Equals(""))
                {
                    int passExpirationTime = Int32.Parse(PassCombo.SelectedItem.ToString());
                    _myforum.forumPolicy.timeToPassExpiration = passExpirationTime;
                }
                if (!TimeCombo.Text.Equals(""))
                {
                    int seniorityChoosen = Int32.Parse(TimeCombo.SelectedItem.ToString());
                    _myforum.forumPolicy.seniorityInForum = seniorityChoosen;
                }
                if (!NumberCombo.Text.Equals(""))
                {
                    int numerOfModerators = Int32.Parse(NumberCombo.SelectedItem.ToString());
                    _myforum.forumPolicy.minNumOfModerator = numerOfModerators;
                }
                toChange = (Capital.IsChecked.Value != _myforum.forumPolicy.hasCapitalInPassword);
                if (toChange)
                {
                    _myforum.forumPolicy.hasCapitalInPassword = Capital.IsChecked.Value;
                }
                toChange = (Number.IsChecked.Value != _myforum.forumPolicy.hasNumberInPassword);
                if (toChange)
                {
                    _myforum.forumPolicy.hasNumberInPassword = Number.IsChecked.Value;
                }
                if (!LengthCombo.Text.Equals(""))
                {
                    int minLengthOfPass = Int32.Parse(LengthCombo.SelectedItem.ToString());
                    _myforum.forumPolicy.minLengthOfPassword = minLengthOfPass;
                }

                if (radBtnNotificationModeOnline.IsChecked.Value)
                    _myforum.forumPolicy.notificationsType = ForumPolicyData.ONLINE_NOTIFICATIONS_TPYE;
                else if (radBtnNotificationModeOffline.IsChecked.Value)
                    _myforum.forumPolicy.notificationsType = ForumPolicyData.OFFLINE_NOTIFICATIONS_TPYE;
                if (radBtnNotificationModeSelective.IsChecked.Value)
                { 
                    _myforum.forumPolicy.notificationsType = ForumPolicyData.SELECTIVE_NOTIFICATIONS_TPYE;
                    _myforum.forumPolicy.selectiveNotificationsUsers.Clear();
                    foreach (Object item in lstBox_SelectedUsersToBeNotified.SelectedItems)
                    {
                        _myforum.forumPolicy.selectiveNotificationsUsers.Add(item as String);
                    }
                }

                _fMC.setForumPreferences(_myforum.forumName, _myforum.description, _myforum.forumPolicy, _userName);
                MessageBox.Show("Preferences was successfully changed!");
                descCheck.IsChecked = false;
                policyCheck.IsChecked = false;
                qIdentifying.IsChecked = false;
                deleteMessages.IsChecked = false;
                Capital.IsChecked = false;
                Number.IsChecked = false;
                PassCombo.Items.Clear();
                TimeCombo.Items.Clear();
                NumberCombo.Items.Clear();
                LengthCombo.Items.Clear();
                setPreferencesWin.Visibility = System.Windows.Visibility.Collapsed;
                mainGrid.Visibility = System.Windows.Visibility.Visible;
            }
        }


        private void btn_createSub(object sender, RoutedEventArgs e)
        {
            int time = 0;
            int unlimited = 120;
            DateTime timeToSend = DateTime.Now;
            String sub_ForumName = subForumName.Text;
            string moderators = moderatorsTextBox.Text;
            List<string> moderatorList = moderators.Split(',').ToList();
            String timeDuration = comboBoxDuration.Text;
            if (timeDuration.Equals("") || moderators == null || sub_ForumName == null || sub_ForumName.Equals(""))
            {
                MessageBox.Show("error has accured");
                return;
            }
            if (!timeDuration.Equals("UnLimited"))
            {
                time = int.Parse(timeDuration);
                timeToSend = DateTime.Now.AddDays(time);
            }
            else
            {
                timeToSend = DateTime.Now.AddYears(unlimited);
            }
            Dictionary<String, DateTime> dic = new Dictionary<string, DateTime>();
            foreach (string userName in moderatorList)
            {
                dic.Add(userName, timeToSend);
            }
            string ansAdd = _fMC.addSubForum(_myforum.forumName, sub_ForumName, dic, _userName);
            Boolean isAdded = ansAdd.Equals("sub-forum added");
            if (isAdded == false)
            {
                MessageBox.Show(ansAdd);
            }
            else
            {
                MessageBox.Show("Sub-Forum " + sub_ForumName + " was successfully created and " + moderators + " are the Sub-Forum moderators.");
                ForumWindow newWin = new ForumWindow(_fMC.getForum(_myforum.forumName), _userName, _cnh, _sessionKey);
                this.Close();
                newWin.Show();
            }
        }

        private void subForumsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get SelectedItems from DataGrid.

            _subForumChosen = subForumsListBox.SelectedItem.ToString();
            SubForumWindow sfw = new SubForumWindow(_myforum.forumName, _subForumChosen, _userName, _sessionKey, _cnh);
            sfw.Show();
            this.Close();
        }


        private void privateMessages_Click(object sender, RoutedEventArgs e)

        {
            privateMessagesWindow newWin = new privateMessagesWindow(_myforum.forumName, _userName, this, _sessionKey);
            this.Visibility = Visibility.Collapsed;
            newWin.Show();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            ForumWindow newWin = new ForumWindow(_myforum, _userName, _cnh, _sessionKey);
            newWin.Show();
            this.Close();
        }

        private void PassComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            PassCombo.Items.Clear();
            int minDays = 30;
            int maxDays = 365;
            for (int i = minDays; i <= maxDays; i++)
            {
                PassCombo.Items.Add(i);
            }
        }

        private void TimeComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            TimeCombo.Items.Clear();
            int minDays = 0;
            int maxDays = 365;
            for (int i = minDays; i <= maxDays; i++)
            {
                TimeCombo.Items.Add(i);
            }
        }

        private void NumberComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            NumberCombo.Items.Clear();
            int minNumOfModerators = 1;
            int maxNumOfModerators = 10;
            for (int i = minNumOfModerators; i <= maxNumOfModerators; i++)
            {
                NumberCombo.Items.Add(i);
            }
        }

        private void LengthComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            LengthCombo.Items.Clear();
            int minPasswordLength = 5;
            int maxPasswordLength = 20;
            for (int i = minPasswordLength; i <= maxPasswordLength; i++)
            {
                LengthCombo.Items.Add(i);
            }
        }

        private void addFriend_Click(object sender, RoutedEventArgs e)
        {
            AddFriendWindow newWin = new AddFriendWindow(_userName, _myforum, _sessionKey);
            newWin.Show();
            this.Close();
        }

        private void usersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<PostData> posts = _fMC.getAdminReportPostOfmember(_userName, _myforum.forumName, usersComboBox.Text);
            foreach (PostData post in posts)
            {
                Expander exp = new Expander();
                exp.Header = post.title;
                exp.Content = post.content;
                reportListBox.Items.Add(exp);
            }
        }

        private void usersComboBox_DropDownOpened(object sender, EventArgs e)
        {
            usersComboBox.Items.Clear();
            for (int i = 0; i < _myforum.members.Count; i++)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = _myforum.members.ElementAt(i);
                usersComboBox.Items.Add(newItem);
            }
        }

        private void usersComboBox_DropDownClosed(object sender, EventArgs e)
        {
            reportListBox.Items.Clear();
            if (!(usersComboBox.Text).Equals(""))
            {
                List<PostData> posts = _fMC.getAdminReportPostOfmember(_userName, _myforum.forumName, usersComboBox.Text);
                foreach (PostData post in posts)
                {
                    Expander exp = new Expander();
                    exp.Header = post.title;
                    exp.Content = post.content;
                    reportListBox.Items.Add(exp);
                }
            }
        }

        private void selectiveNotificationsCheckedEventHandler(object sender, RoutedEventArgs e)
        {
            foreach (string userName in _myforum.members)
            {
                lstBox_SelectedUsersToBeNotified.Items.Add(userName);
            }
        }
    }
}
