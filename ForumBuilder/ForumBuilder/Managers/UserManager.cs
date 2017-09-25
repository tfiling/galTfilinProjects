using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForumBuilder.Controllers;
using ForumBuilder.Common.ServiceContracts;

namespace Service
{
    public class UserManager :IUserManager
    {
        private static UserManager singleton;
        private IUserController userController;

        private UserManager()
        {
            userController = UserController.getInstance;
        }

        public static UserManager getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new UserManager();
                }
                return singleton;
            }
        }

        public String addFriend(String userName, String friendToAdd)
        {
            return userController.addFriend(userName, friendToAdd);
        }
        public String deleteFriend(String userName, String deletedFriend)
        {
            return userController.deleteFriend(userName, deletedFriend);
        }
        public String sendPrivateMessage(String forumName, String fromUserName, String toUserName, String content)
        {
            return userController.sendPrivateMessage(forumName, fromUserName, toUserName, content);
        }
        public List<string[]> getAllPrivateMessages(String userName)
        {
            return userController.getAllPrivateMessages(userName);
        }
        public List<String> getFriendList(String userName)
        {
            List<string> result = userController.getFriendList(userName);
            if (result == null)
                result = new List<string>();

            return result;
        }

        public string restorePassword(string userName, string ans1, string ans2)
        {
            return userController.restorePassword(userName, ans1, ans2);
        }
        public string setNewPassword(string userName, string forumName, string password, string oldPassword)
        {
            return userController.setNewPassword(userName, forumName, password, oldPassword);
        }
    }
}
