using StorageBot.Application.UseCases.Storing.EntryCategories.DeleteEntryCategoryUseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.DeleteEntryCategory
{
    [TestClass]
    public class DeleteEntryCategoryInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3)]
        [DataRow(23, 1111, 2)]
        public void InitializeTest(int userId, int storageOwnershipId, int entryCategoryId)
        {
            var input = new DeleteEntryCategoryInput(userId, storageOwnershipId, entryCategoryId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(entryCategoryId, input.EntryCategoryId);
        }
    }
}
