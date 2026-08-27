using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetMainCategoryUseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.GetMainCategory
{
    [TestClass]
    public class GetMainCategoryOutputTests
    {
        [TestMethod]
        public void InitializeWithDtoTest()
        {
            var result = Output.OutputResult.Success;
            var message = string.Empty;
            var dto = new EntryCategoryDTO(7, "seven");
            var output = new GetMainCategoryOutput(result, message, dto);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(dto, output.MainCategory);
        }

        [TestMethod]
        [DataRow("Fail", ".112")]
        [DataRow("FatalFail", "")]
        public void InitializeTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var dto = new EntryCategoryDTO(-1, string.Empty);
            var output = new GetMainCategoryOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(dto, output.MainCategory);
        }
    }
}
