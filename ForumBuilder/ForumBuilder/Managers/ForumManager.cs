using ForumBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ForumBuilder.Common.ServiceContracts;
using ForumBuilder.Common.ClientServiceContracts;
using BL_Back_End;
using ForumBuilder.Common.DataContracts;

namespace Service
{
    public class ForumManager : IForumManager
    {
        private static ForumManager singleton;
        private IForumController forumController;

        private ForumManager()
        {
            forumController=ForumController.getInstance;
        }

        public static ForumManager getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new ForumManager();
                }
                return singleton;
            }
        }

        public Boolean dismissAdmin(String adminToDismissed, String dismissingUserName, String forumName)
        {
            return forumController.dismissAdmin(adminToDismissed, dismissingUserName, forumName);
        }
        public Boolean banMember(String bannedMember, String bannerUserName, String forumName)
        {
            return forumController.banMember(bannedMember, bannerUserName, forumName);
        }
        public String nominateAdmin(String newAdmin, String nominatorName, String forumName)
        {
            return forumController.nominateAdmin(newAdmin, nominatorName, forumName);
        }
        public String registerUser(String newUser, String password, String mail, String ans1, String ans2, string forumName)
        {
            return forumController.registerUser(newUser, password, mail, ans1, ans2, forumName);
        }

        public String login(String user, String forumName,string password)
        {
            return forumController.login(user, forumName, password);
        }

        public String loginBySessionKey(int sessionKey, String user, String forumName)
        {
            return forumController.loginBySessionKey(sessionKey, user, forumName);
        }

        public Boolean logout(String user, String forumName, String allSession)
        {
            /*OperationContext.Current.OperationCompleted += delegate(object sender, EventArgs e) 
            {
                OperationContext.Current.Channel.Close();
                OperationContext.Current.GetCallbackChannel<IUserNotificationsService>().
                };*/
            return forumController.logout(user, forumName,allSession);
        }

        public String addSubForum(String forumName, String name, Dictionary<String, DateTime> moderators, String userNameAdmin)
        {
            return forumController.addSubForum(forumName, name, moderators, userNameAdmin);
        }
        public Boolean isAdmin(String userName, String forumName)
        {
            return forumController.isAdmin(userName, forumName);
        }
        public Boolean isMember(String userName, String forumName)
        {
            return forumController.isMember(userName, forumName);
        }
        public String setForumPreferences(String forumName, String newDescription, ForumPolicyData fpd, String setterUserName)
        {
            ForumPolicy fp = new ForumPolicy(fpd.policy,fpd.isQuestionIdentifying,fpd.seniorityInForum,fpd.deletePostByModerator,
                fpd.timeToPassExpiration,fpd.minNumOfModerator,fpd.hasCapitalInPassword,fpd.hasNumberInPassword,fpd.minLengthOfPassword,
                fpd.notificationsType, fpd.selectiveNotificationsUsers);
            return forumController.setForumPreferences(forumName, newDescription, fp, setterUserName);
        }
        public String getForumPolicy(String forumName)
        {
            return forumController.getForumPolicy(forumName);
        }
        public String getForumDescription(String forumName)
        {
            return forumController.getForumDescription(forumName);
        }
       
        public ForumData getForum(String forumName)
        {
            Forum temp = forumController.getForum(forumName);
            if (temp == null)
            {
                return null;
            }
            ForumPolicyData fpd = new ForumPolicyData(temp.forumPolicy.policy, temp.forumPolicy.isQuestionIdentifying, temp.forumPolicy.seniorityInForum, temp.forumPolicy.deletePostByModerator,
                                                      temp.forumPolicy.timeToPassExpiration, temp.forumPolicy.minNumOfModerators, temp.forumPolicy.hasCapitalInPassword, temp.forumPolicy.hasNumberInPassword,
                                                      temp.forumPolicy.minLengthOfPassword, temp.forumPolicy.notificationsType, temp.forumPolicy.selectiveNotificationsUsers);
            ForumData toReturn = new ForumData(temp.forumName, temp.description, fpd , temp.subForums, temp.members);
            return toReturn;
            
        }
        public List<String> getForums()
        {
            return forumController.getForums();
        }
        public int getAdminReportNumOfPOst(String AdminName, String forumName)
        {
            return forumController.getAdminReportNumOfPOst( AdminName, forumName);
        }

        public List<PostData> getAdminReportPostOfmember(String AdminName, String forumName, String memberName)
        {
            List<Post> posts = forumController.getAdminReportPostOfmember(AdminName, forumName, memberName);
            List<PostData> postsData = new List<PostData>();
            foreach (Post p in posts)
            {
                postsData.Add(new PostData(p.writerUserName, p.id, p.title, p.content, p.parentId, p.timePublished));
            }
            return postsData;
        }

        public List<String> getAdminReport(String AdminName, String forumName)
        {
            return forumController.getAdminReport(AdminName, forumName);
        }

        public int getUserSessionKey(string username)
        {
            return forumController.getUserSessionKey(username);
        }


        public bool setAnswers(string forumName, string userName, string ans1, string ans2)
        {
            return forumController.setAnswers(forumName, userName, ans1, ans2);
        }


        public List<string> getOfflineNotifications(String forumName, String userName, int sessionKey)
        {
            return forumController.getOfflineNotifications(forumName, userName, sessionKey);
        }
        
    }
}
