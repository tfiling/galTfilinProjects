using System;
using System.ServiceModel;
using System.Threading;
using ForumBuilder.Controllers;
using BL_Back_End;
using System.Collections.Generic;
using ForumBuilder.Systems;
using ForumBuilder.Common.DataContracts;
using ForumBuilder.Common.ServiceContracts;
using ForumBuilder.Common.ClientServiceContracts;
using PL.proxies;
using Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class SelectiveInteractivityTest
    {
        public const int INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS = 1;

        private TestableUserNotifications notifications;
        private IForumManager forumManager;

        private IPostManager postManager;
        private ISubForumManager subForumManager;
        private IUserManager userManager;
        private ForumData forum;
        private UserData userNonMember;
        private UserData userMember1;
        private UserData userMember2;
        private UserData userMod1;
        private UserData userMod2;
        private UserData userAdmin1;
        private UserData userAdmin2;
        private UserData superUser;
        private String forumName = "forum";
        private String subForumName = "subForum";
        private String threadHeadline = "headLine";
        private int postId;
        private String skmem1;
        private String skmem2;
        private String skmod1;
        private String skmod2;
        private String skadmin1;
        private String skadmin2;


        [TestInitialize]
        public void setUp()
        {
            DBClass db = DBClass.getInstance;
            db.clear();

            this.notifications = new TestableUserNotifications();
            this.notifications.clearCounters();


            SuperUserController superUserController = SuperUserController.getInstance;
            this.superUser = new UserData("tomer", "1qW", "fkfkf@wkk.com");
            ForumSystem.initialize(superUser.userName, superUser.password, superUser.email);

            this.forumManager = new ForumManagerClient(new InstanceContext(notifications));

            this.userNonMember = new UserData("nonMem", "nonMempass1", "nonmem@gmail.com");
            this.userMember1 = new UserData("mem", "Mempass1", "mem@gmail.com");
            this.userMember2 = new UserData("mem2", "Mempass1", "mem2@gmail.com");
            this.userAdmin1 = new UserData("admin1", "Adminpass1", "admin1@gmail.com");
            superUserController.addUser(userAdmin1.userName, userAdmin1.password, userAdmin1.email, superUser.userName);
            List<string> adminList = new List<string>();
            adminList.Add(this.userAdmin1.userName);
            this.userAdmin2 = new UserData("admin2", "Adminpass1", "admin2@gmail.com");
            superUserController.addUser(userAdmin2.userName, userAdmin2.password, userAdmin2.email, superUser.userName);
            adminList.Add(this.userAdmin2.userName);
            this.userMod1 = new UserData("mod1", "Modpass1", "mod1@gmail.com");
            this.userMod2 = new UserData("mod2", "Modpass1", "mod2@gmail.com");
            List<String> notifiedUsers = new List<String>();
            notifiedUsers.Add(userAdmin1.userName);
            notifiedUsers.Add(userMember1.userName);
            notifiedUsers.Add(userMod1.userName);
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, ForumPolicyData.SELECTIVE_NOTIFICATIONS_TPYE, notifiedUsers);
            ForumPolicyData fpd = new ForumPolicyData(fp.policy, fp.isQuestionIdentifying, fp.seniorityInForum, fp.deletePostByModerator, fp.timeToPassExpiration, fp.minNumOfModerators,
                                                        fp.hasCapitalInPassword, fp.hasNumberInPassword, fp.minLengthOfPassword, ForumPolicyData.SELECTIVE_NOTIFICATIONS_TPYE, notifiedUsers);
            this.forum = new ForumData(this.forumName, "descr", fpd, new List<String>(), new List<string>());
            superUserController.createForum(this.forum.forumName, "descr", fp, adminList, superUser.userName);
            Assert.AreEqual(this.forumManager.registerUser(userMember1.userName, userMember1.password, userMember1.email, "ansss", "anssss", this.forum.forumName), "Register user succeed");
            Assert.AreEqual(this.forumManager.registerUser(userMember2.userName, userMember2.password, userMember2.email, "ansss", "anssss", this.forum.forumName), "Register user succeed");

            this.postManager = new PostManagerClient();
            this.subForumManager = new SubForumManagerClient();
            this.userManager = new UserManagerClient();
            Dictionary<String, DateTime> modList = new Dictionary<String, DateTime>();
            modList.Add(this.userMod1.userName, new DateTime(2030, 1, 1));
            modList.Add(this.userMod2.userName, new DateTime(2030, 1, 1));
            Assert.IsTrue(ForumController.getInstance.registerUser(userMod1.userName, userMod1.password, userMod1.email, "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            Assert.IsTrue(ForumController.getInstance.registerUser(userMod2.userName, userMod2.password, userMod2.email, "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            Assert.AreEqual("sub-forum added", this.forumManager.addSubForum(this.forumName, this.subForumName, modList, this.userAdmin1.userName));
            Assert.IsTrue(SubForumController.getInstance.createThread(this.threadHeadline, "content", this.userMember1.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
            this.postId = posts[0].id;
            Assert.IsTrue((skmem1 = this.forumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            Assert.IsTrue((skmem2 = this.forumManager.login(userMember2.userName, forum.forumName, userMember2.password)).Contains(","));
            Assert.IsTrue((skmod1 = this.forumManager.login(userMod1.userName, forum.forumName, userMod1.password)).Contains(","));
            Assert.IsTrue((skmod2 = this.forumManager.login(userMod2.userName, forum.forumName, userMod2.password)).Contains(","));
            Assert.IsTrue((skadmin1 = this.forumManager.login(userAdmin1.userName, forum.forumName, userAdmin1.password)).Contains(","));
            Assert.IsTrue((skadmin2 = this.forumManager.login(userAdmin2.userName, forum.forumName, userAdmin2.password)).Contains(","));
            forumManager.setForumPreferences(this.forum.forumName, "descr", fpd, this.userAdmin1.userName);
            this.notifications.clearCounters();
        }

        [TestCleanup]
        public void cleanUp()
        {
            this.forumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            this.forumManager.logout(this.userMember2.userName, this.forum.forumName, skmem2);
            this.forumManager.logout(this.userMod1.userName, this.forum.forumName, skmod1);
            this.forumManager.logout(this.userMod2.userName, this.forum.forumName, skmod2);
            this.forumManager.logout(this.userAdmin1.userName, this.forum.forumName, skadmin1);
            this.forumManager.logout(this.userAdmin2.userName, this.forum.forumName, skadmin2);
            this.forumManager = null;
            this.subForumManager = null;
            this.postManager = null;
            this.forum = null;
            this.userNonMember = null;
            this.userMember1 = null;
            this.userMember2 = null;
            this.userMod1 = null;
            this.userAdmin1 = null;
            this.userMod2 = null;
            this.userAdmin2 = null;

            this.notifications.clearCounters();
            DBClass db = DBClass.getInstance;
            db.clear();
        }

        [TestMethod]
        public void selective_interactivity_test_thread_creation_by_member()
        {
            this.subForumManager.createThread("head", "content", this.userMember1.userName, forum.forumName, this.subForumName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(3, notifications.publishedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_thread_creation_by_NonMember()
        {
            this.subForumManager.createThread("head", "content", this.userNonMember.userName, forum.forumName, this.subForumName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.publishedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_thread_creation_by_admin()
        {
            this.subForumManager.createThread("head", "content", this.userAdmin1.userName, forum.forumName, this.subForumName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(3, notifications.publishedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_thread_delition_by_member()
        {
            Assert.AreEqual("Thread removed", this.subForumManager.deleteThread(postId, this.userMember1.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.deletedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_thread_delition_by_Non_Member()
        {
            Assert.AreNotEqual("Thread removed", this.subForumManager.deleteThread(postId, this.userNonMember.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.deletedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_thread_delition_by_admin()
        {
            Assert.AreEqual("Thread removed", this.subForumManager.deleteThread(postId, this.userAdmin1.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.deletedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_post_delition_by_member()
        {
            Assert.AreEqual("Post removed", this.postManager.deletePost(postId, this.userMember1.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.deletedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_post_delition_by_admin()
        {
            Assert.AreEqual("Post removed", this.postManager.deletePost(postId, this.userAdmin1.userName));
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(1, notifications.deletedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_post_delition_by_member_with_comenter()
        {
            notifications.clearCounters();
            this.postManager.addPost("head", "cont", this.userAdmin1.userName, this.postId);
            Assert.AreEqual("Post removed", this.postManager.deletePost(postId, this.userMember1.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(2, notifications.deletedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_post_delition_by_admin_with_comenter()
        {
            notifications.clearCounters();
            this.postManager.addPost("head", "cont", this.userMember1.userName, this.postId);
            Assert.AreEqual("Post removed", this.postManager.deletePost(postId, this.userAdmin1.userName));
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(2, notifications.deletedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_post_delition_by_non_member_with_comenter()
        {
            notifications.clearCounters();
            this.postManager.addPost("head", "cont", this.userMember1.userName, this.postId);
            this.postManager.deletePost(postId, this.userNonMember.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.deletedCounter);
        }


        [TestMethod]
        public void selective_interactivity_test_post_modification_by_NonMember()
        {
            this.postManager.updatePost(postId, "new", "new", this.userNonMember.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.modifiedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_post_modification_by_admin()
        {
            selective_interactivity_test_thread_creation_by_admin();
            this.postManager.updatePost(1, "new", "new", this.userAdmin1.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.modifiedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_post_modification_by_member_with_comenter()
        {
            this.postManager.addPost("head", "cont", this.userAdmin1.userName, this.postId);
            this.postManager.updatePost(postId, "new", "new", this.userMember1.userName);
            System.Threading.Thread.Sleep(200);
            Assert.AreEqual(1, notifications.modifiedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_post_modification_by_admin_with_comenter()
        {
            selective_interactivity_test_thread_creation_by_admin();
            this.postManager.addPost("head", "cont", this.userMember1.userName, 1);
            this.postManager.updatePost(1, "new", "new", this.userAdmin1.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.modifiedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_post_modification_by_no_member()
        {
            selective_interactivity_test_thread_creation_by_admin();
            this.postManager.addPost("head", "cont", this.userNonMember.userName, 1);
            this.postManager.updatePost(1, "new", "new", this.userNonMember.userName);
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.modifiedCounter);
        }

        [TestMethod]
        public void selective_interactivity_test_privateMessage_member_to_admin()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userMember1.userName, this.userAdmin1.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.privateMessage);
        }

        [TestMethod]
        public void selective_interactivity_test_privateMessage_admin_to_member()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userAdmin1.userName, this.userMember1.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.privateMessage);
        }
        [TestMethod]
        public void selective_interactivity_test_privateMessage_member_to_NonMember()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userMember1.userName, this.userNonMember.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.privateMessage);
        }

        [TestMethod]
        public void selective_interactivity_test_privateMessage_Nomember_to_Member()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userNonMember.userName, this.userMember1.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.privateMessage);
        }

        [TestMethod]
        public void selective_interactivity_test_privateMessage_NoMember_to_Admin()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userNonMember.userName, this.userAdmin1.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, notifications.privateMessage);
        }
        [TestMethod]
        public void selective_interactivity_test_privateMessage_Member_to_Member()
        {
            UserController.getInstance.sendPrivateMessage(this.forumName, this.userMember1.userName, this.userMember1.userName, "hey");
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, notifications.privateMessage);
        }
    }
}
