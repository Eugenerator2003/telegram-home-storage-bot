using StorageBot.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases
{
    [TestClass]
    public class PaginationDTOTests
    {
        [TestMethod]
        public void InitalizeTest()
        {
            int page = 2;
            int pageSize = 15;
            int itemsCount = 10;
            var dto = new PaginationDTO(page, pageSize, itemsCount);
            Assert.AreEqual(page, dto.Page);
            Assert.AreEqual(pageSize, dto.PageSize);
            Assert.AreEqual(itemsCount, dto.Count);
        }

        [TestMethod]
        public void EqualsTrueTest()
        {
            var pagination1 = new PaginationDTO(1, 2, 3);
            var pagination2 = new PaginationDTO(1, 2, 3);
            Assert.IsTrue(pagination1.Equals(pagination2));
        }

        [TestMethod]
        public void EqualsFalseTest()
        {
            var pagination1 = new PaginationDTO(1, 2, 3);
            var pagination2 = new PaginationDTO(1, 2, 4);
            Assert.IsFalse(pagination1.Equals(pagination2));
        }

        [TestMethod]
        public void GetHashCodeSameTest()
        {
            var pagination1 = new PaginationDTO(1, 2, 3);
            var pagination2 = new PaginationDTO(1, 2, 3);
            Assert.AreEqual(pagination2.GetHashCode(), pagination1.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeDifferentTest()
        {
            var pagination1 = new PaginationDTO(1, 2, 3);
            var pagination2 = new PaginationDTO(1, 2, 4);
            Assert.AreNotEqual(pagination2.GetHashCode(), pagination1.GetHashCode());
        }
    }
}
