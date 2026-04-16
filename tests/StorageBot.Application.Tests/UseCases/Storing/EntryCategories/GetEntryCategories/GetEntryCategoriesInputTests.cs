using StorageBot.Application.UseCases.Storing.EntryCategories.GetEntryCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.GetEntryCategories
{
    [TestClass]
    public class GetEntryCategoriesInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3, 4)]
        [DataRow(5, 200, 999, null)]
        [DataRow(8, 127, 1, 2)]
        [DataRow(8, 190, 1, null)]
        public void InitializeTest(int userId, int storageOwnershipId, int page, int? mainCategoryId)
        {
            var input = new GetEntryCategoriesInput(userId, storageOwnershipId, page, mainCategoryId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(page, input.Page);
            Assert.AreEqual(mainCategoryId, input.MainCategoryId);
        }
    }
}
