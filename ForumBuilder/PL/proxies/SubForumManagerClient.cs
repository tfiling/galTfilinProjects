using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ForumBuilder.Common.ServiceContracts;

namespace PL.proxies
{
    public class SubForumManagerClient : ClientBase<ISubForumManager>, ISubForumManager
    {
        public SubForumManagerClient() 
        {
        }
    
        public SubForumManagerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
    
        public SubForumManagerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
    
        public SubForumManagerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public SubForumManagerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }

        public String dismissModerator(String dismissedModerator, String dismissByAdmin, String subForumName, String forumName)
        {
            return Channel.dismissModerator(dismissedModerator, dismissByAdmin, subForumName, forumName);
        }

        public String nominateModerator(String newModerator, String nominatorUser, DateTime date, String subForumName, String forumName)
        {
            return Channel.nominateModerator(newModerator, nominatorUser, date, subForumName, forumName);
        }

        public String createThread(String headLine, String content, String writerName, String forumName, String subForumName)
        {
            return Channel.createThread(headLine, content, writerName, forumName, subForumName);
        }

        public String deleteThread(int firstPostId, string removerName)
        {
            return Channel.deleteThread(firstPostId, removerName);
        }

        public Boolean isModerator(string name, string subForumName, string forumName)
        {
            return Channel.isModerator(name, subForumName, forumName);
        }

    }
}
