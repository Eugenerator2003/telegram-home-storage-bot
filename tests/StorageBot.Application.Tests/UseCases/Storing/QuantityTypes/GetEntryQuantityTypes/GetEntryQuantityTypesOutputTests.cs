using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.QuantityTypes;
using StorageBot.Application.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes
{
    [TestClass]
    public class GetEntryQuantityTypesOutputTests
    {
        [TestMethod]
        [DataRow("Success", "ldsl")]
        [DataRow("Fail", ".112")]
        [DataRow("FatalFail", "")]
        public void InitializeEmptyTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var pagination = new PaginationDTO(0, 0, 0);
            var output = new GetEntryQuantityTypesOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(pagination, output.Pagination);
            Assert.HasCount(0, output.EntryQuantityTypes);
        }

        [TestMethod]
        public void InitializeTest()
        {
            Output.OutputResult result = Output.OutputResult.Success;
            var message = "message";
            var pagination = new PaginationDTO(2, 10, 45);
            var list = new List<EntryQuantityTypeDTO>()
            {
                new EntryQuantityTypeDTO(1, true, "e."),
                new EntryQuantityTypeDTO(2, false, "uuuu"),
                new EntryQuantityTypeDTO(3, true, "l;"),
            };
            var output = new GetEntryQuantityTypesOutput(list, pagination, result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(pagination, output.Pagination);
            Assert.HasCount(list.Count, output.EntryQuantityTypes);
            for(int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i], output.EntryQuantityTypes.ElementAt(i));
            }
        }
    }
}
