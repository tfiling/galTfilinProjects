using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ForumBuilder.Common.DataContracts
{
    [DataContract]
    public class SubForumData
    {
        [DataMember]
        private String _name;

        [DataMember]
        private Dictionary<String, DateTime> _moderators;

        [DataMember]
        private List<Int32> _threads;

        [DataMember]
        private String _forum;

        public SubForumData(String name, String forumName)
        {
            _name = name;
            _moderators = new Dictionary<String, DateTime>();
            _threads = new List<int>();
            _forum = forumName;
        }



        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Dictionary<String, DateTime> moderators
        {
            get { return _moderators; }
            set { _moderators = value; }
        }

        public List<Int32> threads
        {
            get { return _threads; }
            set { _threads = value; }
        }
        public String forum
        {
            get { return _forum; }
            set { _forum = value; }
        }
    }
}
