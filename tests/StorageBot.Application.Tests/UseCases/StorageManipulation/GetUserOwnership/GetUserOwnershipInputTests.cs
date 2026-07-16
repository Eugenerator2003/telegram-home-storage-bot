using StorageBot.Application.UseCases.StorageManipulation.GetUserOwnership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.GetUserOwnership
{
    [TestClass]
    public class GetUserOwnershipInputTests
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(300)]
        public void InitializeTest(int userId)
        {
            var input = new GetUserOwnershipInput(userId);
            Assert.AreEqual(userId, input.UserId);
        }
    }
}
