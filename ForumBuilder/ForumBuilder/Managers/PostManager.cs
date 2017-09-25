using ForumBuilder.Controllers;
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
    public class PostManager :IPostManager
    {
        private static PostManager singleton;
        private IPostController postController;


        private PostManager()
        {
            postController = PostController.getInstance;
        }

        public static PostManager getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new PostManager();
                }
                return singleton;
            }
        }

        public String deletePost(Int32 postId, String deletingUser)
        {
            return postController.removeComment(postId, deletingUser);
        }
        public String addPost(String headLine, String content, String writerName, Int32 commentedPost)
        {
            return postController.addComment(headLine, content, writerName, commentedPost);
        }
        public List<PostData> getAllPosts(String forumName, String subforumName)
        {
            List<Post> posts= postController.getAllPosts(forumName, subforumName);
            List<PostData> postsData = new List<PostData>();
            foreach (Post p in posts)
            {
                postsData.Add(new PostData(p.writerUserName, p.id, p.title, p.content, p.parentId, p.timePublished));
            }
            return postsData;
        }
        public Boolean updatePost(int postID, String title, String content, String userName)
        {
            return postController.updatePost(postID, title, content, userName);
        }
    }
}
