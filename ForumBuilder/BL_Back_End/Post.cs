using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Back_End
{
    public class Post
    {
        private String _writerUserName;
        private Int32 _id;
        private String _title;
        private String _content;
        private Int32 _parentId;
        private List<Int32> _commentsIds;
        private DateTime _timePublished;
        private String _forumName;

        public Post(String writerUserName, Int32 id, String title, String content, Int32 parentId, DateTime timePublished,String forumName)
        {
            _id = id;
            _title = title;
            _content = content;
            _parentId = parentId;
            _commentsIds = new List<int>();
            _timePublished = timePublished;
            _writerUserName = writerUserName;
            _forumName = forumName;
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
            get{return _timePublished;}
            set{_timePublished = value;}
        }
        public String forumName
        {
            get { return _forumName; }
            set { _forumName = value; }
        }
    }
}
