using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LiteDB;
using System.ServiceModel.Web;

namespace Kontur.GameStats.Server.ServiceTests
{
    [TestFixture]
    class GetServerTest
    {
        [SetUp]
        public static void InsertData()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");

                Classes.Server.AddServerInfo(serversCollection,
                    new Classes.Server
                    {
                        Name = "] My P3rfect Server [",
                        GameModes = new[] { "DM", "TDM" },
                        Endpoint = "167.42.23.32-1337"
                    },
                    "167.42.23.32-1337");

                serversCollection.EnsureIndex(x => x.Endpoint);
            }
        }

        [TearDown]
        public static void DropDatabase()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                testDatabase.DropCollection("servers");
            }
        }

        [Test]
        public static void TestServerGetting()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");
                var resultServer = Classes.Server.GetServerInfo(serversCollection, "167.42.23.32-1337");
                Assert.AreEqual("] My P3rfect Server [", resultServer.Name);
                Assert.AreEqual(new[] { "DM", "TDM" }, resultServer.GameModes);
            }
        }

        [Test]
        public static void TestServerGettingException()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");
                var exception = Assert.Throws<WebFaultException>(() => Classes.Server.GetServerInfo(serversCollection, "127.0.0.1-8080"));
                Assert.AreEqual("Not Found", exception.Message);
                Assert.AreEqual(typeof(WebFaultException), exception.GetType());
            }
        }
    }
}
