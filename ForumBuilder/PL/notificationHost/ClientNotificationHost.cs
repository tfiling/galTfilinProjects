using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForumBuilder.Common.ClientServiceContracts;
using System.ServiceModel;
using System.Windows;
using System.Threading;

namespace PL.notificationHost
{
    public class ClientNotificationHost : IUserNotificationsService
    {
        private RefreshedWindow _currentWindow;
        
        public void updateWindow(Window window)
        {
            if (window is RefreshedWindow)
                _currentWindow = (RefreshedWindow)window;
            else
                _currentWindow = null;
        }

        public void applyPostPublishedInForumNotification(String forumName, String subForumName, String publisherName)
        {
            MessageBox.Show(publisherName + " published a post in " + forumName + 
                "'s sub-forum " + subForumName, "new post");
            if(_currentWindow != null)
                _currentWindow.refresh();
            SubForumWindow._generalAddedPostThreadFlag++;
        }

        public void applyPostModificationNotification(String forumName, String publisherName, String title)
        {
                MessageBox.Show(publisherName + "'s post you were following in " + forumName + " was modified (" + title + ")", "post modified");
        }

        public void applyPostDelitionNotification(String forumName, String publisherName, bool toSendMessage)
        {
            if (toSendMessage)
            {
                MessageBox.Show(publisherName + "'s post you were following in " + forumName + " was deleted", "post deleted");
            }
            else
            {
                SubForumWindow._generalDeletedPostThreadFlag++;
            }
        }

        public void sendUserMessage(String senderName, String content)
        {
            MessageBox.Show(content, senderName + " set you a message");
        }

    }
}
