using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestRest
{
    [DataContract(Name = "user", Namespace = "")]
    public class User
    {
        [DataMember(Name = "id", Order = 1)]
        public int userID;
        [DataMember(Name = "firstname", Order = 2)]
        public string firstName;
        [DataMember(Name = "lastname", Order = 3)]
        public string lastName;
        [DataMember(Name = "email", Order = 4)]
        public string email;
    }

    [CollectionDataContract(Name = "users", Namespace = "")]
    public class Users : List<User>
    {
        
    }
}
