using StorageBot.Application.UseCases.StorageManipulation.InitializeStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.InitializeStorage
{
    [TestClass]
    public class InitializeStorageInputTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            int userId = 100;
            var input = new InitializeStorageInput(userId);
            Assert.AreEqual(userId, input.UserId);
        }
    }
}
