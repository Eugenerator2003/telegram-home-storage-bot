using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.Containers;
using StorageBot.Application.UseCases.Storing.Containers.GetContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.GetContainers
{
    [TestClass]
    public class GetContainersOutputTests
    {
        [TestMethod]
        [DataRow("Success", "ldsl")]
        [DataRow("Fail", ".112")]
        [DataRow("FatalFail", "")]
        public void InitializeEmptyTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var output = new GetContainersOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
        }

        public void InitializeTest()
        {
            var result = Output.OutputResult.Success;
            var message = string.Empty;
            var outerContainerDto = new ContainerDTO(2, "outer");
            var dtos = new ContainerDTO[]
            {
                new ContainerDTO(500, "inner-300"),
                new ContainerDTO(303939, "inner-2"),
            };
            var pagination = new PaginationDTO(2, 10, 44);
            var output = new GetContainersOutput(result, message, outerContainerDto, dtos, pagination);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(pagination, output.Pagination);
            Assert.AreEqual(outerContainerDto, output.OuterContainer);
            foreach(var dto in dtos)
            {
                Assert.Contains(dto, output.InnerContainers);
            }
        }
    }
}
