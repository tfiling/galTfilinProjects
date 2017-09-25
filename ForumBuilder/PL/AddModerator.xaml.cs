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
    /// Interaction logic for AddModerator.xaml
    /// </summary>
    public partial class AddModerator : Window
    {
        private string _userName;
        private string _subforum;
        private string _forum;
        private String _sKey;
        private SubForumManagerClient _sm;
        private ForumManagerClient _fMC;

        public AddModerator(string subForum, string userName, string forum, String sKey)
        {
            InitializeComponent();
            _userName = userName;
            _subforum = subForum;
            _forum = forum;
            _sKey = sKey;
            _sm = new SubForumManagerClient();
            _fMC = new ForumManagerClient(new InstanceContext(new ClientNotificationHost()));
            usrLabel.Content = "UserName:  " + userName;
            sKeyLabel.Content = "Session key:  " + sKey.Substring(0, sKey.IndexOf(","));
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            string moderatorName = moderatortextBox.Text;
            int unlimited = 120;
            int time = 1;
            DateTime timeToSend = DateTime.Now;
            string timeDuration = durationComboBox.Text;
            switch (timeDuration)
            {
                case "":
                    MessageBox.Show("You have to choose time from the list!");
                    break;
                case "Unlimited":
                    timeToSend = DateTime.Now.AddYears(unlimited);
                    howToProcced(moderatorName, timeToSend);
                    break;
                default:
                    time = int.Parse(timeDuration);
                    timeToSend = DateTime.Now.AddDays(time);
                    howToProcced(moderatorName, timeToSend);
                    break;
            }
            
        }

        private void howToProcced(string moderator, DateTime time)
        {
            string res = _sm.nominateModerator(moderator, _userName, time, _subforum, _forum);
            if (res.Equals("nominate moderator succeed"))
            {
                MessageBox.Show("moderator was added successfully");
                SubForumWindow sb = new SubForumWindow(_forum, _subforum, _userName, _sKey, new ClientNotificationHost());
                sb.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(res);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            SubForumWindow newWin = new SubForumWindow(_forum, _subforum, _userName, _sKey, new ClientNotificationHost());
            newWin.Show();
            this.Close();
        }

        private void comboBox_DropDownOpened(object sender, EventArgs e)
        {
            durationComboBox.Items.Clear();
            durationComboBox.Items.Add("Unlimited");
            int minDays = 1;
            int maxDays = 365;
            for (int i = minDays; i <= maxDays; i++)
            {
                durationComboBox.Items.Add(i);
            }
        }
    }
}
