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
using PL.proxies;
using ForumBuilder.Common.DataContracts;
using PL.notificationHost;

namespace PL
{
    /// <summary>
    /// Interaction logic for addPostAndThreadWindow.xaml
    /// </summary>
    public partial class addPostAndThreadWindow : Window
    {
        private SubForumWindow _prevWindow;
        private int _parentID;
        private PostManagerClient _pm;
        private SubForumManagerClient _sf;
        private string _userName;
        private string _forumName;
        private string _subForumName;
        private PostData _postToEdit;
        private ClientNotificationHost _cnh;

        public addPostAndThreadWindow(SubForumWindow prevWindow, int parrentId, string userName, string forumName, string subForumName, ClientNotificationHost cnh)//-1 for new thread
        {
            InitializeComponent();
            _cnh = cnh;
            _forumName = forumName;
            _sf = new SubForumManagerClient();
            _subForumName = subForumName;
            _prevWindow = prevWindow;
            _parentID = parrentId;
            usr.Content = "UserName: " + userName;
            key.Content = "Session key: " + prevWindow.Sessionkey.Substring(0, prevWindow.Sessionkey.IndexOf(","));
            if (parrentId == -1)
            {
                whatToAdd.Content = "Add new thread";
            }
            else
            {
                whatToAdd.Content = "Add new post";
            }
            _pm = new PostManagerClient();
            _userName = userName;
            _postToEdit = null;
        }
        public addPostAndThreadWindow(PostData postToEdit, string userName, string forumName, string subForumName, SubForumWindow prevWindow)
        {
            InitializeComponent();
            _prevWindow = prevWindow;
            _postToEdit = postToEdit;
            _userName = userName;
            _sf = new SubForumManagerClient();
            _pm = new PostManagerClient();
            _forumName = forumName;
            _subForumName = subForumName;

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (_postToEdit == null)
            {
                if (_parentID != -1)
                {
                    String addPost = _pm.addPost(title.Text, content.Text, _userName, _parentID);
                    if (addPost.Equals("comment created"))
                    {
                        MessageBox.Show("post was added succesfully");
                        _prevWindow.Close();
                        SubForumWindow newWin = new SubForumWindow(_forumName, _subForumName, _userName, _prevWindow.Sessionkey, _cnh);
                        
                        this.Close();
                        newWin.Show();
                    }
                    else
                    {
                        MessageBox.Show(addPost);
                    }
                }
                else
                {
                    String createTread = _sf.createThread(title.Text, content.Text, _userName, _forumName, _subForumName);
                    if (createTread.Equals("Create tread succeed"))
                    {
                        MessageBox.Show("thread was added succesfully");
                        _prevWindow.Close();
                        SubForumWindow newWin = new SubForumWindow(_forumName, _subForumName, _userName, _prevWindow.Sessionkey, _cnh);
                        
                        this.Close();
                        newWin.Show();
                    }
                    else
                    {
                        MessageBox.Show(createTread);
                    }
                }
            }
            else
            {
                _pm.updatePost(_postToEdit.id, title.Text, content.Text, _userName);
                /*   SubForumWindow newWin = new SubForumWindow(_forumName, _subForumName, _userName, _prevWindow.Sessionkey, _cnh);
                   _prevWindow.Close();
                   newWin.Show();*/
                _prevWindow.showAgain();
                this.Close();
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            SubForumWindow newWin = new SubForumWindow(_forumName, _subForumName, _userName, _prevWindow.Sessionkey, _cnh);
            newWin.Show();
            this.Close();
        }
    }
}
