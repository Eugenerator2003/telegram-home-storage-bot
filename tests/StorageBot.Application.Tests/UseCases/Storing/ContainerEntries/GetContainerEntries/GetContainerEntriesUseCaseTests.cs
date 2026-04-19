using Moq;
using StorageBot.Application.Services;
using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing.ContainerEntries;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.ContainerEntries;
using StorageBot.Application.UseCases.Storing.ContainerEntries.GetContainerEntries;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries.GetContainerEntries
{
    [TestClass]
    public class GetContainerEntriesUseCaseTests
    {
        private class GetContainerEntriesUseCaseFactory
            : ContainerEntryUseCaseFactory<GetContainerEntriesUseCase, GetContainerEntriesInput, GetContainerEntriesOutput>
        {
            public static int PageSize { get; set; } = 3;

            public override GetContainerEntriesUseCase CreateUseCase()
            {
                var pageSizeServiceMock = new Mock<IPageSizeService>();
                pageSizeServiceMock.Setup(p => p.GetPageSize()).Returns(PageSize);
                return new GetContainerEntriesUseCase(Logger, Localizer, UserRepository, OwnershipRepository, pageSizeServiceMock.Object);
            }
        }

        private static ContainerEntryDTO[] FillContainerAndGetPage(Container container, int count, int returnPage)
        {
            var quantityTypes = new EntryQuantityType[]
            {
                new EntryQuantityType(false, "u") { Id = 1 },
                new EntryQuantityType(true, "n") { Id = 2},
                new EntryQuantityType(false, "it") { Id = 3 },
            };
            for (int i = 0; i < count; i++)
            {
                var id = i + 1;
                var quantity = new EntryQuantity(quantityTypes[i % 3]);
                var entry = new ContainerEntry("entry" + id.ToString(), quantity) { Id = id };
                container.AddEntry(entry);
            }
            var expected = container.Entries
                                .Skip((returnPage - 1) * GetContainerEntriesUseCaseFactory.PageSize)
                                .Take(GetContainerEntriesUseCaseFactory.PageSize)
                                .Select(e =>
                                {
                                    var type = e.EntryQuantity.QuantityType;
                                    var quantityDto = new EntryQuantityDTO(type.Id, type.UnitName, type.IsInteger, e.EntryQuantity.Value);
                                    return new ContainerEntryDTO(e.Id, e.Name, quantityDto);
                                });
            return expected.ToArray();
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var page = 1;
            var input = new GetContainerEntriesInput(1, targetOwnershipId, containerId, page);
            var factory = new GetContainerEntriesUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var page = 1;
            var input = new GetContainerEntriesInput(1, targetOwnershipId, containerId, page);
            var factory = new GetContainerEntriesUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var page = 1;
            var input = new GetContainerEntriesInput(1, targetOwnershipId, containerId, page);
            var factory = new GetContainerEntriesUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var page = 1;
            var input = new GetContainerEntriesInput(1, targetOwnershipId, containerId, page);
            var factory = new GetContainerEntriesUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ContainerNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var page = 1;
            var input = new GetContainerEntriesInput(1, targetOwnershipId, containerId, page);
            var factory = new GetContainerEntriesUseCaseFactory();
            UseCaseTester.ContainerNotFoundTest(factory, input);
        }

        [TestMethod]
        public void ExecuteEmptyContainerTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var page = 1;
            var pagination = new PaginationDTO(page, GetContainerEntriesUseCaseFactory.PageSize, 0);
            var input = new GetContainerEntriesInput(1, targetOwnershipId, containerId, page);
            var factory = new GetContainerEntriesUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(0, output.ContainerEntries);
            Assert.AreEqual(pagination, output.Pagination);
        }

        [TestMethod]
        public void ExecuteFirstPageTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var page = 1;
            var count = 200;
            var pagination = new PaginationDTO(page, GetContainerEntriesUseCaseFactory.PageSize, count);
            var input = new GetContainerEntriesInput(1, targetOwnershipId, containerId, page);
            var factory = new GetContainerEntriesUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var dtos = FillContainerAndGetPage(context.TargetContainer, count, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(pagination, output.Pagination);
            Assert.HasCount(dtos.Length, output.ContainerEntries);
            foreach(var dto in dtos)
            {
                Assert.Contains(dto, output.ContainerEntries);
            }
        }

        [TestMethod]
        public void ExecuteZeroPageTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var page = 0;
            var count = 200;
            var pagination = new PaginationDTO(1, GetContainerEntriesUseCaseFactory.PageSize, count);
            var input = new GetContainerEntriesInput(1, targetOwnershipId, containerId, page);
            var factory = new GetContainerEntriesUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var dtos = FillContainerAndGetPage(context.TargetContainer, count, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(pagination, output.Pagination);
            Assert.HasCount(dtos.Length, output.ContainerEntries);
            foreach (var dto in dtos)
            {
                Assert.Contains(dto, output.ContainerEntries);
            }
        }

        [TestMethod]
        public void ExecuteIntermediatePageTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var page = 30;
            var count = 200;
            var pagination = new PaginationDTO(page, GetContainerEntriesUseCaseFactory.PageSize, count);
            var input = new GetContainerEntriesInput(1, targetOwnershipId, containerId, page);
            var factory = new GetContainerEntriesUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var dtos = FillContainerAndGetPage(context.TargetContainer, count, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(pagination, output.Pagination);
            Assert.HasCount(dtos.Length, output.ContainerEntries);
            foreach (var dto in dtos)
            {
                Assert.Contains(dto, output.ContainerEntries);
            }
        }

        [TestMethod]
        public void ExecuteLastPageTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var page = 67;
            var count = 200;
            var pagination = new PaginationDTO(page, GetContainerEntriesUseCaseFactory.PageSize, count);
            var input = new GetContainerEntriesInput(1, targetOwnershipId, containerId, page);
            var factory = new GetContainerEntriesUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var dtos = FillContainerAndGetPage(context.TargetContainer, count, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(pagination, output.Pagination);
            Assert.HasCount(dtos.Length, output.ContainerEntries);
            foreach (var dto in dtos)
            {
                Assert.Contains(dto, output.ContainerEntries);
            }
        }
    }
}
