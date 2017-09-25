using System;
using BL_Back_End;
namespace ForumBuilder.Controllers
{
    public interface ISubForumController
    {
        String dismissModerator(String dismissedModerator, String dismissByAdmin, string subForumName, string forumName);
        String nominateModerator(String newModerator, String nominatorUser, DateTime date, string subForumName, string forumName);
        Boolean isModerator(string name, string subForumName, string forumName);
        SubForum getSubForum(string subForumName, string forumName);
        String createThread(String headLine, String content, String writerName, String forumName, String subForumName);
        String deleteThread(int firstPostId, string removerName);
    }
}
