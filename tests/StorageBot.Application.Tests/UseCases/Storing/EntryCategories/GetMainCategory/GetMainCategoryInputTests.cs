using StorageBot.Application.UseCases.Storing.EntryCategories.GetMainCategoryUseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.GetMainCategory
{
    [TestClass]
    public class GetMainCategoryInputTests
    {
        [TestMethod]
        [DataRow(949, 39129, 301230)]
        [DataRow(3192, 102664, 30585)]
        public void InitializeTest(int userId, int storageOwnershipId, int subcategoryId)
        {
            var input = new GetMainCategoryInput(userId, storageOwnershipId, subcategoryId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(subcategoryId, input.SubcategoryId);
        }
    }
}
