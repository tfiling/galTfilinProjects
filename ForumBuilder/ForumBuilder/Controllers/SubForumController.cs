using System;
using ForumBuilder.Controllers;
using System.Collections.Generic;
using System.Linq;
using ForumBuilder.Systems;
using BL_Back_End;
using Database;

namespace ForumBuilder.Controllers
{
    public class SubForumController : ISubForumController
    {
        private static SubForumController singleton;
        ForumController forumController = ForumController.getInstance;
        DBClass DB = DBClass.getInstance;
        Logger logger = Logger.getInstance;
        public static Object sysLock = new Object();


        public static SubForumController getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new SubForumController();
                    Logger.getInstance.logPrint("Sub-forum contoller created",0);
                    Logger.getInstance.logPrint("Sub-forum contoller created",1);
                }
                return singleton;
            }
        }
        public String dismissModerator(string dismissedModerator, string dismissByAdmin, string subForumName, string forumName)
        {
            SubForum subForum = getSubForum(subForumName, forumName);
            if (subForum == null)
            {
                logger.logPrint("Dismiss moderator failed, sub-forum does not exist",0);
                logger.logPrint("Dismiss moderator failed, sub-forum does not exist",2);
                return "Dismiss moderator failed, sub-forum does not exist";
            }
            if (DB.getforumByName(subForum.forum).forumPolicy.minNumOfModerators >= subForum.moderators.Count|| subForum.moderators.Count==1)
            {
                logger.logPrint("Dismiss moderator failed, sub-forum has not enough moderators",0);
                logger.logPrint("Dismiss moderator failed, sub-forum has not enough moderators",2);
                return "Dismiss moderator failed, sub-forum has not enough moderators";
            }
            else if (!ForumController.getInstance.isAdmin(dismissByAdmin, forumName) && !SuperUserController.getInstance.isSuperUser(dismissByAdmin))
            {
                logger.logPrint("Dismiss moderator failed, "+ dismissByAdmin+" has no permission",0);
                logger.logPrint("Dismiss moderator failed, " + dismissByAdmin + " has no permission",2);
                return "Dismiss moderator failed, " + dismissByAdmin + " has no permission";
            }
            else if(!isModerator(dismissedModerator, subForumName, forumName))
            {
                logger.logPrint("Dismiss moderator failed, " + dismissedModerator + " is not a moderator",0);
                logger.logPrint("Dismiss moderator failed, " + dismissedModerator + " is not a moderator",2);
                return "Dismiss moderator failed, " + dismissedModerator + " is not a moderator";
            }
            else
            {
                logger.logPrint("Dismiss moderator "+ dismissedModerator,0);
                logger.logPrint("Dismiss moderator " + dismissedModerator,2);
                if (DB.dismissModerator(dismissedModerator, subForumName, forumName))
                    return "Moderator dismissed";
                return "Dismiss failed";
            }
        }
        public bool isModerator(string name, string subForumName, string forumName)
        {
            SubForum subForum = getSubForum(subForumName, forumName);
            if (subForum == null)
                return false;
            foreach(string s in subForum.moderators.Keys)
            {
                if (s.Equals(name))
                    return true;
            }
            return false;
        }
        public String nominateModerator(string newModerator, string nominatorUser, DateTime enddate, string subForumName, string forumName)
        {
            SubForum subForum = getSubForum(subForumName, forumName);
            if (subForum == null)
            {
                logger.logPrint("sub forum does not exist",0);
                logger.logPrint("sub forum does not exist",2);
                return "sub forum does not exist";
            }
            if (DB.getUser(newModerator) == null)
            {
                logger.logPrint("user does not exist", 0);
                logger.logPrint("user does not exist", 2);
                return "user does not exist";
            }
            if (isModerator(newModerator, subForumName, forumName))
            {
                logger.logPrint("user is already moderator", 0);
                logger.logPrint("user is already moderator", 2);
                return "user is already moderator";
            }
            if ((ForumController.getInstance.isAdmin(nominatorUser, forumName)|| SuperUserController.getInstance.isSuperUser(nominatorUser)) && 
                ForumController.getInstance.isMember(newModerator, forumName)&&
                DB.getforumByName(forumName).forumPolicy.seniorityInForum <= (DateTime.Today - DB.getUser(newModerator).date).Days)
            {
                if (DateTime.Now.CompareTo(enddate) > 0)
                {
                    logger.logPrint("the date in nominate moderator already past",0);
                    logger.logPrint("the date in nominate moderator already past",2);
                    return "the date in nominate moderator already past";
                }
                if (DB.nominateModerator(newModerator, enddate, subForumName,forumName,nominatorUser))
                {
                    logger.logPrint("nominate moderator " + newModerator + "success",0);
                    logger.logPrint("nominate moderator " + newModerator + "success",1);
                    return "nominate moderator succeed";
                }
            }
            if(!ForumController.getInstance.isAdmin(nominatorUser, forumName)&&!SuperUserController.getInstance.isSuperUser(nominatorUser)){
                logger.logPrint("nominate Moderator fail, To " + nominatorUser+" has no permission to nominate moderator",0);
                logger.logPrint("nominate Moderator fail, To " + nominatorUser+" has no permission to nominate moderator",2);
                return "nominateModerator fail, To " + nominatorUser + " has no permission to nominate moderator";
                }
            if(!ForumController.getInstance.isMember(newModerator, forumName)){
                logger.logPrint("nominate Moderator fail, To " + newModerator + " has no permission to be moderator, he is not a member",0);
                logger.logPrint("nominate Moderator fail, To " + newModerator + " has no permission to be moderator, he is not a member",2);
                return "nominateModerator fail, To " + newModerator + " has no permission to be moderator, he is not a member";
                }
            if(DB.getforumByName(forumName).forumPolicy.seniorityInForum > (DateTime.Today - DB.getUser(newModerator).date).Days){
                logger.logPrint("nominate Moderator fail, To " + newModerator + " has not enough seniority",0);
                logger.logPrint("nominate Moderator fail, To " + newModerator + " has not enough seniority",2);
                return "nominate Moderator fail, To " + newModerator + " has not enough seniority";
                }
            return "nominate Moderator fail";
        }
        public SubForum getSubForum(string subForumName, string forumName)
        {
            return DB.getSubForum(subForumName, forumName);
        }

        public String createThread(String headLine, String content, String writerName,  String forumName, String subForumName)
        {
            lock (PostController.sysLock)
            {
                DateTime timePublished = DateTime.Now;
                if (headLine == null || content == null || (headLine.Equals("") && content.Equals("")))
                {
                    logger.logPrint("Create tread failed, there is no head or content in tread", 0);
                    logger.logPrint("Create tread failed, there is no head or content in tread", 2);
                    return "Create tread failed, there is no head or content in tread";
                }
                else if (DB.getUser(writerName) == null)
                {
                    logger.logPrint("Create tread failed, user does not exist", 0);
                    logger.logPrint("Create tread failed, user does not exist", 2);
                    return "Create tread failed, user does not exist";
                }
                else if (DB.getSubForum(subForumName, forumName) == null)
                {
                    logger.logPrint("Create tread failed, sub-forum does not exist", 0);
                    logger.logPrint("Create tread failed, sub-forum does not exist", 2);
                    return "Create tread failed, sub-forum does not exist";
                }
                else if (!ForumController.getInstance.isMember(writerName, forumName))
                {
                    logger.logPrint("Create tread failed, user " + writerName + " is not memberin forum " + forumName, 0);
                    logger.logPrint("Create tread failed, user " + writerName + " is not memberin forum " + forumName, 2);
                    return "Create tread failed, user " + writerName + " is not memberin forum " + forumName;
                }
                int id = DB.getAvilableIntOfPost();
                logger.logPrint("Add thread " + id, 0);
                logger.logPrint("Add thread " + id, 1);
                if (DB.addPost(writerName, id, headLine, content, -1, timePublished, forumName) && DB.addThread(forumName, subForumName, id))
                {
                    this.forumController.sendThreadCreationNotification(headLine, content, writerName, forumName, subForumName);
                    return "Create tread succeed";
                }
                return "Create tread failed";
            }
        }

        public String deleteThread(int firstPostId,string removerName)
        {
            if (DB.getThreadByFirstPostId(firstPostId) == null)
            {
                logger.logPrint("Delete thread failed, no thread with that id",0);
                logger.logPrint("Delete thread failed, no thread with that id",2);
                return "Delete thread failed, no thread with that id";
            }
            SubForum sf= DB.getSubforumByThreadFirstPostId(firstPostId);
            if ((!DB.getPost(firstPostId).writerUserName.Equals(removerName))
                &&(!SuperUserController.getInstance.isSuperUser(removerName))
                && (!ForumController.getInstance.isAdmin(removerName, sf.forum)
                && !(DB.getforumByName(sf.forum).forumPolicy.deletePostByModerator&&isModerator(removerName, sf.name, sf.forum))))
            {
                logger.logPrint("Delete post failed, there is no permission to that user", 0);
                logger.logPrint("Delete post failed, there is no permission to that user", 2);
                return "Delete post failed, there is no permission to that user";
            }
            else
            {
                List<Post> donePosts = new List<Post>();
                List<Post> undonePosts = new List<Post>();
                undonePosts.Add(DB.getPost(firstPostId));
                Post deletedPost = undonePosts[0];
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
                List<String> usersToBeNotifiedForThreadDelition = new List<String>();
                DB.removeThread(firstPostId);
                for (int i =donePosts.Count-1; i>=0;i--)
                {
                    usersToBeNotifiedForThreadDelition.Add(donePosts.ElementAt(i).writerUserName);                
                    DB.removePost(donePosts.ElementAt(i).id);
                    logger.logPrint("Remove post " + donePosts.ElementAt(i).id,0);
                    logger.logPrint("Remove post " + donePosts.ElementAt(i).id,1);
                }
                foreach (String username in usersToBeNotifiedForThreadDelition)
                {
                    this.forumController.sendPostDelitionNotification(sf.forum, deletedPost.writerUserName, username, true);
                }
                this.forumController.sendPostDelitionNotification(sf.forum, deletedPost.writerUserName, null, false);
                logger.logPrint("Remove thread " + firstPostId,0);
                logger.logPrint("Remove thread " + firstPostId,1);
                return "Thread removed";
            } 
        }
    }
}
