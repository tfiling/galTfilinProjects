using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ForumBuilder.Common.DataContracts;



namespace ForumBuilder.Common.ServiceContracts
{
    [ServiceContract]
    public interface IPostManager
    {
        [OperationContract]
        String deletePost(Int32 postId, String deletingUser);

        [OperationContract]
        String addPost(String headLine, String content, String writerName, Int32 commentedPost);
        
        [OperationContract]
        List<PostData> getAllPosts(String forumName, String subforumName);

        [OperationContract]
        Boolean updatePost(int postID, String title, String content, String userName);
    }
}
