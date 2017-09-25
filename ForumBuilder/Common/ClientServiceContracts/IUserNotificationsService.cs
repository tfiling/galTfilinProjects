using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ForumBuilder.Common.ClientServiceContracts
{
    [ServiceContract]
    public interface IUserNotificationsService
    {

        [OperationContract(IsOneWay = true)]
        void applyPostPublishedInForumNotification(String forumName, String subForumName, String publisherName);

        [OperationContract(IsOneWay = true)]
        void applyPostModificationNotification(String forumName, String publisherName, String title);

        [OperationContract(IsOneWay = true)]
        void applyPostDelitionNotification(String forumName, String publisherName, bool toSendMessage);

        [OperationContract(IsOneWay = true)]
        void sendUserMessage(String senderName, String content);
    }
}
