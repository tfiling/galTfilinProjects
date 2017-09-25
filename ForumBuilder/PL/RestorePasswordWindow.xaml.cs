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


namespace PL
{
    /// <summary>
    /// Interaction logic for RestorePasswordWindow.xaml
    /// </summary>
    public partial class RestorePasswordWindow : Window
    {
        private ForumManagerClient _fMC;
        private UserManagerClient _uMC;
        private string _forum;
        private string _usrName;

        public RestorePasswordWindow(ForumManagerClient forumManClient,string forum)
        {
            InitializeComponent();
            _fMC = forumManClient;
            _uMC = new UserManagerClient();
            _forum = forum;
            ForumName.Content = "ForumName:  " + forum;
        }

        private void SendDetails(object sender, RoutedEventArgs e)
        {
            _usrName = usrName.Text;
            Boolean usr = _fMC.isMember(_usrName, _forum);
            if(!usr)
            {
                MessageBox.Show("Wrong Username! Please try again.");
            }
            else
            {
                mainGrid.Visibility = System.Windows.Visibility.Collapsed;
                UserName.Content = "Hello " + _usrName + " !";
                restoreQuestions.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void submitAnswers(object sender, RoutedEventArgs e)
        {
            string ans1 = ansToq1.Text;;
            string ans2= ansToq2.Text;

            string pass = _uMC.restorePassword(_usrName,ans1, ans2);
            if(pass == null)
            {
                MessageBox.Show("Wrong Answers! Please try again.");
            }
            else
            {
                MessageBox.Show("Your password is : " + pass);
                MainWindow aw = new MainWindow();
                this.Close();
                aw.Show();
            }
        }

        private void BackToMainWindow(object sender, RoutedEventArgs e)
        {
            MainWindow aw = new MainWindow();
            this.Close();
            aw.Show();
        }

    }
}
