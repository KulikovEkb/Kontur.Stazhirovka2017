using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace TestRest
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/users", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Users GetAllUsers();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/user", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void AddNewUser(User user);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        User GetUser(string userID);
    }
}
