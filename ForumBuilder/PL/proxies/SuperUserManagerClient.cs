using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ForumBuilder.Common.ServiceContracts;
using ForumBuilder.Common.DataContracts;

namespace PL.proxies
{
    public class SuperUserManagerClient : ClientBase<ISuperUserManager>, ISuperUserManager
    {
        public SuperUserManagerClient()
        {
        }
    
        public SuperUserManagerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
    
        public SuperUserManagerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
    
        public SuperUserManagerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public SuperUserManagerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }

        public String createForum(String forumName, String descrption, ForumPolicyData fpd, List<String> administrators, String superUserName)
        {
            return Channel.createForum(forumName, descrption, fpd, administrators, superUserName);
        }

        public Boolean login(string name, string pass, string email)
        {
            return Channel.login(name, pass, email);
        }

        public Boolean isSuperUser(string name)
        {
            return Channel.isSuperUser(name);
        }

        public int SuperUserReportNumOfForums(string superUserName)
        {
            return Channel.SuperUserReportNumOfForums(superUserName);
        }

        public List<String> getSuperUserReportOfMembers(string superUserName)
        {
            return Channel.getSuperUserReportOfMembers(superUserName);
        }

        public String addUser(string userName, string password, string mail, string superUserName)
        {
            return Channel.addUser(userName, password, mail, superUserName);
        }
    }
}
