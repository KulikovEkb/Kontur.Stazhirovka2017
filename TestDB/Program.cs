using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestDB
{
    [DataContract(Name = "user", Namespace = "")]
    public class User
    {
        [DataMember(Name = "id", Order = 1)]
        public int Id { get; set; }
        [DataMember(Name = "firstname", Order = 2)]
        public string FirstName { get; set; }
        [DataMember(Name = "lastname", Order = 3)]
        public string LastName { get; set; }
        [DataMember(Name = "email", Order = 4)]
        public string Email { get; set; }
    }

    public class Users : List<User>
    {

    }

    public class UserService
    {
        static Users users = new Users();
        int ID = 1;

        public void GetAllUsers()
        {
            if (users == null || users.Count == 0)
                GenerateFakeUsers();
            FindUsers();
        }

        public void AddNewUser(User user)
        {
            user.Id = ID++;
            users.Add(user);
            Write2Database(user);
        }

        public void GetUser(string userID)
        {
            FindUser(Convert.ToInt32(userID));
        }

        private Users GenerateFakeUsers()
        {
            users.Add(new User { FirstName = "Alik", LastName = "Levin", Email = "alikl@microsoft.com", Id = ID++ });
            Write2Database(new User { Id = 1, FirstName = "Alik", LastName = "Levin", Email = "alikl@microsoft.com" });
            users.Add(new User { FirstName = "Loki", LastName = "Lenin", Email = "lenin@microsoft.com", Id = ID++ });
            Write2Database(new User { FirstName = "Loki", LastName = "Lenin", Email = "lenin@microsoft.com", Id = 2 });
            return users;
        }

        public void Write2Database(User user)
        {
            using (var dataBase = new LiteDatabase(@"TestStorage.db"))
            {
                var userCollection = dataBase.GetCollection<User>("users");
                userCollection.Insert(user);
                Console.WriteLine("WRITING SUCCESS!");
            }
        }

        public void FindUser(int ID)
        {
            using (var dataBase = new LiteDatabase(@"TestStorage.db"))
            {
                var userCollection = dataBase.GetCollection<User>("users");
                userCollection.EnsureIndex(x => x.Id);
                Console.WriteLine("reading success!");
                var result = userCollection.Find(x => x.Id == ID);
                foreach (User us in result)
                    Console.WriteLine(us.FirstName);
            }
        }

        public void FindUsers()
        {
            using (var dataBase = new LiteDatabase(@"TestStorage.db"))
            {
                var userCollection = dataBase.GetCollection<User>("users");
                userCollection.EnsureIndex(x => x.Id);
                Console.WriteLine("reading success!");
                var result = userCollection.FindAll();
                foreach (User us in result)
                    Console.WriteLine(us.FirstName);

            }
        }
    }

    class Program
    {


        static void Main(string[] args)
        {
            UserService instance = new TestDB.UserService();
            instance.GetAllUsers();
            User testUser = new User { Id = 0, FirstName = "a", LastName = "b", Email = "c" };
            instance.AddNewUser(testUser);
            instance.GetUser("3");
            instance.GetAllUsers();
        }
    }
}
