using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.ContainerEntries
{
    [TestClass]
    public class EntryQuantityTypeTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            var unitName = "unit";
            var type = new EntryQuantityType(true, unitName);
            Assert.IsTrue(type.IsInteger);
            Assert.AreEqual(unitName, type.UnitName);
        }

        [TestMethod]
        public void InitializeNullUnitNameTest()
        {
            string unitName = null!;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var type = new EntryQuantityType(false, unitName);
            });
        }

        [TestMethod]
        public void InitializeEmptyUnitNameTest()
        {
            string unitName = null!;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var type = new EntryQuantityType(true, unitName);
            });
        }
    }
}
