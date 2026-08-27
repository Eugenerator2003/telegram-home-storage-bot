using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.Containers;
using StorageBot.Application.UseCases.Storing.Containers.GetSingleContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.GetSingleContainer
{
    [TestClass]
    public class GetSingleContainerOutputTests
    {
        [TestMethod]
        [DataRow("message", 10, "name")]
        [DataRow("scs", 768, "single container")]
        public void InitializeTest(string message, int containerId, string name)
        {
            var result = Output.OutputResult.Success;
            var dto = new ContainerDTO(containerId, name);
            var output = new GetSingleContainerOutput(result, message, dto);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(dto, output.Container);
        }

        [TestMethod]
        [DataRow("Fail", ".112")]
        [DataRow("FatalFail", "")]
        public void InitializeWithoutDtoTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var dto = new ContainerDTO(-1, string.Empty);
            var output = new GetSingleContainerOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(dto, output.Container);
        }
    }
}
