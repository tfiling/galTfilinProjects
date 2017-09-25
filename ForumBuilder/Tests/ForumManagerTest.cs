using ForumBuilder.Common.DataContracts;
using ForumBuilder.Common.ServiceContracts;
using ForumBuilder.Systems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BL_Back_End;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForumBuilder.Controllers;
using Service;
using PL.notificationHost;
using PL.proxies;
using System.ServiceModel.Description;
using System.ServiceModel;
using Database;

namespace Tests
{
    [TestClass]
    public class ForumManagerTest
    {

        private IForumManager forumManager;
        private ForumData forum;
        private UserData userNonMember;
        private UserData userMember;
        private UserData userAdmin;
        private UserData userAdmin2;
        private UserData superUser;

        [TestInitialize]
        public void setUp()
        {
            SuperUserController superUserController = SuperUserController.getInstance;
            this.superUser = new UserData("tomer", "1qW", "fkfkf@wkk.com");
            superUserController.addSuperUser(this.superUser.email, this.superUser.password, this.superUser.userName);
            this.forumManager = new ForumManagerClient(new InstanceContext(new ClientNotificationHost()));
            this.userNonMember = new UserData("nonMem", "nonMempass1", "nonmem@gmail.com");
            this.userMember = new UserData("mem", "Mempass1", "mem@gmail.com");
            this.userAdmin = new UserData("admin", "adminpass", "admin@gmail.com");
            this.userAdmin2 = new UserData("admin2", "adminpass2", "admin2@gmail.com");
            superUserController.addUser(userAdmin.userName, userAdmin.password, userAdmin.email, superUser.userName);
            superUserController.addUser(userAdmin2.userName, userAdmin2.password, userAdmin2.email, superUser.userName);
            List<string> adminList = new List<string>();
            adminList.Add("admin");
            adminList.Add("admin2");
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            ForumPolicyData fpd = new ForumPolicyData(fp.policy, fp.isQuestionIdentifying, fp.seniorityInForum, fp.deletePostByModerator, fp.timeToPassExpiration, fp.minNumOfModerators,
                                                        fp.hasCapitalInPassword, fp.hasNumberInPassword, fp.minLengthOfPassword, 0, new List<string>());
            this.forum = new ForumData("testForum", "descr", fpd, new List<String>(), new List<String>());
            superUserController.createForum("testForum", "descr",fp, adminList, superUser.userName);
            Assert.IsTrue(this.forumManager.registerUser(userMember.userName, userMember.password, userMember.email, "ansss", "anssss", this.forum.forumName).Equals("Register user succeed"));
        }

        [TestCleanup]
        public void cleanUp()
        {
            this.forumManager = null;
            this.forum = null;
            this.userNonMember = null;
            this.userMember = null;
            this.userAdmin = null;
            this.userAdmin2 = null;
            DBClass db = DBClass.getInstance;
            db.clear();
        }

        /******************************dismiss admin***********************************/

        [TestMethod]
        public void test_DismissAdmin_on_non_member()
        {

            Assert.IsFalse(this.forumManager.isAdmin(this.userNonMember.userName, this.forum.forumName), "userNonMember should not be a member in the forum");
            Assert.IsFalse(this.forumManager.dismissAdmin(this.userNonMember.userName, this.userAdmin2.userName, this.forum.forumName), "userNonMember is not a member in the forum hence his dismissal from being administrator should be failure");
            Assert.IsFalse(this.forumManager.isAdmin(this.userNonMember.userName, this.forum.forumName), "userNonMember should not be a administrator in the forum");
        }

        [TestMethod]
        public void test_DismissAdmin_on_member_non_admin()
        {
            String memName = this.userMember.userName;
            String forumName = this.forum.forumName;
            String admin2Name = this.userAdmin2.userName;
            Assert.IsFalse(this.forumManager.isAdmin(memName, forumName), "userMember should not be an admin in the forum");
            Assert.IsFalse(this.forumManager.dismissAdmin(memName, admin2Name, forumName), "userMember is not an administrator in the forum hence his dismissal from being administrator should Not be successful");
            Assert.IsFalse(this.forumManager.isAdmin(memName, forumName), "userMember should not be a administrator in the forum");
        }

        [TestMethod]
        public void test_DismissAdmin_on_member_admin()
        {
            String userAdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            String adminName = this.userAdmin.userName;
            String admin2Name = this.userAdmin2.userName;
            Assert.IsTrue(this.forumManager.isAdmin(adminName, forumName), "userAdmin should be an admin in the forum");
            Assert.IsTrue(this.forumManager.dismissAdmin(adminName, superUser.userName, forumName), "userAdmin is an administrator in the forum. his dismissal from being administrator should be successful");
            Assert.IsFalse(this.forumManager.isAdmin(adminName, forumName), "userAdmin should not be a administrator in the forum");
        }

        [TestMethod]
        public void test_dismissAdmin_on_null_dismissed()
        {
            Assert.IsFalse(this.forumManager.dismissAdmin(null, this.userAdmin.userName, this.forum.forumName), "dismiss admin on null should not be successful");
        }

        [TestMethod]
        public void test_dismissAdmin_on_null_dismissor()
        {
            Assert.IsFalse(this.forumManager.dismissAdmin(this.userAdmin.userName, null, this.forum.forumName), "dismiss admin on null should not be successful");
        }

        /*****************************end of dismiss admin***********************************/

        /******************************ban member***********************************/

        [TestMethod]
        public void test_banMember_on_non_member()
        {
            String NonMemberName = this.userNonMember.userName;
            String AdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumManager.isMember(NonMemberName, forumName), "userNonMember should not be a member");
            Assert.IsFalse(this.forumManager.banMember(NonMemberName, AdminName, forumName), "ban of userNonMember should Not be successful");
            Assert.IsFalse(this.forumManager.isMember(NonMemberName, forumName), "userNonMember should not be a member");
        }
/*
        [TestMethod]
        public void test_banMember_on_member()
        {
            String MemberName = this.userMember.userName;
            String AdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumManager.isMember(MemberName, forumName), "userMember should be a member");
            Assert.IsTrue(this.forumManager.banMember(MemberName, AdminName, forumName), "ban of userMember should be successful");
            Assert.IsFalse(this.forumManager.isMember(MemberName, forumName), "userMember should not be a member when banned");
            Assert.IsFalse(this.forumManager.registerUser(this.userMember.userName, this.userMember.password, this.userMember.email, forumName), "userMember should not be able to become a member since he is banned");
            Assert.IsFalse(this.forumManager.isMember(MemberName, forumName), "userMember should not be a member when banned");
        }
        [TestMethod]
        public void test_banMember_on_admin()
        {
            String AdminName = this.userAdmin.userName;
            String AdminName2 = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumManager.isMember(AdminName, forumName), "userAdmin should be a member");
            Assert.IsTrue(this.forumManager.banMember(AdminName, AdminName2, forumName), "ban of userAdmin should not be successful");
            Assert.IsFalse(this.forumManager.isMember(AdminName, forumName), "userMember should not be a member when banned");
            Assert.IsFalse(this.forumManager.registerUser(AdminName, this.userAdmin.password, this.userAdmin.email, forumName), "userMember should not be able to become a member since he is banned");
            Assert.IsFalse(this.forumManager.isMember(AdminName, forumName), "userMember should not be a member when banned");
        }
*/

        [TestMethod]
        public void test_banMember_on_null()
        {
            Assert.IsFalse(this.forumManager.banMember(null, this.userAdmin2.userName, this.forum.forumName), "ban of null should not be successful");
        }

        /*****************************end of ban member***********************************/

        /******************************nominate admin***********************************/
        [TestMethod]
        public void test_nominateAdmin_on_non_member()
        {
            String NonMemberName = this.userNonMember.userName;
            String AdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumManager.isMember(NonMemberName, forumName), "userNonMember should not be a member");
            Assert.IsFalse(this.forumManager.nominateAdmin(NonMemberName, AdminName, forumName).Equals("admin nominated successfully"), "nomination of non member to be admin should NOT be successful");
        }

        [TestMethod]
        public void test_nominateAdmin_on_member()
        {
            String userMemberName = this.userMember.userName;
            String AdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumManager.isMember(userMemberName, forumName), "userMember should be a member in the forum");
            Assert.IsFalse(this.forumManager.isAdmin(userMemberName, forumName), "userMember should not be an admin in the forum");
            Assert.IsTrue(this.forumManager.nominateAdmin(userMemberName, this.superUser.userName, forumName).Equals("admin nominated successfully"), "the nomination of userMember should be successful");
            Assert.IsTrue(this.forumManager.isMember(userMemberName, forumName), "userMember should be a member in the forum");
            Assert.IsTrue(this.forumManager.isAdmin(userMemberName, forumName), "userMember should be an admin in the forum after the nomination");
        }

        [TestMethod]
        public void test_nominateAdmin_on_admin()
        {
            String userAdminName = this.userAdmin.userName;
            String AdminName2 = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumManager.isMember(userAdminName, forumName), "userAdmin should be a member in the forum");
            Assert.IsTrue(this.forumManager.isAdmin(userAdminName, forumName), "userAdmin should be an admin in the forum");
            Assert.IsFalse(this.forumManager.nominateAdmin(userAdminName, AdminName2, forumName).Equals("admin nominated successfully"), "userAdmin is already admin. the nomination should NOT be successful");
            Assert.IsTrue(this.forumManager.isMember(userAdminName, forumName), "userAdmin should still be a member in the forum");
            Assert.IsTrue(this.forumManager.isAdmin(userAdminName, forumName), "userAdmin should still be an admin in the forum");
        }

        [TestMethod]
        public void test_nominateAdmin_on_null()
        {
            Assert.IsFalse(this.forumManager.nominateAdmin(null, this.userAdmin.userName, this.forum.forumName).Equals("admin nominated successfully"), "nomination of null should return false");
        }

        /******************************end of nominate admin***********************************/

        /******************************register user***********************************/
        [TestMethod]
        public void test_registerUser_on_non_member()
        {
            String userNonMemberName = this.userNonMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumManager.isMember(userNonMemberName, forumName), "userNonMember should not be a member");
            Assert.IsTrue(this.forumManager.registerUser(this.userNonMember.userName, this.userNonMember.password, this.userNonMember.email, "ansss", "anssss", forumName).Equals("Register user succeed"), "registration of a non member should be successful");
            Assert.IsTrue(this.forumManager.isMember(userNonMemberName, forumName), "after registration the user should become a member");
        }

        [TestMethod]
        public void test_registerUser_on_member()
        {
            String userMemberName = this.userMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumManager.isMember(userMemberName, forumName), "userMember should be a member in the forum");
            Assert.IsFalse(this.forumManager.registerUser(this.userMember.userName, this.userMember.password, this.userMember.email, "ansss", "anssss", forumName).Equals("Register user succeed"), "the registration of a member should be unsuccessful");
            Assert.IsTrue(this.forumManager.isMember(userMemberName, forumName), "userMember should still be a member in the forum");
        }

        [TestMethod]
        public void test_registerUser_on_admin()
        {
            String userAdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumManager.isMember(userAdminName, forumName), "userAdmin should be a member in the forum");
            Assert.IsTrue(this.forumManager.isAdmin(userAdminName, forumName), "userAdmin should be an admin in the forum");
            Assert.IsFalse(this.forumManager.registerUser(this.userAdmin.userName, this.userAdmin.password, this.userAdmin.email, "ansss", "anssss", forumName).Equals("Register user succeed"), "the registration of an admin should be successful");
            Assert.IsTrue(this.forumManager.isMember(userAdminName, forumName), "userAdmin should still be a member in the forum");
        }

        [TestMethod]
        public void test_registerUser_on_null()
        {
            Assert.IsFalse(this.forumManager.registerUser(null, null, null,null,null ,null).Equals("Register user succeed"), "registration of null should return false");
        }


        /******************************end of register user***********************************/

        /******************************set Forum Preferences***********************************/
      /*  [TestMethod]
        public void test_setForumPreferences_valid_policy()
        {
            String forumName = this.forum.forumName;
            String oldPolicy = this.forumManager.getForumPolicy(forumName);
            String newPolicy = "new policy for test";
            String oldDescription = this.forumManager.getForumDescription(forumName);
            String newDescr = "new description";
            String adminName = this.userAdmin.userName;
            Assert.AreNotEqual(oldPolicy, newPolicy, false, "the new policy should be different from the old one");
            Assert.AreNotEqual(oldDescription, newDescr, false, "the new description should be different from the old one");
            ForumPolicyData fpd = new ForumPolicyData("new policy for test", true, 0, true, 180, 1, true, true, 5);
            Assert.IsTrue(this.forumManager.setForumPreferences(forumName, newDescr,fpd, adminName).Equals("preferences had changed successfully"), "policy change should be successful");
            Assert.AreEqual(this.forumManager.getForumPolicy(forumName), newPolicy, false, "the new policy should be return after the change");
            Assert.AreEqual(this.forumManager.getForumDescription(forumName), newDescr, false, "the new description should be return after the change");

        }*/


        public void test_setForumPreferences_with_null()
        {
            String forumName = this.forum.forumName;
            String oldPolicy = this.forumManager.getForumPolicy(forumName);
            String oldDescr = this.forumManager.getForumDescription(forumName);
            String adminName = this.userAdmin.userName;
            ForumPolicyData fpd = new ForumPolicyData(null, true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            Assert.IsFalse(this.forumManager.setForumPreferences(forumName, null, fpd, adminName).Equals("preferences had changed successfully"), "policy change with null should not be successful");
            Assert.AreEqual(this.forumManager.getForumPolicy(forumName), oldPolicy, false, "after an unsuccessful change, the old policy should be returned");
            Assert.AreEqual(this.forumManager.getForumDescription(forumName), oldDescr, false, "after an unsuccessful change, the old description should be returned");

        }

        [TestMethod]
        public void test_setForumPreferences_with_empty_string()
        {
            String forumName = this.forum.forumName;
            String oldPolicy = this.forumManager.getForumPolicy(forumName);
            String oldDescr = this.forumManager.getForumDescription(forumName);
            String adminName = this.userAdmin.userName;
            ForumPolicyData fpd = new ForumPolicyData("", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            Assert.IsTrue(this.forumManager.setForumPreferences(forumName,"",fpd, adminName).Equals("preferences had changed successfully"), "policy change with null should not be successful");
            Assert.AreEqual(this.forumManager.getForumPolicy(forumName), "", false, "after an unsuccessful change, the old policy should be returned");
            Assert.AreEqual(this.forumManager.getForumDescription(forumName), "", false, "after an unsuccessful change, the old description should be returned");
        }

        /******************************end of change policy***********************************/

        /******************************is admin***********************************/


        [TestMethod]
        public void test_isAdmin_on_non_member()
        {
            String userNonMemberName = this.userNonMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumManager.isAdmin(userNonMemberName, forumName), "is admin on non member should return false");
        }

        [TestMethod]
        public void test_isAdmin_on_member()
        {
            String userMemberName = this.userMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumManager.isAdmin(userMemberName, forumName), "is admin on member (not admin) should return false");
        }

        [TestMethod]
        public void test_isAdmin_on_admin()
        {
            String userAdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumManager.isAdmin(userAdminName, forumName), "is admin on admin should return true");
        }

        [TestMethod]
        public void test_isAdmin_on_null()
        {
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumManager.isAdmin(null, forumName), "is admin on null should return false");
        }

        [TestMethod]
        public void test_isAdmin_on_empty_string()
        {
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumManager.isAdmin("", forumName), "is admin on an empty string should return false");
        }


        /******************************end of is admin***********************************/

        /******************************is member***********************************/


        [TestMethod]
        public void test_isMember_on_non_member()
        {
            String userNonMemberName = this.userNonMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumManager.isMember(userNonMemberName, forumName), "is member on non member should return false");
        }

        [TestMethod]
        public void test_isMember_on_member()
        {
            String userMemberName = this.userMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumManager.isMember(userMemberName, forumName), "is member on member (not admin) should return true");
        }

        [TestMethod]
        public void test_isMember_on_admin()
        {
            String userAdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumManager.isMember(userAdminName, forumName), "is member on admin should return true");
        }

        [TestMethod]
        public void test_isMember_on_null()
        {
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumManager.isMember(null, forumName), "is member on null should return false");
        }

        [TestMethod]
        public void test_isMember_on_empty_string()
        {
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumManager.isMember("", forumName), "is member on an empty string should return false");
        }



    }
}
