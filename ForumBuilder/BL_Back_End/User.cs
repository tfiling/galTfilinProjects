using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Back_End
{
    public class User
    {
        private String _userName;
        private String _password;
        private String _email;
        private DateTime _dateAdd;
        private List<String> _friends;
        private DateTime _lastTimeUpdatePassword; 

        public User(String userName, String password, String email,DateTime dateAdd)
        {
            _userName = userName;
            _password = password;
            _email = email;
            _dateAdd = dateAdd;
            _friends = new List<String>();
            _lastTimeUpdatePassword=DateTime.Today;
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

        public DateTime date
        {
            get { return _dateAdd; }
            set { _dateAdd = value; }
        }

        public DateTime lastTimeUpdatePassword
        {
            get
            {
                return _lastTimeUpdatePassword;
            }

            set
            {
                _lastTimeUpdatePassword = value;
            }
        }
    }
}
