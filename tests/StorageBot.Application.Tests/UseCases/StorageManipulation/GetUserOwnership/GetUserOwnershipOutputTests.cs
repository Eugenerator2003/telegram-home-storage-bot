using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.StorageManipulation.GetUserOwnership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.GetUserOwnership
{
    [TestClass]
    public class GetUserOwnershipOutputTests
    {
        [TestMethod]
        [DataRow(1, "Success", "msg")]
        [DataRow(-1, "FatalFail", "not null")]
        [DataRow(-1, "Fail", "         ")]
        public void InitializeTest(int ownershipId, string resultStr, string message)
        {
            var result = Enum.Parse<Output.OutputResult>(resultStr);
            var output = new GetUserOwnershipOutput(ownershipId, result, message);
            Assert.AreEqual(ownershipId, output.OwnershipId);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
        }
    }
}
