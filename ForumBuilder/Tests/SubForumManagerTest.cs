using BL_Back_End;
using Database;
using ForumBuilder.Common.DataContracts;
using ForumBuilder.Common.ServiceContracts;
using ForumBuilder.Systems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using PL.notificationHost;
using PL.proxies;
using ForumBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class SubForumManagerTest
    {
        private IForumManager forumManager;
        private IPostManager postManager;
        private ForumData forum;
        private UserData userNonMember;
        private UserData userMember;
        private UserData userModerator;
        private UserData userAdmin;
        private ISubForumManager subForum;
        private String subForumName = "subforum";
        private String forumName = "testForum";
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
            superUserController.addUser(userAdmin.userName, userAdmin.password, userAdmin.email, superUser.userName);
            List<string> adminList = new List<string>();
            adminList.Add(this.userAdmin.userName);
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            ForumPolicyData fpd = new ForumPolicyData(fp.policy, fp.isQuestionIdentifying, fp.seniorityInForum, fp.deletePostByModerator, fp.timeToPassExpiration, fp.minNumOfModerators,
                                                        fp.hasCapitalInPassword, fp.hasNumberInPassword, fp.minLengthOfPassword, 0, new List<string>());
            this.forum = new ForumData(this.forumName, "descr", fpd, new List<String>(), new List<String>());
            superUserController.createForum(this.forum.forumName, "descr", fp, adminList, superUser.userName);
            Assert.IsTrue(this.forumManager.registerUser(userMember.userName, userMember.password, userMember.email, "ansss", "anssss", this.forum.forumName).Equals("Register user succeed"));
            this.postManager = new PostManagerClient();
            this.userModerator = new UserData("mod", "Modpass1", "mod@gmail.com");
            Dictionary<String, DateTime> modList = new Dictionary<String, DateTime>();
            modList.Add(this.userModerator.userName, new DateTime(2030, 1, 1));
            Assert.IsTrue(ForumController.getInstance.registerUser("mod", "Modpass11", "mod@gmail.com", "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            Assert.IsTrue(this.forumManager.addSubForum(this.forumName, this.subForumName, modList, this.userAdmin.userName).Equals("sub-forum added"));
            Assert.IsTrue(SubForumController.getInstance.createThread("headLine", "content", this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            this.subForum = new SubForumManagerClient();

        }

        [TestCleanup]
        public void cleanUp()
        {
            DBClass db = DBClass.getInstance;
            db.clear();
            this.forumManager = null;
            this.forum = null;
            this.userNonMember = null;
            this.userMember = null;
            this.userModerator = null;
            this.userAdmin = null;
            this.subForum = null;
        }


/*        [TestMethod]
        public void test_dismissModerator_on_valid_moderator()
        {
            String userModeratorName = this.userModerator.userName;
            Assert.IsTrue(this.subForum.isModerator(userModeratorName, this.subForumName, this.forumName), "user moderator should be a moderator");
            Assert.IsTrue(this.subForum.dismissModerator(userModeratorName, this.userAdmin.userName, this.subForumName, this.forumName), "the dismissal of user moderator should be successful");
            Assert.IsFalse(this.subForum.isModerator(userModeratorName, this.subForumName, this.forumName), "user moderator should not be a moderator after his dismissal");
        }
        */
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




       /* [TestMethod]
        public void test_nominateModerator_on_member()
        {
            String memberName = this.userMember.userName;
            Assert.IsTrue(this.subForum.nominateModerator(memberName, this.userAdmin.userName, new DateTime(2030, 1, 1), this.subForumName, this.forumName), "nomination of member user should be successful");
            Assert.IsTrue(this.subForum.isModerator(memberName, this.subForumName, this.forumName), "member should be moderator after his successful numonation");
        }

        [TestMethod]
        public void test_nominateModerator_on_moderator()
        {
            String moderatorName = this.userModerator.userName;
            Assert.IsFalse(this.subForum.nominateModerator(moderatorName, this.userAdmin.userName, new DateTime(2030, 1, 1), this.subForumName, this.forumName), "nomination of moderator that already exists should not be successful");
            Assert.IsTrue(this.subForum.isModerator(moderatorName, this.subForumName, this.forumName), "moderator user should still br a moderator");
        }*/

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


/*
        [TestMethod]
        public void test_createThread_mem_valid_input()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
        }

        [TestMethod]
        public void test_createThread_admin_valid_input()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userAdmin.userName);
        }
*/
/*
        [TestMethod]
        public void test_createThread_mem_empty_headLine()
        {
            String headLine = "";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
        }

        [TestMethod]
        public void test_createThread_admin_empty_headLine()
        {
            String headLine = "";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userAdmin.userName);
        }
*/
/*
        [TestMethod]
        public void test_createThread_mem_empty_content()
        {
            String headLine = "head";
            String content = "";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
        }

        [TestMethod]
        public void test_createThread_admin_empty_content()
        {
            String headLine = "head";
            String content = "";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userAdmin.userName);
        }
*/
/*
        [TestMethod]
        public void test_createThread_mem_empty_inputs()
        {
            String headLine = "";
            String content = "";
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }

        [TestMethod]
        public void test_createThread_admin_empty_inputs()
        {
            String headLine = "";
            String content = "";
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }
*/
/*
        [TestMethod]
        public void test_createThread_mem_null_inputs()
        {
            String headLine = null;
            String content = null;
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }

        [TestMethod]
        public void test_createThread_admin_null_inputs()
        {
            String headLine = null;
            String content = null;
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }
*/
/*
        [TestMethod]
        public void test_createThread_invalid_userName()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.subForum.createThread(headLine, content, "donJoe", this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }
*/
/*
        [TestMethod]
        public void test_createThread_invalid_forumName()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userMember.userName, "notForum", this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }

        [TestMethod]
        public void test_createThread_invalid_subForumName()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, "notSubForum"));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }
        [TestMethod]
        public void test_createThread_nonMember()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.subForum.createThread(headLine, content, this.userNonMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }

        


        [TestMethod]
        public void test_deleteMemThread_by_mem()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
            Assert.IsTrue(this.subForum.deleteThread(post.id, this.userMember.userName));
            postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }


        [TestMethod]
        public void test_deleteMemThread_by_admin()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
            Assert.IsTrue(this.subForum.deleteThread(post.id, this.userAdmin.userName));
            postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }


        [TestMethod]
        public void test_deleteAdminThread_by_mem()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userAdmin.userName);
            Assert.IsFalse(this.subForum.deleteThread(post.id, this.userMember.userName));
            postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
        }


        [TestMethod]
        public void test_deleteAdminThread_by_admin()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userAdmin.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userAdmin.userName);
            Assert.IsTrue(this.subForum.deleteThread(post.id, this.userAdmin.userName));
            postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 0);
        }
  */

    /*    [TestMethod]
        public void test_deleteThread_invalid_id()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
            Assert.IsFalse(this.subForum.deleteThread(post.id + 1, this.userAdmin.userName));
            postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
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
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
            Assert.IsFalse(this.subForum.deleteThread(post.id, this.userNonMember.userName));
            postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
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
            Assert.IsTrue(this.subForum.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName));
            List<PostData> postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            PostData post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
            Assert.IsFalse(this.subForum.deleteThread(post.id, null));
            postList = this.postManager.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postList.Count, 1);
            post = postList.First();
            Assert.AreEqual(post.title, headLine);
            Assert.AreEqual(post.content, content);
            Assert.AreEqual(post.writerUserName, this.userMember.userName);
        }
*/

        private bool areListsEqual<T>(List<T> list1, List<T> list2)
        {
            return ((list1.Count == list2.Count) &&
                            list1.Except(list2).Any());
        }
    }
}
