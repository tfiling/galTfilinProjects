using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using BL_Back_End;

namespace ForumBuilder.Controllers
{
    public class SuperUserController : UserController, ISuperUserController
    {
        private static SuperUserController singleton;
        ForumController forumController = ForumController.getInstance;
        DBClass DB = DBClass.getInstance;
        Systems.Logger logger = Systems.Logger.getInstance;
        String loggedInSuperUser = "";


        public static SuperUserController getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new SuperUserController();
                    Systems.Logger.getInstance.logPrint("Super user contoller created",0);
                    Systems.Logger.getInstance.logPrint("Super user contoller created",1);
                }
                return singleton;
            }
        }

        public String createForum(String forumName, String descrption, ForumPolicy fp, List<String> administrators, String superUserName)
        {

            if (forumName.Equals("") || descrption.Equals("") || fp.policy.Equals("") || fp.seniorityInForum<0||
                fp.timeToPassExpiration<30 || fp.minLengthOfPassword<0 || administrators == null||
                administrators.Count==0)
            {
                logger.logPrint("cannot create new forum because one or more of the fields is empty",0);
                logger.logPrint("cannot create new forum because one or more of the fields is empty",2);
                return "cannot create new forum because one or more of the fields is empty";
            }
            if (DB.getSuperUser(superUserName) == null)
            {
                logger.logPrint("create forum fail " + superUserName + " is not super user",0);
                logger.logPrint("create forum fail " + superUserName + " is not super user",2);
                return "create forum fail " + superUserName + " is not super user";
            }    
            foreach(String user in administrators)
            {
                if (!isSuperUser(user)) {
                    if (DB.getUser(user) == null)
                    {
                        logger.logPrint("create forum fail admin does not exist", 0);
                        logger.logPrint("create forum fail admin does not exist", 2);
                        return "create forum fail admin does not exist";
                    }
                    foreach (string f in forumController.getForums())
                    {
                        if (forumController.getForum(f).members.Contains(user))
                        {
                            logger.logPrint("create forum fail admin already member in anouther forum", 0);
                            logger.logPrint("create forum fail admin already member in anouther forum", 2);
                            return "create forum fail admin already member in anouther forum";
                        }
                    }
                }
            }            
            if (DB.createForum(forumName, descrption, fp))
            {
                this.forumController.addForum(forumName);
                foreach(String admin in administrators)
                {
                    ForumController.getInstance.nominateAdmin(admin, superUserName,forumName);
                }
                logger.logPrint("Forum " + forumName + " creation success",0);
                logger.logPrint("Forum " + forumName + " creation success",1);
                return "Forum " + forumName + " creation success";
            }
            return "Forum " + forumName + " creation failed";
        }

        public bool addSuperUser(string email, string password, string userName)
        {
            if (userName.Equals("") || password.Equals("") || email.Equals(""))
            {
                logger.logPrint("one or more of the fields is missing",0);
                logger.logPrint("one or more of the fields is missing",2);
                return false;
            }
            // check if the password is strong enough
            bool isNumExist = false;
            bool isSmallKeyExist = false;
            bool isBigKeyExist = false;
            bool isKeyRepeting3Times = false;
            for (int i = 0; i < password.Length; i++)
            {
                if (password.ElementAt(i) <= '9' && password.ElementAt(i) >= '0')
                {
                    isNumExist = true;
                }
                if (password.ElementAt(i) <= 'Z' && password.ElementAt(i) >= 'A')
                {
                    isBigKeyExist = true;
                }
                if (password.ElementAt(i) <= 'z' && password.ElementAt(i) >= 'a')
                {
                    isSmallKeyExist = true;
                }
                if (i < password.Length - 2 && (password.ElementAt(i).Equals(password.ElementAt(i + 1)) && password.ElementAt(i).Equals(password.ElementAt(i + 2))))
                {
                    isKeyRepeting3Times = true;
                }
            }
            if (!(isNumExist && isSmallKeyExist && isBigKeyExist && !isKeyRepeting3Times))
            {
                logger.logPrint("password isnt strong enough",0);
                logger.logPrint("password isnt strong enough",2);
                return false;
            }
            // check if the the email is in a correct format
            int index = email.IndexOf("@");
            if (index < 0 || index == email.Length - 1)
            {
                logger.logPrint("error in email format",0);
                logger.logPrint("error in email format",2);
                return false;
            }
            return DB.addSuperUser(email, password, userName);
        }

        public Boolean isSuperUser(string user)
        {
            if (DB.getSuperUser(user) == null)
            {
                return false;
            }
            return true;
        }
        public String addUser(string userName, string password, string mail, string superUserName)
        {
            if (!isSuperUser(superUserName))
            {
                return superUserName +"is not a superUser";
            }
            if (userName.Length > 0 && password.Length > 0 && mail.Length > 0)
            {
                if (DB.getUser(userName) != null)
                {
                    logger.logPrint("Register user faild, " + userName + " is already taken",0);
                    logger.logPrint("Register user faild, " + userName + " is already taken",2);
                    return "Register user faild, " + userName + " is already taken";
                }
                if (mail.IndexOf('@') <= 0 || mail.Substring(mail.IndexOf('@') + 1).IndexOf('.') <= 0)
                {
                    logger.logPrint("Register user faild, " + userName + " is already taken", 0);
                    logger.logPrint("Register user faild, " + userName + " is already taken", 2);
                    return "Register user faild, " + mail + " is not a valid mail address!";
                }
                if (DB.addUser(userName, password, mail, " ", " "))
                {
                    return "Register user " + userName + "completed";
                }
                return "fail to add user";
            }
            logger.logPrint("Register user faild, password not strong enough",0);
            logger.logPrint("Register user faild, password not strong enough",2);
            return "Register user failed, one or more of the fields is empty";
        }
        public Boolean login(String user, String password, string email)
        {
            User superUser = DB.getSuperUser(user);
            if (superUser != null && superUser.password.Equals(password) && superUser.email.Equals(email))
            {
                loggedInSuperUser = user;
                return true;
            }
            else
            {
                logger.logPrint("could not login, wrong cerdintals",0);
                logger.logPrint("could not login, wrong cerdintals",2);
                return false;
            }
        }
        public int SuperUserReportNumOfForums(string superUserName)
        {
            if (isSuperUser(superUserName))
                return DB.numOfForums();
            else
                return -1;
        }
        public List<String> getSuperUserReportOfMembers(string superUserName)
        {
            if (isSuperUser(superUserName))
                return DB.getSuperUserReportOfMembers();
            else
                return null;
        }
    }
}
