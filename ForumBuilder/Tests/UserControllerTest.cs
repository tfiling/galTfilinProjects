using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForumBuilder.Controllers;
using BL_Back_End;
using ForumBuilder.Systems;
using Database;

namespace Tests
{
    [TestClass]
    public class UserControllerTest
    {

        private IUserController userController = UserController.getInstance;
        private IForumController forumController;
        private User userNonMember;
        private User userMember;
        private User userAdmin;
        private String ForumName = "testForum";
        private User superUser;

        [TestInitialize]
        public void setUp()
        {
            DBClass db = DBClass.getInstance;
            db.clear();
            ForumSystem.initialize("tomer", "1qW", "fkfkf@wkk.com");
            this.superUser = new User("tomer", "1qW", "fkfkf@wkk.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            SuperUserController.getInstance.addSuperUser(this.superUser.email, this.superUser.password, this.superUser.userName);
            this.forumController = ForumController.getInstance;
            ISuperUserController superUser = SuperUserController.getInstance;
            this.userNonMember = new User("nonMem", "nonMempass1", "nonmem@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userMember = new User("mem", "Mempass1", "mem@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userAdmin = new User("admin", "adminpass", "admin@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            superUser.addUser("admin", "adminpass", "admin@gmail.com", "tomer");
            List<string> adminList = new List<string>();
            adminList.Add("admin");
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            SuperUserController.getInstance.addSuperUser("fkfkf@wkk.com", "1qW", "tomer");
            superUser.createForum(ForumName, "descr", fp, adminList, "tomer");
            Assert.IsTrue(this.forumController.registerUser("mem", "Mempass1", "mem@gmail.com", "ansss", "anssss", this.ForumName).Equals("Register user succeed"));
 //           Assert.IsTrue(this.forumController.registerUser("admin", "adminpass", "admin@gmail.com", this.ForumName));
            
            
        }

        [TestCleanup]
        public void cleanUp()
        {
            DBClass db = DBClass.getInstance;
            db.clear();
            this.forumController = null;
            this.userNonMember = null;
            this.userMember = null;
            this.userAdmin = null;
        }


        /**********************add friend*********************/
        [TestMethod]
        public void test_addFriend_nonMember_with_nonMember()
        {
            List<String> friends = this.userController.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(friends, null, "");
            Assert.IsFalse(this.userController.addFriend(this.userNonMember.userName, "heIsNotAMember").Equals("friend was added successfuly"), "non member cannot add a friend");
            List<String> newFriendList = this.userController.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(newFriendList, null, "unsuccessful friend addition should not change the friend list");
        }

        [TestMethod]
        public void test_addFriend_nonMember_with_member()
        {
            List<String> friends = this.userController.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(friends, null, "");
            Assert.IsFalse(this.userController.addFriend(this.userNonMember.userName, this.userMember.userName).Equals("friend was added successfuly"), "non member cannot add a friend");
            List<String> newFriendList = this.userController.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(newFriendList, null, "unsuccessful friend addition should not change the friend list");
        }

        [TestMethod]
        public void test_addFriend_nonMember_with_admin()
        {
            List<String> friends = this.userController.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(friends, null, "");
            Assert.IsFalse(this.userController.addFriend(this.userNonMember.userName, this.userAdmin.userName).Equals("friend was added successfuly"), "non member cannot add a friend");
            List<String> newFriendList = this.userController.getFriendList(this.userNonMember.userName);
            Assert.AreEqual(newFriendList,null , "unsuccessful friend addition should not change the friend list");
        }

        [TestMethod]
        public void test_addFriend_member_with_nonMember()
        {
            List<String> friends = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsFalse(this.userController.addFriend(this.userMember.userName, "heIsNotAMember").Equals("friend was added successfuly"), "member cannot add a non member");
            List<String> newFriendList = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 0, "unsuccessful friend addition should not change the friend list");
        }

        [TestMethod]
        public void test_addFriend_member_with_member()
        {
            String newMemberName = "mem2";
            List<String> friends = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumController.registerUser(newMemberName, "Mempass12", "mem2@gmail.com", "ansss", "anssss", this.ForumName).Equals("Register user succeed"));
            Assert.IsTrue(this.userController.addFriend(this.userMember.userName, newMemberName).Equals("friend was added successfuly"), "member should be able to add new friend(member)");
            List<String> newFriendList = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(newMemberName));
        }

        [TestMethod]
        public void test_addFriend_member_with_admin()
        {
            List<String> friends = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.userController.addFriend(this.userMember.userName, this.userAdmin.userName).Equals("friend was added successfuly"), "member should be able to add new friend(admin)");
            List<String> newFriendList = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(this.userAdmin.userName));
        }

        [TestMethod]
        public void test_addFriend_admin_with_nonMember()
        {
            List<String> friends = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsFalse(this.userController.addFriend(this.userAdmin.userName, "heIsNotAMember").Equals("friend was added successfuly"), "admin cannot add a non member");
            List<String> newFriendList = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 0, "unsuccessful friend addition should not change the friend list");
        }

        [TestMethod]
        public void test_addFriend_admin_with_member()
        {
            String newMemberName = "mem2";
            List<String> friends = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumController.registerUser(newMemberName, "Mempass12", "mem2@gmail.com", "ansss", "anssss", this.ForumName).Equals("Register user succeed"));
            Assert.IsTrue(this.userController.addFriend(this.userAdmin.userName, newMemberName).Equals("friend was added successfuly"), "admin should be able to add new friend(member)");
            List<String> newFriendList = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(newMemberName));
        }

        [TestMethod]
        public void test_addFriend_admin_with_admin()
        {
            String adminName = "admin2";
            List<String> friends = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumController.registerUser(adminName, "Adminpass1", "admin@gmail.com", "ansss", "anssss", this.ForumName).Equals("Register user succeed"));
            Assert.IsTrue(this.forumController.nominateAdmin(adminName, this.userAdmin.userName, this.ForumName).Equals("admin nominated successfully"));
            Assert.IsTrue(this.userController.addFriend(this.userAdmin.userName, adminName).Equals("friend was added successfuly"), "admin should be able to add new friend(admin)");
            List<String> newFriendList = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(adminName));
        }

        [TestMethod]
        public void test_addFriend_admin_with_null()
        {
            List<String> friends = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsFalse(this.userController.addFriend(this.userAdmin.userName, null).Equals("friend was added successfuly"), "adding friend with null should fail");
            List<String> newFriendList = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 0, "unsuccessful friend addition should not change the friend list");
        }

        /**********************end of add friend*********************/

        /**********************delete friend*********************/

        [TestMethod]
        public void test_deleteFriend_member_with_member()
        {
            String newMemberName = "mem2";
            List<String> friends = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumController.registerUser(newMemberName, "Mempass12", "mem2@gmail.com", "ansss", "anssss", this.ForumName).Equals("Register user succeed"));
            Assert.IsTrue(this.userController.addFriend(userMember.userName, newMemberName).Equals("friend was added successfuly"));
            Assert.IsTrue(this.userController.deleteFriend(this.userMember.userName, newMemberName).Equals("Remove friend Succeeded"), "member should be able to add new friend(member)");
            Assert.IsTrue(this.userController.addFriend(userMember.userName, newMemberName).Equals("friend was added successfuly"));
            List<String> newFriendList = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(newMemberName));
            Assert.IsTrue(this.userController.deleteFriend(this.userMember.userName, newMemberName).Equals("Remove friend Succeeded"), "deleting friend should be successful");
            newFriendList = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 0, "friend list size should increase from 1 to 0");
            Assert.IsFalse(newFriendList.Contains(newMemberName));
        }

        [TestMethod]
        public void test_deleteFriend_member_with_admin()
        {
            List<String> friends = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsFalse(this.userController.deleteFriend(this.userMember.userName, this.userAdmin.userName).Equals("Remove friend Succeeded"), "member should be able to add new friend(admin)");
            Assert.IsTrue(this.userController.addFriend(userMember.userName, userAdmin.userName).Equals("friend was added successfuly"));
            List<String> newFriendList = this.userController.getFriendList(this.userMember.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(this.userAdmin.userName));
            Assert.IsTrue(this.userController.deleteFriend(this.userMember.userName, this.userAdmin.userName).Equals("Remove friend Succeeded"), "deleting friend should be successful");
            newFriendList = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 0, "friend list size should increase from 1 to 0");
            Assert.IsFalse(newFriendList.Contains(this.userAdmin.userName));
        }

        [TestMethod]
        public void test_deleteFriend_admin_with_member()
        {
            String newMemberName = "mem2";
            List<String> friends = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumController.registerUser(newMemberName, "Mempass12", "mem2@gmail.com", "ansss", "anssss", this.ForumName).Equals("Register user succeed"));
            Assert.IsTrue(this.userController.addFriend(userAdmin.userName, newMemberName).Equals("friend was added successfuly"));
            Assert.IsTrue(this.userController.deleteFriend(this.userAdmin.userName, newMemberName).Equals("Remove friend Succeeded"), "admin should be able to add new friend(member)");
            Assert.IsTrue(this.userController.addFriend(userAdmin.userName, newMemberName).Equals("friend was added successfuly"));
            List<String> newFriendList = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(newMemberName));
            Assert.IsTrue(this.userController.deleteFriend(this.userAdmin.userName, newMemberName).Equals("Remove friend Succeeded"), "deleting friend should be successful");
            newFriendList = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 0, "friend list size should increase from 1 to 0");
            Assert.IsFalse(newFriendList.Contains(newMemberName));
        }

        [TestMethod]
        public void test_deleteFriend_admin_with_admin()
        {
            String adminName = "admin2";
            List<String> friends = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsTrue(this.forumController.registerUser(adminName, "Adminpass1", "admin@gmail.com", "ansss", "anssss", this.ForumName).Equals("Register user succeed"));
            Assert.IsTrue(this.forumController.nominateAdmin(adminName, this.userAdmin.userName, this.ForumName).Equals("admin nominated successfully"));
            Assert.IsFalse(this.userController.deleteFriend(this.userAdmin.userName, adminName).Equals("Remove friend Succeeded"), "admin should be able to add new friend(admin)");
            Assert.IsTrue(this.userController.addFriend(userAdmin.userName, adminName).Equals("friend was added successfuly"));
            List<String> newFriendList = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 1, "friend list size should increase from 0 to 1");
            Assert.IsTrue(newFriendList.Contains(adminName));
            Assert.IsTrue(this.userController.deleteFriend(this.userAdmin.userName, adminName).Equals("Remove friend Succeeded"), "deleting friend should be successful");
            newFriendList = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 0, "friend list size should increase from 1 to 0");
            Assert.IsFalse(newFriendList.Contains(adminName));
        }

        [TestMethod]
        public void test_deleteFriend_admin_with_null()
        {
            List<String> friends = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(friends.Count, 0, "initial friend list should be empty");
            Assert.IsFalse(this.userController.deleteFriend(this.userAdmin.userName, null).Equals("Remove friend Succeeded"), "deleting friend with null should fail");
            List<String> newFriendList = this.userController.getFriendList(this.userAdmin.userName);
            Assert.AreEqual(newFriendList.Count, 0, "unsuccessful friend addition should not change the friend list");
        }

        /******************************restore password*****************************************/
        [TestMethod]
        public void test_restore_password_on_member_where_no_answers()
        {
            string password = userController.restorePassword(userMember.userName, "", "");
            Assert.AreEqual(password, null);
        }
        [TestMethod]
        public void test_restore_password_on_member_with_answers()
        {
            forumController.setAnswers(ForumName, userMember.userName, "tomer", "tomer");
            string password = userController.restorePassword(userMember.userName, "tomer", "tomer");
            Assert.AreEqual(password, userMember.password);
        }

    }
}
