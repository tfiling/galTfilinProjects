using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ForumBuilder.Common.ServiceContracts
{
    [ServiceContract]
    public interface ISubForumManager
    {
        [OperationContract]
        String dismissModerator(String dismissedModerator, String dismissByAdmin, String subForumName, String forumName);

        [OperationContract]
        String nominateModerator(String newModerator, String nominatorUser, DateTime date, String subForumName, String forumName);

        [OperationContract]
        String createThread(String headLine, String content, String writerName, String forumName, String subForumName);

        [OperationContract]
        String deleteThread(int firstPostId, string removerName);
                
        [OperationContract]
        Boolean isModerator(string name, string subForumName, string forumName);

    }
}
