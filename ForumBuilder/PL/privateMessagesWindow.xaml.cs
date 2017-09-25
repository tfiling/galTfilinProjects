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
    /// Interaction logic for privateMessagesWindow.xaml
    /// </summary>
    public partial class privateMessagesWindow : Window
    {
        private string _userName;
        private UserManagerClient _um;
        private Window _prevWindow;
        private String forumName;

        public privateMessagesWindow(String forum, string userName, Window prevWindow, String key)
        {
            InitializeComponent();
            _userName = userName;
            forumName = forum;
            _um = new UserManagerClient();
            _prevWindow = prevWindow;
            usr.Content = "UserName:  " + userName;
            session.Content = "Session key:  " + key.Substring(0,key.IndexOf(","));
            List<string[]> privateMessages = _um.getAllPrivateMessages(_userName);
            foreach (string[] msg in privateMessages)
            {
                Expander exp = new Expander();
                exp.Header = msg[0];//the sender's name;
                exp.Content = msg[1];//message's content
                listBox.Items.Add(exp);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _prevWindow.Visibility = System.Windows.Visibility.Visible;
            this.Close();
        }

        private void sengMessageButton_Click(object sender, RoutedEventArgs e)
        {
            sendMessageWindow newWin = new sendMessageWindow(forumName, _userName, this);
            this.Visibility = Visibility.Collapsed;
            newWin.Show();
        }
    }
}
