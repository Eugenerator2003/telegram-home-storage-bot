using StorageBot.Application.UseCases.StorageManipulation.RelinquishStorageAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.RelinquishStorageAccess
{
    [TestClass]
    public class RelinquishStorageAccessInputTests
    {
        [TestMethod]
        public void Initialize()
        {
            int userId = 948855;
            int storageOwnerId = 99;
            var input = new RelinquishStorageAccessInput(userId, storageOwnerId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnerId, input.StorageOwnerId);
        }
    }
}
