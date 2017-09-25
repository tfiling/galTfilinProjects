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
using PL.proxies;

namespace PL
{
    /// <summary>
    /// Interaction logic for sendMessageWindow.xaml
    /// </summary>
    public partial class sendMessageWindow : Window
    {
        string _userName;
        Window _prevWindow;
        private UserManagerClient _um;
        private String forumName;

        public sendMessageWindow(String forum, string userName, Window prevWindow)
        {
            forumName = forum;
            InitializeComponent();
            _userName = userName;
            _prevWindow = prevWindow;
            _um = new UserManagerClient();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _prevWindow.Visibility = System.Windows.Visibility.Visible;
            this.Close();
        }

        private void sendMessage_Click(object sender, RoutedEventArgs e)
        {
            string targetUser = to.Text;
            string msgContent = content.Text;
            if (_um.sendPrivateMessage(forumName, _userName, targetUser, msgContent).Equals("message was sent successfully"))
            {
                MessageBox.Show("message was sent successfully");
                _prevWindow.Visibility = System.Windows.Visibility.Visible;
                this.Close();
            }
            else
            {
                MessageBox.Show("couln't send message");
            }

        }

    }
}
