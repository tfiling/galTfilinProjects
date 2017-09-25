using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Back_End
{
    public class Moderator
    {
        String _userName;
        DateTime _endTerm;
        DateTime _dateAdded;
        String _nominatorName;
        public Moderator(String userName,DateTime endTerm,DateTime dateAdded,String nominatorName)
        {
            _userName=userName;
            _endTerm=endTerm;
            _dateAdded=dateAdded;
            _nominatorName=nominatorName;
        }
        public string userName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }

        public DateTime endTerm
        {
            get
            {
                return _endTerm;
            }

            set
            {
                _endTerm = value;
            }
        }

        public DateTime dateAdded
        {
            get
            {
                return _dateAdded;
            }

            set
            {
                _dateAdded = value;
            }
        }

        public string nominatorName
        {
            get
            {
                return _nominatorName;
            }

            set
            {
                _nominatorName = value;
            }
        }
    }
}
