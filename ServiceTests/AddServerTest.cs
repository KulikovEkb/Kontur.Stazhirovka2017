using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LiteDB;

namespace Kontur.GameStats.Server.ServiceTests
{
    [TestFixture]
    class AddServerTest
    {
        [Test]
        public static void TestServerAddition()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");
                serversCollection.EnsureIndex(x => x.Endpoint);

            }
        }
    }
}
