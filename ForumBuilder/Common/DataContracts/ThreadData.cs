using System;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ForumBuilder.Common.DataContracts
{
    [DataContract]
    public class ThreadData
    {
        [DataMember]
        private PostData _firstPost;

        [DataMember]
        private String _title;

        public ThreadData(PostData post)
        {
            _firstPost = post;
            _title = post.title;
        }

        public PostData firstPost
        {
            get { return _firstPost; }
            set { _firstPost = value; }
        }
    }
}
