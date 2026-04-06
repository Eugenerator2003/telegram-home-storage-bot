using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.StorageManipulation.GetAccessibleStorages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.GetAccessibleStorages
{
    [TestClass]
    public class GetAccessibleStoragesOutputTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            List<int> ids = [1, 2, 123, 22];
            Output.OutputResult result = Output.OutputResult.Success;
            var output = new GetAccessibleStoragesOutput(result, ids, string.Empty);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreNotEqual(ids, output.UserIds);
            foreach (var id in ids)
            {
                Assert.Contains(id, output.UserIds);
            }
        }

        [TestMethod]
        [DataRow("Success", "1231")]
        [DataRow("Fail", ".")]
        [DataRow("FatalFail", "abcd")]
        public void InitializeEmptyTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var output = new GetAccessibleStoragesOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.HasCount(0, output.UserIds);
        }
    }
}
