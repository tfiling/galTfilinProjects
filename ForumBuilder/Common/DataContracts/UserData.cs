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
    public class UserData
    {
        [DataMember]
        private String _userName;

        [DataMember]
        private String _password;

        [DataMember]
        private String _email;

        [DataMember]
        private List<String> _friends;

        public UserData(String userName, String password, String email)
        {
            _userName = userName;
            _password = password;
            _email = email;
            _friends = new List<String>();
        }

        public String userName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public String password
        {
            get { return _password; }
            set { _password = value; }
        }

        public String email
        {
            get { return _email; }
            set { _email = value; }
        }

        public List<String> friends
        {
            get { return _friends; }
            set { _friends = value; }
        }
    }
}
