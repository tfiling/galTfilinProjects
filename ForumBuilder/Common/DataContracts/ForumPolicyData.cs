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
    public class ForumPolicyData
    {

        public const int ONLINE_NOTIFICATIONS_TPYE = 0;
        public const int OFFLINE_NOTIFICATIONS_TPYE = 1;
        public const int SELECTIVE_NOTIFICATIONS_TPYE = 2;

        [DataMember]
        private String _policy;

        [DataMember]
        private bool _isQuestionIdentifying;

        [DataMember]
        private int _seniorityInForum;

        [DataMember]
        private bool _deletePostByModerator;

        [DataMember]
        private int _timeToPassExpiration;

        [DataMember]
        private int _minNumOfModerator;

        [DataMember]
        private bool _hasCapitalInPassword;

        [DataMember]
        private bool _hasNumberInPassword;

        [DataMember]
        private int _minLengthOfPassword;

        [DataMember]
        private int _notificationsType;

        [DataMember]
        private List<String> _selectiveNotificationsUsers;

        public ForumPolicyData()
        {
            _policy = " ";
            _isQuestionIdentifying = false;
            _seniorityInForum = 0;
            _deletePostByModerator = false;
            _timeToPassExpiration = 30;
            _minNumOfModerator = 1;
            _hasCapitalInPassword = false;
            _hasNumberInPassword = false;
            _minLengthOfPassword = 5;
            _notificationsType = ONLINE_NOTIFICATIONS_TPYE;
            _selectiveNotificationsUsers = new List<string>();
        }

        public ForumPolicyData(String policy, bool isQuestionIdentifying, int seniorityInForum, bool deletePostByMderator, int timeToPassExpiration,
                                int minNumOfModerator, bool hasCapitalInPassword, bool hasNumberInPassword, int minLengthOfPassword, int notificationsType,
                                List<string> selectiveNotificationsUsers)
        {
            _policy = policy;
            _isQuestionIdentifying = isQuestionIdentifying;
            _seniorityInForum = seniorityInForum;
            _deletePostByModerator = deletePostByMderator;
            _timeToPassExpiration = timeToPassExpiration;
            _minNumOfModerator = minNumOfModerator;
            _hasCapitalInPassword = hasCapitalInPassword;
            _hasNumberInPassword = hasCapitalInPassword;
            _minLengthOfPassword = minLengthOfPassword;
            _notificationsType = notificationsType;
            _selectiveNotificationsUsers = (selectiveNotificationsUsers != null) ? new List<String>(selectiveNotificationsUsers) : new List<string>();
        }

        public Boolean isQuestionIdentifying
        {
            get { return _isQuestionIdentifying; }
            set { _isQuestionIdentifying = value; }
        }

        public Boolean deletePostByModerator
        {
            get { return _deletePostByModerator; }
            set { _deletePostByModerator = value; }
        }

        public int seniorityInForum
        {
            get { return _seniorityInForum; }
            set { _seniorityInForum = value; }
        }

        public int timeToPassExpiration
        {
            get { return _timeToPassExpiration; }
            set { _timeToPassExpiration = value; }
        }

        public string policy
        {
            get { return _policy; }
            set { _policy = value; }
        }

        public int minNumOfModerator
        {
            get { return _minNumOfModerator; }
            set { _minNumOfModerator = value; }
        }

        public bool hasCapitalInPassword
        {
            get { return _hasCapitalInPassword; }
            set { _hasCapitalInPassword = value; }
        }

        public bool hasNumberInPassword
        {
            get { return _hasNumberInPassword; }
            set { _hasNumberInPassword = value; }
        }

        public int minLengthOfPassword
        {
            get { return _minLengthOfPassword; }
            set { _minLengthOfPassword = value; }
        }

        public int notificationsType
        {
            get { return _notificationsType; }
            set { _notificationsType = value; }
        }

        public List<String> selectiveNotificationsUsers
        {
            get { return _selectiveNotificationsUsers; }
            set { _selectiveNotificationsUsers = (value != null) ? new List<String>(value) : new List<string>(); }
        }

    }
}
