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
using System.ServiceModel;
using PL.notificationHost;
using PL.proxies;
using ForumBuilder.Common.DataContracts;

namespace PL
{
    /// <summary>
    /// Interaction logic for SessionKeyWindow.xaml
    /// </summary>
    public partial class SessionKeyWindow : Window
    {
        private string _userName;
        private string _forumName;
        private ForumManagerClient _fMC;

        public SessionKeyWindow(string userName, string forumName)
        {
            InitializeComponent();
            _userName = userName;
            _forumName = forumName;
            _fMC = new ForumManagerClient(new InstanceContext(new ClientNotificationHost()));
            nameLabel.Content = "The user " + userName + " is already logged in.";
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            String result = "";
            string seesionKey = userKey.Text;
            try
            {
                result = _fMC.loginBySessionKey(Int32.Parse(seesionKey), _userName, _forumName);
            }
            catch
            {
                MessageBox.Show("invalid session key!, digits only");
            }
            if (result.Contains(","))
            {
                ForumData toSend = _fMC.getForum(_forumName);
                ForumWindow fw = new ForumWindow(toSend, _userName, new ClientNotificationHost(), result);
                this.Close();
                fw.Show();
            }
            else
            {
                MessageBox.Show(result);
            }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

    }
}
