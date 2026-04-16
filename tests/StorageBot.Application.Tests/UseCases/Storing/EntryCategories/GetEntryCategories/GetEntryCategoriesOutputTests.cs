using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetEntryCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.GetEntryCategories
{
    [TestClass]
    public class GetEntryCategoriesOutputTests
    {
        [TestMethod]
        [DataRow("Success", "ldsl")]
        [DataRow("Fail", ".112")]
        [DataRow("FatalFail", "")]
        public void InitializeEmptyTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var output = new GetEntryCategoriesOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
        }

        [TestMethod]
        public void InitialiazeSingleTest()
        {
            var result = Output.OutputResult.Success;
            var message = string.Empty;
            EntryCategoryDTO[] dtos = [new EntryCategoryDTO(1, "category")];
            EntryCategoryDTO? mainCategory = null;
            PaginationDTO pagination = new PaginationDTO(2, 10, 300);
            var output = new GetEntryCategoriesOutput(result, message, mainCategory, dtos, pagination);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(mainCategory, output.MainCategory);
            Assert.AreEqual(pagination, output.Pagination);
            foreach(var dto in dtos)
            {
                Assert.Contains(dto, output.SubCategories);
            }
        }
    }
}
