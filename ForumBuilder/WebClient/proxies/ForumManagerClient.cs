using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ForumBuilder.Common.ServiceContracts;
using ForumBuilder.Common.DataContracts;

namespace WebClient.proxies
{
    public class ForumManagerClient : DuplexClientBase<IForumManager>, IForumManager
    {

        public ForumManagerClient(InstanceContext instanceContext) :
            base(instanceContext)
        {
        }
    
        public ForumManagerClient(InstanceContext instanceContext, string endpointConfigurationName) :
            base(instanceContext, endpointConfigurationName)
        {
        }

        public ForumManagerClient(InstanceContext instanceContext, string endpointConfigurationName, string remoteAddress) :
            base(instanceContext, endpointConfigurationName, remoteAddress)
        {
        }

        public ForumManagerClient(InstanceContext instanceContext, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(instanceContext, endpointConfigurationName, remoteAddress)
        {
        }

        public ForumManagerClient(InstanceContext instanceContext, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(instanceContext, binding, remoteAddress)
        {
        }

        public Boolean dismissAdmin(String adminToDismissed, String dismissingUserName, String forumName)
        {
            return Channel.dismissAdmin(adminToDismissed, dismissingUserName, forumName);
        }

        public Boolean banMember(String bannedMember, String bannerUserName, String forumName)
        {
            return Channel.banMember(bannedMember, bannerUserName, forumName);
        }

        public String nominateAdmin(String newAdmin, String nominatorName, String forumName)
        {
            return Channel.nominateAdmin(newAdmin, nominatorName, forumName);
        }

        public String registerUser(String newUser, String password, String mail,String ans1,String ans2, String forumName)
        {
            return Channel.registerUser(newUser, password, mail,ans1,ans2, forumName);
        }

        public String login(String user, String forumName, string password)
        {
            return Channel.login(user, forumName, password);
        }

        public String loginBySessionKey(int sessionKey, String user, String forumName)
        {
            return Channel.loginBySessionKey(sessionKey, user, forumName);
        }
        
        public Boolean logout(String user, String forumName, String allSession)
        {
            return Channel.logout(user, forumName, allSession);
        }

        public String addSubForum(String forumName, String name, Dictionary<String, DateTime> moderators, String userNameAdmin)
        {
            return Channel.addSubForum(forumName, name, moderators, userNameAdmin);
        }

        public Boolean isAdmin(String userName, String forumName)
        {
            return Channel.isAdmin(userName, forumName);
        }

        public Boolean isMember(String userName, String forumName)
        {
            return Channel.isMember(userName, forumName);
        }

        public String setForumPreferences(String forumName, String newDescription, ForumPolicyData fpd, String setterUserName)
        {
            return Channel.setForumPreferences(forumName, newDescription,  fpd, setterUserName);
        }

        public String getForumPolicy(String forumName)
        {
            return Channel.getForumPolicy(forumName);
        }

        public String getForumDescription(String forumName)
        {
            return Channel.getForumDescription(forumName);
        }

        public ForumData getForum(String forumName)
        {
            return Channel.getForum(forumName);
        }
        public List<String> getForums()
        {
            return Channel.getForums();
        }
        public int getAdminReportNumOfPOst(String AdminName, String forumName)
        {
            return Channel.getAdminReportNumOfPOst( AdminName,  forumName);
        }

        public List<PostData> getAdminReportPostOfmember(String AdminName, String forumName, String memberName)
        {
            return Channel.getAdminReportPostOfmember( AdminName,  forumName,  memberName);
        }

        public List<String> getAdminReport(String AdminName, String forumName)
        {
            return Channel.getAdminReport( AdminName, forumName);
        }

        public int getUserSessionKey(string username)
        {
            return Channel.getUserSessionKey(username);
        }


        public bool setAnswers(string forumName, string userName, string ans1, string ans2)
        {
            return Channel.setAnswers(forumName, userName, ans1, ans2);
        }
        
        public List<string> getOfflineNotifications(String forumName, String userName, int sessionKey)
        {
            return new List<string>();//this functionality is not required in the web client
        }
    }
}
