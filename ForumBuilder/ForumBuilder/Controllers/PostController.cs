using System;
using System.Collections.Generic;

using System.Linq;
using BL_Back_End;
using Database;
namespace ForumBuilder.Controllers
{
    public class PostController :IPostController
    {
        private static PostController singleton;
        DBClass DB = DBClass.getInstance;
        ForumController forumController = ForumController.getInstance;
        Systems.Logger logger = Systems.Logger.getInstance;
        public static Object sysLock = new Object();

        public static PostController getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new PostController();
                    Systems.Logger.getInstance.logPrint("Post contoller created",0);
                    Systems.Logger.getInstance.logPrint("Post contoller created",2);
                }
                return singleton;
            }
        }
        private SubForum getSubforumByPost(int postId)
        {
            Post p = getPost(postId);
            if (p == null)
                return null;
            while (p.parentId != -1)
            {
                p = getPost(p.parentId);
            }
            Thread t= DB.getThreadByFirstPostId(p.id);
            return DB.getSubforumByThreadFirstPostId(p.id);
        }
        private Post getPost(int postId)
        {
            return DB.getPost(postId);
        }

        public String addComment(String headLine, String content, String writerName, int commentedPost)
        {
            lock (sysLock)
            {
                SubForum sf = getSubforumByPost(commentedPost);
                if (getPost(commentedPost) == null)
                {
                    logger.logPrint("Add comment failed, there is no post to comment at", 0);
                    logger.logPrint("Add comment failed, there is no post to comment at", 2);
                    return "Add comment failed, there is no post to comment at";
                }
                if (headLine.Equals("") && content.Equals(""))
                {
                    logger.logPrint("Add comment failed, there is no head or content in tread", 0);
                    logger.logPrint("Add comment failed, there is no head or content in tread", 2);
                    return "Add comment failed, there is no head or content in tread";
                }
                else if (DB.getUser(writerName) == null)
                {
                    logger.logPrint("Add comment failed, user does not exist", 0);
                    logger.logPrint("Add comment failed, user does not exist", 2);
                    return "Add comment failed, user does not exist";
                }
                else if (sf == null || !ForumController.getInstance.isMember(writerName, sf.forum))
                {
                    logger.logPrint("Add comment failed, user is not a member in forum", 0);
                    logger.logPrint("Add comment failed, user is not a member in forum", 2);
                    return "Add comment failed, user is not a member in forum";
                }
                else
                {
                    int id = DB.getAvilableIntOfPost();
                    logger.logPrint("Create comment " + id + " to " + commentedPost, 0);
                    logger.logPrint("Create comment " + id + " to " + commentedPost, 1);
                    if (DB.addPost(writerName, id, headLine, content, commentedPost, DateTime.Now, sf.forum))
                    {
                        this.forumController.sendThreadCreationNotification(headLine, content, writerName, sf.forum, sf.name);
                        return "comment created";
                    }
                    return "Create comment failed";
                }
            }
        }
        public String removeComment(int postId, String removerName)
        {
            if (getPost(postId) == null)
            {
                logger.logPrint("Delete comment failed, there is no post with that id",0);
                logger.logPrint("Delete comment failed, there is no post with that id",2);
                return "Delete comment failed, there is no post with that id";
            }
            else if (getPost(postId).parentId == -1)
            {
                //logger.logPrint("Delete comment failed, this is not a comment");
                //return false;
                if(SubForumController.getInstance.deleteThread(postId, removerName).Equals("Thread removed"))
                    return "Post removed";
                return "Post remove failed";
            }
            SubForum sf = getSubforumByPost(postId);
            if ((!DB.getPost(postId).writerUserName.Equals(removerName))
                && (DB.getSuperUser(removerName)==null)
                && !(ForumController.getInstance.isAdmin(removerName, sf.forum))
                && !(DB.getforumByName(sf.forum).forumPolicy.deletePostByModerator && SubForumController.getInstance.isModerator(removerName, sf.name, sf.forum)))
            {
                logger.logPrint("Delete post failed, there is no permission to that user", 0);
                logger.logPrint("Delete post failed, there is no permission to that user", 2);
                return "Delete post failed, there is no permission to that user";
            }
//user notification mechanism
            List<String> usersToBeNotifiedForThreadDelition = new List<String>();
            Post deletedPost = getPost(postId);
            List<Post> siblingPosts = DB.getRelatedPosts(deletedPost.parentId);
            foreach (Post post in siblingPosts)
            {
                if (!usersToBeNotifiedForThreadDelition.Contains(post.writerUserName))
                {
                    usersToBeNotifiedForThreadDelition.Add(post.writerUserName);
                }
            }
            foreach (String username in usersToBeNotifiedForThreadDelition)
            {
                this.forumController.sendPostDelitionNotification(sf.forum, deletedPost.writerUserName, username, true);
            }
            /////////////////////////

            //find the posts that have to delete
            List<Post> donePosts = new List<Post>();
            List<Post> undonePosts = new List<Post>();
            undonePosts.Add(DB.getPost(postId));
            while (undonePosts.Count != 0)
            {
                Post post = undonePosts.ElementAt(0);
                undonePosts.RemoveAt(0);
                List<Post> related = DB.getRelatedPosts(post.id);
                while (related != null && related.Count != 0)
                {
                    undonePosts.Add(related.ElementAt(0));
                    related.RemoveAt(0);
                }
                donePosts.Add(post);
            }
            bool hasSucceed = true;
            for (int i = donePosts.Count - 1; i >= 0; i--)
            {
                hasSucceed = hasSucceed && DB.removePost(donePosts.ElementAt(i).id);
                logger.logPrint("Remove post " +donePosts.ElementAt(i).id,0);
                logger.logPrint("Remove post " + donePosts.ElementAt(i).id,1);
            }
            //ForumController.getInstance.sendPostDelitionNotification(DB.getPost(postId).forumName, removerName);
            this.forumController.sendPostDelitionNotification(sf.forum, deletedPost.writerUserName, null, false);
            return "Post removed";
        }
        public List<Post> getAllPosts(String forumName, String subforumName)
        {
            lock (sysLock)
            {
                List<Post> list = new List<Post>();
                SubForum sf = SubForumController.getInstance.getSubForum(subforumName, forumName);
                if (sf.threads == null)
                {
                    return new List<Post>();
                }
                foreach (int t in sf.threads)
                {
                    List<Post> donePosts = new List<Post>();
                    List<Post> undonePosts = new List<Post>();
                    undonePosts.Add(DB.getPost(t));
                    while (undonePosts.Count != 0)
                    {
                        Post post = undonePosts.ElementAt(0);
                        undonePosts.RemoveAt(0);
                        List<Post> related = DB.getRelatedPosts(post.id);
                        while (related != null && related.Count != 0)
                        {
                            undonePosts.Add(related.ElementAt(0));
                            related.RemoveAt(0);
                        }
                        donePosts.Add(post);
                    }
                    foreach (Post p in donePosts)
                    {
                        list.Add(p);
                    }
                }
                return list;
            }
        }
        public Boolean updatePost(int postID, String title, String content,String userName)
        {
            Post p = DB.getPost(postID);
            if (p == null)
            {
                logger.logPrint("update post failed, post id hasnt post",0);
                logger.logPrint("update post failed, post id hasnt post",2);
                return false;
            }
            if (!p.writerUserName.Equals(userName))
            {
                logger.logPrint("update post failed, user is not allowed to update post",0);
                logger.logPrint("update post failed, user is not allowed to update post",2);
                return false;
            }
            else
            {

        //user notification mechanism
                List<String> usersToBeNotifiedForThreadModification = new List<String>();
                Post modifiedPost = getPost(postID);
                List<Post> siblingPosts = DB.getRelatedPosts(modifiedPost.parentId);
                if (modifiedPost.parentId == -1)
                    siblingPosts = DB.getRelatedPosts(modifiedPost.id);
                foreach (Post post in siblingPosts)
                {
                    if (!usersToBeNotifiedForThreadModification.Contains(post.writerUserName))
                    {
                        usersToBeNotifiedForThreadModification.Add(post.writerUserName);
                    }
                }
                foreach (String username in usersToBeNotifiedForThreadModification)
                {
                    this.forumController.sendPostModificationNotification(modifiedPost.forumName, modifiedPost.writerUserName, modifiedPost.title, username);
                }
                this.forumController.sendPostDelitionNotification(modifiedPost.forumName, modifiedPost.writerUserName, null, false);
        /////////////////////////
                return DB.updatePost(postID, title, content);
            }
        }
    }
}
