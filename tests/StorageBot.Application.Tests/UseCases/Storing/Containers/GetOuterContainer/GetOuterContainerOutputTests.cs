using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.Containers;
using StorageBot.Application.UseCases.Storing.Containers.GetOuterContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.GetOuterContainer
{
    [TestClass]
    public class GetOuterContainerOutputTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            var result = Output.OutputResult.Success;
            var message = "Success";
            var dto = new ContainerDTO(2, "container");
            var output = new GetOuterContainerOutput(result, message, dto);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(dto, output.OuterContainer);
        }

        [TestMethod]
        public void InitializeWithoutContainerTest()
        {
            var result = Output.OutputResult.FatalFail;
            var message = "Container not found";
            var expectedDto = new ContainerDTO(-1, string.Empty);
            var output = new GetOuterContainerOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(expectedDto, output.OuterContainer);
        }
    }
}
