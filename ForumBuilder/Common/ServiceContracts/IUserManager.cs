using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ForumBuilder.Common.ServiceContracts
{
    [ServiceContract]
    public interface IUserManager
    {
        [OperationContract]
        String addFriend(String userName, String friendToAdd);

        [OperationContract]
        String deleteFriend(String userName, String deletedFriend);

        [OperationContract]
        String sendPrivateMessage(String forumName, String fromUserName, String toUserName, String content);

        [OperationContract]
        List<String> getFriendList(String userName);

        [OperationContract]
        List<string[]> getAllPrivateMessages(string userName);

        [OperationContract]
        string restorePassword(string userName, string ans1, string ans2);

        [OperationContract]
        string setNewPassword(string userName, string forumName, string password, string oldPassword);
    }
}
