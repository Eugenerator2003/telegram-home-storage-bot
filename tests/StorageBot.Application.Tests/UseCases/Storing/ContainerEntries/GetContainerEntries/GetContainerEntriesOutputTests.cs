using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.ContainerEntries;
using StorageBot.Application.UseCases.Storing.ContainerEntries.GetContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries.GetContainerEntries
{
    [TestClass]
    public class GetContainerEntriesOutputTests
    {
        [TestMethod]
        [DataRow("Success", "ldsl")]
        [DataRow("Fail", ".112")]
        [DataRow("FatalFail", "")]
        public void InitializeEmpyTest(string resultString, string message)
        {
            Output.OutputResult result = Enum.Parse<Output.OutputResult>(resultString);
            var pagination = new PaginationDTO(0, 0, 0);
            var output = new GetContainerEntriesOutput(result, message);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.HasCount(0, output.ContainerEntries);
            Assert.AreEqual(pagination, output.Pagination);
        }

        [TestMethod]
        public void InitializeTest()
        {
            Output.OutputResult result = Output.OutputResult.Success;
            var message = string.Empty;
            var entries = new ContainerEntryDTO[]
            {
                new ContainerEntryDTO(1, "entrya", new EntryQuantityDTO(1, "un.", false, 30.3m)),
                new ContainerEntryDTO(2, "entryb", new EntryQuantityDTO(1, "un.", false, 399.111m)),
                new ContainerEntryDTO(3, "entryc", new EntryQuantityDTO(2, "it.", true, 1)),
                new ContainerEntryDTO(4, "entryd", new EntryQuantityDTO(2, "it.", true, 2)),
            };
            var pagination = new PaginationDTO(20, 3, 199);
            var output = new GetContainerEntriesOutput(result, message, entries, pagination);
            Assert.AreEqual(result, output.Result);
            Assert.AreEqual(message, output.Message);
            Assert.AreEqual(pagination, output.Pagination);
            Assert.HasCount(entries.Length, output.ContainerEntries);
            Assert.AreNotEqual(entries, output.ContainerEntries);
            foreach(var dto in entries)
            {
                Assert.Contains(dto, output.ContainerEntries);
            }
        }
    }
}
