using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Kontur.GameStats.Server.ServiceTests
{
    [TestFixture]
    class CheckCountTest
    {
        [Test]
        public static void CheckCountGreaterFifty()
        {
            Assert.AreEqual(50, new Service.Service().CheckCount("51"));
        }
        [Test]
        public static void CheckCountEqualZero()
        {
            var exception = Assert.Throws<Exception>(() => new Service.Service().CheckCount("0"));
            Assert.AreEqual("Count is less or equal zero", exception.Message);
        }
        [Test]
        public static void CheckCountLessZero()
        {
            var exception = Assert.Throws<Exception>(() => new Service.Service().CheckCount("-1"));
            Assert.AreEqual("Count is less or equal zero", exception.Message);
        }
        [Test]
        public static void CheckCountBetweenZeroAndFifty()
        {
            Assert.AreEqual(8, new Service.Service().CheckCount("8"));
        }
    }
}
