using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ForumBuilder.Common.DataContracts;


namespace ForumBuilder.Common.ServiceContracts
{
    [ServiceContract]
    public interface ISuperUserManager
    {
        [OperationContract]
        String createForum(String forumName, String descrption, ForumPolicyData fpd, List<String> administrators, String superUserName);

        [OperationContract]
        Boolean login(String newUser, String forumName, string email);

        [OperationContract]
        Boolean isSuperUser(String user);

        [OperationContract]
        int SuperUserReportNumOfForums(string superUserName);

        [OperationContract]
        List<String> getSuperUserReportOfMembers(string superUserName);

        [OperationContract]
        String addUser(string userName, string password, string mail, string superUserName);
    }
}
