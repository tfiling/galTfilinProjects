using System;

namespace BL_Back_End
{
    public class Thread
    {
        private Post _firstPost;
        private String _title;

        public Thread(Post post)
        {
            _firstPost = post;
            _title = post.title;
        }

        public Post firstPost
        {
            get { return _firstPost; }
            set { _firstPost = value; }
        }
    }
}
