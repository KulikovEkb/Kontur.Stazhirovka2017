using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRest
{
    public class UserService : IUserService, IDatabaseCommunication
    {
        static Users users = new Users();
        int ID = 1;

        public Users GetAllUsers()
        {
            if (users == null || users.Count == 0)
                return GenerateFakeUsers();
            else
                return FindUsers();
        }

        public void AddNewUser(User user)
        {
            user.userID = ID++;
            users.Add(user);
            Write2Database(user);
        }

        public User GetUser(string userID)
        {
            return FindUser(Convert.ToInt32(userID));
        }

        private Users GenerateFakeUsers()
        {
            users.Add(new User { firstName = "Alik", lastName = "Levin", email = "alikl@microsoft.com", userID = ID++ });
            Write2Database(new User { firstName = "Alik", lastName = "Levin", email = "alikl@microsoft.com", userID = 1 });
            users.Add(new User { firstName = "Loki", lastName = "Lenin", email = "lenin@microsoft.com", userID = ID++ });
            Write2Database(new User { firstName = "Loki", lastName = "Lenin", email = "lenin@microsoft.com", userID = 2 });
            return users;
        }

        //private User FindUser(int userID)
        //{
        //    return users.Find(x => x.userID == userID);
        //}

        public void Write2Database(User user)
        {
            using (var dataBase = new LiteDatabase(@"TestStorage.db"))
            {
                var userCollection = dataBase.GetCollection<User>("users");
                userCollection.Insert(user);
                Console.WriteLine("WRITING SUCCESS!");
            }
        }

        public User FindUser(int ID)
        {
            using (var dataBase = new LiteDatabase(@"TestStorage.db"))
            {
                var userCollection = dataBase.GetCollection<User>("users");
                userCollection.EnsureIndex(x => x.userID);
                Console.WriteLine("reading success!");
                return (User)userCollection.Find(x => x.userID == ID);
            }
        }

        public Users FindUsers()
        {
            using (var dataBase = new LiteDatabase(@"TestStorage.db"))
            {
                var userCollection = dataBase.GetCollection<User>("users");
                userCollection.EnsureIndex(x => x.userID);
                return (Users)userCollection.FindAll();

            }
        }
    }
}
