using System;
using System.ServiceModel;
using System.Threading;
using ForumBuilder.Controllers;
using BL_Back_End;
using System.Collections.Generic;
using ForumBuilder.Systems;
using ForumBuilder.Common.DataContracts;
using ForumBuilder.Common.ServiceContracts;
using PL.proxies;
using Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class InteractivityTest
    {
        private TestableUserNotifications notifications;
        private IForumManager forumManager;
        private IPostManager postManager;
        private ISubForumManager subForumManager;
        private ForumData forum;
        private UserData userNonMember;
        private UserData userMember;
        private UserData userMod;
        private UserData userAdmin;
        private UserData superUser;
        private String forumName = "forum";
        private String subForumName = "subForum";
        private int postId;
        private String sk ;
        private String sk2;


        [TestInitialize]
        public void setUp()
        {
            DBClass db = DBClass.getInstance;
            db.clear();
            notifications = new TestableUserNotifications();
            notifications.clearCounters();
            SuperUserController superUserController = SuperUserController.getInstance;
            this.superUser = new UserData("tomer", "1qW", "fkfkf@wkk.com");
            ForumSystem.initialize(superUser.userName, superUser.password, superUser.email);
            this.forumManager = new ForumManagerClient(new InstanceContext(notifications));
            this.userNonMember = new UserData("nonMem", "nonMempass1", "nonmem@gmail.com");
            this.userMember = new UserData("mem", "Mempass1", "mem@gmail.com");
            this.userAdmin = new UserData("admin", "Adminpass1", "admin@gmail.com");
            superUserController.addUser(userAdmin.userName, userAdmin.password, userAdmin.email, superUser.userName);
            List<string> adminList = new List<string>();
            adminList.Add(this.userAdmin.userName);
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            ForumPolicyData fpd = new ForumPolicyData(fp.policy, fp.isQuestionIdentifying, fp.seniorityInForum, fp.deletePostByModerator, fp.timeToPassExpiration, fp.minNumOfModerators,
                                                        fp.hasCapitalInPassword, fp.hasNumberInPassword, fp.minLengthOfPassword, 0, new List<string>());
            this.forum = new ForumData(this.forumName, "descr", fpd, new List<String>(), new List<string>());
            superUserController.createForum(this.forum.forumName, "descr", fp, adminList, superUser.userName);
            Assert.AreEqual(this.forumManager.registerUser(userMember.userName, userMember.password, userMember.email, "ansss", "anssss", this.forum.forumName), "Register user succeed");
            this.postManager = new PostManagerClient();
            this.subForumManager = new SubForumManagerClient();
            this.userMod = new UserData("mod", "Modpass1", "mod@gmail.com");
            Dictionary<String, DateTime> modList = new Dictionary<String, DateTime>();
            modList.Add(this.userMod.userName, new DateTime(2030, 1, 1));
            Assert.IsTrue(ForumController.getInstance.registerUser("mod", "Modpass11", "mod@gmail.com", "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            Assert.IsTrue(this.forumManager.addSubForum(this.forumName, this.subForumName, modList, this.userAdmin.userName).Equals("sub-forum added"));
            Assert.IsTrue(SubForumController.getInstance.createThread("headLine", "content", this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
            this.postId = posts[0].id;
            Assert.IsTrue((sk=this.forumManager.login(userMember.userName, forum.forumName, userMember.password)).Contains(","));
            Assert.IsTrue((sk2=this.forumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
        }

        [TestCleanup]
        public void cleanUp()
        {
            this.subForumManager.deleteThread(postId, this.userMember.userName);
            this.forumManager.logout(this.userMember.userName, this.forum.forumName,sk);
            this.forumManager.logout(this.userAdmin.userName, this.forum.forumName,sk2);
            this.forumManager = null;
            this.subForumManager = null;
            this.postManager = null;
            this.forum = null;
            this.userNonMember = null;
            this.userMember = null;
            this.userMod = null;
            this.userAdmin = null;
            DBClass db = DBClass.getInstance;
            db.clear();
        }

        [TestMethod]
        public void interactivity_test_thread_creation_by_member()
        {
            this.subForumManager.createThread("head", "content", this.userMember.userName, forum.forumName, this.subForumName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(2, notifications.publishedCounter);
        }

        [TestMethod]
        public void interactivity_test_thread_creation_by_NonMember()
        {
            this.subForumManager.createThread("head", "content", this.userNonMember.userName, forum.forumName, this.subForumName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.publishedCounter);
        }

        [TestMethod]
        public void interactivity_test_thread_creation_by_admin()
        {
            this.subForumManager.createThread("head", "content", this.userAdmin.userName, forum.forumName, this.subForumName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(2, notifications.publishedCounter);
        }

        [TestMethod]
        public void interactivity_test_thread_delition_by_member()
        {
            //interactivity_test_thread_creation_by_member();
            Assert.AreEqual("Thread removed", this.subForumManager.deleteThread(postId, this.userMember.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.deletedCounter);
        }

        [TestMethod]
        public void interactivity_test_thread_delition_by_Non_Member()
        {
            //interactivity_test_thread_creation_by_member();
            Assert.AreNotEqual("Thread removed", this.subForumManager.deleteThread(postId, this.userNonMember.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.deletedCounter);
        }

        [TestMethod]
        public void interactivity_test_thread_delition_by_admin()
        {
            //interactivity_test_thread_creation_by_admin();
            Assert.AreEqual("Thread removed", this.subForumManager.deleteThread(postId, this.userAdmin.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.deletedCounter);
        }

        [TestMethod]
        public void interactivity_test_post_delition_by_member()
        {
            //interactivity_test_thread_creation_by_member();
            Assert.AreEqual("Post removed", this.postManager.deletePost(postId, this.userMember.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.deletedCounter);
        }

        [TestMethod]
        public void interactivity_test_post_delition_by_admin()
        {
            //interactivity_test_thread_creation_by_admin();
            Assert.AreEqual("Post removed", this.postManager.deletePost(postId, this.userAdmin.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.deletedCounter);
        }

        [TestMethod]
        public void interactivity_test_post_delition_by_member_with_comenter()
        {
            notifications.clearCounters();
            this.postManager.addPost("head", "cont", this.userAdmin.userName, this.postId);
            Assert.AreEqual("Post removed", this.postManager.deletePost(postId, this.userMember.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(2, notifications.deletedCounter);
        }

        [TestMethod]
        public void interactivity_test_post_delition_by_admin_with_comenter()
        {
            notifications.clearCounters();
            this.postManager.addPost("head", "cont", this.userMember.userName, this.postId);
            Assert.AreEqual("Post removed", this.postManager.deletePost(postId, this.userAdmin.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(2, notifications.deletedCounter);
        }

        [TestMethod]
        public void interactivity_test_post_delition_by_non_member_with_comenter()
        {
            notifications.clearCounters();
            this.postManager.addPost("head", "cont", this.userMember.userName, this.postId);
            this.postManager.deletePost(postId, this.userNonMember.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.deletedCounter);
        }


        [TestMethod]
        public void interactivity_test_post_modification_by_NonMember()
        {
            this.postManager.updatePost(postId, "new", "new", this.userNonMember.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.modifiedCounter);
        }

        [TestMethod]
        public void interactivity_test_post_modification_by_admin()
        {
            interactivity_test_thread_creation_by_admin();
            this.postManager.updatePost(1, "new", "new", this.userAdmin.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.modifiedCounter);
        }

        [TestMethod]
        public void interactivity_test_post_modification_by_member_with_comenter()
        {
            this.postManager.addPost("head", "cont", this.userAdmin.userName, this.postId);
            this.postManager.updatePost(postId, "new", "new", this.userMember.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.modifiedCounter);
        }

        [TestMethod]
        public void interactivity_test_post_modification_by_admin_with_comenter()
        {
            interactivity_test_thread_creation_by_admin();
            this.postManager.addPost("head", "cont", this.userMember.userName, 1);
            this.postManager.updatePost(1, "new", "new", this.userAdmin.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.modifiedCounter);
        }

        [TestMethod]
        public void interactivity_test_post_modification_by_no_member()
        {
            interactivity_test_thread_creation_by_admin();
            this.postManager.addPost("head", "cont", this.userNonMember.userName, 1);
            this.postManager.updatePost(1, "new", "new", this.userNonMember.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.modifiedCounter);
        }

        [TestMethod]
        public void interactivity_test_privateMessage_member_to_admin()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userMember.userName, this.userAdmin.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.privateMessage);
        }

        [TestMethod]
        public void interactivity_test_privateMessage_admin_to_member()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userAdmin.userName, this.userMember.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.privateMessage);
        }
        [TestMethod]
        public void interactivity_test_privateMessage_member_to_NonMember()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userMember.userName, this.userNonMember.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.privateMessage);
        }

        [TestMethod]
        public void interactivity_test_privateMessage_Nomember_to_Member()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userNonMember.userName, this.userMember.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.privateMessage);
        }

        [TestMethod]
        public void interactivity_test_privateMessage_NoMember_to_Admin()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userNonMember.userName, this.userAdmin.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.privateMessage);
        }
        [TestMethod]
        public void interactivity_test_privateMessage_Member_to_Member()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userMember.userName, this.userMember.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.privateMessage);
        }
    }
}
