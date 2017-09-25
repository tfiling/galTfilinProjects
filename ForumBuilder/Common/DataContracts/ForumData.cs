using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ForumBuilder.Common.DataContracts
{
    [DataContract]
    public class ForumData
    {
        [DataMember]
        private String _forumName;

        [DataMember]
        private String _description;

        [DataMember]
        private ForumPolicyData _forumPolicy;

        [DataMember]
        private List<string> _subForums;
        
        [DataMember]
        private List<string> _members;

        public ForumData(string forumName, string descrption, ForumPolicyData forumPolicy, List<String> subForums, List<String> members)
        {
            _forumName = forumName;
            _description = descrption;
            _forumPolicy = forumPolicy;
            _subForums = subForums;
            _members = members;
        }

        public ForumPolicyData forumPolicy
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

        public List<String> subForums
        {
            get { return _subForums; }
            set { _subForums = value; }
        }

        public List<string> members
        {
            get { return _members; }
            set { _members = value;}
        }
    }
}
