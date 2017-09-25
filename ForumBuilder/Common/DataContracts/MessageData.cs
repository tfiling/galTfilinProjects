using System;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ForumBuilder.Common.DataContracts
{
    [DataContract]
    public class MessageData
    {
        [DataMember]
        private Int32 _id;

        [DataMember]
        private String _sender;

        [DataMember]
        private String _reciver;

        [DataMember]
        private String _content;

        public MessageData(Int32 id, String sender, String reciver, String content)
        {
            _id = id;
            _sender = sender;
            _reciver = reciver;
            _content = content;
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
    }
}
