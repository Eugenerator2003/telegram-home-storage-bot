using StorageBot.Application.UseCases.StorageManipulation.DisallowStorageAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.DisallowStorageAccess
{
    [TestClass]
    public class DisallowStorageAccessInputTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            var userId = 54;
            var disallowUserId = 13;
            var input = new DisallowStorageAccessInput(userId, disallowUserId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(disallowUserId, input.DisallowUserId);
        }
    }
}
