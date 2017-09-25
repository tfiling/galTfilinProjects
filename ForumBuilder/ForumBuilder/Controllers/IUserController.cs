using System;
using System.Collections.Generic;

namespace ForumBuilder.Controllers
{
    public interface IUserController
    {
        String addFriend(String userName, String friendToAdd);
        String deleteFriend(String userName, String deletedFriend);
        String sendPrivateMessage(String forumName, string fromUserName, string toUserName, string content);
        List<String> getFriendList(String userName);
        List<string[]> getAllPrivateMessages(string userName);
        string restorePassword(string userName, string ans1, string ans2);
        string setNewPassword(string userName, string forumName, string password, string oldPassword);
    }
}
