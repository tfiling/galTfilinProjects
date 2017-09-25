using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using BL_Back_End;
using Database;

namespace DataBase
{
    public class Cache
    {
        private Dictionary<string, User> _users;
        private Dictionary<string, User> _superUsers;
        private Dictionary<string, Forum> _forums;
        private List<SubForum> _subForums;
        private Dictionary<int, Thread> _threads;
        private Dictionary<int, Post> _posts;
        private static Cache singleton;

        private Cache()
        {
          _users = new Dictionary<string, User>() ;
          _forums = new Dictionary<string, Forum>() ;
          _subForums = new  List<SubForum>() ;
          _threads = new  Dictionary<int, Thread>() ;
          _posts = new  Dictionary<int, Post>();
          _superUsers = new Dictionary<string, User>();
        }

        public static Cache getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new Cache();
                }
                return singleton;
            }
        }

        public void clear()
        {
            if (this._forums != null)
                this._forums.Clear();
            if (this._subForums != null)
                this._subForums.Clear();
            if (this._threads != null)
                this._threads.Clear();
            if (this._posts != null)
                this._posts.Clear();
            if (this._users != null)
                this._users.Clear();
            if (this._superUsers != null)
                this._superUsers.Clear();

        }
        internal void intialLists(List<Forum> forums, List<SubForum> subForums, List<User> users, List<string> superUsers, List<Thread> threads, List<Post> posts)
        {
            foreach(Forum f in forums)
            {
                _forums.Add(f.forumName, f);
            }
            foreach (SubForum sf in subForums)
            {
                _subForums.Add(sf);
            }
            foreach (User u in users)
            {
                _users.Add(u.userName, u);
                if (superUsers.Contains(u.userName))
                {
                    _superUsers.Add(u.userName, u);
                }
            }
            foreach (Thread t in threads)
            {
                _threads.Add(t.firstPost.id, t);
            }
            foreach (Post p in posts)
            {
                _posts.Add(p.id, p);
            }
        }

        public int getMaxIntOfPost()
        {
            int maxPost = 0;
            foreach (int p in _posts.Keys)
            {
                if (p > maxPost)
                {
                    maxPost = p;
                }
            }
            return maxPost;
        }

        public int numOfForums()
        {
            try
            {
                return _forums.Count;
            }
            catch
            {
                return 0;
            }

        }

        public bool dismissModerator(string dismissedModerator, string subForumName, string forumName)
        {
            try
            {
                return getSubForum(subForumName, forumName).moderators.Remove(dismissedModerator);
            }
            catch
            {
                return false;
            }
        }

        public SubForum getSubForum(string subForumName, string forumName)
        {
            try
            {
                foreach (SubForum sf in _subForums)
                {
                    if (sf.forum.Equals(forumName) && sf.name.Equals(subForumName))
                        return sf;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool addSuperUser(string email, string password, string userName)
        {
            try
            {
                User su = new User(userName, password, email, DateTime.Today);
                _users.Add(userName, su);
                _superUsers.Add(userName, su);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool nominateModerator(string newModerator, DateTime endDate, string subForumName, string forumName, String nominator)
        {
            try
            {
                SubForum sf = getSubForum(subForumName, forumName);
                sf.moderators.Add(newModerator, new Moderator(newModerator, endDate, DateTime.Today, nominator));
                return true;
            }
            catch
            {
                return false;
            }

        }

        public Forum getforumByName(string forumName)
        {
            try
            {
                return _forums[forumName];
            }
            catch
            {
                return null;
            }
            
        }

        public List<string> getsubForumsNamesOfForum(string forumName)
        {
            try
            {
                Forum f = _forums[forumName];
                return f.subForums;
            }
            catch
            {
                return null;
            }
        }

        public List<string> getForums()
        {
            try
            {
                return _forums.Keys.ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<string> getModertorsReport(String forumName)
        {
            try
            {
                List<string> report = new List<string>();
                List<string> subforum = getsubForumsNamesOfForum(forumName);
                foreach (string sf in subforum)
                {
                    string ans = "";
                    SubForum subf = getSubForum(sf, forumName);
                    foreach (Moderator m in subf.moderators.Values)
                    {
                        ans += "subForum: " + sf + ", \t moderator: " + m.userName + ", \t nominator: " + m.nominatorName + ",\t DateAdded:" + m.dateAdded.ToString("dd MM yyyy") + "\t added posts:";
                        foreach (Post p in _posts.Values)
                        {
                            if (p.writerUserName.Equals(m.userName))
                            {
                                ans += " \n\t post title: " + p.title + " \n\t post content:" + p.content;
                            }
                        }
                        report.Add(ans);
                        ans = "";
                    }
                    
                }
                return report;
            }
            catch
            {
                return null;
            }
        }

        public List<String> getSuperUserReportOfMembers()
        {
            try
            {
                List<string> emails = new List<string>();
                foreach(User u in _users.Values)
                {
                    if (!emails.Contains(u.email))
                    {
                        emails.Add(u.email);
                    }
                }
                List<string> report = new List<string>();
                foreach (string e in emails)
                {
                    foreach (User u in _users.Values)
                    {
                        string ans = "";
                        if (u.email.Equals(e))
                        {
                            foreach (Forum f in _forums.Values)
                            {
                                if (f.members.Contains(u.userName))
                                {
                                    ans = "Email : " + e + "  UserName : " + u.userName + "  In forum : " + f.forumName;
                                    break;
                                }
                            }
                        }
                        if(!ans.Equals(""))
                            report.Add(ans);
                    }
                }
                return report;
            }
            catch
            {
                return null;
            }
        }

        public List<string> getUserFriends(string userName)
        {
            try
            {
                return _users[userName].friends;
            }
            catch
            {
                return null;
            }
        }

        public string getPassword(string userName)
        {
            try
            {
                return _users[userName].password;
            }
            catch
            {
                return null;
            }
        }

        public bool banMember(string bannedMember, string forumName)
        {
            try
            {
                _forums[forumName].members.Remove(bannedMember);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool changePolicy(string forumName, string policy, bool isQuestionIdentifying, int seniorityInForum,
        bool deletePostByModerator, int timeToPassExpiration, int minNumOfModerators, bool hasCapitalInPassword,
        bool hasNumberInPassword, int minLengthOfPassword, int notificationsType, List<String> selectiveNotificationsUsers)
        {
            try
            {
                _forums[forumName].forumPolicy.policy = policy;
                _forums[forumName].forumPolicy.isQuestionIdentifying = isQuestionIdentifying;
                _forums[forumName].forumPolicy.seniorityInForum = seniorityInForum;
                _forums[forumName].forumPolicy.deletePostByModerator = deletePostByModerator;
                _forums[forumName].forumPolicy.timeToPassExpiration = timeToPassExpiration;
                _forums[forumName].forumPolicy.minNumOfModerators = minNumOfModerators;
                _forums[forumName].forumPolicy.hasCapitalInPassword = hasCapitalInPassword;
                _forums[forumName].forumPolicy.hasNumberInPassword = hasNumberInPassword;
                _forums[forumName].forumPolicy.minLengthOfPassword = minLengthOfPassword;
                _forums[forumName].forumPolicy.notificationsType = notificationsType;
                _forums[forumName].forumPolicy.selectiveNotificationsUsers = new List<string>();
                foreach (String mem in selectiveNotificationsUsers)
                {
                    _forums[forumName].forumPolicy.selectiveNotificationsUsers.Add(mem);
                }
                _forums[forumName].forumPolicy.selectiveNotificationsUsers = selectiveNotificationsUsers;
                return true;
            }
            catch
            {
                return false;
            }

        }

        internal bool setPassword(string userName, string password)
        {
            _users[userName].password = password;
            _users[userName].lastTimeUpdatePassword = DateTime.Today;
            if (_superUsers.ContainsKey(userName))
            {
                _superUsers[userName].password = password;
                _superUsers[userName].lastTimeUpdatePassword = DateTime.Today;
            }
            return true;
        }

        public bool nominateAdmin(string newAdmin, string forumName)
        {
            try
            {
                _forums[forumName].administrators.Add(newAdmin);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public User getSuperUser(string userName)
        {
            try
            {
                return _superUsers[userName];
            }
            catch
            {
                return null;
            }
        }

        public bool dismissAdmin(string adminToDismissed, string forumName)
        {
            try
            {
                return _forums[forumName].administrators.Remove(adminToDismissed);
            }
            catch
            {
                return false;
            }
        }

        public User getUser(string userName)
        {
            try
            {
                return _users[userName];
            }
            catch
            {
                return null;
            }
        }

        public Boolean addUser(string userName, string password, string email)
        {
            try
            {
                User us = new User(userName, password, email, DateTime.Today);
                _users.Add(userName, us);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public Boolean addMemberToForum(string userName, string forumName) 
        {
            try
            {
                _forums[forumName].members.Add(userName);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<string> getMembersOfForum(string forumName)
        {
            try
            {
                List<String> mems = new List<string>();
                foreach (String mem in _forums[forumName].members)
                    mems.Add(mem);
                return mems;

            }
            catch
            {
                return null;
            }
        }

        public List<string> getSimularForumsOf2users(string userName1, string userName2)
        {
            try
            {
                List<string> simularForum = new List<string>();
                foreach (Forum f in _forums.Values)
                {
                    if (f.members.Contains(userName1) && f.members.Contains(userName2))
                        simularForum.Add(f.forumName);
                }
                return simularForum;
            }
            catch
            {
                return null;
            }

        }

        public Boolean createForum(string forumName, string description, ForumPolicy fp)
        {
            try
            {
                List<string> administrators = new List<string>();
                Forum f = new Forum(forumName, description, fp, administrators);
                _forums.Add(forumName, f);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public Boolean setForumPreferences(String forumName, String newDescription, ForumPolicy fp)
        {
            try
            {
                _forums[forumName].description = newDescription;
                _forums[forumName].forumPolicy = fp;
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool addFriendToUser(string userName, string friendToAddName)
        {
            try
            {
                _users[userName].friends.Add(friendToAddName);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool removeFriendOfUser(string userName, string deletedFriendName)
        {
            try
            {
                _users[userName].friends.Remove(deletedFriendName);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public Boolean addSubForum(String subForumName, String forumName)
        {
            try
            {
                SubForum sf = new SubForum(subForumName, forumName);
                _subForums.Add(sf);
                _forums[forumName].subForums.Add(subForumName);
                return true;
            }
            catch
            {
                return false;
            }

        }
        
        public Post getPost(int postId)
        {
            try
            {
                return _posts[postId];
            }
            catch
            {
                return null;
            }
        }

        public List<Post> getRelatedPosts(int postId)
        {
            try
            {
                List<Post> relatedPosts = new List<Post>();
                foreach (Post p in _posts.Values)
                {
                    if (p.parentId == postId)
                    {
                        relatedPosts.Add(p);
                    }
                }
                return relatedPosts;
            }
            catch
            {
                return null;
            }

        }

        public SubForum getSubforumByThreadFirstPostId(int id)
        {
            try
            {
                foreach (SubForum sf in _subForums)
                {
                    if (sf.threads.Contains(id))
                    {
                        return sf;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Thread getThreadByFirstPostId(int postId)
        {
            try
            {
                foreach (Thread t in _threads.Values)
                {
                    if (t.firstPost.id == postId)
                        return t;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool addThread(string forumName, string subForumName, int firstMessageId)
        {
            try
            {
                Thread thread = new Thread(getPost(firstMessageId));
                _threads.Add(firstMessageId, thread);
                getSubForum(subForumName, forumName).threads.Add(firstMessageId);
                return true;
            }
            catch
            {
                return false;
            }

        }
        
        public bool addPost(String writerUserName, Int32 postID, String headLine, String content, Int32 parentId, DateTime timePublished, String forumName) 
        {
            try
            {
                Post post = new Post(writerUserName, postID, headLine, content, parentId, timePublished, forumName);
                _posts.Add(postID, post);
                if (parentId != -1)
                {
                    getPost(parentId).commentsIds.Add(postID);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public int numOfPostInForum(String forumName)
        {
            int count = 0;
            foreach (Post p in _posts.Values)
            {
                if (p.forumName.Equals(forumName))
                    count++;
            }
            return count;
        }

        public bool removeThread(int id)
        {
            try
            {
                _threads.Remove(id);
                getSubforumByThreadFirstPostId(id).threads.Remove(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool removePost(int id)
        {
            try
            {
                _posts.Remove(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean updatePost(int postID, String title, String content)
        {
            try
            {
                _posts[postID].title = title;
                _posts[postID].content = content;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Post> getMemberPosts(String memberName, String forumName)
        {
            try
            {
                List<Post> memPost = new List<Post>();
                foreach (Post p in _posts.Values)
                {
                    if (p.forumName.Equals(forumName) && p.writerUserName.Equals(memberName))
                        memPost.Add(p);
                }
                return memPost;
            }
            catch
            {
                return null;
            }

        }

        private string enc(string password)
        {
            char[] passArray = password.ToArray();
            string res = "";
            for (int i = 0; i < passArray.Length; i++)
            {
                passArray[i] = (char)(((int)passArray[i]) + i % 5 + 1);
            }
            for (int i = 0; i < passArray.Length; i++)
            {
                res = res + passArray[i];
            }
            return res;
        }

        private string dec(string password)
        {
            char[] passArray = password.ToArray();
            string res = "";
            for (int i = 0; i < passArray.Length; i++)
            {
                passArray[i] = (char)(((int)passArray[i]) - i % 5 - 1);
            }
            for (int i = 0; i < passArray.Length; i++)
            {
                res = res + passArray[i];
            }
            return res;
        }

        internal bool changePassword(string userName, string newPaswword)
        {
            try
            {
                _users[userName].password = newPaswword;
                _users[userName].lastTimeUpdatePassword = DateTime.Today;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /*public int getAvilableIntOfPost()
        {
            return 0;
        }*/


    }
}
