using Microsoft.VisualStudio.TestPlatform.Utilities;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories.UpdateEntryCategoryNameUseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.UpdateEntryCategoryName
{
    [TestClass]
    public class UpdateEntryCategoryNameOutputTests
    {
        [TestMethod]
        [DataRow("Success", "ldsl")]
        [DataRow("Fail", ".112")]
        [DataRow("FatalFail", "")]
        public void InitializeTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var output = new UpdateEntryCategoryNameOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
        }
    }
}
