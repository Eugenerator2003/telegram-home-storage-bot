using StorageBot.Application.UseCases.Storing;
using StorageBot.Application.UseCases.Storing.EntryCategories.UpdateEntryCategoryNameUseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.UpdateEntryCategoryName
{
    [TestClass]
    public class UpdateEntryCategoryNameInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3, "category")]
        [DataRow(25, 99, 400, "")]
        [DataRow(3, 2, 1, "aaaa")]
        public void InitializeTest(int userId, int storageOwnershipId, int entryCategoryId, string categoryName)
        {
            var input = new UpdateEntryCategoryNameInput(userId, storageOwnershipId, entryCategoryId, categoryName);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(entryCategoryId, input.EntryCategoryId);
            Assert.AreEqual(categoryName, input.CategoryName);
        }
    }
}
