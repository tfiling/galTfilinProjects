using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForumBuilder.Controllers;
using BL_Back_End;
using ForumBuilder.Systems;
using System.Collections.Generic;
using Database;

namespace Tests
{
    [TestClass]
    public class PostControllerTest
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
            this.forum = new Forum(this.forumName, "descr",fp, adminList);
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

        /**************************add comment*********************************/

        [TestMethod]
        public void test_addComment_mem()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.postController.addComment(headLine, content, this.userMember.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 2);
            Post comment = posts[1];
            Assert.AreEqual(comment.title, headLine);
            Assert.AreEqual(comment.content, content);
            Assert.AreEqual(comment.writerUserName, this.userMember.userName);
            Assert.AreEqual(comment.parentId, this.postId);
            Assert.AreNotEqual(comment.id, this.postId);
        }

        [TestMethod]
        public void test_addComment_admin()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.postController.addComment(headLine, content, this.userAdmin.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 2);
            Post comment = posts[1];
            Assert.AreEqual(comment.title, headLine);
            Assert.AreEqual(comment.content, content);
            Assert.AreEqual(comment.writerUserName, this.userAdmin.userName);
            Assert.AreEqual(comment.parentId, this.postId);
            Assert.AreNotEqual(comment.id, this.postId);
        }

        [TestMethod]
        public void test_addComment_mod()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsTrue(this.postController.addComment(headLine, content, this.userMod.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 2);
            Post comment = posts[1];
            Assert.AreEqual(comment.title, headLine);
            Assert.AreEqual(comment.content, content);
            Assert.AreEqual(comment.writerUserName, this.userMod.userName);
            Assert.AreEqual(comment.parentId, this.postId);
            Assert.AreNotEqual(comment.id, this.postId);
        }

        [TestMethod]
        public void test_addComment_nonMember()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.postController.addComment(headLine, content, this.userNonMember.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }


        [TestMethod]
        public void test_addComment_mem_empty_headLine()
        {
            String headLine = "";
            String content = "content";
            Assert.IsTrue(this.postController.addComment(headLine, content, this.userMember.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 2);
            Post comment = posts[1];
            Assert.AreEqual(comment.title, headLine);
            Assert.AreEqual(comment.content, content);
            Assert.AreEqual(comment.writerUserName, this.userMember.userName);
            Assert.AreEqual(comment.parentId, this.postId);
            Assert.AreNotEqual(comment.id, this.postId);
        }

        [TestMethod]
        public void test_addComment_admin_empty_headLine()
        {
            String headLine = "";
            String content = "content";
            Assert.IsTrue(this.postController.addComment(headLine, content, this.userAdmin.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 2);
            Post comment = posts[1];
            Assert.AreEqual(comment.title, headLine);
            Assert.AreEqual(comment.content, content);
            Assert.AreEqual(comment.writerUserName, this.userAdmin.userName);
            Assert.AreEqual(comment.parentId, this.postId);
            Assert.AreNotEqual(comment.id, this.postId);
        }

        [TestMethod]
        public void test_addComment_mod_empty_headLine()
        {
            String headLine = "";
            String content = "content";
            Assert.IsTrue(this.postController.addComment(headLine, content, this.userMod.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 2);
            Post comment = posts[1];
            Assert.AreEqual(comment.title, headLine);
            Assert.AreEqual(comment.content, content);
            Assert.AreEqual(comment.writerUserName, this.userMod.userName);
            Assert.AreEqual(comment.parentId, this.postId);
            Assert.AreNotEqual(comment.id, this.postId);
        }

        [TestMethod]
        public void test_addComment_nonMember_empty_headLine()
        {
            String headLine = "";
            String content = "content";
            Assert.IsFalse(this.postController.addComment(headLine, content, this.userNonMember.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_addComment_mem_empty_content()
        {
            String headLine = "head";
            String content = "";
            Assert.IsTrue(this.postController.addComment(headLine, content, this.userMember.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 2);
            Post comment = posts[1];
            Assert.AreEqual(comment.title, headLine);
            Assert.AreEqual(comment.content, content);
            Assert.AreEqual(comment.writerUserName, this.userMember.userName);
            Assert.AreEqual(comment.parentId, this.postId);
            Assert.AreNotEqual(comment.id, this.postId);
        }

        [TestMethod]
        public void test_addComment_admin_empty_content()
        {
            String headLine = "head";
            String content = "";
            Assert.IsTrue(this.postController.addComment(headLine, content, this.userAdmin.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 2);
            Post comment = posts[1];
            Assert.AreEqual(comment.title, headLine);
            Assert.AreEqual(comment.content, content);
            Assert.AreEqual(comment.writerUserName, this.userAdmin.userName);
            Assert.AreEqual(comment.parentId, this.postId);
            Assert.AreNotEqual(comment.id, this.postId);
        }

        [TestMethod]
        public void test_addComment_mod_empty_content()
        {
            String headLine = "head";
            String content = "";
            Assert.IsTrue(this.postController.addComment(headLine, content, this.userMod.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 2);
            Post comment = posts[1];
            Assert.AreEqual(comment.title, headLine);
            Assert.AreEqual(comment.content, content);
            Assert.AreEqual(comment.writerUserName, this.userMod.userName);
            Assert.AreEqual(comment.parentId, this.postId);
            Assert.AreNotEqual(comment.id, this.postId);
        }

        [TestMethod]
        public void test_addComment_nonMember_empty_content()
        {
            String headLine = "head";
            String content = "";
            Assert.IsFalse(this.postController.addComment(headLine, content, this.userNonMember.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_addComment_mem_empty_inputs()
        {
            String headLine = "";
            String content = "";
            Assert.IsFalse(this.postController.addComment(headLine, content, this.userMember.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_addComment_admin_empty_inputs()
        {
            String headLine = "";
            String content = "";
            Assert.IsFalse(this.postController.addComment(headLine, content, this.userAdmin.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_addComment_mod_empty_inputs()
        {
            String headLine = "";
            String content = "";
            Assert.IsFalse(this.postController.addComment(headLine, content, this.userMod.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_addComment_nonMember_empty_inputs()
        {
            String headLine = "head";
            String content = "";
            Assert.IsFalse(this.postController.addComment(headLine, content, this.userNonMember.userName, this.postId).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_addComment_invalid_postId()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.postController.addComment(headLine, content, this.userMember.userName, this.postId + 1).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_addComment_null_headLine()
        {
            String headLine = null;
            String content = "content";
            Assert.IsFalse(this.postController.addComment(headLine, content, this.userMember.userName, this.postId + 1).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_addComment_null_content()
        {
            String headLine = "head";
            String content = null;
            Assert.IsFalse(this.postController.addComment(headLine, content, this.userMember.userName, this.postId + 1).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_addComment_null_user()
        {
            String headLine = "head";
            String content = "content";
            Assert.IsFalse(this.postController.addComment(headLine, content, null, this.postId + 1).Equals("comment created"));
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, 1);
        }

        /**************************end of add comment*********************************/

        /************************** remove comment*********************************/

        [TestMethod]
        public void test_removeComment_mem_noSubComments()
        {
            test_addComment_mem();
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.writerUserName == this.userMember.userName)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int id = newPost.id;
            Assert.IsTrue(this.postController.removeComment(id, this.userMember.userName).Equals("Post removed"));
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            foreach (Post p in posts)
            {
                Assert.AreNotEqual(p.id, id);
            }
        }

        [TestMethod]
        public void test_removeComment_admin_noSubComments()
        {
            test_addComment_admin();
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.writerUserName == this.userAdmin.userName)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int id = newPost.id;
            Assert.IsTrue(this.postController.removeComment(id, this.userAdmin.userName).Equals("Post removed"));
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            foreach (Post p in posts)
            {
                Assert.AreNotEqual(p.id, id);
            }
        }

        [TestMethod]
        public void test_removeComment_not_owner_by_mem_noSubComments()
        {
            test_addComment_admin();
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.parentId == this.postId)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int id = newPost.id;
            Assert.IsFalse(this.postController.removeComment(id, this.userMember.userName).Equals("Post removed"));
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            newPost = null;
            foreach (Post p in posts)
            {
                if (p.id == id)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should not be deleted by other user");
        }

        [TestMethod]
        public void test_removeComment_not_owner_admin_noSubComments()
        {
            test_addComment_mem();
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.parentId == this.postId)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int id = newPost.id;
            Assert.IsTrue(this.postController.removeComment(id, this.userAdmin.userName).Equals("Post removed"));
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            newPost = null;
            foreach (Post p in posts)
            {
                if (p.id == id)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNull(newPost, "the added post should not be deleted by other user");
        }

        [TestMethod]
        public void test_removeComment_mem_withSubComments()
        {
            test_addComment_mem();
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.parentId == this.postId)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int id = newPost.id;
            Assert.IsTrue(this.postController.addComment("head", "subcomment", this.userMember.userName, id).Equals("comment created"));
            Assert.IsTrue(this.postController.addComment("head2", "subcomment2", this.userAdmin.userName, id).Equals("comment created"));
            Assert.IsTrue(this.postController.removeComment(id, this.userMember.userName).Equals("Post removed"));
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            foreach (Post p in posts)
            {
                Assert.AreNotEqual(p.id, id);
                Assert.AreNotEqual(p.parentId, id);
            }
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_removeComment_admin_withSubComments()
        {
            test_addComment_admin();
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.writerUserName == this.userAdmin.userName)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int id = newPost.id;
            Assert.IsTrue(this.postController.addComment("head", "subcomment", this.userMember.userName, id).Equals("comment created"));
            Assert.IsTrue(this.postController.addComment("head2", "subcomment2", this.userAdmin.userName, id).Equals("comment created"));
            Assert.IsTrue(this.postController.removeComment(id, this.userAdmin.userName).Equals("Post removed"));
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            foreach (Post p in posts)
            {
                Assert.AreNotEqual(p.id, id);
                Assert.AreNotEqual(p.parentId, id);
            }
            Assert.AreEqual(posts.Count, 1);
        }

        [TestMethod]
        public void test_removeComment_subComment_without_subcomments_by_mem()
        {
            int commentCounter = INITIAL_COMMENT_COUNT;
            test_addComment_mem();
            commentCounter++;
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.writerUserName == this.userMember.userName)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int parentId = newPost.id;
            Assert.IsTrue(this.postController.addComment("head", "subcomment", this.userMember.userName, parentId).Equals("comment created"));
            commentCounter++;
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            newPost = null;
            foreach (Post p in posts)
            {
                if (p.parentId == parentId)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int id = newPost.id;
            Assert.IsTrue(this.postController.removeComment(id, this.userMember.userName).Equals("Post removed"));
            commentCounter--;
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, commentCounter);
            foreach (Post p in posts)
            {
                Assert.AreNotEqual(p.id, id);
            }
        }

        [TestMethod]
        public void test_removeComment_subComment_without_subcomments_by_admin()
        {
            int commentCounter = INITIAL_COMMENT_COUNT;
            test_addComment_admin();
            commentCounter++;
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.writerUserName == this.userAdmin.userName)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int parentId = newPost.id;
            Assert.IsTrue(this.postController.addComment("head", "subcomment", this.userAdmin.userName, parentId).Equals("comment created"));
            commentCounter++;
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            newPost = null;
            foreach (Post p in posts)
            {
                if (p.parentId == parentId)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int id = newPost.id;
            Assert.IsTrue(this.postController.removeComment(id, this.userAdmin.userName).Equals("Post removed"));
            commentCounter--;
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, commentCounter);
            foreach (Post p in posts)
            {
                Assert.AreNotEqual(p.id, id);
            }
        }

        [TestMethod]
        public void test_removeComment_subComment_with_subcomments_by_mem()
        {
            int commentCounter = INITIAL_COMMENT_COUNT;
            test_addComment_mem();
            commentCounter++;
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.parentId == this.postId)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int parentId = newPost.id;
            Assert.IsTrue(this.postController.addComment("head", "subcomment", this.userMember.userName, parentId).Equals("comment created"));
            commentCounter++;
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            newPost = null;
            foreach (Post p in posts)
            {
                if (p.parentId == parentId)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int id = newPost.id;
            Assert.IsTrue(this.postController.addComment("head", "content", this.userMember.userName, id).Equals("comment created"));
            Assert.IsTrue(this.postController.addComment("head", "content", this.userAdmin.userName, id).Equals("comment created"));
            commentCounter += 2;
            Assert.IsTrue(this.postController.removeComment(id, this.userMember.userName).Equals("Post removed"));
            commentCounter -= 3;
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, commentCounter);
            foreach (Post p in posts)
            {
                Assert.AreNotEqual(p.id, id);
                Assert.AreNotEqual(p.parentId, id);
            }
        }

        [TestMethod]
        public void test_removeComment_subComment_with_subcomments_by_admin()
        {
            int commentCounter = INITIAL_COMMENT_COUNT;
            test_addComment_admin();
            commentCounter++;
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.parentId == this.postId)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int parentId = newPost.id;
            Assert.IsTrue(this.postController.addComment("head", "subcomment", this.userAdmin.userName, parentId).Equals("comment created"));
            commentCounter++;
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            newPost = null;
            foreach (Post p in posts)
            {
                if (p.parentId == parentId)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int id = newPost.id;
            Assert.IsTrue(this.postController.addComment("head", "content", this.userMember.userName, id).Equals("comment created"));
            Assert.IsTrue(this.postController.addComment("head", "content", this.userAdmin.userName, id).Equals("comment created"));
            commentCounter += 2;
            Assert.IsTrue(this.postController.removeComment(id, this.userAdmin.userName).Equals("Post removed"));
            commentCounter -= 3;
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(posts.Count, commentCounter);
            foreach (Post p in posts)
            {
                Assert.AreNotEqual(p.id, id);
                Assert.AreNotEqual(p.parentId, id);
            }
        }

        [TestMethod]
        public void test_removeComment_subcomment_with_nested_subComments()
        {
            int commentCounter = INITIAL_COMMENT_COUNT;
            test_addComment_admin();
            commentCounter++;
            List<Post> posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post newPost = null;
            foreach (Post p in posts)
            {
                if (p.writerUserName == this.userAdmin.userName)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int parentId = newPost.id;
            Assert.IsTrue(this.postController.addComment("head", "subcomment", this.userAdmin.userName, parentId).Equals("comment created"));
            commentCounter++;
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            newPost = null;
            foreach (Post p in posts)
            {
                if (p.parentId == parentId)
                {
                    newPost = p;
                    break;
                }
            }
            Assert.IsNotNull(newPost, "the added post should exist");
            int originalId = newPost.id;
            int id = newPost.id;
            Assert.IsTrue(this.postController.addComment("head", "content", this.userMember.userName, originalId).Equals("comment created"));
            Assert.IsTrue(this.postController.addComment("head", "content", this.userAdmin.userName, originalId).Equals("comment created"));
            commentCounter += 2;
            for (int i = 0; i < 5; i++)
            {
                posts = this.postController.getAllPosts(this.forumName, this.subForumName);
                newPost = null;
                foreach (Post p in posts)
                {
                    if (p.parentId == id)
                    {
                        newPost = p;
                        break;
                    }
                }
                Assert.IsNotNull(newPost, "the added post should exist");
                id = newPost.id;
                Assert.IsTrue(this.postController.addComment("head", "content", this.userMember.userName, id).Equals("comment created"));
                Assert.IsTrue(this.postController.addComment("head", "content", this.userAdmin.userName, id).Equals("comment created"));
                commentCounter += 2;
            }
            Assert.IsTrue(this.postController.removeComment(originalId, this.userAdmin.userName).Equals("Post removed"));
            commentCounter -= 13;
            posts = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(commentCounter, posts.Count);
            foreach (Post p in posts)
            {
                Assert.AreNotEqual(p.id, id);
                Assert.AreNotEqual(p.parentId, id);
            }
            Assert.AreEqual(commentCounter, 2);
        }


        [TestMethod]
        public void test_removeComment_invalidId_notExists()
        {
            int invalidId = INITIAL_COMMENT_COUNT + 1;
            Boolean foundInvalid;
            List<Post> postsPre = this.postController.getAllPosts(this.forumName, this.subForumName);
            while (true)
            {
                foundInvalid = true;
                foreach(Post p in postsPre)
                {
                    if (p.id == invalidId)
                    {
                        foundInvalid = false;
                        break;
                    }
                }
                if (foundInvalid)
                    break;
                else
                    invalidId++;
            }
            Assert.IsFalse(this.postController.removeComment(invalidId, this.userMember.userName).Equals("Post removed"));
            List<Post> postsAfter = this.postController.getAllPosts(this.forumName, this.subForumName);
            Assert.AreEqual(postsPre.Count, postsAfter.Count);
        }



        [TestMethod]
        public void test_removeComment_mem_by_moderator_where_not_allowed()
        {
            String headLine = "head";
            String content = "content";
            ForumPolicy newfp = new ForumPolicy("p", true, 2, false, 180, 2, true, true, 5, 0, new List<string>());
            Assert.IsTrue(forumController.setForumPreferences(this.forumName, "new Des", newfp, userAdmin.userName).Equals("preferences had changed successfully"));
            Assert.IsTrue(this.subForumController.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post post = postList[0];
            Assert.IsTrue(this.postController.addComment("hii", "post to delete", this.userMember.userName, post.id).Equals("comment created"));
            postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            post = postList[1];
            Assert.IsFalse(this.postController.removeComment(post.id, this.userMod.userName).Equals("Post removed"));
        }

        [TestMethod]
        public void test_removeComment_mem_by_moderator_where_allowed()
        {
            String headLine = "head";
            String content = "content";
            ForumPolicy newfp = new ForumPolicy("p", true, 2, true, 180, 2, true, true, 5, 0, new List<string>());
            Assert.IsTrue(forumController.setForumPreferences(this.forumName, "new Des", newfp, userAdmin.userName).Equals("preferences had changed successfully"));
            Assert.IsTrue(this.subForumController.createThread(headLine, content, this.userMember.userName, this.forumName, this.subForumName).Equals("Create tread succeed"));
            List<Post> postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            Post post = postList[0];
            Assert.IsTrue(this.postController.addComment("hii", "post to delete", this.userMember.userName, post.id).Equals("comment created"));
            postList = this.postController.getAllPosts(this.forumName, this.subForumName);
            post = postList[1];
            Assert.IsTrue(this.postController.removeComment(post.id, this.userMod.userName).Equals("Post removed"));
        }


        /**************************end of remove comment*********************************/
        

    }
}
