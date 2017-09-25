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
    /// Interaction logic for SuperUserLogInWindow.xaml
    /// </summary>
    public partial class SuperUserLogInWindow : Window
    {
        private SuperUserManagerClient _su;

        public SuperUserLogInWindow()
        {
            InitializeComponent();
            _su = new SuperUserManagerClient();
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = userNameTextBox.Text;
            string pass = passwordTextBox.Password;
            string mail = emailTextBox.Text;
            if (_su.login(userName, pass, mail))
            {
                SuperUserWindow newWin = new SuperUserWindow(userName, pass, mail);
                newWin.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("couldn't log in");
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWin = new MainWindow();
            newWin.Show();
            this.Close();
        }
    }
}
