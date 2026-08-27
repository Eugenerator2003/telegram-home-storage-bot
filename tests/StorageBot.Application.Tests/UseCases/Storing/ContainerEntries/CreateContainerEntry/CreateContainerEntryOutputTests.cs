using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.ContainerEntries.CreateContainerEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries.CreateContainerEntry
{
    [TestClass]
    public class CreateContainerEntryOutputTests
    {
        [TestMethod]
        [DataRow("Success", "ldsl")]
        [DataRow("Fail", ".112")]
        public void InitializeTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var output = new CreateContainerEntryOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(CreateContainerEntryOutput.InputFailState.None, output.State);
        }

        [TestMethod]
        [DataRow("NotIntegerValue", "err1")]
        [DataRow("NotPositiveValue", "err2")]
        public void InitializeWithFailStateTest(string stateString, string message)
        {
            CreateContainerEntryOutput.InputFailState state = Enum.Parse<CreateContainerEntryOutput.InputFailState>(stateString);
            var output = new CreateContainerEntryOutput(Output.OutputResult.Fail, message, state);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(state, output.State);
        }
    }
}
