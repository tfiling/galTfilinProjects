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

namespace PL
{
    /// <summary>
    /// Interaction logic for DismissModerator.xaml
    /// </summary>
    public partial class DismissModerator : Window
    {
        private SubForumManagerClient _sf;
        private SubForumWindow _oldWin;
        private string _userName;
        private string _forumName;
        private string _subForumName;

        public DismissModerator(SubForumWindow oldWin, string userName, string forumName, string subForumName)
        {
            InitializeComponent();
            _sf = new SubForumManagerClient();
            _oldWin = oldWin;
            _userName = userName;
            _forumName = forumName;
            _subForumName = subForumName;
            user.Content = "UserName:  " + userName;
            key.Content = "Session key:  " + oldWin.Sessionkey.Substring(0, oldWin.Sessionkey.IndexOf(","));
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _oldWin.Show();
            this.Close();
        }

        private void dismissButton_Click(object sender, RoutedEventArgs e)
        {
            string dismiss = _sf.dismissModerator(textBox.Text, _userName, _subForumName, _forumName);
                if (dismiss.Equals("Moderator dismissed"))
                {
                    MessageBox.Show("moderator was dismissed");
                    _oldWin.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(dismiss);
                }
        }
    }
}
