using ForumBuilder.Controllers;
using System;
using System.Net.Mail;
using Database;
using System.ServiceModel;
using Service;
using System.ComponentModel.DataAnnotations;
using ForumBuilder.Common.ServiceContracts;
using System.Collections.Generic;
using BL_Back_End;

namespace ForumBuilder.Systems
{
    public class ForumSystem
    {
        private static ForumSystem singleton;
        Logger logger = Logger.getInstance;
        private static ForumSystem getInstance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new ForumSystem();
                }
                return singleton;
            }
        }

        public static ForumSystem initialize(String userName, String password, String email)
        {
            if (singleton == null)
            {
                singleton = new ForumSystem();
                //adding the user
                SuperUserController superUserController = SuperUserController.getInstance;
                if (superUserController.addSuperUser(email, password, userName))
                {
                    //  send configuration email to the super user's 
                    //sendmail(email);
                }
                /*else
                {
                    singleton = null;
                    return null;
                }*/
                
                    Logger logger = Logger.getInstance;
                try
                {

                    /*
                     * //for this to work the exe/vs should be run in administrator mode
                     */
                    ServiceHost forumService = new ServiceHost(typeof(ForumManager));
                    forumService.Open(); 
                    logger.logPrint("forum service was initialized under localhost:8081",0);
                    logger.logPrint("forum service was initialized under localhost:8081",1);

                    ServiceHost postService = new ServiceHost(typeof(PostManager));
                    postService.Open();
                    logger.logPrint("post service was initialized under localhost:8082",0);
                    logger.logPrint("post service was initialized under localhost:8082",1);

                    ServiceHost subForumService = new ServiceHost(typeof(SubForumManager));
                    subForumService.Open();
                    logger.logPrint("sub forum service was initialized under localhost:8083",0);
                    logger.logPrint("sub forum service was initialized under localhost:8083",1);

                    ServiceHost superUserService = new ServiceHost(typeof(SuperUserManager));
                    superUserService.Open();
                    logger.logPrint("super user service was initialized under localhost:8084",0);
                    logger.logPrint("super user service was initialized under localhost:8084",1);

                    ServiceHost userService = new ServiceHost(typeof(UserManager));
                    userService.Open();
                    logger.logPrint("user service was initialized under localhost:8085",0);
                    logger.logPrint("user service was initialized under localhost:8085",1);
                }
                catch (CommunicationException)
                {
                    logger.logPrint("failed to initialize services",0);
                    logger.logPrint("failed to initialize services",2);
                    return null;
                }

                logger.logPrint("The System was initialized successully",0);
                logger.logPrint("The System was initialized successully",1);
            }
            else
            {
                SuperUserController superUserController = SuperUserController.getInstance;
                superUserController.addSuperUser(email, password, userName);
            }
            return getInstance;
        }
        private static void sendmail(string email)
        {
            String ourEmail = "ourEmail@gmail.com";
            MailMessage mail = new MailMessage(ourEmail, email);
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.google.com";
            mail.Subject = "please configure your account";
            mail.Body = "please configure your account";
            client.Send(mail);
        }

        public static int Main(string[] args)
        {
            Console.WriteLine("welcome to your forum builder!\n" +
                "would you like to initialize the system? Y = yes:");
            String s= Console.ReadLine();
            if (s.Equals("Y"))
            {
                var item = DBClass.getInstance;
                DBClass.getInstance.clear();
                Console.WriteLine("welcome to your forum builder!\n" +
                                    "please insert your desired user name:");
                String username = "idan";//getUserName();
                String password = "idanA1";//getUserPassword();
                String email = "d@d.d";//getEmail();

                initialize(username, password, email);
                setUpDB();
                runServer(username, password, email);
                return 0;
            }
            else
            {
                var item = DBClass.getInstance;
                Console.WriteLine("welcome to your forum builder!\n" +
                                    "please insert your desired user name:");
                String username = "idan";//getUserName();
                String password = "idanA1";//getUserPassword();
                String email = "d@d.d";//getEmail();

                initialize(username, password, email);
                runServer(username, password, email);
                return 0;
            }
        }
        private static void setUpDB()
        {
            ForumPolicy fp1 = new ForumPolicy("policy sport", true, 0, true, 30, 1, true, true, 5, 0, new List<string>());
            ForumPolicy fp2 = new ForumPolicy("polocy music", false, 0, false, 180, 2, false, false, 5, 1, new List<string>());
            List<String> list = new List<String>();
            list.Add("idan");
            if (!SuperUserController.getInstance.createForum("Sport", "all about sport", fp1, list, "idan").Equals("Forum " + "Sport" + " creation success"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!1");
            if (!SuperUserController.getInstance.createForum("Music", "all about music", fp2, list, "idan").Equals("Forum " + "Music" + " creation success"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!2");
            if (!ForumController.getInstance.registerUser("admin_sport", "gG1111", "as@as.as", "sad", "bad", "Sport").Equals("Register user succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!2");
            if (!ForumController.getInstance.registerUser("user_sport", "gG1234", "us@us.us", "sad", "bad", "Sport").Equals("Register user succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!3");
            if (!ForumController.getInstance.registerUser("mod_basketball", "1234gG", "ms@ms.ms", "sad", "bad", "Sport").Equals("Register user succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!4");
            if (!ForumController.getInstance.registerUser("mod_football", "12Ff34", "mf@mf.mf", "sad", "bad", "Sport").Equals("Register user succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!5");
            if (!ForumController.getInstance.registerUser("admin_music", "m12M34", "am@am.am", "", "", "Music").Equals("Register user succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!6");
            if (!ForumController.getInstance.registerUser("user_music", "Mm5656", "um@um.um", "", "", "Music").Equals("Register user succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!7");
            if (!ForumController.getInstance.registerUser("mod1_guitar", "Mg3333", "m1g@m1g.m1g", "", "", "Music").Equals("Register user succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!8");
            if (!ForumController.getInstance.registerUser("mod2_guitar", "G4444g", "m2g@m2g.m2g", "", "", "Music").Equals("Register user succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!9");
            if (!ForumController.getInstance.registerUser("mod1_concert", "Ll6666", "m1c@m1c.m1c", "", "", "Music").Equals("Register user succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!10");
            if (!ForumController.getInstance.registerUser("mod2_concert", "Cc7890", "m2c@m2c.m2c", "", "", "Music").Equals("Register user succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!11");
            if (!ForumController.getInstance.nominateAdmin("admin_sport", "idan", "Sport").Equals("admin nominated successfully"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!12");
            if (!ForumController.getInstance.nominateAdmin("admin_music", "idan", "Music").Equals("admin nominated successfully"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!13");
            Dictionary<String, DateTime> mod_basketball = new Dictionary<String, DateTime>();
            mod_basketball.Add("mod_basketball", new DateTime(2017, 1, 1));
            if (!ForumController.getInstance.addSubForum("Sport", "basketball", mod_basketball, "admin_sport").Equals("sub-forum added"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!14");
            Dictionary<String, DateTime> mod_football = new Dictionary<String, DateTime>();
            mod_football.Add("mod_football", new DateTime(2017, 1, 1));
            if (!ForumController.getInstance.addSubForum("Sport", "football", mod_football, "admin_sport").Equals("sub-forum added"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!15");
            Dictionary<String, DateTime> mod_guitar = new Dictionary<String, DateTime>();
            mod_guitar.Add("mod1_guitar", new DateTime(2017, 1, 1));
            mod_guitar.Add("mod2_guitar", new DateTime(2017, 1, 1));
            if (!ForumController.getInstance.addSubForum("Music", "guitar", mod_guitar, "admin_music").Equals("sub-forum added"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!16");
            Dictionary<String, DateTime> mod_concert = new Dictionary<String, DateTime>();
            mod_concert.Add("mod1_concert", new DateTime(2017, 1, 1));
            mod_concert.Add("mod2_concert", new DateTime(2017, 1, 1));
            if (!ForumController.getInstance.addSubForum("Music", "concert", mod_concert, "admin_music").Equals("sub-forum added"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!17");
            if (!SubForumController.getInstance.createThread("headline1", "content1", "user_music", "Music", "guitar").Equals("Create tread succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!18");
            if (!SubForumController.getInstance.createThread("headline2", "content2", "admin_music", "Music", "guitar").Equals("Create tread succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!19");
            if (!SubForumController.getInstance.createThread("headline3", "content3", "mod1_concert", "Music", "concert").Equals("Create tread succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!20");
            if (!SubForumController.getInstance.createThread("headline4", "content4", "mod2_concert", "Music", "concert").Equals("Create tread succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!21");
            if (!SubForumController.getInstance.createThread("headline5", "content5", "user_sport", "Sport", "basketball").Equals("Create tread succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!22");
            if (!SubForumController.getInstance.createThread("headline6", "content6", "admin_sport", "Sport", "basketball").Equals("Create tread succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!23");
            if (!SubForumController.getInstance.createThread("headline7", "content7", "mod_basketball", "Sport", "football").Equals("Create tread succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!24");
            if (!SubForumController.getInstance.createThread("headline8", "content8", "mod_football", "Sport", "football").Equals("Create tread succeed"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!25");
            if (!PostController.getInstance.addComment("headline1", "content1", "admin_music", 0).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!26");
            if (!PostController.getInstance.addComment("headline2", "content2", "admin_music", 0).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!27");
            if (!PostController.getInstance.addComment("headline3", "content3", "mod1_guitar", 1).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!28");
            if (!PostController.getInstance.addComment("headline4", "content4", "mod2_guitar", 1).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!29");
            if (!PostController.getInstance.addComment("headline5", "content5", "mod2_concert", 2).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!30");
            if (!PostController.getInstance.addComment("headline6", "content6", "mod1_concert", 2).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!31");
            if (!PostController.getInstance.addComment("headline7", "content7", "admin_music", 3).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!32");
            if (!PostController.getInstance.addComment("headline8", "content8", "user_music", 3).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!33");
            if (!PostController.getInstance.addComment("headline1", "content1", "admin_sport", 4).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!34");
            if (!PostController.getInstance.addComment("headline2", "content2", "user_sport", 4).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!35");
            if (!PostController.getInstance.addComment("headline3", "content3", "admin_sport", 5).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!36");
            if (!PostController.getInstance.addComment("headline4", "content4", "user_sport", 5).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!37");
            if (!PostController.getInstance.addComment("headline5", "content5", "mod_basketball", 6).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!38");
            if (!PostController.getInstance.addComment("headline6", "content6", "user_sport", 6).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!39");
            if (!PostController.getInstance.addComment("headline7", "content7", "admin_sport", 7).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!40");
            if (!PostController.getInstance.addComment("headline8", "content8", "mod_football", 7).Equals("comment created"))
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!41");
        }
        private static String getUserName()
        {
            String userName = Console.ReadLine();
            String ans;
            while (true)
            {
                Console.WriteLine("please confirm your user name: \"" + userName + "\"  yes/no");
                ans = Console.ReadLine();
                if (ans.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    break;
                else if (ans.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("user name: \"" + userName + "\" was rejected, please insert new one");
                    userName = Console.ReadLine();
                }
            }
            return userName;
        }

        private static String getUserPassword()
        {
            Console.WriteLine("please insert your desired password");
            String userPass = Console.ReadLine();
            String ans;
            while (true)
            {
                Console.WriteLine("please confirm your password: \"" + userPass + "\"  yes/no");
                ans = Console.ReadLine();
                if (ans.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    break;
                else if (ans.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("password: \"" + userPass + "\" was rejected, please insert new one");
                    userPass = Console.ReadLine();
                }
            }
            return userPass;
        }

        private static String getEmail()
        {
            Console.WriteLine("please insert your email address");
            String email = Console.ReadLine();
            String ans;
            while (true)
            {
                if (!new EmailAddressAttribute().IsValid(email))
                {
                    Console.WriteLine("email address is invalid, please insert a new one");
                    email = Console.ReadLine();
                    continue;
                }
                Console.WriteLine("please confirm your email address: \"" + email + "\"  yes/no");
                ans = Console.ReadLine();
                if (ans.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    break;
                else if (ans.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("email address: \"" + email + "\" was rejected \nplease insert you valid email address");
                    email = Console.ReadLine();
                }
            }
            return email;
        }

        public static void runServer(String userName, String password, String email)
        {
            Console.WriteLine("server is running. \n" +
                                "super user credentials:" +
                                "user name: " + userName + "  password: " + password + 
                                "  email: " + email);
            /*while (true)
            {
            }*/
            Console.ReadLine();
        }
    }
}
