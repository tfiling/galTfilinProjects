using System;
using System.Collections.Generic;
using BL_Back_End;
using Database;
using System.ServiceModel;
using ForumBuilder.Common.ClientServiceContracts;

namespace ForumBuilder.Controllers
{
    public class ForumController :IForumController
    {
        private static ForumController singleton;
        DBClass DB = DBClass.getInstance;
        Systems.Logger logger = Systems.Logger.getInstance;
        Dictionary<String, List<String>> loggedInUsersByForum = new Dictionary<String, List<String>>();
        Dictionary<String, IUserNotificationsService> channelsByLoggedInUsers = new Dictionary<String, IUserNotificationsService>();
        Dictionary<String, int> Sessions = new Dictionary<String, int>();
        Dictionary<String, List<string>> clientSessionKeyByUser = new Dictionary<String, List<string>>();
        Dictionary<String, int> openConnectionsCounterByUser = new Dictionary<String, int>();
        List<int> occupied=new List<int>();
        public static Object sysLock = new Object();

        public static ForumController getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new ForumController();
                    Systems.Logger.getInstance.logPrint("Forum contoller created", 0);
                    Systems.Logger.getInstance.logPrint("Forum contoller created", 1);
                }
                return singleton;
            }
        }

        public String addSubForum(string forumName, string name, Dictionary<String, DateTime> moderators, string creatorName)
        {
            if (DB.getSuperUser(creatorName) != null || isAdmin(creatorName, forumName))
            {
                Forum forum = DB.getforumByName(forumName);
                if (forum != null)
                {
                    if (forum.subForums.Contains(name))
                    {
                        logger.logPrint("Add sub-forum failed, "+name+ " already exist", 0);
                        logger.logPrint("Add sub-forum failed, " + name + " already exist", 2);
                        return "Add sub-forum failed, " + name + " already exist";
                    }
                    if (moderators==null||moderators.Count < 1)
                    {
                        logger.logPrint("Add sub-forum failed, there is not enough moderators", 0);
                        logger.logPrint("Add sub-forum failed, there is not enough moderators", 2);
                        return "Add sub-forum failed, there is not enough moderators";
                    }
                    foreach (string s in moderators.Keys)
                    {
                        if (!isMember(s, forumName))
                        {
                            logger.logPrint("Add sub-forum failed, the moderator " + s + " is not member of forum", 0);
                            logger.logPrint("Add sub-forum failed, the moderator " + s + " is not member of forum", 2);
                            return "Add sub-forum failed, the moderator " + s + " is not member of forum";
                        }
                        else if ((DateTime.Today - DB.getUser(s).date).Days < forum.forumPolicy.seniorityInForum)
                        {
                            logger.logPrint("Add sub-forum failed, the moderator " + s + " is not enough time in forum", 0);
                            logger.logPrint("Add sub-forum failed, the moderator " + s + " is not enough time in forum", 2);
                            return "Add sub-forum failed, the moderator " + s + " is not enough time in forum";
                        }
                        DateTime date;
                        moderators.TryGetValue(s, out date);
                        if (date < DateTime.Now)
                        {
                            logger.logPrint("Add sub-forum failed, date is already passed", 0);
                            logger.logPrint("Add sub-forum failed, date is already passed", 2);
                            return "Add sub-forum failed, date of moderator" + s + " is already passed";
                        }
                    }
                    if (forum.forumPolicy.minNumOfModerators > moderators.Count)
                    {
                        logger.logPrint("Add sub-forum failed, there is not enough moderators", 0);
                        logger.logPrint("Add sub-forum failed, there is not enough moderators", 2);
                        return "Add sub-forum failed, there is not enough moderators according to forum policy";
                    }
                    if (DB.addSubForum(name, forumName))
                    {
                        foreach (string s in moderators.Keys)
                        {
                            DateTime date;
                            moderators.TryGetValue(s, out date);
                            SubForumController.getInstance.nominateModerator(s, creatorName, date, name, forumName);
                        }
                        logger.logPrint("Add sub-forum failed", 0);
                        logger.logPrint("Add sub-forum failed", 2);
                        return "sub-forum added";
                    }
                    return "sub-forum failed";
                }
                return "sub-forum failed";
            }
            else
            {
                logger.logPrint("Add sub-forum failed, " + creatorName + " is not allowed", 0);
                logger.logPrint("Add sub-forum failed, " + creatorName + " is not allowed", 2);
                return "Add sub-forum failed, " + creatorName + " is not allowed";
            }
        }
        public List<String> getForums()
        {
            return DB.getForums();
        }
        internal bool isMembersOfSameForum(string friendToAdd, string userName)
        {
            if (DB.getSimularForumsOf2users(friendToAdd, userName) != null &&
                DB.getSimularForumsOf2users(friendToAdd, userName).Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool banMember(string bannedMember, string bannerUserName, string forumName)
        {
            if (!isMember(bannedMember, forumName))
            {
                logger.logPrint("Ban Member failed, " + bannedMember + " is not a member", 0);
                logger.logPrint("Ban Member failed, " + bannedMember + " is not a member", 2);
                return false;
            }
            else if (!isAdmin(bannerUserName, forumName) && DB.getSuperUser(bannerUserName) == null)
            {
                logger.logPrint("Ban Member failed, " + bannedMember + " is not a admin or super user", 0);
                logger.logPrint("Ban Member failed, " + bannedMember + " is not a admin or super user", 2);
                return false;
            }
            else
            {
                return DB.banMember(bannedMember, forumName);
            }
        }

        public bool dismissAdmin(string adminToDismissed, string dismissingUserName, string forumName)
        {
            if (!isAdmin(adminToDismissed, forumName))
            {
                logger.logPrint("Dismiss admin failed, " + adminToDismissed + " is not a admin", 0);
                logger.logPrint("Dismiss admin failed, " + adminToDismissed + " is not a admin", 2);
                return false;
            }
            else if (DB.getSuperUser(dismissingUserName) == null)
            {
                logger.logPrint("Ban Member failed, " + dismissingUserName + " is not a super user", 0);
                logger.logPrint("Ban Member failed, " + dismissingUserName + " is not a super user", 2);
                return false;
            }
            else
            {
                return DB.dismissAdmin(adminToDismissed, forumName);
            }
        }

        public bool isAdmin(string userName, string forumName)
        {
            Forum forum = DB.getforumByName(forumName);
            if (forum == null)
                return false;
            foreach (string s in forum.administrators)
            {
                if (s.Equals(userName))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isMember(string userName, string forumName)
        {
            Forum forum = DB.getforumByName(forumName);
            List<string> users = DB.getMembersOfForum(forumName);
            if (users == null)
            {
                return false;
            }
            if (users.Contains(userName))
            {
                return true;
            }
            return false;
        }

        public String nominateAdmin(string newAdmin, string nominatorName, string forumName)
        {
            Forum forum = DB.getforumByName(forumName);
            if (forum == null)
            {
                logger.logPrint("nominate admin fail, forum does not exist", 0);
                logger.logPrint("nominate admin fail, forum does not exist", 2);
                return "nominate admin fail, forum does not exist";
            }
            if (forum.administrators.Contains(newAdmin))
            {
                logger.logPrint("nominate admin fail, " + newAdmin + "is already admin", 0);
                logger.logPrint("nominate admin fail, " + newAdmin + "is already admin", 2);
                return "nominate admin fail, " + newAdmin + "is already admin";
            }
            if (DB.getUser(newAdmin) == null)
                return "nominate admin fail, " + newAdmin + "is not a user";
            if ((DB.getSuperUser(nominatorName) != null || DB.getforumByName(forumName).administrators.Contains(nominatorName)))
            {
                bool isMem = isMember(newAdmin, forumName);
                if (!isMem)
                {
                    isMem = isMem || DB.addMemberToForum(newAdmin, forumName);
                }
                if (isMem && DB.nominateAdmin(newAdmin, forumName))
                {
                    logger.logPrint("admin nominated successfully", 0);
                    logger.logPrint("admin nominated successfully", 1);
                    if (DB.getforumByName(forumName).administrators.Contains(nominatorName) && DB.getSuperUser(nominatorName) == null)
                        DB.dismissAdmin(nominatorName, forumName);
                    return "admin nominated successfully";
                }
                //return false;
            }
            logger.logPrint("nominate admin fail " + nominatorName + " is not super user", 0);
            logger.logPrint("nominate admin fail " + nominatorName + " is not super user", 2);
            return "nominate admin fail " + nominatorName + " is not super user";
        }

        public String registerUser(string userName, string password, String mail, string ans1, string ans2, string forumName)
        {
            lock (PostController.sysLock)
            {
                Forum f = DB.getforumByName(forumName);
                if (f == null)
                {
                    logger.logPrint("Register user faild, the forum, " + forumName + " does not exist", 0);
                    logger.logPrint("Register user faild, the forum, " + forumName + " does not exist", 2);
                    return "Register user faild, the forum, " + forumName + " does not exist";
                }
                if (mail.Length == 0 || mail.IndexOf('@') <= 0 || mail.Substring(mail.IndexOf('@') + 1).IndexOf('.') <= 0)
                {
                    logger.logPrint("Register user faild, mail format is wrong", 0);
                    logger.logPrint("Register user faild, mail format is wrong", 2);
                    return "Register user faild, mail format is wrong";
                }
                if (!((ans1 != null && ans2 != null) &&
                    ((!f.forumPolicy.isQuestionIdentifying && ans1.Equals("") && ans2.Equals("")) ||
                    (f.forumPolicy.isQuestionIdentifying && !ans1.Equals("") && !ans2.Equals("")))))
                {
                    logger.logPrint("Register user faild, ansers are worng acording to forum policy", 0);
                    logger.logPrint("Register user faild, ansers are worng acording to forum policy", 2);
                    return "Register user faild, ansers are worng acording to forum policy";
                }
                if (userName.Length > 0 && f.forumPolicy.minLengthOfPassword <= password.Length && mail.Length > 0 &&
                    (!f.forumPolicy.hasCapitalInPassword || (f.forumPolicy.hasCapitalInPassword && hasCapital(password))) &&
                    (!f.forumPolicy.hasNumberInPassword || (f.forumPolicy.hasNumberInPassword && hasNumber(password)))) //&&
                                                                                                                        //(ans1 != null && ans2 != null) &&
                                                                                                                        //((!f.forumPolicy.isQuestionIdentifying && ans1.Equals("") && ans2.Equals("")) ||
                                                                                                                        //(f.forumPolicy.isQuestionIdentifying && !ans1.Equals("") && !ans2.Equals(""))))
                {
                    User user = DB.getUser(userName);
                    /*
                    if (user !=null)
                    {
                        if (user.userName.Equals(userName) && user.password.Equals(password))
                        {
                            return DB.addMemberToForum(userName, forumName);
                        }
                        logger.logPrint("Register user failed, " + userName + " is already taken", 0);
                        logger.logPrint("Register user failed, " + userName + " is already taken", 2);
                        return false;
                    }*/
                    if (user != null)
                    {
                        logger.logPrint("Register user failed, " + userName + " is already taken", 0);
                        logger.logPrint("Register user failed, " + userName + " is already taken", 2);
                        return "Register user failed, " + userName + " is already taken";
                    }
                    if (userName.Equals(""))
                    {
                        logger.logPrint("Register user failed, no name entered", 0);
                        logger.logPrint("Register user failed, no name entered", 2);
                        return "Register user failed, no name entered";
                    }
                    if (DB.addUser(userName, password, mail, ans1, ans2))
                    {
                        DB.addMemberToForum(userName, forumName);
                        logger.logPrint("Register user succeed", 0);
                        logger.logPrint("Register user succeed", 1);
                        return "Register user succeed";
                    }
                    return "Register user failed, there was problem to conecting DB";
                }
                logger.logPrint("Register user failed, password is not good enough acording to forum policy", 0);
                logger.logPrint("Register user failed, password is not good enough acording to forum policy", 2);
                return "Register user failed, password is not good enough acording to forum policy";
            }
        }

        public bool hasNumber(string password)
        {
            char[] array = password.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] - '0' >= 0 && array[i] - '9' <= 0)
                    return true;
            }
            return false;
        }

        public bool hasCapital(string password)
        {
            char[] array = password.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] - 'A' >= 0 && array[i] - 'Z' <= 0)
                    return true;
            }
            return false;
        }

        public Boolean addForum(String forumName)
        {
            if (!this.loggedInUsersByForum.ContainsKey(forumName))
            {
                this.loggedInUsersByForum.Add(forumName, new List<String>());
                return true;
            }
            else
                return false;

        }

        public String login(String user, String forumName, string pass)
        {
            if (!loggedInUsersByForum.ContainsKey(forumName))
            {
                loggedInUsersByForum.Add(forumName, new List<string>());
            }
            User usr = DB.getUser(user);
            Forum loggedInForum = DB.getforumByName(forumName);
            if (usr != null && usr.password.Equals(pass) && loggedInForum != null)
            {
                if (needsToAddQuestions(user, forumName))
                {
                    return "-6";
                }
                if (needToChangePassword(user, forumName))
                    return "-5";
                if (!(loggedInForum.forumPolicy.minLengthOfPassword <= pass.Length &&
                (!loggedInForum.forumPolicy.hasCapitalInPassword || (loggedInForum.forumPolicy.hasCapitalInPassword && hasCapital(pass))) &&
                (!loggedInForum.forumPolicy.hasNumberInPassword || (loggedInForum.forumPolicy.hasNumberInPassword && hasNumber(pass)))))
                    return "-7";
                int sessionKey = generateRandomSessionKey();
                if (!this.loggedInUsersByForum[forumName].Contains(user))
                {
                    this.loggedInUsersByForum[forumName].Add(user);
                    this.openConnectionsCounterByUser[user] = 1;
                }
                else
                {
                    return "-3";//the error code for a login while a session key exists
                    //one the user is logged in with one or more client the future logins should be made only by the session key
                    //requirement 1-d in assignment 4 version 3 document
                }
                this.channelsByLoggedInUsers[user+ "," + sessionKey]=OperationContext.Current.GetCallbackChannel<IUserNotificationsService>();
                string session;
                //this.clientSessionKeyByUser[user] = sessionKey;
                if (!clientSessionKeyByUser.ContainsKey(user) )
                {
                    clientSessionKeyByUser[user] = new List<string>();
                }
                if (clientSessionKeyByUser[user].Count == 0)
                {
                    session=sessionKey + "," + sessionKey;
                }
                else
                {
                    session=clientSessionKeyByUser[user][0].Substring(clientSessionKeyByUser[user][0].IndexOf(",")) + "," + sessionKey;
                }
                clientSessionKeyByUser[user].Add(session);
                Sessions[user]=(sessionKey);
                occupied.Add(sessionKey);
                return session;
            }
            else
            {
                logger.logPrint("could not login, wrong credentials", 0);
                logger.logPrint("could not login, wrong credentials", 2);
                return "-1";
            }
        }

        private bool needToChangePassword(string userName, string forumName)
        {
            Forum forum = DB.getforumByName(forumName);
            int time = forum.forumPolicy.timeToPassExpiration;
            User user = DB.getUser(userName);
            if ((DateTime.Today - user.lastTimeUpdatePassword).Days > time)
                return true;
            return false;
        }
        public String loginBySessionKey(int sessionKey, String user, String forumName)
        {
            User usr = DB.getUser(user);
            Forum loggedInForum = DB.getforumByName(forumName);
            if (usr != null && loggedInForum != null)
            {
                if (!loggedInUsersByForum.ContainsKey(forumName) || !this.loggedInUsersByForum[forumName].Contains(user))
                {
                    logger.logPrint("login error, the user was not logged in when using session key", 0);
                    logger.logPrint("login error, the user was not logged in when using session key", 2);
                    return "invalid session key: you logged out hence this session key is invalid";
                }
                if (sessionKey != this.Sessions[user])
                {
                    logger.logPrint("login error, invalid session key", 0);
                    logger.logPrint("login error, invalid session key", 2);
                    return "invalid session key";
                }
                int sessionKey2 = generateRandomSessionKey();
                this.channelsByLoggedInUsers[user+ "," + sessionKey2]=OperationContext.Current.GetCallbackChannel<IUserNotificationsService>();
                this.openConnectionsCounterByUser[user]++;
                string session = sessionKey + "," + sessionKey2;
                clientSessionKeyByUser[user].Add(session);
                return session;//GUY
            //return "success";
            }
            else
            {
                logger.logPrint("could not login, wrong user name", 0);
                logger.logPrint("could not login, wrong user name", 2);
                return "wrong user name";
            }


        }

        public Boolean logout(String user, String forumName,String allSession)
        { 
            if (allSession.IndexOf(",") <= 0)
            {
                return false;
            }
            if (!this.loggedInUsersByForum.ContainsKey(forumName))
                return false;
            if (!this.loggedInUsersByForum[forumName].Contains(user))
                return false;
            if (this.openConnectionsCounterByUser[user] == 1)
            {//last open connection, session key will be discarded
                this.loggedInUsersByForum[forumName].Remove(user);
                this.channelsByLoggedInUsers.Remove(user+ allSession.Substring(allSession.IndexOf(",")));
                Sessions.Remove(user);
                clientSessionKeyByUser.Remove(user);
                openConnectionsCounterByUser.Remove(user);
            }
            else
            {
                //GUY
                this.openConnectionsCounterByUser[user]--;
                this.channelsByLoggedInUsers.Remove(user+ allSession.Substring(allSession.IndexOf(",")));
                clientSessionKeyByUser[user].Remove(allSession);
            }
            return true;
        }

        public Boolean sendThreadCreationNotification(String headLine, String content, String publisherName, String forumName, String subForumName)
        {
            ForumPolicy forumPolicy = DB.getforumByName(forumName).forumPolicy;
            if (loggedInUsersByForum == null)
                this.loggedInUsersByForum = new Dictionary<String, List<String>>();
            List<String> loggedInUsers=null;
            if (loggedInUsersByForum.ContainsKey(forumName)) 
                loggedInUsers = this.loggedInUsersByForum[forumName];
            if (loggedInUsers == null)
                return false;
            foreach (String userName in loggedInUsers)
            {
                if (//channelsByLoggedInUsers[userName] != null &&
                    (forumPolicy.notificationsType == ForumPolicy.SELECTIVE_NOTIFICATIONS_TPYE &&
                                forumPolicy.selectiveNotificationsUsers.Contains(userName)
                     //selective notifications is enabled and the current user is included
                     ||
                     forumPolicy.notificationsType != ForumPolicy.SELECTIVE_NOTIFICATIONS_TPYE)
                    //offline/online notifications are enabled so all of the online users will be notified
                    )
                {
                    foreach (IUserNotificationsService channel in getUserChannels(userName))
                    {
                        if(channel != null)
                            channel.applyPostPublishedInForumNotification(forumName, subForumName, publisherName);
                    }
                }
            }
            if (forumPolicy.notificationsType == ForumPolicy.OFFLINE_NOTIFICATIONS_TPYE)
            {
                List<string> offlineUsers = DB.getMembersOfForum(forumName);
                foreach (String user in loggedInUsers)
                {
                    offlineUsers.Remove(user);
                }
                foreach (String offUser in offlineUsers)
                {
                    DB.NotifyOfflineUser(forumName, offUser,
                        publisherName + " published a post in " + forumName + "'s sub-forum " + subForumName);
                }
            }
            return true;
        }

        public Boolean sendPostModificationNotification(String forumName, String publisherName, String title, String notifiedUser)
        {
            ForumPolicy forumPolicy = DB.getforumByName(forumName).forumPolicy;
            if (loggedInUsersByForum == null)
                return false;
            List<String> loggedInUsers = this.loggedInUsersByForum[forumName];
            if (loggedInUsers == null)
                return false;
            if (loggedInUsers.Contains(notifiedUser))
            {
                List<IUserNotificationsService> channels = this.getUserChannels(notifiedUser);
                if (channels != null)
                {
                    foreach (IUserNotificationsService channel in channels)
                    {
                        channel.applyPostModificationNotification(forumName, publisherName, title);
                    }
                }
            }
            else if (forumPolicy.notificationsType == ForumPolicy.OFFLINE_NOTIFICATIONS_TPYE)
            {
                    DB.NotifyOfflineUser(forumName, notifiedUser,
                        publisherName + "'s post you were following in " + forumName + " was modified (" + title + ")");
            }
            return true;
        }

        public Boolean sendPostDelitionNotification(String forumName, String publisherName, String notifiedUser, bool toSendMessage)
        {
            if (toSendMessage)
            {
                ForumPolicy forumPolicy = DB.getforumByName(forumName).forumPolicy;
                if (loggedInUsersByForum == null)
                    return false;
                List<String> loggedInUsers = this.loggedInUsersByForum[forumName];
                if (loggedInUsers == null)
                    return false;
                if (loggedInUsers.Contains(notifiedUser))
                {
                    List<IUserNotificationsService> channels = this.getUserChannels(notifiedUser);
                    if (channels != null)
                    {
                        foreach (IUserNotificationsService channel in channels)
                        {
                            channel.applyPostDelitionNotification(forumName, publisherName, toSendMessage);
                        }
                    }
                }
                else if (forumPolicy.notificationsType == ForumPolicy.OFFLINE_NOTIFICATIONS_TPYE)
                {
                    DB.NotifyOfflineUser(forumName, notifiedUser,
                        publisherName + "'s post you were following in " + forumName + " was deleted");
                }
                return true;
            }
            else /// refresh to all users
            {
                ForumPolicy forumPolicy = DB.getforumByName(forumName).forumPolicy;
                if (loggedInUsersByForum == null)
                    this.loggedInUsersByForum = new Dictionary<String, List<String>>();
                List<String> loggedInUsers = this.loggedInUsersByForum[forumName];
                if (loggedInUsers == null)
                    return false;
                foreach (String userName in loggedInUsers)
                {

                        foreach (IUserNotificationsService channel in getUserChannels(userName))
                        {
                            if (channel != null)
                                channel.applyPostDelitionNotification(forumName, publisherName, false);
                        }
                }
              
                return true;
            }
        }

        public String setForumPreferences(String forumName, String newDescription, ForumPolicy fp, string setterUserName)
        {
            if (DB.getforumByName(forumName) == null)
            {
                logger.logPrint("Set forum preferences failed, Forum" + forumName + " do not exist", 0);
                logger.logPrint("Set forum preferences failed, Forum" + forumName + " do not exist", 2);
                return "Set forum preferences failed, Forum" + forumName + " do not exist";
            }
            else if (!isAdmin(setterUserName, forumName))
            {
                logger.logPrint("Set forum preferences failed, " + setterUserName + " is not an admin", 0);
                logger.logPrint("Set forum preferences failed, " + setterUserName + " is not an admin", 2);
                return "Set forum preferences failed, " + setterUserName + " is not an admin";
            }
            else if (forumName == null | newDescription == null | setterUserName == null|fp==null)
            {
                logger.logPrint("Set forum preferences failed, one or more of the arguments is null", 0);
                logger.logPrint("Set forum preferences failed, one or more of the arguments is null", 2);
                return "Set forum preferences failed, one or more of the arguments is null";
            }
            else if(fp.minLengthOfPassword<1||fp.minNumOfModerators<1||fp.notificationsType>3||fp.notificationsType<0||
                fp.seniorityInForum < 0 || fp.timeToPassExpiration < 0)
            {
                logger.logPrint("Set forum preferences failed, forum policy is not logic", 0);
                logger.logPrint("Set forum preferences failed, forum policy is not logic", 2);
                return "Set forum preferences failed, forum policy is not logic";
            }
            else if (DB.setForumPreferences(forumName, newDescription, fp))
            {
                logger.logPrint(forumName + "preferences had changed successfully", 0);
                logger.logPrint(forumName + "preferences had changed successfully", 1);
                return "preferences had changed successfully";
            }
            return "preferences change faild";
        }

        public String getForumPolicy(String forumName)
        {
            return DB.getforumByName(forumName).forumPolicy.policy;
        }

        public String getForumDescription(String forumName)
        {
            return DB.getforumByName(forumName).description;
        }

        public Forum getForum(String forumName)
        {
            Forum f = DB.getforumByName(forumName);
            return f;
        }

        public int getAdminReportNumOfPOst(String AdminName, String forumName)
        {
            if (isAdmin(AdminName, forumName))
                return DB.numOfPostInForum(forumName);
            return -1;
        }
        public List<Post> getAdminReportPostOfmember(String AdminName, String forumName, String memberName)
        {
            if (isAdmin(AdminName, forumName) && isMember(memberName, forumName))
                return DB.getMemberPosts(memberName, forumName);
            return null;
        }
        public List<String> getAdminReport(String AdminName, String forumName)
        {
            if (isAdmin(AdminName, forumName))
                return DB.getModertorsReport(forumName);
            return null;

        }

        private int generateRandomSessionKey()
        {
            Random random = new Random();
            int result = random.Next(10000000, 100000000);
            while (occupied.Contains(result))
            {
                result = random.Next(10000000, 100000000);
            }
            return result;
        }


        public void notifyUserOnNewPrivateMessage(String forumName, String sender, String addressee, String content)
        {
            ForumPolicy forumPolicy = DB.getforumByName(forumName).forumPolicy;
            List<String> currentlyLoggedInUsers = this.loggedInUsersByForum[forumName];
            if (currentlyLoggedInUsers == null) //|| !currentlyLoggedInUsers.Contains(addressee))
                return;
            List<IUserNotificationsService> addresseeChannels = this.getUserChannels(addressee);
            if (addresseeChannels != null && addresseeChannels.Count > 0)
            {
                foreach (IUserNotificationsService channel in addresseeChannels)
                {
                    channel.sendUserMessage(sender, content);
                }
            }
            else if (forumPolicy.notificationsType == ForumPolicy.OFFLINE_NOTIFICATIONS_TPYE)
            {
                DB.NotifyOfflineUser(forumName, addressee,
                    sender + "'s post you were following in " + forumName + " was deleted");
            }
        }

        public int getUserSessionKey(string username)
        {
            return Sessions[username];
        }


        public Boolean needsToAddQuestions (string userName, string forumName)
        {
            Forum forum = getForum(forumName);
            
            if(userName==null || forum == null)
            {
                return false;
            }
            bool isForumHaveQuestionsIdentifing = forum.forumPolicy.isQuestionIdentifying;
            List<string> userQuestions = DB.getAnswers(userName);
            if (userQuestions == null)
                return false;
            bool isUserHaveQuestionsIdentifing = !(userQuestions.Count == 2 && userQuestions.Contains(""));
            if (isForumHaveQuestionsIdentifing && !isUserHaveQuestionsIdentifing)
                return true;
            return false;
        }

        public bool setAnswers(string forumName, string userName, string ans1, string ans2)
        {
            if (ans1 == null || ans2 == null || ans1.Equals("") || ans2.Equals(""))
                return false;
            return DB.setAnswers(userName, ans1, ans2);
		}

            
        public List<string> getOfflineNotifications(String forumName, String userName, int sessionKey)
        {
            Forum forum = DB.getforumByName(forumName);
            if (forum == null || forum.forumPolicy.notificationsType != ForumPolicy.OFFLINE_NOTIFICATIONS_TPYE)
            {
                return new List<string>();
            }
            if (sessionKey == Sessions[userName])//authentication succeeded
                return DB.clearOfflineNotifications(forumName, userName);
            else 
                return new List<string>();
        }
        private List<IUserNotificationsService> getUserChannels(String userName)
        {
            List<IUserNotificationsService> channels = new List<IUserNotificationsService>();
            foreach (String chanName in channelsByLoggedInUsers.Keys)
            {
                if (chanName.Substring(0, chanName.IndexOf(",")).Equals(userName))
                {
                    channels.Add(channelsByLoggedInUsers[chanName]);
                }
            }
            return channels;
        }
    }
}
