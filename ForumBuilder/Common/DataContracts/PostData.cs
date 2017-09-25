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
    public class PostData
    {
        [DataMember]
        private String _writerUserName;

        [DataMember]
        private Int32 _id;

        [DataMember]
        private String _title;

        [DataMember]
        private String _content;

        [DataMember]
        private Int32 _parentId;

        [DataMember]
        private List<Int32> _commentsIds;

        [DataMember]
        private DateTime _timePublished;

        public PostData(String writerUserName, Int32 id, String title, String content, Int32 parentId, DateTime timePublished)
        {
            _id = id;
            _title = title;
            _content = content;
            _parentId = parentId;
            _commentsIds = new List<int>();
            _timePublished = timePublished;
            _writerUserName = writerUserName;
        }

        public Int32 id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Int32 parentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        public String title
        {
            get { return _title; }
            set { _title = value; }
        }

        public String content
        {
            get { return _content; }
            set { _content = value; }
        }

        public String writerUserName
        {
            get { return _writerUserName; }
            set { _writerUserName = value; }
        }
        public List<Int32> commentsIds
        {
            get { return _commentsIds; }
            set { _commentsIds = value; }
        }

        public DateTime timePublished
        {
            get { return _timePublished; }
            set { _timePublished = value; }
        }

        
    }
}
