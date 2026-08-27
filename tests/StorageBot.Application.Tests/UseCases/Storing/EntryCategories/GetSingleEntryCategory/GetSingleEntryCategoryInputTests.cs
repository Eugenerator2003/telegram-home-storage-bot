using StorageBot.Application.UseCases.Storing.EntryCategories.GetSingleEntryCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.GetSingleEntryCategory
{
    [TestClass]
    public class GetSingleEntryCategoryInputTests
    {
        [TestMethod]
        [DataRow(4505, 88686, 301203)]
        [DataRow(9697997, 6596912, 30123)]
        [DataRow(60706, 6969, 439182)]
        public void InitializeTest(int userId, int storageOwnershipId, int entryCategoryId)
        {
            var input = new GetSingleEntryCategoryInput(userId, storageOwnershipId, entryCategoryId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(entryCategoryId, input.EntryCategoryId);
        }
    }
}
