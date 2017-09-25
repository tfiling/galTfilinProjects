using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ForumBuilder.Common.DataContracts;
using ForumBuilder.Common.ClientServiceContracts;
using PL.proxies;
using System.ServiceModel;
using PL.notificationHost;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace PL
{

    public class dataContainer
    {
        private int _id;
        private string _title;
        private string _writer;
        private string _time;

        public int Id
        {
            get { return _id;}
            set{ _id = value;}
        }

        public string Title
        {
            get{ return _title;}
            set{ _title = value;}
        }

        public string Writer
        {
            get{ return _writer;}
            set{ _writer = value;}
        }

        public string Time
        {
            get{ return _time;}
            set{ _time = value;}
        }

        public dataContainer()
        {

        }

        public dataContainer(int id, string title, string writer, string time)
        {
            Id = id;
            Title = title;
            Writer = writer;
            Time = time;
        }

    }

    public partial class SubForumWindow : Window, IUserNotificationsService
    {
        private PostManagerClient _pm;
        private ForumManagerClient _fm;
        private SubForumManagerClient _sfm;
        private SuperUserManagerClient _sUMC;
        private string _userName;
        private int _patentId;//used for adding post;
        private List<dataContainer> dataOfEachPost;
        private String _sessionKey;
        private string _forumName;
        private string _subName;
        private ClientNotificationHost _cnh;
        public static int _generalAddedPostThreadFlag = 0;
        private int _myAddedPostThreadFlag;
        public static int _generalDeletedPostThreadFlag = 0;
       // public static bool _isPost = false
        private int _myDeletedPostThreadFlag;
        private dataContainer _selected;
        public static int x = 0;

        public SubForumWindow(string fName, string sfName, string userName, String skey, ClientNotificationHost cnh)//forum subforum names and userName
        {
            InitializeComponent();
            _cnh = cnh;
            _myAddedPostThreadFlag = _generalAddedPostThreadFlag;
            _myDeletedPostThreadFlag = _generalDeletedPostThreadFlag;
            _fm = new ForumManagerClient(new InstanceContext(_cnh));
            _pm = new PostManagerClient();
            _sfm = new SubForumManagerClient();
            _sUMC = new SuperUserManagerClient();
            forumName.Content = "ForumName: " + fName;
            sForumName.Content = "Sub-ForumName: " + sfName;
            _userName = userName;
            UsrMenu.Header = "UserName: " + userName;
            sessionMenu.Header = "Session key: " + skey.Substring(0, skey.IndexOf(","));
            _sessionKey = skey;
            _forumName = fName;
            _subName = sfName;
            _patentId = -1;
            dataOfEachPost = new List<dataContainer>();
            InitializePermissons(userName);
            makeRefresh();
        }

        private void makeRefresh()
        {
            _cnh.updateWindow(this);
            Window window = this;
            _selected = null;
            if (x == 0)
            {
                BackgroundWorker wrk = new BackgroundWorker();
                wrk.WorkerReportsProgress = true;
                wrk.DoWork += (a, b) =>
                {
                    while (_generalAddedPostThreadFlag <= _myAddedPostThreadFlag && _generalDeletedPostThreadFlag <= _myDeletedPostThreadFlag)
                    {

                    }
                };
                wrk.RunWorkerCompleted += (s, e) =>
                {
                    if (_generalAddedPostThreadFlag > _myAddedPostThreadFlag)
                    {
                        _myAddedPostThreadFlag++;
                    }
                    else
                    {
                        _myDeletedPostThreadFlag++;
                    }
                    if (listBox.Visibility == Visibility.Collapsed)
                    {
                        DataGrid_Loaded(this, null);
                    }
                    else
                    {
                        listBox.Items.Clear();
                        selectionChangedHelp(_selected);
                    }
                    wrk.RunWorkerAsync();

                };
                wrk.RunWorkerAsync();
            }
            x++;
        }

        internal void showAgain()
        {
            listBox.Items.Clear();
            selectionChangedHelp(_selected);
            this.Visibility = Visibility.Visible;
        }

        private void InitializePermissons(string userName)
        {
            ForumData fd = _fm.getForum(_forumName);
            // a guest
            if (userName.Equals("Guest"))
            {
                addPostButton.IsEnabled = false;
                addModeratorButton.IsEnabled = false;
                privateMessages.IsEnabled = false;
                dismissModerator.IsEnabled = false;
                editMassege.IsEnabled = false;
                deleteMessageButton.IsEnabled = false;
            }
            // a member but not an admin
            else if (!_fm.isAdmin(userName, fd.forumName) && !_sUMC.isSuperUser(userName))
            {
                addModeratorButton.IsEnabled = false;
                dismissModerator.IsEnabled = false;
            }
            // an admin or super user
            else
            {
                // all open
            }
        }
       
        private void ThreadView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            var selected = grid.SelectedItem as dataContainer;
            _selected = selected;
            selectionChangedHelp(selected);


        }

        private void selectionChangedHelp(dataContainer selected)
        {
            if (selected == null)
            {
                return;
            }
            List<PostData> posts = _pm.getAllPosts(_forumName, _subName);
            foreach (PostData pd1 in posts)
            {
                if (pd1.id == selected.Id)//needs to show the thread of this post
                {
                    List<int> commentsIds = new List<int>();
                    commentsIds.Add(selected.Id);
                    foreach (PostData tempPostData in posts)
                    {
                        if (tempPostData.parentId == pd1.id)
                        {
                            commentsIds.Add(tempPostData.id);
                        }
                    }
                    //going over all comments to make a new table
                    _patentId = pd1.id;
                    foreach (int singleCommentId in commentsIds)
                    {
                        foreach (PostData pd2 in posts)
                        {
                            if (pd2.id == singleCommentId)
                            {
                                ListBox innerListBox = new ListBox();
                                Expander exp = new Expander();
                                exp.Header = pd2.title + "                                     " + pd2.timePublished + "\n pulished by:" + pd2.writerUserName;
                                exp.Content = pd2.content;
                                CheckBox cb = new CheckBox();
                                innerListBox.Items.Add(exp);
                                innerListBox.Items.Add(cb);
                                listBox.Items.Add(innerListBox);
                                dataContainer dt = new dataContainer(pd2.id, pd2.title, pd2.writerUserName, pd2.timePublished.ToString());
                                dataOfEachPost.Add(dt);
                            }
                        }
                    }
                }
            }
            if (!listBox.Items.IsEmpty)
            {
                threadView.Visibility = Visibility.Collapsed;
                threadTextBox.Text = "   Posts";
                addPostButton.Header = "add post";
                addPostButton.Visibility = Visibility.Visible;
                listBox.Visibility = Visibility.Visible;
            }
            else
            {
                DataGrid_Loaded(this, null);
            }
        }

        private void setNotifications(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            List<PostData> posts = _pm.getAllPosts(_forumName, _subName);
            var table = new List<dataContainer>();
            foreach (PostData pd in posts)
            {
                if (pd.parentId == -1)//if its the first message in thread
                {
                    dataContainer dt = new dataContainer();
                    dt.Id = pd.id;
                    dt.Title = pd.title;
                    dt.Writer = pd.writerUserName;
                    dt.Time = pd.timePublished.ToString();
                    table.Add(dt);
                }
            }
            threadView.ItemsSource = table;
            addPostButton.Header = "Add Thread";
            listBox.Visibility = Visibility.Collapsed;
            threadView.Visibility = Visibility.Visible;
        }

        private void singleThread_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.Visibility == Visibility.Visible)
            {
                SubForumWindow newWin = new SubForumWindow(_forumName, _subName, _userName, _sessionKey, _cnh);
                newWin.Show();
                this.Close();
            }
            else//needs to go back to previous page
            {
                ForumWindow newWin = new ForumWindow(_fm.getForum(_forumName), _userName, _cnh,_sessionKey);
                newWin.Show();
                this.Close();
            }
        }

        private void deleteMessageButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            int tempIndex = 0;
            bool found = false;
            foreach (ListBox item in listBox.Items)
            {
                if (!found)
                {
                    if (item != null && item.Items[0] != null && item.Items[1] != null)
                    {
                        CheckBox cb = (CheckBox)(item.Items[1]);
                        if (cb.IsChecked.Value)
                        {
                            found = true;
                            index = tempIndex;
                        }
                    }
                    tempIndex++;
                }
            }
            if (index == -1)
            {
                MessageBox.Show("no box is checked");
                return;
            }
            dataContainer selected = dataOfEachPost[index];
            List<PostData> posts = _pm.getAllPosts(_forumName, _subName);
            PostData postToDelete = null;
            foreach (PostData pd in posts)
            {
                if (pd.id == selected.Id)
                {
                    postToDelete = pd;
                }
            }
            if (postToDelete != null)
            {
                string ans;
                if (postToDelete.parentId == -1)
                {
                    ans = _sfm.deleteThread(postToDelete.id, _userName);
                }
                else
                {
                    ans = _pm.deletePost(postToDelete.id, _userName);
                }
                if (ans != "Delete post failed, there is no permission to that user")
                {
                    listBox.Items.RemoveAt(index);
                    if (index == 0)
                    {
                        SubForumWindow newWin = new SubForumWindow(_forumName, _subName, _userName, _sessionKey, _cnh);
                        newWin.Show();
                        this.Close();
                    }
                }
                else { MessageBox.Show(ans); }
            }
            else
            {
                MessageBox.Show("error: couldn't find message");
            }
        }

        private void addPostButton_Click(object sender, RoutedEventArgs e)
        {
            addPostAndThreadWindow win = new addPostAndThreadWindow(this, _patentId, _userName, _forumName, _subName, _cnh);
            win.Show();
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            switch (menuItem.Name)
            {
                case "addPostButton": { addPostButton_Click(sender, e); } break;
                case "addModeratorButton": { addModeratorButton_Click(sender, e); } break;
                case "deleteMessageButton": { deleteMessageButton_Click(sender, e); } break;
                case "backButton": { back_Click(sender, e); } break;
                case "logOutButton": { logOut(sender, e); } break;
                case "privateMessages": { privateMessages_Click(sender, e); } break;
                case "editMassege": { editMessage_Click(sender, e); } break;
                case "dismissModerator": { dismissModerator_Click(sender, e); } break;

            }
        }

        private void addModeratorButton_Click(object sender, RoutedEventArgs e)
        {
            AddModerator newWin = new AddModerator(_subName, _userName, _forumName, _sessionKey);
            newWin.Show();
            this.Close();
        }

        private void dismissModerator_Click(object sender, RoutedEventArgs e)
        {
            DismissModerator newWin = new DismissModerator(this, _userName, _forumName, _subName);
            newWin.Show();
            this.Visibility = Visibility.Collapsed;
        }

        private void editMessage_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            int tempIndex = 0;
            bool found = false;
            foreach (ListBox item in listBox.Items)
            {
                if (!found)
                {
                    if (item != null && item.Items[0] != null && item.Items[1] != null)
                    {
                        CheckBox cb = (CheckBox)(item.Items[1]);
                        if (cb.IsChecked.Value)
                        {
                            found = true;
                            index = tempIndex;
                        }
                    }
                    tempIndex++;
                }
            }
            if (index == -1)
            {
                MessageBox.Show("no box is checked");
                return;
            }
            dataContainer selected = dataOfEachPost[index];
            List<PostData> posts = _pm.getAllPosts(_forumName, _subName);
            PostData postToEdit = null;
            foreach (PostData pd in posts)
            {
                if (pd.id == selected.Id)
                {
                    postToEdit = pd;
                }
            }
            if (postToEdit != null)//if the wanted post exists
            {
                addPostAndThreadWindow newWin = new addPostAndThreadWindow(postToEdit, _userName, _forumName, _subName, this);
                newWin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("error: couldn't find message");
            }
        }

        private void privateMessages_Click(object sender, RoutedEventArgs e)

        {
             privateMessagesWindow newWin = new privateMessagesWindow(_forumName, _userName, this, _sessionKey);
              this.Visibility = Visibility.Collapsed;
              newWin.Show();
        }

        private void logOut(object sender, RoutedEventArgs e)
        {
            _fm.logout(_userName, _forumName,_sessionKey);
            MainWindow newWin = new MainWindow();
            newWin.Show();
            this.Close();
        }

        public String Sessionkey
        {
            get { return _sessionKey; }
        }

        public void applyPostPublishedInForumNotification(String forumName, String subForumName, String publisherName)
        {
            MessageBox.Show(publisherName + " published a post in " + forumName +
                "'s sub-forum " + subForumName, "new post");
        }

        public void applyPostModificationNotification(String forumName, String publisherName, String title)
        {
            MessageBox.Show(publisherName + "'s post you were following in " + forumName + " was modified (" + title + ")", "post modified");
        }

        public void applyPostDelitionNotification(String forumName, String publisherName, bool toSendMessage)
        {
            MessageBox.Show(publisherName + "'s post you were following in " + forumName + " was deleted", "post deleted");
        }

        public void sendUserMessage(String senderName, String content)
        {
            MessageBox.Show(content, senderName + " set you a message");
        }

    }
}
