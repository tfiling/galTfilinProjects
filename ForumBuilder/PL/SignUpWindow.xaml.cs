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
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        private ForumManagerClient _fMC;
        private String _forumToRegister;

        public SignUpWindow(ForumManagerClient forumManager, String forumName)
        {
            if (forumName == null)
                throw new Exception();
            InitializeComponent();
            _fMC = forumManager;
            _forumToRegister = forumName;
            if (_fMC.getForum(forumName).forumPolicy.isQuestionIdentifying){ 
                withQuestions.Visibility = System.Windows.Visibility.Visible;
                withoutQuestions.Visibility = System.Windows.Visibility.Collapsed;
            }
            else { 
                withoutQuestions.Visibility = System.Windows.Visibility.Visible;
                withQuestions.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void UserRegistration(object sender, RoutedEventArgs e)
        {
            string userName = name.Text;
            string pass = Password.Password;
            string userMail = mail.Text;
            string register = _fMC.registerUser(userName, pass, userMail, "", "", _forumToRegister); 
            bool suc = register.Equals("Register user succeed");
            if (suc == false)
            {
                MessageBox.Show(register + " Please try again!");
            }
            else
            {
                MessageBox.Show(userName + "  Registration succeeded!");
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
        }

        private void secUserRegistration(object sender, RoutedEventArgs e)
        {
            string userName = secname.Text;
            string pass = secPassword.Password;
            string userMail = secmail.Text;
            string ans1 = ansToq1.Text;
            string ans2 = ansToq2.Text;
            String register= _fMC.registerUser(userName, pass, userMail, ans1, ans2, _forumToRegister);
            bool suc = register.Equals("Register user succeed");
            if (suc == false)
            {
                MessageBox.Show(register+ " Please try again!");
            }
            else
            {
                MessageBox.Show(userName + "  Registration succeeded!");
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
        }

        private void backToMain(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        } 
    }
}
