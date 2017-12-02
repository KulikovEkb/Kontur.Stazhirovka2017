using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Kontur.GameStats.Server.ServiceTests
{
    [TestFixture]
    public class CheckCountTests
    {
        [Test]
        public void CheckCountGreaterFiftyTest()
        {
            Assert.AreEqual(50, new Service.Service().CheckCount("51"));
        }
        [Test]
        public void CheckCountEqualZeroTest()
        {
            var exception = Assert.Throws<Exception>(() => new Service.Service().CheckCount("0"));
            Assert.AreEqual("Count is less or equal zero", exception.Message);
        }
        [Test]
        public void CheckCountLessZeroTest()
        {
            var exception = Assert.Throws<Exception>(() => new Service.Service().CheckCount("-1"));
            Assert.AreEqual("Count is less or equal zero", exception.Message);
        }
        [Test]
        public void CheckCountBetweenZeroAndFiftyTest()
        {
            Assert.AreEqual(8, new Service.Service().CheckCount("8"));
        }
    }
}
