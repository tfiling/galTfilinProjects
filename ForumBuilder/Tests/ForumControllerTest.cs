using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForumBuilder.Controllers;
using BL_Back_End;
using System.Collections.Generic;
using ForumBuilder.Systems;
using Database;

namespace Tests
{
    [TestClass]
    public class ForumControllerTest
    {
        private IForumController forumController;
        private Forum forum;
        private User userNonMember;
        private User userMember;
        private User userAdmin;
        private User userAdmin2;
        private User superUser1;
        private User superUser;

        [TestInitialize]
        public void setUp()
        {
            DBClass db = DBClass.getInstance;
            db.clear();
            ForumSystem.initialize("guy", "AG36djs", "hello@dskkl.com");
            this.superUser = new User("guy", "AG36djs", "hello@dskkl.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            SuperUserController.getInstance.addSuperUser(this.superUser.email, this.superUser.password, this.superUser.userName);
            this.forumController = ForumController.getInstance;
            this.userNonMember = new User("nonMem", "nonMempass1", "nonmem@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userMember = new User("mem", "Mempass1", "mem@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userAdmin = new User("admin", "adminpass", "admin@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userAdmin2 = new User("admin2", "adminpass2", "admin2@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            ISuperUserController superUser = SuperUserController.getInstance;
            superUser.addUser("admin", "adminpass", "admin@gmail.com", "guy");
            superUser.addUser("admin2", "adminpass2", "admin2@gmail.com", "guy");
            List<string> adminList = new List<string>();
            adminList.Add("admin");
            adminList.Add("admin2");
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            this.forum = new Forum("testForum", "descr",fp,adminList);            
            superUser1 = new User("tomer", "1qW23", "fkfkf@wkk.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            SuperUserController.getInstance.addSuperUser(superUser1.email, superUser1.password, superUser1.userName);
            superUser.createForum("testForum", "descr",fp, adminList, "tomer");            
            Assert.IsTrue(this.forumController.registerUser("mem", "Mempass1", "mem@gmail.com", "ansss", "anssss", this.forum.forumName).Equals("Register user succeed"));

        }

        [TestCleanup]
        public void cleanUp()
        {
            this.forumController = null;
            this.forum = null;
            this.userNonMember = null;
            this.userMember = null;
            this.userAdmin = null;
            this.userAdmin2 = null;
            DBClass db = DBClass.getInstance;
            db.clear();
        }


        /******************************nominate admin***********************************/
        [TestMethod]
        public void test_nominateAdmin_on_non_member()
        {
            String NonMemberName = this.userNonMember.userName;
            String AdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumController.isMember(NonMemberName, forumName), "userNonMember should not be a member");
            Assert.IsFalse(this.forumController.nominateAdmin(NonMemberName, AdminName, forumName).Equals("admin nominated successfully"), "nomination of non member to be admin should NOT be successful");
        }

        [TestMethod]
        public void test_nominateAdmin_on_member()
        {
            String userMemberName = this.userMember.userName;
            String AdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumController.isMember(userMemberName, forumName), "userMember should be a member in the forum");
            Assert.IsFalse(this.forumController.isAdmin(userMemberName, forumName), "userMember should not be an admin in the forum");
            Assert.IsTrue(this.forumController.nominateAdmin(userMemberName, this.superUser1.userName, forumName).Equals("admin nominated successfully"), "the nomination of userMember should be successful");
            Assert.IsTrue(this.forumController.isMember(userMemberName, forumName), "userMember should be a member in the forum");
            Assert.IsTrue(this.forumController.isAdmin(userMemberName, forumName), "userMember should be an admin in the forum after the nomination");
        }

        [TestMethod]
        public void test_nominateAdmin_on_admin()
        {
            String userAdminName = this.userAdmin.userName;
            String AdminName2 = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumController.isMember(userAdminName, forumName), "userAdmin should be a member in the forum");
            Assert.IsTrue(this.forumController.isAdmin(userAdminName, forumName), "userAdmin should be an admin in the forum");
            Assert.IsFalse(this.forumController.nominateAdmin(userAdminName, AdminName2, forumName).Equals("admin nominated successfully"), "userAdmin is already admin. the nomination should NOT be successful");
            Assert.IsTrue(this.forumController.isMember(userAdminName, forumName), "userAdmin should still be a member in the forum");
            Assert.IsTrue(this.forumController.isAdmin(userAdminName, forumName), "userAdmin should still be an admin in the forum");
        }

        [TestMethod]
        public void test_nominateAdmin_on_null()
        {
            Assert.IsFalse(this.forumController.nominateAdmin(null, this.userAdmin.userName, this.forum.forumName).Equals("admin nominated successfully"), "nomination of null should return false");
        }

        /******************************end of nominate admin***********************************/

        /******************************register user***********************************/
        [TestMethod]
        public void test_registerUser_on_non_member()
        {
            String userNonMemberName = this.userNonMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumController.isMember(userNonMemberName, forumName), "userNonMember should not be a member");
            Assert.IsTrue(this.forumController.registerUser(this.userNonMember.userName, this.userNonMember.password, this.userNonMember.email, "ansss", "anssss", forumName).Equals("Register user succeed"), "registration of a non member should be successful");
            Assert.IsTrue(this.forumController.isMember(userNonMemberName, forumName), "after registration the user should become a member");
        }

        [TestMethod]
        public void test_registerUser_on_member()
        {
            String userMemberName = this.userMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumController.isMember(userMemberName, forumName), "userMember should be a member in the forum");
            Assert.IsFalse(this.forumController.registerUser(this.userMember.userName, this.userMember.password, this.userMember.email, "ansss", "anssss", forumName).Equals("Register user succeed"), "the registration of a member should be unsuccessful");
            Assert.IsTrue(this.forumController.isMember(userMemberName, forumName), "userMember should still be a member in the forum");
        }

        [TestMethod]
        public void test_registerUser_on_admin()
        {
            String userAdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumController.isMember(userAdminName, forumName), "userAdmin should be a member in the forum");
            Assert.IsTrue(this.forumController.isAdmin(userAdminName, forumName), "userAdmin should be an admin in the forum");
            Assert.IsFalse(this.forumController.registerUser(this.userAdmin.userName, this.userAdmin.password, this.userAdmin.email, "ansss", "anssss", forumName).Equals("Register user succeed"), "the registration of an admin should be successful");
            Assert.IsTrue(this.forumController.isMember(userAdminName, forumName), "userAdmin should still be a member in the forum");
        }

        [TestMethod]
        public void test_registerUser_on_null()
        {
            Assert.IsFalse(this.forumController.registerUser(null, null, null,null,null,null).Equals("Register user succeed"), "registration of null should return false");
        }

         [TestMethod]
        public void test_registerUser_with_shorter_password_then_needed()
       {
           String forumName = this.forum.forumName;
           Assert.IsFalse(this.forumController.registerUser("newUser", "123", "use@gmail.com", "", "", forumName).Equals("Register user succeed"));

       }

         [TestMethod]
         public void test_registerUser_without_answers_where_needed()
         {
             String forumName = this.forum.forumName;
             String adminName = this.userAdmin.userName;
             ForumPolicy fp = new ForumPolicy("new policy", true, 0, true, 365, 1, false, false, 5, 0, new List<string>());
             Assert.IsTrue(this.forumController.setForumPreferences(forumName, "newDescr", fp, adminName).Equals("preferences had changed successfully"), "policy change should be successful");
             Assert.IsFalse(this.forumController.registerUser("newUser", "123", "use@gmail.com", "", "", forumName).Equals("Register user succeed"));
         }
         [TestMethod]
         public void test_registerUser_without_one_answer_where_needed()
         {
             String forumName = this.forum.forumName;
             String adminName = this.userAdmin.userName;
             ForumPolicy fp = new ForumPolicy("new policy", true, 0, true, 365, 1, false, false, 5, 0, new List<string>());
             Assert.IsTrue(this.forumController.setForumPreferences(forumName, "newDescr", fp, adminName).Equals("preferences had changed successfully"), "policy change should be successful");
             Assert.IsFalse(this.forumController.registerUser("newUser", "123", "use@gmail.com", "sss", "", forumName).Equals("Register user succeed"));
         }


         [TestMethod]
         public void test_registerUser_with_answers_where_needed()
         {
             String forumName = this.forum.forumName;
             String adminName = this.userAdmin.userName;
             ForumPolicy fp = new ForumPolicy("new policy", true, 0, true, 365, 1, false, false, 5, 0, new List<string>());
             Assert.IsTrue(this.forumController.setForumPreferences(forumName, "newDescr", fp, adminName).Equals("preferences had changed successfully"), "policy change should be successful");
             Assert.IsTrue(this.forumController.registerUser("newUser", "123452", "use@gmail.com", "ans1", "ans2", forumName).Equals("Register user succeed"));
         }

         [TestMethod]
         public void test_registerUser_without_number_in_password_where_needed()
         {
             String forumName = this.forum.forumName;
             String adminName = this.userAdmin.userName;
             ForumPolicy fp = new ForumPolicy("new policy", true, 0, true, 365, 1, false, true, 5, 0, new List<string>());
             Assert.IsTrue(this.forumController.setForumPreferences(forumName, "newDescr", fp, adminName).Equals("preferences had changed successfully"), "policy change should be successful");
             Assert.IsFalse(this.forumController.registerUser("newUser", "asssdf", "use@gmail.com", "", "", forumName).Equals("Register user succeed"));
         }

         [TestMethod]
         public void test_registerUser_without_capital_in_password_where_needed()
         {
             String forumName = this.forum.forumName;
             String adminName = this.userAdmin.userName;
             ForumPolicy fp = new ForumPolicy("new policy", true, 0, true, 365, 1, true, false, 5, 0, new List<string>());
             Assert.IsTrue(this.forumController.setForumPreferences(forumName, "newDescr", fp, adminName).Equals("preferences had changed successfully"), "policy change should be successful");
             Assert.IsFalse(this.forumController.registerUser("newUser", "asssdf", "use@gmail.com", "", "", forumName).Equals("Register user succeed"));
         }


        /******************************end of register user***********************************/

        /******************************set Forum Preferences***********************************/
        [TestMethod]
        public void test_setForumPreferences_valid_policy()
        {
            String forumName = this.forum.forumName;
            String oldPolicy = this.forumController.getForumPolicy(forumName);
            String newPolicy = "new policy for test";
            String oldDescription = this.forumController.getForumDescription(forumName);
            String newDescr = "new description";
            String adminName = this.userAdmin.userName;
            Assert.AreNotEqual(oldPolicy, newPolicy, false, "the new policy should be different from the old one");
            Assert.AreNotEqual(oldDescription, newDescr, false, "the new description should be different from the old one");
            ForumPolicy fp = new ForumPolicy(newPolicy, true, 0, true, 180, 1, true, true, 2, 0, new List<string>());
            Assert.IsTrue(this.forumController.setForumPreferences(forumName, newDescr, fp, adminName).Equals("preferences had changed successfully"), "policy change should be successful");
            Assert.AreEqual(this.forumController.getForumPolicy(forumName), newPolicy, false, "the new policy should be return after the change");
            Assert.AreEqual(this.forumController.getForumDescription(forumName), newDescr, false, "the new description should be return after the change");;
        }

        [TestMethod]
        public void test_setForumPreferences_some_policy()
        {
            String forumName = this.forum.forumName;
            String adminName = this.userAdmin.userName;
            ForumPolicy fp = new ForumPolicy("new policy", false, 0, true, 365, 1, false, false, 5, 0, new List<string>());
            Assert.IsTrue(this.forumController.setForumPreferences(forumName, "newDescr", fp, adminName).Equals("preferences had changed successfully"), "policy change should be successful");
            Assert.IsFalse(this.forumController.registerUser("newUser", "123", "use@gmail.com", "", "", forumName).Equals("Register user succeed"));
        }


        [TestMethod]
        public void test_setForumPreferences_with_null()
        {
            String forumName = this.forum.forumName;
            String oldPolicy = this.forumController.getForumPolicy(forumName);
            String oldDescr = this.forumController.getForumDescription(forumName);
            String adminName = this.userAdmin.userName;
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            Assert.IsFalse(this.forumController.setForumPreferences(forumName, null, fp, adminName).Equals("preferences had changed successfully"), "policy change with null should not be successful");
            Assert.AreEqual(this.forumController.getForumPolicy(forumName), oldPolicy, false, "after an unsuccessful change, the old policy should be returned");
            Assert.AreEqual(this.forumController.getForumDescription(forumName), oldDescr, false, "after an unsuccessful change, the old description should be returned");

        }
        
        [TestMethod]
        public void test_setForumPreferences_with_empty_string()
        {
            String forumName = this.forum.forumName;
            String oldPolicy = this.forumController.getForumPolicy(forumName);
            String oldDescr = this.forumController.getForumDescription(forumName);
            String adminName = this.userAdmin.userName;
            ForumPolicy fp = new ForumPolicy("", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            Assert.IsTrue(this.forumController.setForumPreferences(forumName, "", fp, adminName).Equals("preferences had changed successfully"), "policy change with empty should not be successful");
            Assert.AreEqual(this.forumController.getForumPolicy(forumName), "", false, "after an unsuccessful change, the old policy should be returned");
            Assert.AreEqual(this.forumController.getForumDescription(forumName), "", false, "after an unsuccessful change, the old description should be returned");
        }
        [TestMethod]
        public void test_setForumPreferences_wrong_value_negative_seniority()
        {
            String forumName = this.forum.forumName;
            String oldPolicy = this.forumController.getForumPolicy(forumName);
            String oldDescr = this.forumController.getForumDescription(forumName);
            String adminName = this.userAdmin.userName;
            ForumPolicy fp = new ForumPolicy("", true,-5, true, 180, 1, true, true, 5, 0, new List<string>());
            Assert.IsFalse(this.forumController.setForumPreferences(forumName, "", fp, adminName).Equals("preferences had changed successfully"), "policy change with negative should not be successful");
            Assert.IsTrue(0==forumController.getForum(forumName).forumPolicy.seniorityInForum,"after an unsuccessful change, the old policy should be returned");
        }
        [TestMethod]
        public void test_setForumPreferences_wrong_value_negative_time_password_expired()
        {
            String forumName = this.forum.forumName;
            String oldPolicy = this.forumController.getForumPolicy(forumName);
            String oldDescr = this.forumController.getForumDescription(forumName);
            String adminName = this.userAdmin.userName;
            ForumPolicy fp = new ForumPolicy("", true, 0, true, -3, 1, true, true, 5, 0, new List<string>());
            Assert.IsFalse(this.forumController.setForumPreferences(forumName, "", fp, adminName).Equals("preferences had changed successfully"), "policy change with negative should not be successful");
            Assert.IsTrue(180 == forumController.getForum(forumName).forumPolicy.timeToPassExpiration, "after an unsuccessful change, the old policy should be returned");

        }
        [TestMethod]
        public void test_setForumPreferences_wrong_value_negative_num_of_moderators()
        {
            String forumName = this.forum.forumName;
            String oldPolicy = this.forumController.getForumPolicy(forumName);
            String oldDescr = this.forumController.getForumDescription(forumName);
            String adminName = this.userAdmin.userName;
            ForumPolicy fp = new ForumPolicy("", true, 0, true, 180,-1, true, true, 5, 0, new List<string>());
            Assert.IsFalse(this.forumController.setForumPreferences(forumName, "", fp, adminName).Equals("preferences had changed successfully"), "policy change with negative should not be successful");
            Assert.IsTrue(1== forumController.getForum(forumName).forumPolicy.minNumOfModerators, "after an unsuccessful change, the old policy should be returned");
        }
        [TestMethod]
        public void test_setForumPreferences_wrong_value_negative_length_of_password()
        {
            String forumName = this.forum.forumName;
            String oldPolicy = this.forumController.getForumPolicy(forumName);
            String oldDescr = this.forumController.getForumDescription(forumName);
            String adminName = this.userAdmin.userName;
            ForumPolicy fp = new ForumPolicy("", true, 0, true, 180, 1, true, true, -1, 0, new List<string>());
            Assert.IsFalse(this.forumController.setForumPreferences(forumName, "", fp, adminName).Equals("preferences had changed successfully"), "policy change with negative should not be successful");
            Assert.IsTrue(5 == forumController.getForum(forumName).forumPolicy.minLengthOfPassword, "after an unsuccessful change, the old policy should be returned");
        }

        /******************************end of change policy***********************************/

        /******************************is admin***********************************/


        [TestMethod]
        public void test_isAdmin_on_non_member()
        {
            String userNonMemberName = this.userNonMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumController.isAdmin(userNonMemberName, forumName), "is admin on non member should return false");
        }

        [TestMethod]
        public void test_isAdmin_on_member()
        {
            String userMemberName = this.userMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumController.isAdmin(userMemberName, forumName), "is admin on member (not admin) should return false");
        }

        [TestMethod]
        public void test_isAdmin_on_admin()
        {
            String userAdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumController.isAdmin(userAdminName, forumName), "is admin on admin should return true");
        }

        [TestMethod]
        public void test_isAdmin_on_null()
        {
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumController.isAdmin(null, forumName), "is admin on null should return false");
        }

        [TestMethod]
        public void test_isAdmin_on_empty_string()
        {
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumController.isAdmin("", forumName), "is admin on an empty string should return false");
        }


        /******************************end of is admin***********************************/

        /******************************is member***********************************/


        [TestMethod]
        public void test_isMember_on_non_member()
        {
            String userNonMemberName = this.userNonMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumController.isMember(userNonMemberName, forumName), "is member on non member should return false");
        }

        [TestMethod]
        public void test_isMember_on_member()
        {
            String userMemberName = this.userMember.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumController.isMember(userMemberName, forumName), "is member on member (not admin) should return true");
        }

        [TestMethod]
        public void test_isMember_on_admin()
        {
            String userAdminName = this.userAdmin.userName;
            String forumName = this.forum.forumName;
            Assert.IsTrue(this.forumController.isMember(userAdminName, forumName), "is member on admin should return true");
        }

        [TestMethod]
        public void test_isMember_on_null()
        {
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumController.isMember(null, forumName), "is member on null should return false");
        }

        [TestMethod]
        public void test_isMember_on_empty_string()
        {
            String forumName = this.forum.forumName;
            Assert.IsFalse(this.forumController.isMember("", forumName), "is member on an empty string should return false");
        }


        /******************************end of is member***********************************/
        [TestMethod]
        public void test_get_Admin_Report_Post_Of_member_no_post()
        {
            String forumName = this.forum.forumName;
            Assert.AreEqual(this.forumController.getAdminReportPostOfmember(userAdmin.userName, forumName, userMember.userName).Count, 0);
        }

        [TestMethod]
        public void test_get_Admin_Report_Post_Of_member_not_admin()
        {
            String forumName = this.forum.forumName;
            Assert.IsNull(this.forumController.getAdminReportPostOfmember(userMember.userName, forumName, userMember.userName),"user is not admin and not allowed to get report, didn't return null");
        }
        [TestMethod]
        public void test_get_Admin_Report_Post_Of_member()
        {
            String forumName = this.forum.forumName;
            Dictionary<String,DateTime> mods=new Dictionary<String,DateTime>();
            mods.Add(userAdmin.userName, new DateTime (2017,8,8));
            Assert.IsTrue(this.forumController.addSubForum(this.forum.forumName, "sub",mods , this.userAdmin.userName).Equals("sub-forum added"));
            SubForumController.getInstance.createThread("head", "content", userMember.userName, forumName, "sub");
            Assert.AreEqual(this.forumController.getAdminReportPostOfmember(userAdmin.userName, forumName, userMember.userName).Count, 1);
            Post s=this.forumController.getAdminReportPostOfmember(userAdmin.userName, forumName, userMember.userName)[0];
            Assert.AreEqual(s.title, "head");
            Assert.AreEqual(s.content, "content");
        }




        /******************************dismiss member***********************************/
/*
        [TestMethod]
        public void test_dismissMember_on_non_member()
        {
            String userNonMemberName = this.userNonMember.userName;
            String forumName = this.forum.forumName;
            String AdminName = this.userAdmin.userName;
            Assert.IsFalse(this.forumController.isMember(userNonMemberName, forumName), "non member user should not be a member");
            Assert.IsTrue(this.forumController.dismissMember(this.userNonMember, AdminName, forumName), "dismiss member on a non member should be successful");
            Assert.IsFalse(this.forumController.isMember(userNonMemberName), "non member user should remain as non member after his dismissal");
        }

        [TestMethod]
        public void test_dismissMember_on_member()
        {
            String userMemberName = this.userMember.userName;
            Assert.IsTrue(this.forumController.isMember(userMemberName), "user member should be a member in the forum");
            Assert.IsTrue(this.forumController.dismissMember(this.userMember), "dismiss member on member user should be successful");
            Assert.IsFalse(this.forumController.isMember(userMemberName), "after dismissal member user should not be a member anymore");
        }

        [TestMethod]
        public void test_dismissMember_on_admin()
        {
            String userAdminName = this.userAdmin.userName;
            Assert.IsTrue(this.forumController.isMember(userAdminName), "user admin should be a member in the forum");
            Assert.IsFalse(this.forumController.dismissMember(this.userAdmin), "dismiss member on admin user should be unsuccessful");
            Assert.IsTrue(this.forumController.isAdmin(userAdminName), "after unsuccessful dismissal, admin user should still be an admin");
            Assert.IsTrue(this.forumController.isMember(userAdminName), "after unsuccessful dismissal, admin user should still not be a member");
        }

        [TestMethod]
        public void test_dismissMember_on_null()
        {
            Assert.IsFalse(this.forumController.dismissMember(null), "dismiss member on null should return false");
        }

*/
        /******************************end of dismiss member***********************************/






    }
}
