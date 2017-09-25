using System;
using System.Collections.Generic;
using BL_Back_End;

namespace ForumBuilder.Controllers
{
    public interface IPostController
    {
        String removeComment(int postId, String removerName);
        String addComment(String headLine, String content, String writerName, int commentedPost/*if new thread, -1*/);
        List<Post> getAllPosts(String forumName, String subforumName);
        Boolean updatePost(int postID, String title, String content, String userName);
    }
}
