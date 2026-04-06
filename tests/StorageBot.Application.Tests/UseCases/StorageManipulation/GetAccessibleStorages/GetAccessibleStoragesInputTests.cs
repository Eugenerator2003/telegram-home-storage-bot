using StorageBot.Application.UseCases.StorageManipulation.GetAccessibleStorages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.GetAccessibleStorages
{
    [TestClass]
    public class GetAccessibleStoragesInputTests
    {
        [TestMethod]
        public void InitializeMethod()
        {
            var userId = 13201213;
            var input = new GetAccessibleStoragesInput(userId);
            Assert.AreEqual(userId, input.UserId);
        }
    }
}
