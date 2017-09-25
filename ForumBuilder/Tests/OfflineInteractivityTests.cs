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
    public class OfflineInteractivityTests
    {

        public const int INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS = 1;

        private TestableUserNotifications mem1Notifications;
        private TestableUserNotifications mem2Notifications;
        private TestableUserNotifications modNotifications;
        private TestableUserNotifications adminNotifications;

        private IForumManager mem1ForumManager;
        private IForumManager mem2ForumManager;
        private IForumManager modForumManager;
        private IForumManager adminForumManager;

        private IPostManager postManager;
        private ISubForumManager subForumManager;
        private IUserManager userManager;
        private ForumData forum;
        private UserData userNonMember;
        private UserData userMember1;
        private UserData userMember2;
        private UserData userMod;
        private UserData userAdmin;
        private UserData superUser;
        private String forumName = "forum";
        private String subForumName = "subForum";
        private String threadHeadline = "headLine";
        private int postId;
        private String skmem1;
        private String skmem2;
        private String skmod;
        private String skadmin;


        [TestInitialize]
        public void setUp()
        {
            DBClass db = DBClass.getInstance;
            db.clear();

            this.mem1Notifications = new TestableUserNotifications();
            this.mem2Notifications = new TestableUserNotifications();
            this.modNotifications = new TestableUserNotifications();
            this.adminNotifications = new TestableUserNotifications();

            this.mem1Notifications.clearCounters();
            this.mem2Notifications.clearCounters();
            this.modNotifications.clearCounters();
            this.adminNotifications.clearCounters();

            SuperUserController superUserController = SuperUserController.getInstance;
            this.superUser = new UserData("tomer", "1qW", "fkfkf@wkk.com");
            ForumSystem.initialize(superUser.userName, superUser.password, superUser.email);

            this.mem1ForumManager = new ForumManagerClient(new InstanceContext(mem1Notifications));
            this.mem2ForumManager = new ForumManagerClient(new InstanceContext(mem2Notifications));
            this.modForumManager = new ForumManagerClient(new InstanceContext(modNotifications));
            this.adminForumManager = new ForumManagerClient(new InstanceContext(adminNotifications));

            this.userNonMember = new UserData("nonMem", "nonMempass1", "nonmem@gmail.com");
            this.userMember1 = new UserData("mem", "Mempass1", "mem@gmail.com");
            this.userMember2 = new UserData("mem2", "Mempass1", "mem2@gmail.com");
            this.userAdmin = new UserData("admin", "Adminpass1", "admin@gmail.com");
            superUserController.addUser(userAdmin.userName, userAdmin.password, userAdmin.email, superUser.userName);
            List<string> adminList = new List<string>();
            adminList.Add(this.userAdmin.userName);
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, ForumPolicyData.OFFLINE_NOTIFICATIONS_TPYE, new List<string>());
            ForumPolicyData fpd = new ForumPolicyData(fp.policy, fp.isQuestionIdentifying, fp.seniorityInForum, fp.deletePostByModerator, fp.timeToPassExpiration, fp.minNumOfModerators,
                                                        fp.hasCapitalInPassword, fp.hasNumberInPassword, fp.minLengthOfPassword, ForumPolicyData.OFFLINE_NOTIFICATIONS_TPYE, new List<string>());
            this.forum = new ForumData(this.forumName, "descr", fpd, new List<String>(), new List<string>());
            superUserController.createForum(this.forum.forumName, "descr", fp, adminList, superUser.userName);
            Assert.AreEqual(this.mem1ForumManager.registerUser(userMember1.userName, userMember1.password, userMember1.email, "ansss", "anssss", this.forum.forumName), "Register user succeed");
            Assert.AreEqual(this.mem2ForumManager.registerUser(userMember2.userName, userMember2.password, userMember2.email, "ansss", "anssss", this.forum.forumName), "Register user succeed");

            this.postManager = new PostManagerClient();
            this.subForumManager = new SubForumManagerClient();
            this.userManager = new UserManagerClient();
            this.userMod = new UserData("mod", "Modpass1", "mod@gmail.com");
            Dictionary<String, DateTime> modList = new Dictionary<String, DateTime>();
            modList.Add(this.userMod.userName, new DateTime(2030, 1, 1));
            Assert.IsTrue(ForumController.getInstance.registerUser(userMod.userName, userMod.password, userMod.email, "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            Assert.IsTrue(this.adminForumManager.addSubForum(this.forumName, this.subForumName, modList, this.userAdmin.userName).Equals("sub-forum added"));
            Assert.IsTrue(SubForumController.getInstance.createThread(this.threadHeadline, "content", this.userMember2.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
            this.postId = posts[0].id;
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            Assert.IsTrue((skmem2 = this.mem1ForumManager.login(userMember2.userName, forum.forumName, userMember2.password)).Contains(","));
            Assert.IsTrue((skmod = this.modForumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            Assert.IsTrue((skadmin = this.adminForumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
        }

        [TestCleanup]
        public void cleanUp()
        {
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            this.mem2ForumManager.logout(this.userMember2.userName, this.forum.forumName, skmem2);
            this.modForumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            this.adminForumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            this.mem1ForumManager = null;
            this.mem2ForumManager = null;
            this.modForumManager = null;
            this.adminForumManager = null;
            this.subForumManager = null;
            this.postManager = null;
            this.forum = null;
            this.userNonMember = null;
            this.userMember1 = null;
            this.userMember2 = null;
            this.userMod = null;
            this.userAdmin = null;
            DBClass db = DBClass.getInstance;
            db.clear();
        }


        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_thread_creation_by_mem()
        {
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            this.subForumManager.createThread("head", "cont", userMember2.userName, this.forum.forumName, this.subForumName);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(threadCreationScheme(userMember2.userName, this.forum.forumName, this.subForumName)));

        }

        [TestMethod]
        public void offline_interactivity_test_mod_gets_offline_notification_thread_creation_by_mem()
        {
            this.modForumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            this.subForumManager.createThread("head", "cont", userMember2.userName, this.forum.forumName, this.subForumName);
            Assert.IsTrue((skmod = this.modForumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            List<string> offlineNotifications = this.modForumManager.getOfflineNotifications(this.forum.forumName, this.userMod.userName, Int32.Parse(skmod.Substring(0, skmod.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(threadCreationScheme(userMember2.userName, this.forum.forumName, this.subForumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_admin_gets_offline_notification_thread_creation_by_mem()
        {
            this.adminForumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            this.subForumManager.createThread("head", "cont", userMember2.userName, this.forum.forumName, this.subForumName);
            Assert.IsTrue((skadmin = this.adminForumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
            List<string> offlineNotifications = this.adminForumManager.getOfflineNotifications(this.forum.forumName, this.userAdmin.userName, Int32.Parse(skadmin.Substring(0, skadmin.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(threadCreationScheme(userMember2.userName, this.forum.forumName, this.subForumName)));
        }


        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_thread_creation_by_mod()
        {
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            this.subForumManager.createThread("head", "cont", userMod.userName, this.forum.forumName, this.subForumName);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(threadCreationScheme(userMod.userName, this.forum.forumName, this.subForumName)));

        }

        [TestMethod]
        public void offline_interactivity_test_mod_gets_offline_notification_thread_creation_by_mod()
        {
            this.modForumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            this.subForumManager.createThread("head", "cont", userMod.userName, this.forum.forumName, this.subForumName);
            Assert.IsTrue((skmod = this.modForumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            List<string> offlineNotifications = this.modForumManager.getOfflineNotifications(this.forum.forumName, this.userMod.userName, Int32.Parse(skmod.Substring(0, skmod.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(threadCreationScheme(userMod.userName, this.forum.forumName, this.subForumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_admin_gets_offline_notification_thread_creation_by_mod()
        {
            this.adminForumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            this.subForumManager.createThread("head", "cont", userMod.userName, this.forum.forumName, this.subForumName);
            Assert.IsTrue((skadmin = this.adminForumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
            List<string> offlineNotifications = this.adminForumManager.getOfflineNotifications(this.forum.forumName, this.userAdmin.userName, Int32.Parse(skadmin.Substring(0, skadmin.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(threadCreationScheme(userMod.userName, this.forum.forumName, this.subForumName)));
        }


        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_thread_creation_by_admin()
        {
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            this.subForumManager.createThread("head", "cont", userAdmin.userName, this.forum.forumName, this.subForumName);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(threadCreationScheme(userAdmin.userName, this.forum.forumName, this.subForumName)));

        }

        [TestMethod]
        public void offline_interactivity_test_mod_gets_offline_notification_thread_creation_by_admin()
        {
            this.modForumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            this.subForumManager.createThread("head", "cont", userAdmin.userName, this.forum.forumName, this.subForumName);
            Assert.IsTrue((skmod = this.modForumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            List<string> offlineNotifications = this.modForumManager.getOfflineNotifications(this.forum.forumName, this.userMod.userName, Int32.Parse(skmod.Substring(0, skmod.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(threadCreationScheme(userAdmin.userName, this.forum.forumName, this.subForumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_admin_gets_offline_notification_thread_creation_by_admin()
        {
            this.adminForumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            this.subForumManager.createThread("head", "cont", userAdmin.userName, this.forum.forumName, this.subForumName);
            Assert.IsTrue((skadmin = this.adminForumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
            List<string> offlineNotifications = this.adminForumManager.getOfflineNotifications(this.forum.forumName, this.userAdmin.userName, Int32.Parse(skadmin.Substring(0, skadmin.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(threadCreationScheme(userAdmin.userName, this.forum.forumName, this.subForumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_private_msg_by_mem()
        {
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            String msgContent = "hey";
            this.userManager.sendPrivateMessage(this.forum.forumName, userMember2.userName, userMember1.userName, msgContent);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(privateMsgScheme(userMember2.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_mod_gets_offline_notification_private_msg_by_mem()
        {
            this.modForumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            String msgContent = "hey";
            this.userManager.sendPrivateMessage(this.forum.forumName, userMember2.userName, userMod.userName, msgContent);
            Assert.IsTrue((skmod = this.modForumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            List<string> offlineNotifications = this.modForumManager.getOfflineNotifications(this.forum.forumName, this.userMod.userName, Int32.Parse(skmod.Substring(0, skmod.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(privateMsgScheme(userMember2.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_admin_gets_offline_notification_private_msg_by_mem()
        {
            this.adminForumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            String msgContent = "hey";
            this.userManager.sendPrivateMessage(this.forum.forumName, userMember2.userName, userAdmin.userName, msgContent);
            Assert.IsTrue((skadmin = this.adminForumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
            List<string> offlineNotifications = this.adminForumManager.getOfflineNotifications(this.forum.forumName, this.userAdmin.userName, Int32.Parse(skadmin.Substring(0, skadmin.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(privateMsgScheme(userMember2.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_private_msg_by_mod()
        {
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            String msgContent = "hey";
            this.userManager.sendPrivateMessage(this.forum.forumName, userMod.userName, userMember1.userName, msgContent);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(privateMsgScheme(userMod.userName, this.forum.forumName)));
        }


        [TestMethod]
        public void offline_interactivity_test_admin_gets_offline_notification_private_msg_by_mod()
        {
            this.adminForumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            String msgContent = "hey";
            this.userManager.sendPrivateMessage(this.forum.forumName, userMod.userName, userAdmin.userName, msgContent);
            Assert.IsTrue((skadmin = this.adminForumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
            List<string> offlineNotifications = this.adminForumManager.getOfflineNotifications(this.forum.forumName, this.userAdmin.userName, Int32.Parse(skadmin.Substring(0, skadmin.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(privateMsgScheme(userMod.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_private_msg_by_admin()
        {
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            String msgContent = "hey";
            this.userManager.sendPrivateMessage(this.forum.forumName, userMember2.userName, userMember1.userName, msgContent);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(privateMsgScheme(userMember2.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_mod_gets_offline_notification_private_msg_by_admin()
        {
            this.modForumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            String msgContent = "hey";
            this.userManager.sendPrivateMessage(this.forum.forumName, userAdmin.userName, userMod.userName, msgContent);
            Assert.IsTrue((skmod = this.modForumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            List<string> offlineNotifications = this.modForumManager.getOfflineNotifications(this.forum.forumName, this.userMod.userName, Int32.Parse(skmod.Substring(0, skmod.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(privateMsgScheme(userAdmin.userName, this.forum.forumName)));
        }


        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_postModification_by_mem()
        {
            this.postManager.addPost("head comment", "cont", this.userMember1.userName, this.postId);
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            String newTitle = "new";
            this.postManager.updatePost(this.postId, newTitle, "new cont", this.userMember2.userName);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postModificationScheme(this.userMember2.userName, this.forum.forumName, this.threadHeadline)));
        }

        [TestMethod]
        public void offline_interactivity_test_mod_gets_offline_notification_postModification_by_mem()
        {
            this.postManager.addPost("head comment", "cont", this.userMod.userName, this.postId);
            this.modForumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            String newTitle = "new";
            this.postManager.updatePost(this.postId, newTitle, "new cont", this.userMember2.userName);
            Assert.IsTrue((skmod = this.modForumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            List<string> offlineNotifications = this.modForumManager.getOfflineNotifications(this.forum.forumName, this.userMod.userName, Int32.Parse(skmod.Substring(0, skmod.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postModificationScheme(this.userMember2.userName, this.forum.forumName, this.threadHeadline)));
        }

        [TestMethod]
        public void offline_interactivity_test_admin_gets_offline_notification_postModification_by_mem()
        {
            this.postManager.addPost("head comment", "cont", this.userAdmin.userName, this.postId);
            this.adminForumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            String newTitle = "new";
            this.postManager.updatePost(this.postId, newTitle, "new cont", this.userMember2.userName);
            Assert.IsTrue((skadmin = this.adminForumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
            List<string> offlineNotifications = this.adminForumManager.getOfflineNotifications(this.forum.forumName, this.userAdmin.userName, Int32.Parse(skadmin.Substring(0, skadmin.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postModificationScheme(this.userMember2.userName, this.forum.forumName, this.threadHeadline)));
        }

        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_postModification_by_mod()
        {
            String newThreadHeadline = "new";
            this.subForumManager.createThread(newThreadHeadline, "cont", userMod.userName, this.forum.forumName, this.subForumName);
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            int newPostId = posts[1].id;
            this.postManager.addPost("head comment", "cont", this.userMember1.userName, newPostId);
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            String newTitle = "new";
            this.postManager.updatePost(newPostId, newTitle, "new cont", this.userMod.userName);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postModificationScheme(this.userMod.userName, this.forum.forumName, newThreadHeadline)));
        }

        [TestMethod]
        public void offline_interactivity_test_admin_gets_offline_notification_postModification_by_mod()
        {
            String newThreadHeadline = "new";
            this.subForumManager.createThread(newThreadHeadline, "cont", userMod.userName, this.forum.forumName, this.subForumName);
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            int newPostId = posts[1].id;
            this.postManager.addPost("head comment", "cont", this.userAdmin.userName, newPostId);
            this.adminForumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            String newTitle = "new";
            this.postManager.updatePost(newPostId, newTitle, "new cont", this.userMod.userName);
            Assert.IsTrue((skadmin = this.adminForumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
            List<string> offlineNotifications = this.adminForumManager.getOfflineNotifications(this.forum.forumName, this.userAdmin.userName, Int32.Parse(skadmin.Substring(0, skadmin.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postModificationScheme(this.userMod.userName, this.forum.forumName, newThreadHeadline)));
        }

        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_postModification_by_admin()
        {
            String newThreadHeadline = "new";
            this.subForumManager.createThread(newThreadHeadline, "cont", userAdmin.userName, this.forum.forumName, this.subForumName);
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            int newPostId = posts[1].id;
            this.postManager.addPost("head comment", "cont", this.userMember1.userName, newPostId);
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            String newTitle = "new";
            this.postManager.updatePost(newPostId, newTitle, "new cont", this.userAdmin.userName);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postModificationScheme(this.userAdmin.userName, this.forum.forumName, newThreadHeadline)));
        }

        [TestMethod]
        public void offline_interactivity_test_mod_gets_offline_notification_postModification_by_admin()
        {
            String newThreadHeadline = "new";
            this.subForumManager.createThread(newThreadHeadline, "cont", userAdmin.userName, this.forum.forumName, this.subForumName);
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            int newPostId = posts[1].id;
            this.postManager.addPost("head comment", "cont", this.userMod.userName, newPostId);
            this.modForumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            String newTitle = "new";
            this.postManager.updatePost(newPostId, newTitle, "new cont", this.userAdmin.userName);
            Assert.IsTrue((skmod = this.modForumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            List<string> offlineNotifications = this.modForumManager.getOfflineNotifications(this.forum.forumName, this.userMod.userName, Int32.Parse(skmod.Substring(0, skmod.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postModificationScheme(this.userAdmin.userName, this.forum.forumName, newThreadHeadline)));
        }


        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_postDelition_by_mem()
        {
            this.postManager.addPost("head comment", "cont", this.userMember1.userName, this.postId);
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            this.postManager.deletePost(this.postId, this.userMember2.userName);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postDelitionScheme(this.userMember2.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_mod_gets_offline_notification_postDelition_by_mem()
        {
            this.postManager.addPost("head comment", "cont", this.userMod.userName, this.postId);
            this.modForumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            this.postManager.deletePost(this.postId, this.userMember2.userName);
            Assert.IsTrue((skmod = this.modForumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            List<string> offlineNotifications = this.modForumManager.getOfflineNotifications(this.forum.forumName, this.userMod.userName, Int32.Parse(skmod.Substring(0, skmod.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postDelitionScheme(this.userMember2.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_admin_gets_offline_notification_postDelition_by_mem()
        {
            this.postManager.addPost("head comment", "cont", this.userAdmin.userName, this.postId);
            this.adminForumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            this.postManager.deletePost(this.postId, this.userMember2.userName);
            Assert.IsTrue((skadmin = this.adminForumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
            List<string> offlineNotifications = this.adminForumManager.getOfflineNotifications(this.forum.forumName, this.userAdmin.userName, Int32.Parse(skadmin.Substring(0, skadmin.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postDelitionScheme(this.userMember2.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_postDelition_by_mod()
        {
            String newThreadHeadline = "new";
            this.subForumManager.createThread(newThreadHeadline, "cont", userMod.userName, this.forum.forumName, this.subForumName);
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            int newPostId = posts[1].id;
            this.postManager.addPost("head comment", "cont", this.userMember1.userName, newPostId);
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            this.postManager.deletePost(newPostId, this.userMod.userName);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postDelitionScheme(this.userMod.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_admin_gets_offline_notification_postDelition_by_mod()
        {
            String newThreadHeadline = "new";
            this.subForumManager.createThread(newThreadHeadline, "cont", userMod.userName, this.forum.forumName, this.subForumName);
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            int newPostId = posts[1].id;
            this.postManager.addPost("head comment", "cont", this.userAdmin.userName, newPostId);
            this.adminForumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            this.postManager.deletePost(newPostId, this.userMod.userName);
            Assert.IsTrue((skadmin = this.adminForumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));
            List<string> offlineNotifications = this.adminForumManager.getOfflineNotifications(this.forum.forumName, this.userAdmin.userName, Int32.Parse(skadmin.Substring(0, skadmin.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postDelitionScheme(this.userMod.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_mem_gets_offline_notification_postDelition_by_admin()
        {
            String newThreadHeadline = "new";
            this.subForumManager.createThread(newThreadHeadline, "cont", userAdmin.userName, this.forum.forumName, this.subForumName);
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            int newPostId = posts[1].id;
            this.postManager.addPost("head comment", "cont", this.userMember1.userName, newPostId);
            this.mem1ForumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            this.postManager.deletePost(newPostId, this.userAdmin.userName);
            Assert.IsTrue((skmem1 = this.mem1ForumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            List<string> offlineNotifications = this.mem1ForumManager.getOfflineNotifications(this.forum.forumName, this.userMember1.userName, Int32.Parse(skmem1.Substring(0, skmem1.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postDelitionScheme(this.userAdmin.userName, this.forum.forumName)));
        }

        [TestMethod]
        public void offline_interactivity_test_mod_gets_offline_notification_postDelition_by_admin()
        {
            String newThreadHeadline = "new";
            this.subForumManager.createThread(newThreadHeadline, "cont", userAdmin.userName, this.forum.forumName, this.subForumName);
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            int newPostId = posts[1].id;
            this.postManager.addPost("head comment", "cont", this.userMod.userName, newPostId);
            this.modForumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            this.postManager.deletePost(newPostId, this.userAdmin.userName);
            Assert.IsTrue((skmod = this.modForumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            List<string> offlineNotifications = this.modForumManager.getOfflineNotifications(this.forum.forumName, this.userMod.userName, Int32.Parse(skmod.Substring(0, skmod.IndexOf(","))));
            Assert.AreEqual(INITIAL_OFFLINE_THREAD_CREAT_PENDING_NOTS + 1, offlineNotifications.Count);
            Assert.IsTrue(offlineNotifications.Contains(postDelitionScheme(this.userAdmin.userName, this.forum.forumName)));
        }



      


        private String threadCreationScheme(String publisherName, String forumName, String subForumName)
        {
            return publisherName + " published a post in " + forumName + "'s sub-forum " + subForumName;
        }

        private String privateMsgScheme(String sender, String forumName)
        {
            return sender + "'s post you were following in " + forumName + " was deleted";
        }

        private String postModificationScheme(String publisherName, String forumName, String title)
        {
            String res = publisherName + "'s post you were following in " + forumName + " was modified (" + title + ")";
            return res;
        }

        private String postDelitionScheme(String publisherName, String forumName)
        {
            String res = publisherName + "'s post you were following in " + forumName + " was deleted";
            return res;
        }

    }
}
