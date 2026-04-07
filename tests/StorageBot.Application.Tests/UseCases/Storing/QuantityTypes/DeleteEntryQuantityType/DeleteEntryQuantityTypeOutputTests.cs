using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.QuantityTypes.DeleteEntryQuantityType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.DeleteEntryQuantityType
{
    [TestClass]
    public class DeleteEntryQuantityTypeOutputTests
    {
        [TestMethod]
        [DataRow("Success", "ldsl")]
        [DataRow("Fail", ".112")]
        [DataRow("FatalFail", "")]
        public void InitializeTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var output = new DeleteEntryQuantityTypeOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
        }
    }
}
