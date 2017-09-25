using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForumBuilder.Common.ClientServiceContracts;

namespace Tests
{
    class TestableUserNotifications : IUserNotificationsService
    {
        public int publishedCounter;
        public int modifiedCounter;
        public int deletedCounter;
        public int privateMessage;


        public void applyPostPublishedInForumNotification(String forumName, String subForumName, String publisherName)
        {
            publishedCounter++;
        }

        public void applyPostModificationNotification(String forumName, String publisherName, String title)
        {
            modifiedCounter++;
        }

        public void applyPostDelitionNotification(String forumName, String publisherName, bool toSendMessage)
        {
            if (toSendMessage)
                deletedCounter++;
        }

        public void sendUserMessage(String senderName, String content)
        {
            privateMessage++;
        }

        public void clearCounters()
        {
            publishedCounter = 0;
            modifiedCounter = 0;
            deletedCounter = 0;
            privateMessage = 0;
        }

    }
}
