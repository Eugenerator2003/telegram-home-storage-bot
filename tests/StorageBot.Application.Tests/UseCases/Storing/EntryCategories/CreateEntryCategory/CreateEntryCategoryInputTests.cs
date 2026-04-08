using StorageBot.Application.UseCases.Storing.EntryCategories.CreateEntryCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.CreateEntryCategory
{
    [TestClass]
    public class CreateEntryCategoryInputTests
    {
        [TestMethod]
        [DataRow(3, 9, 7, "category")]
        [DataRow(1, 1100, null, "main-category")]
        [DataRow(9990, 9990, 8, "sub-category")]
        public void InitializeTest(int userId, int storageOwnershipId, int? mainCategoryId, string categoryName)
        {
            var input = new CreateEntryCategoryInput(userId, storageOwnershipId, mainCategoryId, categoryName);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(mainCategoryId, input.MainCategoryId);
            Assert.AreEqual(categoryName, input.CategoryName);
        }
    }
}
