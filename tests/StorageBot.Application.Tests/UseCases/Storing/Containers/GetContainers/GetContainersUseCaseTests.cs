using Moq;
using StorageBot.Application.Services;
using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.Containers;
using StorageBot.Application.UseCases.Storing.Containers.GetContainers;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.GetContainers
{
    [TestClass]
    public class GetContainersUseCaseTests
    {
        private class GetContainersUseCaseFactory
            : StorageUseCaseFactory<GetContainersUseCase, GetContainersInput, GetContainersOutput>
        {
            public static int PageSize { get; set; } = 3;

            public override GetContainersUseCase CreateUseCase()
            {
                var pageSizeServiceMock = new Mock<IPageSizeService>();
                pageSizeServiceMock.Setup(p => p.GetPageSize()).Returns(PageSize);
                return new GetContainersUseCase(Logger, Localizer, UserRepository, OwnershipRepository, pageSizeServiceMock.Object);
            }
        }

        private static ContainerDTO[] FillStorageAndGetPage(Storage storage, int count, int returnPage)
        {
            return FillAndGetPage(c => storage.AddContainer(c),
                                  () => storage.Containers,
                                  count, returnPage);
        }

        private static ContainerDTO[] FillContainerAndGetPage(Container outerContainer, int count, int returnPage)
        {
            return FillAndGetPage(c => outerContainer.AddInnerContainer(c),
                                  () => outerContainer.InnerContainers,
                                  count, returnPage);
        }

        private static ContainerDTO[] FillAndGetPage(
            Action<Container> addMethod,
            Func<IEnumerable<Container>> getAllMethod,
            int count, int returnPage)
        {
            for (int i = 0; i < count; i++)
            {
                var id = i + 1;
                var container = new Container($"container {id}") { Id = id };
                addMethod.Invoke(container);
            }
            var expected = getAllMethod.Invoke()
                                  .Skip((returnPage - 1) * GetContainersUseCaseFactory.PageSize)
                                  .Take(GetContainersUseCaseFactory.PageSize)
                                  .Select(t => new ContainerDTO(t.Id, t.Name));
            return expected.ToArray();
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var targetOwnershipId = 999;
            var page = 1;
            int? outerContainerId = 1;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var targetOwnershipId = 999;
            var page = 1;
            int? outerContainerId = 1;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var targetOwnershipId = 999;
            var page = 1;
            int? outerContainerId = 1;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStroageAccessTest()
        {
            var targetOwnershipId = 999;
            var page = 1;
            int? outerContainerId = 1;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteOuterStorageNotFoundTest()
        {
            var targetOwnershipId = 999;
            var page = 1;
            int? outerContainerId = 1;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.OuterContainerNotFound"], output.Message);
            Assert.IsNull(output.OuterContainer);
            Assert.HasCount(0, output.InnerContainers);
        }

        [TestMethod]
        public void ExecuteEmptyStorageTest()
        {
            var targetOwnershipId = 999;
            var page = 1;
            int? outerContainerId = null;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var pagination = new PaginationDTO(page, GetContainersUseCaseFactory.PageSize, 0);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.IsNull(output.OuterContainer);
            Assert.HasCount(0, output.InnerContainers);
            Assert.AreEqual(pagination, output.Pagination);
        }

        [TestMethod]
        public void ExecuteStorageFirstPageTest()
        {
            var targetOwnershipId = 999;
            var page = 1;
            var itemsCount = 1000;
            int? outerContainerId = null;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var pagination = new PaginationDTO(page, GetContainersUseCaseFactory.PageSize, itemsCount);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var dtos = FillStorageAndGetPage(context.TargetOwnership.PersonalStorage, itemsCount, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.IsNull(output.OuterContainer);
            Assert.HasCount(dtos.Length, output.InnerContainers);
            Assert.AreEqual(pagination, output.Pagination);
            foreach(var dto in dtos)
            {
                Assert.Contains(dto, output.InnerContainers);
            }
        }

        [TestMethod]
        public void ExecuteStorageZeroPageTest()
        {
            var targetOwnershipId = 999;
            var page = 0;
            var itemsCount = 1000;
            int? outerContainerId = null;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var pagination = new PaginationDTO(1, GetContainersUseCaseFactory.PageSize, itemsCount);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var dtos = FillStorageAndGetPage(context.TargetOwnership.PersonalStorage, itemsCount, 1);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.IsNull(output.OuterContainer);
            Assert.HasCount(dtos.Length, output.InnerContainers);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var dto in dtos)
            {
                Assert.Contains(dto, output.InnerContainers);
            }
        }

        [TestMethod]
        public void ExecuteStorageIntermediatePageTest()
        {
            var targetOwnershipId = 999;
            var page = 39;
            var itemsCount = 1000;
            int? outerContainerId = null;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var pagination = new PaginationDTO(page, GetContainersUseCaseFactory.PageSize, itemsCount);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var dtos = FillStorageAndGetPage(context.TargetOwnership.PersonalStorage, itemsCount, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.IsNull(output.OuterContainer);
            Assert.HasCount(dtos.Length, output.InnerContainers);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var dto in dtos)
            {
                Assert.Contains(dto, output.InnerContainers);
            }
        }

        [TestMethod]
        public void ExecuteStorageLastPageTest()
        {
            var targetOwnershipId = 999;
            var page = 34;
            var itemsCount = 100;
            int? outerContainerId = null;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var pagination = new PaginationDTO(page, GetContainersUseCaseFactory.PageSize, itemsCount);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var dtos = FillStorageAndGetPage(context.TargetOwnership.PersonalStorage, itemsCount, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.IsNull(output.OuterContainer);
            Assert.HasCount(dtos.Length, output.InnerContainers);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var dto in dtos)
            {
                Assert.Contains(dto, output.InnerContainers);
            }
        }

        [TestMethod]
        public void ExecuteEmptyOuterContainerTest()
        {
            var targetOwnershipId = 999;
            var page = 1;
            int? outerContainerId = 3123131;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var pagination = new PaginationDTO(page, GetContainersUseCaseFactory.PageSize, 0);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var container = new Container("outer") { Id = outerContainerId.Value };
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var outerContainerDto = new ContainerDTO(container.Id, container.Name);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(outerContainerDto, output.OuterContainer);
            Assert.HasCount(0, output.InnerContainers);
            Assert.AreEqual(pagination, output.Pagination);
        }

        [TestMethod]
        public void ExecuteOuterContainerFirstTest()
        {
            var targetOwnershipId = 999;
            var page = 1;
            var itemsCount = 1000;
            int? outerContainerId = 3123131;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var pagination = new PaginationDTO(page, GetContainersUseCaseFactory.PageSize, itemsCount);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var container = new Container("outer") { Id = outerContainerId.Value };
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var outerContainerDto = new ContainerDTO(container.Id, container.Name);
            var dtos = FillContainerAndGetPage(container, itemsCount, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(outerContainerDto, output.OuterContainer);
            Assert.HasCount(dtos.Length, output.InnerContainers);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var dto in dtos)
            {
                Assert.Contains(dto, output.InnerContainers);
            }
        }

        [TestMethod]
        public void ExecuteOuterContainerZeroPageTest()
        {
            var targetOwnershipId = 999;
            var page = 0;
            var itemsCount = 1000;
            int? outerContainerId = 3123131;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var pagination = new PaginationDTO(1, GetContainersUseCaseFactory.PageSize, itemsCount);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var container = new Container("outer") { Id = outerContainerId.Value };
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var outerContainerDto = new ContainerDTO(container.Id, container.Name);
            var dtos = FillContainerAndGetPage(container, itemsCount, 1);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(outerContainerDto, output.OuterContainer);
            Assert.HasCount(dtos.Length, output.InnerContainers);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var dto in dtos)
            {
                Assert.Contains(dto, output.InnerContainers);
            }
        }

        [TestMethod]
        public void ExecuteOuterContainerIntermediatePageTest()
        {
            var targetOwnershipId = 999;
            var page = 39;
            var itemsCount = 1000;
            int? outerContainerId = 3123131;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var pagination = new PaginationDTO(page, GetContainersUseCaseFactory.PageSize, itemsCount);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var container = new Container("outer") { Id = outerContainerId.Value };
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var outerContainerDto = new ContainerDTO(container.Id, container.Name);
            var dtos = FillContainerAndGetPage(container, itemsCount, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(outerContainerDto, output.OuterContainer);
            Assert.HasCount(dtos.Length, output.InnerContainers);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var dto in dtos)
            {
                Assert.Contains(dto, output.InnerContainers);
            }
        }

        [TestMethod]
        public void ExecuteOuterContainerLastPageTest()
        {
            var targetOwnershipId = 999;
            var page = 34;
            var itemsCount = 100;
            int? outerContainerId = 3123131;
            var input = new GetContainersInput(1, targetOwnershipId, page, outerContainerId);
            var factory = new GetContainersUseCaseFactory();
            var pagination = new PaginationDTO(page, GetContainersUseCaseFactory.PageSize, itemsCount);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var container = new Container("outer") { Id = outerContainerId.Value };
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var outerContainerDto = new ContainerDTO(container.Id, container.Name);
            var dtos = FillContainerAndGetPage(container, itemsCount, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(outerContainerDto, output.OuterContainer);
            Assert.HasCount(dtos.Length, output.InnerContainers);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var dto in dtos)
            {
                Assert.Contains(dto, output.InnerContainers);
            }
        }
    }
}
