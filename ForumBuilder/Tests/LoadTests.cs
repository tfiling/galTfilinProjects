using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL_Back_End;
using Database;
using ForumBuilder.Controllers;
using ForumBuilder.Systems;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class LoadTests
    {
        private IForumController forumController;
        private IPostController postController;
        private ISubForumController subForumController;
        private Forum forum;
        private User userNonMember;
        private User userMember;
        private User userMod;
        private User userAdmin;
        private User superUser;
        private String forumName = "forum";
        private String subForumName = "subForum";
        private int postId;

        const int INITIAL_COMMENT_COUNT = 1;

        [TestInitialize]
        public void setUp()
        {
            DBClass db = DBClass.getInstance;
            db.clear();
            ForumSystem.initialize("guy", "AG36djs", "hello@dskkl.com");
            ISuperUserController superUserController = SuperUserController.getInstance;
            this.superUser = new User("guy", "AG36djs", "hello@dskkl.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            SuperUserController.getInstance.addSuperUser(this.superUser.email, this.superUser.password, this.superUser.userName);
            this.forumController = ForumController.getInstance;
            this.postController = PostController.getInstance;
            this.subForumController = SubForumController.getInstance;
            this.userNonMember = new User("nonMem", "nonMempass1", "nonmem@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userMember = new User("mem", "Mempass1", "mem@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userMod = new User("mod", "Modpass1", "mod@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            this.userAdmin = new User("admin", "Adminpass1", "admin@gmail.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            superUserController.addUser("admin", "Adminpass1", "admin@gmail.com", "guy");
            List<string> adminList = new List<string>();
            adminList.Add(this.userAdmin.userName);
            Dictionary<String, DateTime> modList = new Dictionary<String, DateTime>();
            modList.Add(this.userMod.userName, new DateTime(2030, 1, 1));
            ForumPolicy fp = new ForumPolicy("p", true, 0, true, 180, 1, true, true, 5, 0, new List<string>());
            this.forum = new Forum(this.forumName, "descr", fp, adminList);
            this.superUser = new User("tomer", "1qW", "fkfkf@wkk.com", new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day));
            SuperUserController.getInstance.addSuperUser(this.superUser.email, superUser.password, superUser.userName);
            superUserController.createForum(this.forumName, "descr", fp, adminList, "tomer");
            Assert.IsTrue(this.forumController.registerUser("mem", "Mempass1", "mem@gmail.com", "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            Assert.IsTrue(this.forumController.registerUser("mod", "Modpass1", "mod@gmail.com", "ansss", "anssss", this.forumName).Equals("Register user succeed"));
            //Assert.IsTrue(this.forumController.registerUser("admin", "adminpass", "admin@gmail.com", this.forumName));
            Assert.IsTrue(this.forumController.addSubForum(this.forumName, this.subForumName, modList, this.userAdmin.userName).Equals("sub-forum added"));
            Assert.IsTrue(this.subForumController.createThread("headLine", "content", this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
            this.postId = posts[0].id;
        }

        [TestCleanup]
        public void cleanUp()
        {
            this.forumController = null;
            this.subForumController = null;
            this.postController = null;
            this.forum = null;
            this.userNonMember = null;
            this.userMember = null;
            this.userMod = null;
            this.userAdmin = null;
            DBClass db = DBClass.getInstance;
            db.clear();
        }




        [TestMethod]
        public void registerUserTest()
        {
            var threads = new List<System.Threading.Thread>();
            for (int p = 0; p < 100; p++)
            {
                threads.Add(new System.Threading.Thread(addUserTask));
            }
            //start all threads
            foreach (var thread in threads)
            {
                thread.Start();
            }

            //waiting for all threads to complete
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }

        }

        [TestMethod]
        public void sendThreadTest()
        {
            var threads = new List<System.Threading.Thread>();
            for (int p = 0; p < 100; p++)
            {
                threads.Add(new System.Threading.Thread(sendThreadTask));
            }
            //start all threads
            foreach (var thread in threads)
            {
                thread.Start();
            }

            //waiting for all threads to complete
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }

        }
        [TestMethod]
        public void sendPostTest()
        {
            var threads = new List<System.Threading.Thread>();
            for (int p = 0; p < 100; p++)
            {
                threads.Add(new System.Threading.Thread(sendPostTask));
            }
            //start all threads
            foreach (var thread in threads)
            {
                thread.Start();
            }

            //waiting for all threads to complete
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }

        }
        [TestMethod]
        public void getPostsTest()
        {
            var threads = new List<System.Threading.Thread>();
            for (int p = 0; p < 100; p++)
            {
                threads.Add(new System.Threading.Thread(readPostTask));
            }
            //start all threads
            foreach (var thread in threads)
            {
                thread.Start();
            }

            //waiting for all threads to complete
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }

        }


        private void addUserTask()
        {
            int p = System.Threading.Thread.CurrentThread.ManagedThreadId;
            this.forumController.registerUser("userName" + p, "idanA1", "d@d.d", "tomer", "tomer", forum.forumName);
        }
        private void sendThreadTask()
        {
            int p = System.Threading.Thread.CurrentThread.ManagedThreadId;
            this.forumController.registerUser("userName" + p, "idanA1", "d@d.d", "tomer", "tomer", forum.forumName);
            subForumController.createThread("" + p, "" + p, "userName" + p, forum.forumName, subForumName);
        }
        private void sendPostTask()
        {
            int p = System.Threading.Thread.CurrentThread.ManagedThreadId;
            this.forumController.registerUser("userName" + p, "idanA1", "d@d.d", "tomer", "tomer", forum.forumName);
            this.postController.addComment(p.ToString(), p.ToString(), "userName" + p, postId);
        }
        private void readPostTask()
        {
            List<Post> posts = this.postController.getAllPosts(this.forum.forumName, this.subForumName);
        }
    }
}
