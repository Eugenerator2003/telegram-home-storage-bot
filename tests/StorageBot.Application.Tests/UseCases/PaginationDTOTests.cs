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
    }
}
