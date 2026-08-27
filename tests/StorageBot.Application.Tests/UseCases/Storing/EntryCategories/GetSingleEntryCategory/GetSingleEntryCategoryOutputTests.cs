using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetSingleEntryCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.GetSingleEntryCategory
{
    [TestClass]
    public class GetSingleEntryCategoryOutputTests
    {
        [TestInitialize]
        public void InitializeWithDtoTest()
        {
            var result = Output.OutputResult.Success;
            var message = string.Empty;
            var dto = new EntryCategoryDTO(99999, "over");
            var output = new GetSingleEntryCategoryOutput(result, message, dto);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(dto, output.EntryCategory);
        }

        [TestMethod]
        [DataRow("Fail", ".112")]
        [DataRow("FatalFail", "")]
        public void InitializeTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var dto = new EntryCategoryDTO(-1, string.Empty);
            var output = new GetSingleEntryCategoryOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(dto, output.EntryCategory);
        }
    }
}
