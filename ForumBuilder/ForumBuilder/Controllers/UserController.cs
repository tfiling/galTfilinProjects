using System;
using System.Collections.Generic;
using BL_Back_End;
using Database;

namespace ForumBuilder.Controllers
{
    public class UserController : IUserController
    {
        private static UserController singleton;
        ForumController forumController = ForumController.getInstance;
        DBClass DB = DBClass.getInstance;
        Systems.Logger logger = Systems.Logger.getInstance;

        public static UserController getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new UserController();
                    Systems.Logger.getInstance.logPrint("User contoller created",0);
                    Systems.Logger.getInstance.logPrint("User contoller created",1);
                }
                return singleton;
            }

        }
        
        public String addFriend(string userName, string friendToAddName)
        {
            User user = DB.getUser(userName);
            User friendToAdd = DB.getUser(friendToAddName);
            if (user == null)
            {
                logger.logPrint("Add friend faild, " + userName + "is not a user",0);
                logger.logPrint("Add friend faild, " + userName + "is not a user",2);
                return "Add friend faild, " + userName + "is not a user";
            }
            if (friendToAdd == null)
            {
                logger.logPrint("Add friend faild, " + friendToAddName + "is not a user",0);
                logger.logPrint("Add friend faild, " + friendToAddName + "is not a user",2);
                return "Add friend faild, " + friendToAddName + "is not a user";
            }
            if (friendToAddName.Equals(userName) )
            {
                logger.logPrint("Add friend faild, " + friendToAddName + " can't add himself", 0);
                logger.logPrint("Add friend faild, " + friendToAddName + " can't add himself", 2);
                return "Add friend faild, " + friendToAddName + " can't add himself";
            }
            if(!ForumController.getInstance.isMembersOfSameForum(friendToAddName, userName))
            {
                logger.logPrint("Add friend faild, " + friendToAddName + " and "+userName + " are not in the same forum",0);
                logger.logPrint("Add friend faild, " + friendToAddName + " and " + userName + " are not in the same forum",2);
                return "Add friend faild, " + friendToAddName + " and " + userName + " are not in the same forum";
            }
            if (DB.getUserFriends(userName).Contains(friendToAddName))
            {
                logger.logPrint("Add friend faild, " + userName + " and " + friendToAddName + " are already friends", 0);
                logger.logPrint("Add friend faild, " + userName + " and " + friendToAddName + " are already friends", 2);
                return "Add friend faild, " + userName + " and " + friendToAddName + " are already friends";
            }
            if (DB.addFriendToUser(userName, friendToAddName))
            {
                return "friend was added successfuly";
            }
            return "Add friend faild";            
        }

        public String deleteFriend(string userName, string deletedFriendName)
        {
            User user = DB.getUser(userName);
            User friendTodelete = DB.getUser(deletedFriendName);
            if (user == null)
            {
                logger.logPrint("Remove friend faild, " + userName + "is not a user",0);
                logger.logPrint("Remove friend faild, " + userName + "is not a user",2);
                return "Remove friend faild, " + userName + "is not a user";
            }
            if (friendTodelete == null)
            {
                logger.logPrint("Remove friend faild, " + deletedFriendName + "is not a user",0);
                logger.logPrint("Remove friend faild, " + deletedFriendName + "is not a user",2);
                return "Remove friend faild, " + deletedFriendName + "is not a user";
            }
            if (!getFriendList(userName).Contains(deletedFriendName))
            {
                logger.logPrint("Remove friend faild, " + userName + " and " + deletedFriendName + " are not friends",0);
                logger.logPrint("Remove friend faild, " + userName + " and " + deletedFriendName + " are not friends",2);
                return "Remove friend faild, " + userName + " and " + deletedFriendName + " are not friends";
            }
            if (DB.removeFriendOfUser(userName, deletedFriendName))
            {
                return "Remove friend Succeeded";
            }
            return "Remove friend faild";
        }

        public String sendPrivateMessage(String forumName, string fromUserName, string toUserName, string content)
        {
            User sender = DB.getUser(fromUserName);
            User reciver = DB.getUser(toUserName);
            if (sender == null)
            {
                logger.logPrint("Send message faild, " + fromUserName + "is not a user",0);
                logger.logPrint("Send message faild, " + fromUserName + "is not a user",2);
                return "Send message faild, " + fromUserName + "is not a user";
            }
            else if (reciver == null)
            {
                logger.logPrint("Send message faild, " + fromUserName + "is not a user",0);
                logger.logPrint("Send message faild, " + fromUserName + "is not a user",2);
                return "Send message faild, " + fromUserName + "is not a user";
            }
            else if (!ForumController.getInstance.isMembersOfSameForum(fromUserName, toUserName) && !SuperUserController.getInstance.isSuperUser(sender.userName) )
            {
                logger.logPrint("Send message faild, " + fromUserName + " and " + toUserName + " are not in the same forum",0);
                logger.logPrint("Send message faild, " + fromUserName + " and " + toUserName + " are not in the same forum",2);
                return "Send message faild, " + fromUserName + " and " + toUserName + " are not in the same forum";
            }
            else if (content.Equals(""))
            {
                logger.logPrint("Send message faild, no content in message",0);
                logger.logPrint("Send message faild, no content in message",2);
                return ("Send message faild, no content in message");
            }
            else
            {
                if(DB.addMessage(fromUserName, toUserName, content))
                {
                    forumController.notifyUserOnNewPrivateMessage(forumName, fromUserName, toUserName, content);
                    return "message was sent successfully";
                }
                return "Send message faild";
            }
        }

        public List<String> getFriendList(String userName)
        {
            if (DB.getUser(userName) == null)
                return null;
            return DB.getUserFriends(userName);
        }

        public List<string[]> getAllPrivateMessages(string userName)
        {
            List<Message> messageList = DB.getMessagesOfUserAsReciver(userName);
            List<string[]> messagesOfWantedUser = new List<string[]>();
            foreach(Message msg in messageList)
            {
                string[] messageAsStringArray = { msg.sender, msg.Content };
                messagesOfWantedUser.Add(messageAsStringArray);
            }
            return messagesOfWantedUser;
        }
        
        public string restorePassword(string userName, string ans1, string ans2)
        {
            string password = null;
            List<String> answers = DB.getAnswers(userName);
            if (answers == null)
                return null;
            if (ans1.Equals(answers[0]) && ans2.Equals(answers[1]))
            {
                password = DB.getPassword(userName);
            }
            return password;
        }

        public string setNewPassword(string userName, string forumName, string password,string oldPassword)
        {
            Forum forum = forumController.getForum(forumName);
            if (!DB.getPassword(userName).Equals(oldPassword))
            {
                logger.logPrint("change password failed, fake user with rong password tried to enter", 0);
                logger.logPrint("change password failed, fake user with rong password tried to enter", 2);
                return "change password failed, fake user with rong password tried to enter";
            }
            if (forumController.isMember(userName, forumName)&&forum!=null)
            {
                if (forum.forumPolicy.minLengthOfPassword <= password.Length &&
                (!forum.forumPolicy.hasCapitalInPassword || (forum.forumPolicy.hasCapitalInPassword && forumController.hasCapital(password))) &&
                (!forum.forumPolicy.hasNumberInPassword || (forum.forumPolicy.hasNumberInPassword && forumController.hasNumber(password))))
                {
                    DB.setPassword(userName, password);
                    logger.logPrint("change password succeed", 0);
                    logger.logPrint("change password succeed", 1);
                    return "change password succeed";
                }
                logger.logPrint("change password failed, password is not storng enough according to forum policy", 0);
                logger.logPrint("change password failed, password is not storng enough according to forum policy", 2);
                return "change password failed, password is not storng enough according to forum policy";
            }
            logger.logPrint("change password failed, " + userName + " is not a member in " + forumName, 0);
            logger.logPrint("change password failed, " + userName + " is not a member in " + forumName, 2);
            return "change password failed, "+userName + " is not a member in " + forumName;
        }
    }
}
