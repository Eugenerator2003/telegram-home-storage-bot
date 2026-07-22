using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.QuantityTypes;
using StorageBot.Application.UseCases.Storing.QuantityTypes.GetSingleEntryQuantityType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.GetSingleEntryQuantityType
{
    [TestClass]
    public class GetSingleEntryQuantityTypeOutputTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            var result = Output.OutputResult.Success;
            var message = string.Empty;
            var dto = new EntryQuantityTypeDTO(540, false, "hehe");
            var output = new GetSingleEntryQuantityTypeOutput(dto, result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(dto, output.QuantityTypeDTO);
        }

        [TestMethod]
        public void InitializeWithoutDTOTest()
        {
            var result = Output.OutputResult.FatalFail;
            var message = "extremly failure";
            var dto = new EntryQuantityTypeDTO(-1, false, string.Empty);
            var output = new GetSingleEntryQuantityTypeOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(dto, output.QuantityTypeDTO);
        }
    }
}
