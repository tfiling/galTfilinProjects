
using BL_Back_End;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
      [TestClass]
      public class PostManagerTest
      {

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

          const int INITIAL_COMMENT_COUNT = 1;

          [TestInitialize]
          public void setUp()
          {
              SuperUserController superUserController = SuperUserController.getInstance;
              this.superUser = new UserData("tomer", "1qW", "fkfkf@wkk.com");
              superUserController.addSuperUser(this.superUser.email, this.superUser.password, this.superUser.userName);
              this.forumManager = new ForumManagerClient(new InstanceContext(new ClientNotificationHost()));
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
              Assert.IsTrue(this.forumManager.registerUser(userMember.userName, userMember.password, userMember.email, "ansss", "anssss", this.forum.forumName).Equals("Register user succeed"));
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
          }

          [TestCleanup]
          public void cleanUp()
          {
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

          /**************************add comment*********************************/

           [TestMethod]
           public void test_addComment_mem()
           {
               String headLine = "head";
               String content = "content";
               Assert.IsTrue(this.postManager.addPost(headLine, content, this.userMember.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 2);
               PostData comment = posts[1];
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
               Assert.IsTrue(this.postManager.addPost(headLine, content, this.userAdmin.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 2);
               PostData comment = posts[1];
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
               Assert.IsTrue(this.postManager.addPost(headLine, content, this.userMod.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 2);
               PostData comment = posts[1];
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
               Assert.IsFalse(this.postManager.addPost(headLine, content, this.userNonMember.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }


           [TestMethod]
           public void test_addComment_mem_empty_headLine()
           {
               String headLine = "";
               String content = "content";
               Assert.IsTrue(this.postManager.addPost(headLine, content, this.userMember.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 2);
               PostData comment = posts[1];
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
               Assert.IsTrue(this.postManager.addPost(headLine, content, this.userAdmin.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 2);
               PostData comment = posts[1];
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
               Assert.IsTrue(this.postManager.addPost(headLine, content, this.userMod.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 2);
               PostData comment = posts[1];
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
               Assert.IsFalse(this.postManager.addPost(headLine, content, this.userNonMember.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }

           [TestMethod]
           public void test_addComment_mem_empty_content()
           {
               String headLine = "head";
               String content = "";
               Assert.IsTrue(this.postManager.addPost(headLine, content, this.userMember.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 2);
               PostData comment = posts[1];
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
               Assert.IsTrue(this.postManager.addPost(headLine, content, this.userAdmin.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 2);
               PostData comment = posts[1];
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
               Assert.IsTrue(this.postManager.addPost(headLine, content, this.userMod.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 2);
               PostData comment = posts[1];
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
               Assert.IsFalse(this.postManager.addPost(headLine, content, this.userNonMember.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }

           [TestMethod]
           public void test_addComment_mem_empty_inputs()
           {
               String headLine = "";
               String content = "";
               Assert.IsFalse(this.postManager.addPost(headLine, content, this.userMember.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }

           [TestMethod]
           public void test_addComment_admin_empty_inputs()
           {
               String headLine = "";
               String content = "";
               Assert.IsFalse(this.postManager.addPost(headLine, content, this.userAdmin.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }

           [TestMethod]
           public void test_addComment_mod_empty_inputs()
           {
               String headLine = "";
               String content = "";
               Assert.IsFalse(this.postManager.addPost(headLine, content, this.userMod.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }

           [TestMethod]
           public void test_addComment_nonMember_empty_inputs()
           {
               String headLine = "head";
               String content = "";
               Assert.IsFalse(this.postManager.addPost(headLine, content, this.userNonMember.userName, this.postId).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }

           [TestMethod]
           public void test_addComment_invalid_postId()
           {
               String headLine = "head";
               String content = "content";
               Assert.IsFalse(this.postManager.addPost(headLine, content, this.userMember.userName, this.postId + 1).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }

           [TestMethod]
           public void test_addComment_null_headLine()
           {
               String headLine = null;
               String content = "content";
               Assert.IsFalse(this.postManager.addPost(headLine, content, this.userMember.userName, this.postId + 1).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }

           [TestMethod]
           public void test_addComment_null_content()
           {
               String headLine = "head";
               String content = null;
               Assert.IsFalse(this.postManager.addPost(headLine, content, this.userMember.userName, this.postId + 1).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }

           [TestMethod]
           public void test_addComment_null_user()
           {
               String headLine = "head";
               String content = "content";
               Assert.IsFalse(this.postManager.addPost(headLine, content, null, this.postId + 1).Equals("comment created"));
               List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
               Assert.AreEqual(posts.Count, 1);
           }

           /**************************end of add comment*********************************/

    /**************************remove comment*********************************/

    
      [TestMethod]
          public void test_removeComment_mem_noSubComments()
          {
              test_addComment_mem();
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.writerUserName == this.userMember.userName)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int id = newPost.id;
              Assert.IsTrue(this.postManager.deletePost(id, this.userMember.userName).Equals("Post removed"));
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              foreach (PostData p in posts)
              {
                  Assert.AreNotEqual(p.id, id);
              }
          }

          [TestMethod]
          public void test_removeComment_admin_noSubComments()
          {
              test_addComment_admin();
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.writerUserName == this.userAdmin.userName)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int id = newPost.id;
              Assert.IsTrue(this.postManager.deletePost(id, this.userAdmin.userName).Equals("Post removed"));
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              foreach (PostData p in posts)
              {
                  Assert.AreNotEqual(p.id, id);
              }
          }

          [TestMethod]
          public void test_removeComment_not_owner_by_mem_noSubComments()
          {
              test_addComment_admin();
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.parentId == this.postId)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int id = newPost.id;
              Assert.IsFalse(this.postManager.deletePost(id, this.userMember.userName).Equals("Post removed"));
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              newPost = null;
              foreach (PostData p in posts)
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
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.parentId == this.postId)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int id = newPost.id;
              Assert.IsTrue(this.postManager.deletePost(id, this.userAdmin.userName).Equals("Post removed"));
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              newPost = null;
              foreach (PostData p in posts)
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
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.parentId == this.postId)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int id = newPost.id;
              Assert.IsTrue(this.postManager.addPost("head", "subcomment", this.userMember.userName, id).Equals("comment created"));
              Assert.IsTrue(this.postManager.addPost("head2", "subcomment2", this.userAdmin.userName, id).Equals("comment created"));
              Assert.IsTrue(this.postManager.deletePost(id, this.userMember.userName).Equals("Post removed"));
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              foreach (PostData p in posts)
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
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.writerUserName == this.userAdmin.userName)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int id = newPost.id;
              Assert.IsTrue(this.postManager.addPost("head", "subcomment", this.userMember.userName, id).Equals("comment created"));
              Assert.IsTrue(this.postManager.addPost("head2", "subcomment2", this.userAdmin.userName, id).Equals("comment created"));
              Assert.IsTrue(this.postManager.deletePost(id, this.userAdmin.userName).Equals("Post removed"));
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              foreach (PostData p in posts)
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
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.writerUserName == this.userMember.userName)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int parentId = newPost.id;
              Assert.IsTrue(this.postManager.addPost("head", "subcomment", this.userMember.userName, parentId).Equals("comment created"));
              commentCounter++;
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.parentId == parentId)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int id = newPost.id;
              Assert.IsTrue(this.postManager.deletePost(id, this.userMember.userName).Equals("Post removed"));
              commentCounter--;
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              Assert.AreEqual(posts.Count, commentCounter);
              foreach (PostData p in posts)
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
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.writerUserName == this.userAdmin.userName)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int parentId = newPost.id;
              Assert.IsTrue(this.postManager.addPost("head", "subcomment", this.userAdmin.userName, parentId).Equals("comment created"));
              commentCounter++;
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.parentId == parentId)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int id = newPost.id;
              Assert.IsTrue(this.postManager.deletePost(id, this.userAdmin.userName).Equals("Post removed"));
              commentCounter--;
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              Assert.AreEqual(posts.Count, commentCounter);
              foreach (PostData p in posts)
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
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.parentId == this.postId)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int parentId = newPost.id;
              Assert.IsTrue(this.postManager.addPost("head", "subcomment", this.userMember.userName, parentId).Equals("comment created"));
              commentCounter++;
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.parentId == parentId)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int id = newPost.id;
              Assert.IsTrue(this.postManager.addPost("head", "content", this.userMember.userName, id).Equals("comment created"));
              Assert.IsTrue(this.postManager.addPost("head", "content", this.userAdmin.userName, id).Equals("comment created"));
              commentCounter += 2;
              Assert.IsTrue(this.postManager.deletePost(id, this.userMember.userName).Equals("Post removed"));
              commentCounter -= 3;
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              Assert.AreEqual(posts.Count, commentCounter);
              foreach (PostData p in posts)
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
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.parentId == this.postId)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int parentId = newPost.id;
              Assert.IsTrue(this.postManager.addPost("head", "subcomment", this.userAdmin.userName, parentId).Equals("comment created"));
              commentCounter++;
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.parentId == parentId)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int id = newPost.id;
              Assert.IsTrue(this.postManager.addPost("head", "content", this.userMember.userName, id).Equals("comment created"));
              Assert.IsTrue(this.postManager.addPost("head", "content", this.userAdmin.userName, id).Equals("comment created"));
              commentCounter += 2;
              Assert.IsTrue(this.postManager.deletePost(id, this.userAdmin.userName).Equals("Post removed"));
              commentCounter -= 3;
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              Assert.AreEqual(posts.Count, commentCounter);
              foreach (PostData p in posts)
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
              List<PostData> posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              PostData newPost = null;
              foreach (PostData p in posts)
              {
                  if (p.writerUserName == this.userAdmin.userName)
                  {
                      newPost = p;
                      break;
                  }
              }
              Assert.IsNotNull(newPost, "the added post should exist");
              int parentId = newPost.id;
              Assert.IsTrue(this.postManager.addPost("head", "subcomment", this.userAdmin.userName, parentId).Equals("comment created"));
              commentCounter++;
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              newPost = null;
              foreach (PostData p in posts)
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
              Assert.IsTrue(this.postManager.addPost("head", "content", this.userMember.userName, originalId).Equals("comment created"));
              Assert.IsTrue(this.postManager.addPost("head", "content", this.userAdmin.userName, originalId).Equals("comment created"));
              commentCounter += 2;
              for (int i = 0; i < 5; i++)
              {
                  posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
                  newPost = null;
                  foreach (PostData p in posts)
                  {
                      if (p.parentId == id)
                      {
                          newPost = p;
                          break;
                      }
                  }
                  Assert.IsNotNull(newPost, "the added post should exist");
                  id = newPost.id;
                  Assert.IsTrue(this.postManager.addPost("head", "content", this.userMember.userName, id).Equals("comment created"));
                  Assert.IsTrue(this.postManager.addPost("head", "content", this.userAdmin.userName, id).Equals("comment created"));
                  commentCounter += 2;
              }
              Assert.IsTrue(this.postManager.deletePost(originalId, this.userAdmin.userName).Equals("Post removed"));
              commentCounter -= 13;
              posts = this.postManager.getAllPosts(this.forumName, this.subForumName);
              Assert.AreEqual(commentCounter, posts.Count);
              foreach (PostData p in posts)
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
              List<PostData> postsPre = this.postManager.getAllPosts(this.forumName, this.subForumName);
              while (true)
              {
                  foundInvalid = true;
                  foreach (PostData p in postsPre)
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
              Assert.IsFalse(this.postManager.deletePost(invalidId, this.userMember.userName).Equals("Post removed"));
              List<PostData> postsAfter = this.postManager.getAllPosts(this.forumName, this.subForumName);
              Assert.AreEqual(postsPre.Count, postsAfter.Count);
          }

      }
      /**************************end of remove comment*********************************/

}
