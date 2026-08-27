using StorageBot.Application.UseCases.Storing.Containers.GetOuterContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.GetOuterContainer
{
    [TestClass]
    public class GetOuterContainerInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3)]
        [DataRow(45, 80, 11)]
        public void InitializeMethod(int userId, int storageOwnershipId, int innerContainerId)
        {
            var input = new GetOuterContainerInput(userId, storageOwnershipId, innerContainerId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(innerContainerId, input.InnerContainerId);
        }
    }
}
