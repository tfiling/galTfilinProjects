using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using PL.notificationHost;
using PL.proxies;
using ForumBuilder.Common.DataContracts;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> _forumsList;
        private String _choosenForum;
        private ForumManagerClient _fMC;
        private ClientNotificationHost _cnh;

        public MainWindow()
        {
            InitializeComponent();
            //Thread.Sleep(500);
            _cnh = new ClientNotificationHost();
            _fMC = new ForumManagerClient(new InstanceContext(_cnh));
            _forumsList = _fMC.getForums();
            this.Show();
        }

        private void LoginPressed(object sender, RoutedEventArgs e)
        {
            String sessionKey = "";
            string userName = ID.Text;
            string pass = Password.Password;
            //string sessionKeyField = sessionKeyTextBox.Text;
            if (_choosenForum != null)
            {
                ForumData toSend = _fMC.getForum(_choosenForum);
                if (guestCheck.IsChecked.Value)
                {
                    ForumWindow fw = new ForumWindow(toSend, "Guest", _cnh, sessionKey);
                    this.Close();
                    fw.Show();
                }
                else if (pass != "" && (sessionKey = _fMC.login(userName, _choosenForum, pass)).Contains(","))
                {
                    if (toSend.forumPolicy.notificationsType == ForumPolicyData.OFFLINE_NOTIFICATIONS_TPYE)
                    {
                        List<string> offlineNotifications = _fMC.getOfflineNotifications(_choosenForum, userName, Int32.Parse(sessionKey.Substring(0,sessionKey.IndexOf(","))));
                        if (offlineNotifications != null && offlineNotifications.Count > 0)
                        {
                            OfflineNotificationsWindow offlineNotificationsWindow = new OfflineNotificationsWindow(offlineNotifications);
                            offlineNotificationsWindow.Show();
                        }
                    }

                    MessageBox.Show("Login successful! your session code for is " +sessionKey.Substring(0, sessionKey.IndexOf(",")));
                    ForumWindow fw = new ForumWindow(toSend, userName, _cnh,sessionKey);
                    this.Close();
                    fw.Show();
                }
                else if (pass == "" )
                {
                    MessageBox.Show("please fill the required fields");
                }
                else
                {
                    switch (sessionKey)
                    {
                        case "-1":
                            MessageBox.Show("could not login, wrong credentials");
                            break;

                        case "-2":
                            MessageBox.Show("user name or password are invalid");
                            break;

                        case "-3":
                            MessageBox.Show("you are already connected via another client, " +
                                            "please login using your session key");
                            SessionKeyWindow sk = new SessionKeyWindow(userName, _choosenForum);
                            sk.Show();
                            this.Close();
                            break;
                        case "-5":
                            MessageBox.Show("Your password has expierd! it's time to change password");
                            correntProblemsFromSetPrefferencesWindow newWin3 = new correntProblemsFromSetPrefferencesWindow(userName, _choosenForum, pass, sessionKey);
                            newWin3.Show();
                            this.Close();
                            break;
                        case "-6":
                            MessageBox.Show("needs to add new identifying questions");
                            correntProblemsFromSetPrefferencesWindow newWin4 = new correntProblemsFromSetPrefferencesWindow(userName, _choosenForum, pass, sessionKey);
                            newWin4.Show();
                            this.Close();
                            break;
                        case "-7":
                            MessageBox.Show("you have to change password, policy  changed");
                            correntProblemsFromSetPrefferencesWindow newWin5 = new correntProblemsFromSetPrefferencesWindow(userName, _choosenForum, pass, sessionKey);
                            newWin5.Show();
                            this.Close();
                            break;
                        default:
                            MessageBox.Show("login failed");
                            break;
                    }

                }
            }
            else
            {
                MessageBox.Show("You have to choose forum from the list");
            }
        }
        
        private void SignUpUser(object sender, RoutedEventArgs e)
        {
            SignUpWindow suw = null;
            try
            {
                suw = new SignUpWindow(_fMC,_choosenForum);
            }
            catch
            {
                MessageBox.Show("please choose a forum to register", "error");
            }
            try
            {
                suw.Show();
                this.Close();
            }
            catch(NullReferenceException)
            {
                //if the forum was not successfully instantiated it will throw null reference exception
            }
        }

        private void guestChoose(object sender, RoutedEventArgs e)
        {
            bool toChange = guestCheck.IsChecked.Value;
            if (toChange) 
            { 
                ID.IsEnabled = false;
                Password.IsEnabled = false;
                superUserViewButton.IsEnabled = false;
                signUP.IsEnabled = false;
                restore.IsEnabled = false;
            }
            else 
            {
                ID.IsEnabled = true;
                Password.IsEnabled = true;
                superUserViewButton.IsEnabled = true;
                signUP.IsEnabled = true;
                restore.IsEnabled = true;
            }
        }
        private void superUserViewButton_Click(object sender, RoutedEventArgs e)
        {
            SuperUserLogInWindow newWin = new SuperUserLogInWindow();
            newWin.Show();
            this.Close();
        }

        private void ComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            comboBox.Items.Clear();
            while (_forumsList == null) { Thread.Sleep(20); }
            foreach (String forumName in this._forumsList)
                comboBox.Items.Add(forumName);
        }

        private void ComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            try
            {
                _choosenForum = comboBox.SelectedItem.ToString();
                _forumsList = _fMC.getForums();
                if (!_fMC.getForum(_choosenForum).forumPolicy.isQuestionIdentifying)
                {
                    restore.IsEnabled = false;
                }
                else
                {
                    restore.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please choose a forum from the list");
            }
        }

        private void restorePass(object sender, RoutedEventArgs e)
        {
            if (_choosenForum != null)
            {
                RestorePasswordWindow rpw = new RestorePasswordWindow(_fMC, _choosenForum);
                this.Close();
                rpw.Show();
            }
            else
            {
                MessageBox.Show("You have to choose forum from the list");
            }
        }

    }
}
