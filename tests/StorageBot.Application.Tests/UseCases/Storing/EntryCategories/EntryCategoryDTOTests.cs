using StorageBot.Application.UseCases.Storing.EntryCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories
{
    [TestClass]
    public class EntryCategoryDTOTests
    {
        [TestMethod]
        [DataRow(2, "category")]
        [DataRow(11, "cat")]
        public void InitializeTest(int id, string name)
        {
            var dto = new EntryCategoryDTO(id, name);
            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(name, dto.Name);
        }

        [TestMethod]
        public void EqualsTrueTest()
        {
            var id = 1;
            var name = "name";
            var dto1 = new EntryCategoryDTO(id, name);
            var dto2 = new EntryCategoryDTO(id, name);
            Assert.IsTrue(dto1.Equals(dto2));
        }

        [TestMethod]
        public void EqualsFalseTest()
        {
            var id = 1;
            var name = "name";
            var dto1 = new EntryCategoryDTO(id, name);
            var dto2 = new EntryCategoryDTO(id, name + name);
            Assert.IsFalse(dto1.Equals(dto2));
        }

        [TestMethod]
        public void GetHashCodeSameTest()
        {
            var id = 1;
            var name = "name";
            var dto1 = new EntryCategoryDTO(id, name);
            var dto2 = new EntryCategoryDTO(id, name);
            Assert.AreEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeDifferentTest()
        {
            var id = 1;
            var name = "name";
            var dto1 = new EntryCategoryDTO(id, name);
            var dto2 = new EntryCategoryDTO(id, name + name);
            Assert.AreNotEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }
    }
}
