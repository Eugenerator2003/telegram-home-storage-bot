using StorageBot.Application.UseCases.StorageManipulation.AllowStorageAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.AllowStorageAccess
{
    [TestClass]
    public class AllowStorageAccessInputTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            int userId = 10;
            int allowUserId = 11;
            var input = new AllowStorageAccessInput(userId, allowUserId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(allowUserId, input.AllowUserId);
        }
    }
}
