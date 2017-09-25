using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Back_End
{
    public class ForumPolicy
    {
        public const int ONLINE_NOTIFICATIONS_TPYE = 0;
        public const int OFFLINE_NOTIFICATIONS_TPYE = 1;
        public const int SELECTIVE_NOTIFICATIONS_TPYE = 2;

        private String _policy;
        private bool _isQuestionIdentifying;
        private int _seniorityInForum;
        private bool _deletePostByModerator;
        private int _timeToPasswordExpiration;
        private int _minNumOfModerators;
        private bool _hasCapitalInPassword;
        private bool _hasNumberInPassword;
        private int _minLengthOfPassword;
        private int _notificationsType;
        private List<String> _selectiveNotificationsUsers;

        public ForumPolicy(string policy,bool isQuestionIdentifying,int seniorityInForum,
         bool deletePostByModerator,int timeToPasswordExpiration,int minNumOfModerators,bool hasCapitalInPassword,
         bool hasNumberInPassword, int minLengthOfPassword, int notificationsType, List<String> selectiveNotificationsUsers)
        {
             _policy=policy;
             _isQuestionIdentifying=isQuestionIdentifying;
             _seniorityInForum=seniorityInForum;
             _deletePostByModerator=deletePostByModerator;
             _timeToPasswordExpiration=timeToPasswordExpiration;
             _minNumOfModerators=minNumOfModerators;
             _hasCapitalInPassword=hasCapitalInPassword;
             _hasNumberInPassword=hasNumberInPassword;
             _minLengthOfPassword=minLengthOfPassword;
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
            get { return _timeToPasswordExpiration; }
            set { _timeToPasswordExpiration = value; }
        }

        public string policy
        {
            get { return _policy; }
            set { _policy = value; }
        }

        public int minNumOfModerators
        {
            get
            {
                return _minNumOfModerators;
            }

            set
            {
                _minNumOfModerators = value;
            }
        }

        public bool hasCapitalInPassword
        {
            get
            {
                return _hasCapitalInPassword;
            }

            set
            {
                _hasCapitalInPassword = value;
            }
        }

        public bool hasNumberInPassword
        {
            get
            {
                return _hasNumberInPassword;
            }

            set
            {
                _hasNumberInPassword = value;
            }
        }

        public int minLengthOfPassword
        {
            get
            {
                return _minLengthOfPassword;
            }

            set
            {
                _minLengthOfPassword = value;
            }
        }

        public int notificationsType
        {
            get 
            { 
                return _notificationsType; 
            }

            set 
            { 
                _notificationsType = value; 
            }
        }

        public List<String> selectiveNotificationsUsers
        {
            get 
            { 
                return _selectiveNotificationsUsers; 
            }
            set 
            { 
                _selectiveNotificationsUsers = (value != null) ? new List<String>(value) : new List<string>(); 
            }
        }

    }
}
