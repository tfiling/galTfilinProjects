using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForumBuilder.Controllers;
using System.Collections.Generic;
using System.Linq;
using BL_Back_End;
using Database;
using ForumBuilder.Systems;

namespace Tests
{
    [TestClass]
    public class SubForumControllerTest
    {
        private IForumController forumController;
        private IPostController postController;
        private Forum forum;
        private User userNonMember;
        private User userMember;
        private User userModerator;
        private User userModerator2;
        private User userAdmin;
        private User superUser;
        private ISubForumController subForum;
        private String subForumName = "subforum";
        private String forumName = "testForum";
        private User superUser1;


        [TestInitialize]
        public void setUp()
        {
            DBClass db = DBClass.getInstance;
            db.clear();
            ForumSystem.initialize("tomer", "1qW", "fkfkf@wkk.com");
            this.superUser = new User("tomer", "1qW", "fkfkf@wkk.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            SuperUserController.getInstance.addSuperUser(this.superUser.email, this.superUser.password, this.superUser.userName);
            this.forumController = ForumController.getInstance;
            this.postController = PostController.getInstance;
            ISuperUserController superUser = SuperUserController.getInstance;
            this.userNonMember = new User("nonMem", "nonMempass1", "nonmem@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userMember = new User("mem", "Mempass1", "mem@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userModerator = new User("mod", "Modpass1", "mod@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userModerator2 = new User("mod2", "Modpass1", "mod2@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userAdmin = new User("admin", "Adminpass1", "admin@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            superUser.addUser("admin", "Adminpass1", "admin@gmail.com", "tomer");
            Dictionary<String, DateTime> modList = new Dictionary<String, DateTime>();
            modList.Add(this.userModerator.userName, new DateTime(2030, 1, 1));
            modList.Add(this.userModerator2.userName, new DateTime(2030, 1, 1));
            List<string> adminList = new List<string>();
            adminList.Add("admin");
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            this.forum = new Forum(this.forumName, "descr",fp, adminList);
            superUser1 = DBClass.getInstance.getSuperUser("tomer");
            SuperUserController.getInstance.addSuperUser("fkfkf@wkk.com", "1qW", "tomer");
            superUser.createForum("1", "1", fp, null, "tomer");
            Assert.IsTrue(superUser.createForum("testForum", "descr", fp, adminList, "tomer").Equals("Forum " + "testForum" + " creation success"));
            //Assert.IsTrue(this.forumController.registerUser("admin", "adminpass", "admin@gmail.com", this.forumName));
            Assert.IsTrue(this.forumController.registerUser("mem", "Mempass1", "mem@gmail.com", "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            Assert.IsTrue(this.forumController.registerUser("mod", "Modpass1", "mod@gmail.com", "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            Assert.IsTrue(this.forumController.registerUser(userModerator2.userName, userModerator2.password, userModerator2.email, "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            Assert.IsTrue(this.forumController.addSubForum(this.forum.forumName, this.subForumName, modList, this.userAdmin.userName).Equals("sub-forum added"));
            this.subForum = SubForumController.getInstance;

        }

        [TestCleanup]
        public void cleanUp()
        {
            DBClass db = DBClass.getInstance;
            db.clear();
            this.forumController = null;
            this.forum = null;
            this.userNonMember = null;
            this.userMember = null;
            this.userModerator = null;
            this.userAdmin = null;
            this.subForum = null;
        }

        /******************************dismiss moderator***************************************/

        [TestMethod]
        public void test_dismissModerator_on_valid_moderator()
        {
            String userModeratorName = this.userModerator2.userName;
            Assert.IsTrue(this.subForum.isModerator(userModeratorName, this.subForumName, this.forumName), "user moderator should be a moderator");
            Assert.AreEqual("Moderator dismissed", this.subForum.dismissModerator(userModeratorName, this.userAdmin.userName, this.subForumName, this.forumName));
            Assert.IsFalse(this.subForum.isModerator(userModeratorName, this.subForumName, this.forumName), "user moderator should not be a moderator after his dismissal");
        }

        [TestMethod]
        public void test_dismissModerator_on_non_moderator()
        {
            String memberUserName = this.userMember.userName;
            Assert.IsFalse(this.subForum.isModerator(memberUserName, this.subForumName, this.forumName), "the moderatorList list should not contain the non moderator member");
            Assert.IsFalse(subForum.dismissModerator(memberUserName, this.userAdmin.userName, this.subForumName, this.forumName).Equals("Moderator dismissed"), "dismiss moderator on non moderator should return false");
            Assert.IsFalse(this.subForum.isModerator(memberUserName, this.subForumName, this.forumName), "the moderatorList list should not contain the non moderator member");
        }

        [TestMethod]
        public void test_dismissModerator_on_null()
        {
            Assert.IsFalse(subForum.dismissModerator(null, this.userAdmin.userName, this.subForumName, this.forumName).Equals("Moderator dismissed"), "dismiss moderator on null should return false");
        }

        [TestMethod]
        public void test_dismissModerator_on_empty_string()
        {

            Assert.IsFalse(subForum.dismissModerator("", this.userAdmin.userName, this.subForumName, this.forumName).Equals("Moderator dismissed"), "dismiss moderator on an empty string should return false");
        }


        /******************************end of dismiss moderator***************************************



        /******************************create thread***************************************

        [TestMethod]
        public void test_createThread_on_valid_input()
        {
            List<IThread> threadsPriorAddition = this.subForum.getThreads();
            Assert.IsTrue(this.subForum.createThread("good headline", "great content"), "the addition of thread with valid input should be successful");
            List<IThread> threadsAfterAddition = this.subForum.getThreads();
            Assert.IsTrue((threadsPriorAddition.Count == threadsAfterAddition.Count + 1), "thread list size should increase by 1");
            Assert.IsTrue((threadsAfterAddition.Except(threadsPriorAddition).Count() == 1));
        }

        [TestMethod]
        public void test_createThread_on_empty_headline()
        {
            List<IThread> threadsPriorAddition = this.subForum.getThreads();
            Assert.IsTrue(this.subForum.createThread("", "great content"), "the addition of thread with an empty headline should be successful");
            List<IThread> threadsAfterAddition = this.subForum.getThreads();
            Assert.IsTrue((threadsPriorAddition.Count == threadsAfterAddition.Count + 1), "thread list size should increase by 1");
            Assert.IsTrue((threadsAfterAddition.Except(threadsPriorAddition).Count() == 1));
        }

        [TestMethod]
        public void test_createThread_on_empty_content()
        {
            List<IThread> threadsPriorAddition = this.subForum.getThreads();
            Assert.IsTrue(this.subForum.createThread("good headline", ""), "the addition of thread with an empty content string should be successful");
            List<IThread> threadsAfterAddition = this.subForum.getThreads();
            Assert.IsTrue((threadsPriorAddition.Count == threadsAfterAddition.Count + 1), "thread list size should increase by 1");
            Assert.IsTrue((threadsAfterAddition.Except(threadsPriorAddition).Count() == 1));
        }

        [TestMethod]
        public void test_createThread_on_empty_string_inputs()
        {
            List<IThread> threadsPriorAddition = this.subForum.getThreads();
            Assert.IsFalse(this.subForum.createThread("", ""), "the addition of thread with an empty string inputs should be unsuccessful");
            List<IThread> threadsAfterAddition = this.subForum.getThreads();
            Assert.IsTrue((threadsPriorAddition.Count == threadsAfterAddition.Count), "thread list size should not increase");
            Assert.IsTrue((threadsAfterAddition.Except(threadsPriorAddition).Count() == 0), "thread list should stay the same");
        }

        [TestMethod]
        public void test_createThread_on_null_headline()
        {
            List<IThread> threadsPriorAddition = this.subForum.getThreads();
            Assert.IsFalse(this.subForum.createThread(null, "great content"), "the addition of thread with a null headline should be unsuccessful");
            List<IThread> threadsAfterAddition = this.subForum.getThreads();
            Assert.IsTrue((threadsPriorAddition.Count == threadsAfterAddition.Count), "thread list size should stay the same");
            Assert.IsTrue((threadsAfterAddition.Except(threadsPriorAddition).Count() == 0), "thread list should stay the same");
        }

        [TestMethod]
        public void test_createThread_on_null_content()
        {
            List<IThread> threadsPriorAddition = this.subForum.getThreads();
            Assert.IsTrue(this.subForum.createThread("good headline", null), "the addition of thread with a null content should be unsuccessful");
            List<IThread> threadsAfterAddition = this.subForum.getThreads();
            Assert.IsTrue((threadsPriorAddition.Count == threadsAfterAddition.Count), "thread list size should stay the same");
            Assert.IsTrue((threadsAfterAddition.Except(threadsPriorAddition).Count() == 0), "thread list should stay the same");
        }

        [TestMethod]
        public void test_createThread_on_null_inputs()
        {
            List<IThread> threadsPriorAddition = this.subForum.getThreads();
            Assert.IsFalse(this.subForum.createThread(null, null), "the addition of thread with null as inputs be unsuccessful");
            List<IThread> threadsAfterAddition = this.subForum.getThreads();
            Assert.IsTrue((threadsPriorAddition.Count == threadsAfterAddition.Count), "thread list size should not increase");
            Assert.IsTrue((threadsAfterAddition.Except(threadsPriorAddition).Count() == 0), "thread list should stay the same");
        }


        /******************************end of create thread***************************************/


        /******************************nominate moderator***************************************/

        [TestMethod]
        public void test_nominateModerator_on_member()
        {
            String memberName = this.userMember.userName;
            Assert.IsTrue(this.subForum.nominateModerator(memberName, this.userAdmin.userName, new DateTime(2030, 1, 1), this.subForumName, this.forumName).Equals("nominate moderator succeed"), "nomination of member user should be successful");
            Assert.IsTrue(this.subForum.isModerator(memberName, this.subForumName, this.forumName), "member should be moderator after his successful numonation");
        }

        [TestMethod]
        public void test_nominateModerator_on_moderator()
        {
            String moderatorName = this.userModerator.userName;
            Assert.IsFalse(this.subForum.nominateModerator(moderatorName, this.userAdmin.userName, new DateTime(2030, 1, 1), this.subForumName, this.forumName).Equals("nominate moderator succeed"), "nomination of moderator that already exists should not be successful");
            Assert.IsTrue(this.subForum.isModerator(moderatorName, this.subForumName, this.forumName), "moderator user should still br a moderator");
        }

        [TestMethod]
        public void test_nominateModerator_on_null()
        {
            Assert.IsFalse(this.subForum.nominateModerator(null, this.userAdmin.userName, new DateTime(2030, 1, 1), this.subForumName, this.forumName).Equals("nominate moderator succeed"), "nomination of null should not be successful");
        }

        [TestMethod]
        public void test_nominateModerator_on_empty_string_name()
        {
            Assert.IsFalse(this.subForum.nominateModerator("", this.userAdmin.userName, new DateTime(2030, 1, 1), this.subForumName, this.forumName).Equals("nominate moderator succeed"), "nomination with empty string as name should not be successful");
        }
        
        [TestMethod]
        public void test_nominateModerator_when_seniority_to_short()
        {
            ForumPolicy newfp = new ForumPolicy("p", true, 2, true, 180, 1, true, true, 5, 0, new List<string>());
            Assert.IsTrue(forumController.setForumPreferences(this.forumName, "new Des", newfp, userAdmin.userName).Equals("preferences had changed successfully"));
            this.userMember = new User("mem", "Mempass1", "mem@gmail.com", new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day));
            Assert.IsFalse(this.subForum.nominateModerator(this.userMember.userName, this.userAdmin.userName, new DateTime(2030, 1, 1), this.subForumName, this.forumName).Equals("nominate moderator succeed"), "nomination with empty string as name should not be successful");
        }

        [TestMethod]
        public void test_nominateModerator_not_enough_moderators()
        {
            ForumPolicy newfp = new ForumPolicy("p", true, 2, true, 180, 2, true, true, 5, 0, new List<string>());
            Assert.IsTrue(forumController.setForumPreferences(this.forumName, "new Des", newfp, userAdmin.userName).Equals("preferences had changed successfully"));
            this.userMember = new User("mem", "Mempass1", "mem@gmail.com", new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day));
            Assert.IsFalse(this.subForum.nominateModerator(this.userMember.userName, this.userAdmin.userName, new DateTime(2030, 1, 1), this.subForumName, this.forumName).Equals("nominate moderator succeed"), "nomination with empty string as name should not be successful");
        }



        /******************************end of nominate moderator***************************************/

        /******************************get moderators***************************************

        [TestMethod]
        public void test_getModerators_check_different_list_instances()
        {
            List<String> moderatorList1 = this.subForum.getModerators();
            List<String> moderatorList2 = this.subForum.getModerators();
            Assert.IsFalse(Object.Equals(moderatorList1, moderatorList2), "get moderators should not return a reference to it's inner moderator list");
        }

        [TestMethod]
        public void test_getModerators_check_different_list_elements_instances()
        {
            List<String> moderatorList1 = this.subForum.getModerators();
            List<String> moderatorList2 = this.subForum.getModerators();
            foreach (String s1 in moderatorList1)
            {
                foreach (String s2 in moderatorList2)
                {
                    Assert.IsFalse(Object.Equals(s1, s2), "get moderators should not return a reference to it's inner moderator list");
                }
            }
        }

        [TestMethod]
        public void test_getModerators_check_result_consistent()
        {
            List<String> moderatorList1 = this.subForum.getModerators();
            List<String> moderatorList2 = this.subForum.getModerators();
            Assert.IsTrue(areListsEqual<String>(moderatorList1, moderatorList2), "get moderators returned different list in two calls");
        }


        /******************************end of get moderators***************************************/

        /******************************get threads***************************************

        [TestMethod]
        public void test_getThreads_check_different_list_instances()
        {
            List<IThread> threadList1 = this.subForum.getThreads();
            List<IThread> threadList2 = this.subForum.getThreads();
            Assert.IsFalse(Object.Equals(threadList1, threadList2), "get moderators should not return a reference to it's inner moderator list");
        }

        [TestMethod]
        public void test_getThreads_check_different_list_elements_instances()
        {
            List<IThread> threadList1 = this.subForum.getThreads();
            List<IThread> threadList2 = this.subForum.getThreads();
            foreach (IThread t1 in threadList1)
            {
                foreach (IThread t2 in threadList2)
                {
                    Assert.IsFalse(Object.Equals(t1, t2), "get moderators should not return a reference to it's inner moderator list");
                }
            }
        }

        /******************************end of get threads***************************************/

        /*******************************create thread*****************************************/

        [TestMethod]
        public void test_createThread_mem_valid_input()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
        }


        [TestMethod]
        public void test_createThread_admin_valid_input()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userAdmin.userName);
        }

        [TestMethod]
        public void test_createThread_mem_empty_headLine()
        {
            String headLine = "";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
        }


        [TestMethod]
        public void test_createThread_admin_empty_headLine()
        {
            String headLine = "";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userAdmin.userName);
        }

        [TestMethod]
        public void test_createThread_mem_empty_content()
        {
            String headLine = "head";
            String content = "";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
        }


        [TestMethod]
        public void test_createThread_admin_empty_content()
        {
            String headLine = "head";
            String content = "";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userAdmin.userName);
        }

        [TestMethod]
        public void test_createThread_mem_empty_inputs()
        {
            String headLine = "";
            String content = "";
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }


        [TestMethod]
        public void test_createThread_admin_empty_inputs()
        {
            String headLine = "";
            String content = "";
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }

        [TestMethod]
        public void test_createThread_mem_null_inputs()
        {
            String headLine = null;
            String content = null;
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }


        [TestMethod]
        public void test_createThread_admin_null_inputs()
        {
            String headLine = null;
            String content = null;
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }

        [TestMethod]
        public void test_createThread_invalid_userName()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.subForum.createThread(headLine, content, "donJoe", this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }

        [TestMethod]
        public void test_createThread_invalid_forumName()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userMember.userName, "notForum", this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }

        [TestMethod]
        public void test_createThread_invalid_subForumName()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, "notSubForum").Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }

        [TestMethod]
        public void test_createThread_nonMember()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userNonMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }


        /*******************************end of create thread*****************************************/

        /*******************************delete thread*****************************************/


        [TestMethod]
        public void test_deleteMemThread_by_mem()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
            Assert.IsTrue(this.subForum.deleteThread(post.id, this.userMember.userName).Equals("Thread removed"));
            postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }


        [TestMethod]
        public void test_deleteMemThread_by_admin()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
            Assert.IsTrue(this.subForum.deleteThread(post.id, this.userAdmin.userName).Equals("Thread removed"));
            postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }


        [TestMethod]
        public void test_deleteAdminThread_by_mem()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userAdmin.userName);
            Assert.IsFalse(this.subForum.deleteThread(post.id, this.userMember.userName).Equals("Thread removed"));
            postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
        }


        [TestMethod]
        public void test_deleteAdminThread_by_admin()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userAdmin.userName);
            Assert.IsTrue(this.subForum.deleteThread(post.id, this.userAdmin.userName).Equals("Thread removed"));
            postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }

        [TestMethod]
        public void test_deleteThread_invalid_id()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
            Assert.IsFalse(this.subForum.deleteThread(post.id + 1, this.userAdmin.userName).Equals("Thread removed"));
            postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
        }

        [TestMethod]
        public void test_deleteThread_by_nonMember()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
            Assert.IsFalse(this.subForum.deleteThread(post.id, this.userNonMember.userName).Equals("Thread removed"));
            postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
        }

        [TestMethod]
        public void test_deleteThread_null_remover()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            Post post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
            Assert.IsFalse(this.subForum.deleteThread(post.id, null).Equals("Thread removed"));
            postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
        }

        /*******************************end of delete thread*****************************************/


        private bool areListsEqual<T>(List<T> list1, List<T> list2)
        {
            return ((list1.Count == list2.Count) &&
                            list1.Except(list2).Any());
        }
    }
}
