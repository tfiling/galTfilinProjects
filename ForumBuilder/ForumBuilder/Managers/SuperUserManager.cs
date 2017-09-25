using ForumBuilder.Controllers;
using ForumBuilder.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL_Back_End;
using ForumBuilder.Common.ServiceContracts;
using ForumBuilder.Common.DataContracts;

namespace Service
{
    public class SuperUserManager :ISuperUserManager
    {
        private static SuperUserManager singleton;
        private ISuperUserController superUserController;

        private SuperUserManager()
        {
            superUserController = SuperUserController.getInstance;
        }

        public static SuperUserManager getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new SuperUserManager();
                }
                return singleton;
            }
        }

        public String createForum(String forumName, String descrption, ForumPolicyData fpd, List<String> administrators, String superUserName)
        {
            return superUserController.createForum(forumName, descrption, new ForumPolicy(fpd.policy,
                fpd.isQuestionIdentifying, fpd.seniorityInForum, fpd.deletePostByModerator,
                    fpd.timeToPassExpiration, fpd.minNumOfModerator, fpd.hasCapitalInPassword, fpd.hasNumberInPassword,
                    fpd.minLengthOfPassword, 0, new List<string>()), administrators, superUserName);
        }

        public Boolean login(String user, String forumName, string email)
        {
            return superUserController.login(user, forumName, email);
        }

        public Boolean isSuperUser(string userName)
        {
            return superUserController.isSuperUser(userName);
        }
        public int SuperUserReportNumOfForums(string superUserName)
        {
            return superUserController.SuperUserReportNumOfForums(superUserName);
        }
        public List<String> getSuperUserReportOfMembers(string superUserName)
        {
            return superUserController.getSuperUserReportOfMembers(superUserName);
        }

        public String addUser(string userName, string password, string mail, string superUserName)
        {
            return superUserController.addUser(userName,  password,  mail,  superUserName);
        }
    }
}
