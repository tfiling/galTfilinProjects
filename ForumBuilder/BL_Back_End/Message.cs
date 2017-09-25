using System;

namespace BL_Back_End
{
    public class Message
    {
        private Int32 _id;
        private String _sender;
        private String _reciver;
        private String _content;

        public Message(Int32 id, String sender, String reciver, String content)
        {
            _id = id;
            _sender = sender;
            _reciver = reciver;
            Content = content;
        }

        public Int32 id
        {
            get { return _id; }
            set { _id = value; }
        }

        public String sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        public String reciver
        {
            get { return _reciver; }
            set { _reciver = value; }
        }

        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;
            }
        }
    }
}
