using Microsoft.VisualStudio.TestPlatform.Utilities;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.StorageManipulation.RelinquishStorageAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.RelinquishStorageAccess
{
    [TestClass]
    public class RelinquishStorageAccessOutputTests
    {
        [TestMethod]
        [DataRow("Success", "1231")]
        [DataRow("Fail", ".")]
        [DataRow("FatalFail", "abcd")]
        public void InitializeTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var output = new RelinquishStorageAccessOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
        }
    }
}
