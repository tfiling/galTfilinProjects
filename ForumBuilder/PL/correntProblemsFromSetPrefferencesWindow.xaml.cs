using ForumBuilder.Common.DataContracts;
using PL.notificationHost;
using PL.proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for correntProblemsFromSetPrefferencesWindow.xaml
    /// </summary>
    public partial class correntProblemsFromSetPrefferencesWindow : Window
    {
        private ForumManagerClient _fMC;
        private string _userName;
        private string _forumName;
        private string _password;
        private String _whatToDo;

        public correntProblemsFromSetPrefferencesWindow(string userName, string forumName, string password, String whatToDo)
        {
            InitializeComponent();
            _fMC = new ForumManagerClient(new InstanceContext(new ClientNotificationHost()));
            _userName = userName;
            _forumName = forumName;
            _password = password;
            _whatToDo = whatToDo;
            if(_whatToDo.Equals("-5")|| _whatToDo.Equals("-7"))
            {
                setPassGrid.Visibility = Visibility.Visible;
                backButton.Visibility = Visibility.Visible;
                setIdentifyingQuestionsGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                setPassGrid.Visibility = Visibility.Collapsed;
                backButton.Visibility = Visibility.Visible;
                setIdentifyingQuestionsGrid.Visibility = Visibility.Visible;
            }
        }

        private void setPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string newPass = setPassTextBox.Password;
            UserManagerClient um = new UserManagerClient();
            string response = um.setNewPassword(_userName, _forumName, newPass, _password);
            if (response.Equals("change password succeed"))
            {
                login(newPass);
            }
            else
            {
                MessageBox.Show("could not update the answers, please try again");
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWin = new MainWindow();
            newWin.Show();
            this.Close();
        }

        private void addIdentifyingQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            string ans1 = ansToq1.Text;
            string ans2 = ansToq2.Text;
            bool response = _fMC.setAnswers(_forumName, _userName, ans1, ans2);
            if (!response)
            {
                MessageBox.Show("could not update the answers, please try again");
            }
            else
            {
                login(_password);
            }
        }

        private void login(string password)
        {
            String sessionKey = _fMC.login(_userName, _forumName, password);

            if (sessionKey.Contains(","))
            {
                MessageBox.Show("Login successful! your session code for is " + sessionKey.Substring(0, sessionKey.IndexOf(",")));
                ForumData forum = _fMC.getForum(_forumName);
                ForumWindow fw = new ForumWindow(forum, _userName, new ClientNotificationHost(),sessionKey);
                fw.Show();
                this.Close();
                
            }
            else
            {
                
                switch (sessionKey)
                {
                    case "-1":
                        MessageBox.Show("login failed");
                        MainWindow newWin = new MainWindow();
                        newWin.Show();
                        this.Close();
                        break;

                    case "-2":
                        MessageBox.Show("user name or password are invalid");
                        MainWindow newWin2 = new MainWindow();
                        newWin2.Show();
                        this.Close();
                        break;

                    case "-3":
                        MessageBox.Show("you are already connected via another client, " +
                                        "please login using your session key");
                        SessionKeyWindow sk = new SessionKeyWindow(_userName, _forumName);
                        sk.Show();
                        this.Close();
                        break;
                    case "-5":
                        MessageBox.Show("Your password has expierd! it's time to change password");
                        correntProblemsFromSetPrefferencesWindow newWin3 = new correntProblemsFromSetPrefferencesWindow(_userName, _forumName, password, sessionKey);
                        newWin3.Show();
                        this.Close();
                        break;
                    case "-6":
                        MessageBox.Show("needs to add new identifying questions");
                        correntProblemsFromSetPrefferencesWindow newWin4 = new correntProblemsFromSetPrefferencesWindow(_userName, _forumName, password, sessionKey);
                        newWin4.Show();
                        this.Close();
                        break;
                    case "-7":
                        MessageBox.Show("you have to change password, policy  changed");
                        correntProblemsFromSetPrefferencesWindow newWin5 = new correntProblemsFromSetPrefferencesWindow(_userName, _forumName, password, sessionKey);
                        newWin5.Show();
                        this.Close();
                        break;
                    default:
                        MessageBox.Show("login failed");
                        MainWindow newWin6 = new MainWindow();
                        newWin6.Show();
                        this.Close();
                        break;
                }
            }
        }
    }
}
