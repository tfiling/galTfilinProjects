using ForumBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForumBuilder.Common.ServiceContracts;


namespace Service
{
    public class SubForumManager :ISubForumManager
    {
        private static SubForumManager singleton;
        private ISubForumController subForumController;

        private SubForumManager()
        {
            subForumController = SubForumController.getInstance;
        }

        public static SubForumManager getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new SubForumManager();
                }
                return singleton;
            }
        }

        public String dismissModerator(String dismissedModerator, String dismissByAdmin, String subForumName, String forumName)
        {
            return subForumController.dismissModerator(dismissedModerator, dismissByAdmin, subForumName, forumName);
        }
        public String nominateModerator(String newModerator, String nominatorUser, DateTime date, String subForumName, String forumName)
        {
            return subForumController.nominateModerator(newModerator, nominatorUser, date, subForumName, forumName);
        }
        public String createThread(String headLine, String content, String writerName, String forumName, String subForumName)
        {
            return subForumController.createThread(headLine, content, writerName, forumName, subForumName);
        }
        public String deleteThread(int firstPostId, string removerName)
        {
            return subForumController.deleteThread(firstPostId, removerName);
        }
        public Boolean isModerator(string name, string subForumName, string forumName)
        {
            return subForumController.isModerator(name, subForumName, forumName);
        }
    }
}
