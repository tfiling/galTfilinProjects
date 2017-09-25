using Database;
using ForumBuilder.Common.DataContracts;
using ForumBuilder.Common.ServiceContracts;
using ForumBuilder.Systems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using ForumBuilder.Controllers;
using PL.notificationHost;
using PL.proxies;
using System;
using System.Collections.Generic;
using BL_Back_End;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class UserManagerTest
    {

        private IForumManager forumManager;
        private IPostManager postManager;
        private ISubForumManager subForumManager;
        private IUserManager userManager;
        private UserData userNonMember;
        private UserData userMember1;
        private UserData userMember2;
        private UserData userMod;
        private UserData userAdmin;
        private UserData superUser;
        private ForumData forum;
        private String forumName = "testForum";
        private String subForumName = "sub-forum";
        private String threadHeadline = "head";

        private String skmem1;
        private String skmem2;
        private String skmod;
        private String skadmin;


        [TestInitialize]
        public void setUp()
        {
    /*        SuperUserController superUserController = SuperUserController.getInstance;
            UserData superUser = new UserData("tomer", "1qW", "fkfkf@wkk.com");
            superUserController.addSuperUser(superUser.email, superUser.password, superUser.userName);
            this.forumManager = new ForumManagerClient(new InstanceContext(new ClientNotificationHost()));
            this.userNonMember = new UserData("nonMem", "nonMempass1", "nonmem@gmail.com");
            this.userMember = new UserData("mem", "Mempass1", "mem@gmail.com");
            this.userAdmin = new UserData("admin", "adminpass", "admin@gmail.com");
            superUserController.addUser(userAdmin.userName, userAdmin.password, userAdmin.email, superUser.userName);
            List<string> adminList = new List<string>();
            adminList.Add("admin");
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            ForumPolicyData fpd = new ForumPolicyData(fp.policy, fp.isQuestionIdentifying, fp.seniorityInForum, fp.deletePostByModerator, fp.timeToPassExpiration, fp.minNumOfModerators,
                                                        fp.hasCapitalInPassword, fp.hasNumberInPassword, fp.minLengthOfPassword, 0, new List<string>());
            ForumData forum = new ForumData(forumName, "descr", fpd, new List<String>(), new List<String>());
            superUserController.createForum(forumName, "descr", fp, adminList, superUser.userName);
            Assert.IsTrue(this.forumManager.registerUser(userMember.userName, userMember.password, userMember.email, "ansss", "anssss", forumName).Equals("Register user succeed"));*/

            DBClass db = DBClass.getInstance;
            db.clear();


            SuperUserController superUserController = SuperUserController.getInstance;
            this.superUser = new UserData("tomer", "1qW", "fkfkf@wkk.com");
            ForumSystem.initialize(superUser.userName, superUser.password, superUser.email);

            this.forumManager = new ForumManagerClient(new InstanceContext(new ClientNotificationHost()));

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
            Assert.AreEqual(this.forumManager.registerUser(userMember1.userName, userMember1.password, userMember1.email, "ansss", "anssss", this.forum.forumName), "Register user succeed");
            Assert.AreEqual(this.forumManager.registerUser(userMember2.userName, userMember2.password, userMember2.email, "ansss", "anssss", this.forum.forumName), "Register user succeed");

            this.postManager = new PostManagerClient();
            this.subForumManager = new SubForumManagerClient();
            this.userManager = new UserManagerClient();
            this.userMod = new UserData("mod", "Modpass1", "mod@gmail.com");
            Dictionary<String, DateTime> modList = new Dictionary<String, DateTime>();
            modList.Add(this.userMod.userName, new DateTime(2030, 1, 1));
            Assert.IsTrue(ForumController.getInstance.registerUser(userMod.userName, userMod.password, userMod.email, "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            Assert.IsTrue(this.forumManager.addSubForum(this.forumName, this.subForumName, modList, this.userAdmin.userName).Equals("sub-forum added"));
            Assert.IsTrue(SubForumController.getInstance.createThread(this.threadHeadline, "content", this.userMember2.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> posts = PostController.getInstance.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
            Assert.IsTrue((skmem1 = this.forumManager.login(userMember1.userName, forum.forumName, userMember1.password)).Contains(","));
            Assert.IsTrue((skmem2 = this.forumManager.login(userMember2.userName, forum.forumName, userMember2.password)).Contains(","));
            Assert.IsTrue((skmod = this.forumManager.login(userMod.userName, forum.forumName, userMod.password)).Contains(","));
            Assert.IsTrue((skadmin = this.forumManager.login(userAdmin.userName, forum.forumName, userAdmin.password)).Contains(","));

        }

        [TestCleanup]
        public void cleanUp()
        {
            this.forumManager.logout(this.userMember1.userName, this.forum.forumName, skmem1);
            this.forumManager.logout(this.userMember2.userName, this.forum.forumName, skmem2);
            this.forumManager.logout(this.userMod.userName, this.forum.forumName, skmod);
            this.forumManager.logout(this.userAdmin.userName, this.forum.forumName, skadmin);
            this.forumManager = null;
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


        /**********************add friend*********************/
        [TestMethod]
        public void test_addFriend_nonMember_with_nonMember()
        {
            List<String> friends = this.userManager.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(0, friends.Count, "");
            Assert.IsFalse(this.userManager.addFriend(this.userNonMember.userName, "heIsNotAMember").Equals("friend was added successfuly"), "non member cannot add a friend");
            List<String> newFriendList = this.userManager.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(0, newFriendList.Count, "unsuccessful friend addition should not change the friend list");
        }

        [TestMethod]
        public void test_addFriend_nonMember_with_member()
        {
            List<String> friends = this.userManager.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(0, friends.Count, "");
            Assert.IsFalse(this.userManager.addFriend(this.userNonMember.userName, this.userMember1.userName).Equals("friend was added successfuly"), "non member cannot add a friend");
            List<String> newFriendList = this.userManager.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(0, newFriendList.Count, "unsuccessful friend addition should not change the friend list");
        }

        [TestMethod]
        public void test_addFriend_nonMember_with_admin()
        {
            List<String> friends = this.userManager.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(0, friends.Count, "");
            Assert.IsFalse(this.userManager.addFriend(this.userNonMember.userName, this.userAdmin.userName).Equals("friend was added successfuly"), "non member cannot add a friend");
            List<String> newFriendList = this.userManager.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(0, newFriendList.Count, "unsuccessful friend addition should not change the friend list");
        }
/*//TODO gal let them live!
        [TestMethod]
        public void test_addFriend_member_with_nonMember()
        {
            List<String> friends = this.userManager.getFriendList(this.userMember.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsFalse(this.userManager.addFriend(this.userMember.userName, "heIsNotAMember"), "member cannot add a non member");
            List<String> newFriendList = this.userManager.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 0, "unsuccessful friend addition should not change the friend list");
        }
        [TestMethod]
        public void test_addFriend_member_with_member()
        {
            String newMemberName = "mem2";
            List<String> friends = this.userManager.getFriendList(this.userMember.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumManager.registerUser(newMemberName, "Mempass12", "mem2@gmail.com", this.forumName));
            Assert.IsTrue(this.userManager.addFriend(this.userMember.userName, newMemberName), "member should be able to add new friend(member)");
            List<String> newFriendList = this.userManager.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(newMemberName));
        }

        [TestMethod]
        public void test_addFriend_member_with_admin()
        {
            List<String> friends = this.userManager.getFriendList(this.userMember.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.userManager.addFriend(this.userMember.userName, this.userAdmin.userName), "member should be able to add new friend(admin)");
            List<String> newFriendList = this.userManager.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(this.userAdmin.userName));
        }

        [TestMethod]
        public void test_addFriend_admin_with_nonMember()
        {
            List<String> friends = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsFalse(this.userManager.addFriend(this.userAdmin.userName, "heIsNotAMember"), "admin cannot add a non member");
            List<String> newFriendList = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 0, "unsuccessful friend addition should not change the friend list");
        }

        [TestMethod]
        public void test_addFriend_admin_with_member()
        {
            String newMemberName = "mem2";
            List<String> friends = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumManager.registerUser(newMemberName, "Mempass12", "mem2@gmail.com", this.forumName));
            Assert.IsTrue(this.userManager.addFriend(this.userAdmin.userName, newMemberName), "admin should be able to add new friend(member)");
            List<String> newFriendList = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(newMemberName));
        }

        [TestMethod]
        public void test_addFriend_admin_with_admin()
        {
            String adminName = "admin2";
            List<String> friends = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumManager.registerUser(adminName, "adminpass", "admin@gmail.com", this.forumName));
            Assert.IsTrue(this.forumManager.nominateAdmin(adminName, this.userAdmin.userName, this.forumName));
            Assert.IsTrue(this.userManager.addFriend(this.userAdmin.userName, adminName), "admin should be able to add new friend(admin)");
            List<String> newFriendList = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(adminName));
        }
*/
/*
        [TestMethod]
        public void test_addFriend_admin_with_null()
        {
            List<String> friends = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsFalse(this.userManager.addFriend(this.userAdmin.userName, null), "adding friend with null should fail");
            List<String> newFriendList = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 0, "unsuccessful friend addition should not change the friend list");
        }
*/
        /**********************end of add friend*********************/

        /**********************delete friend*********************/

   /*     [TestMethod]
        public void test_deleteFriend_member_with_member()
        {
            String newMemberName = "mem2";
            List<String> friends = this.userManager.getFriendList(this.userMember.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumManager.registerUser(newMemberName, "Mempass12", "mem2@gmail.com", this.forumName));
            Assert.IsTrue(this.userManager.addFriend(userMember.userName, newMemberName));
            Assert.IsTrue(this.userManager.deleteFriend(this.userMember.userName, newMemberName), "member should be able to add new friend(member)");
            Assert.IsTrue(this.userManager.addFriend(userMember.userName, newMemberName));
            List<String> newFriendList = this.userManager.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(newMemberName));
            Assert.IsTrue(this.userManager.deleteFriend(this.userMember.userName, newMemberName), "deleting friend should be successful");
            Assert.AreEqual(newFriendList.Count, 0, "friend list size should increase from 1 to 0");
            Assert.IsFalse(newFriendList.Contains(newMemberName));
        }

        [TestMethod]
        public void test_deleteFriend_member_with_admin()
        {
            List<String> friends = this.userManager.getFriendList(this.userMember.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsFalse(this.userManager.deleteFriend(this.userMember.userName, this.userAdmin.userName), "member should be able to add new friend(admin)");
            Assert.IsTrue(this.userManager.addFriend(userMember.userName, userAdmin.userName));
            List<String> newFriendList = this.userManager.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(this.userAdmin.userName));
            Assert.IsTrue(this.userManager.deleteFriend(this.userMember.userName, this.userAdmin.userName), "deleting friend should be successful");
            Assert.AreEqual(newFriendList.Count, 0, "friend list size should increase from 1 to 0");
            Assert.IsFalse(newFriendList.Contains(this.userAdmin.userName));
        }
        [TestMethod]
        public void test_deleteFriend_admin_with_member()
        {
            String newMemberName = "mem2";
            List<String> friends = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumManager.registerUser(newMemberName, "Mempass12", "mem2@gmail.com", this.forumName));
            Assert.IsTrue(this.userManager.addFriend(userAdmin.userName, newMemberName));
            Assert.IsTrue(this.userManager.deleteFriend(this.userAdmin.userName, newMemberName), "admin should be able to add new friend(member)");
            Assert.IsTrue(this.userManager.addFriend(userAdmin.userName, newMemberName));
            List<String> newFriendList = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(newMemberName));
            Assert.IsTrue(this.userManager.deleteFriend(this.userAdmin.userName, newMemberName), "deleting friend should be successful");
            Assert.AreEqual(newFriendList.Count, 0, "friend list size should increase from 1 to 0");
            Assert.IsFalse(newFriendList.Contains(newMemberName));
        }

        [TestMethod]
        public void test_deleteFriend_admin_with_admin()
        {
            String adminName = "admin2";
            List<String> friends = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumManager.registerUser(adminName, "adminpass", "admin@gmail.com", this.forumName));
            Assert.IsTrue(this.forumManager.nominateAdmin(adminName, this.userAdmin.userName, this.forumName));
            Assert.IsFalse(this.userManager.deleteFriend(this.userAdmin.userName, adminName), "admin should be able to add new friend(admin)");
            Assert.IsTrue(this.userManager.addFriend(userAdmin.userName, adminName));
            List<String> newFriendList = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(adminName));
            Assert.IsTrue(this.userManager.deleteFriend(this.userAdmin.userName, adminName), "deleting friend should be successful");
            Assert.AreEqual(newFriendList.Count, 0, "friend list size should increase from 1 to 0");
            Assert.IsFalse(newFriendList.Contains(adminName));
        }
        */
        /*
        [TestMethod]
        public void test_deleteFriend_admin_with_null()
        {
            List<String> friends = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsFalse(this.userManager.deleteFriend(this.userAdmin.userName, null), "deleting friend with null should fail");
            List<String> newFriendList = this.userManager.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 0, "unsuccessful friend addition should not change the friend list");
        }
        */
    }
}
