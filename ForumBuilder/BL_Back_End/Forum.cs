using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Back_End
{
    public class 
        Forum
    {
        private String _forumName;
        private String _description;
        private ForumPolicy _forumPolicy;
        private List<string> _administrators;
        private List<string> _members;
        private List<string> _subForums;

        public Forum(string forumName, string descrption,ForumPolicy policy, List<string> administrators)
        {
            _forumName = forumName;
            _description = descrption;
            _forumPolicy = policy;
            _administrators = administrators;
            _members = new List<string>();
            _subForums = new List<string>();
        }

        public Forum(string forumName, string descrption, ForumPolicy forumPolicy, List<string> administrators, List<string> subForums)
        {
            _forumName = forumName;
            _description = descrption;
            _forumPolicy = forumPolicy;
            _administrators = administrators;
            _members = new List<string>();
            _subForums = subForums;
        }

        public ForumPolicy forumPolicy
        {
            get { return _forumPolicy; }
            set { _forumPolicy = value; }
        }

        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        public String forumName
        {
            get { return _forumName; }
            set { _forumName = value; }
        }

        public List<String> administrators
        {
            get { return _administrators; }
            set { _administrators = value; }
        }

        public List<String> subForums
        {
            get { return _subForums; }
            set { _subForums = value; }
        }

        public List<string> members
        {
            get
            {
                return _members;
            }

            set
            {
                _members = value;
            }
        }
        
        public static int Main(string[] args)
        {
            return -1;
        }
    }
}
