using Moq;
using StorageBot.Application.Services;
using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetEntryCategories;
using StorageBot.Application.UseCases.Storing.QuantityTypes;
using StorageBot.Application.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes
{
    [TestClass]
    public class GetEntryQuantityTypesUseCaseTests
    {
        private class GetEntryQuantityTypesUseCaseFactory
            : StorageUseCaseFactory<GetEntryQuantityTypesUseCase, GetEntryQuantityTypesInput, GetEntryQuantityTypesOutput>
        {
            public static int PageSize = 3;

            public override GetEntryQuantityTypesUseCase CreateUseCase()
            {
                var pageServiceMock = new Mock<IPageSizeService>();
                pageServiceMock.Setup(m => m.GetPageSize()).Returns(PageSize);
                return new GetEntryQuantityTypesUseCase(Logger, Localizer, UserRepository, OwnershipRepository, pageServiceMock.Object);
            }
        }

        private static EntryQuantityTypeDTO[] FillStorageAndGetPage(Storage storage, int count, int returnPage)
        {
            for (int i = 0; i < count; i++)
            {
                var id = i + 1;
                var type = new EntryQuantityType(i % 1 == 1, id.ToString()) { Id = id };
                storage.AddEntryQuantityType(type);
            }
            var expected = storage.EntryQuantityTypes
                                  .Skip((returnPage - 1) * GetEntryQuantityTypesUseCaseFactory.PageSize)
                                  .Take(GetEntryQuantityTypesUseCaseFactory.PageSize)
                                  .Select(t => new EntryQuantityTypeDTO(t.Id, t.IsInteger, t.UnitName));
            return expected.ToArray();
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new GetEntryQuantityTypesInput(1, 1, 1);
            var factory = new GetEntryQuantityTypesUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new GetEntryQuantityTypesInput(1, 1, 1);
            var factory = new GetEntryQuantityTypesUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new GetEntryQuantityTypesInput(1, 1, 1);
            var factory = new GetEntryQuantityTypesUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new GetEntryQuantityTypesInput(1, 1, 1);
            var factory = new GetEntryQuantityTypesUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteFirstPageTest()
        {
            var targetOwnershipId = 2;
            var targetPage = 1;
            var itemsCount = 21;
            var storagePage = 1;
            ExecuteWithAssert(targetOwnershipId, targetPage, itemsCount, storagePage);
        }

        [TestMethod]
        public void ExecuteZeroPageTest()
        {
            var targetOwnershipId = 2;
            var targetPage = 0;
            var itemsCount = 21;
            var storagePage = 1;
            ExecuteWithAssert(targetOwnershipId, targetPage, itemsCount, storagePage);
        }

        [TestMethod]
        public void ExecuteIntermediatePageTest()
        {
            var targetOwnershipId = 2;
            int targetPage = 8;
            var itemsCount = 310;
            var storagePage = 8;
            ExecuteWithAssert(targetOwnershipId, targetPage, itemsCount, storagePage);
        }

        [TestMethod]
        public void ExecuteLastPageTest()
        {
            var targetOwnershipId = 2;
            int targetPage = 9;
            var itemsCount = 26;
            var storagePage = 9;
            ExecuteWithAssert(targetOwnershipId, targetPage, itemsCount, storagePage);
        }

        private void ExecuteWithAssert(int targetOwnershipId, int targetPage, int itemsCount, int storagePage)
        {
            var input = new GetEntryQuantityTypesInput(1, targetOwnershipId, targetPage);
            var factory = new GetEntryQuantityTypesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var storage = context.TargetOwnership.PersonalStorage;
            var expectedDtos = FillStorageAndGetPage(storage, itemsCount, storagePage);

            var pagination = new PaginationDTO(storagePage, GetEntryQuantityTypesUseCaseFactory.PageSize, storage.EntryQuantityTypes.Count);
            var output = factory.CreateUseCase().Execute(input);
            
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(pagination, output.Pagination);
            Assert.HasCount(expectedDtos.Length, output.EntryQuantityTypes);
            for (int i = 0; i < expectedDtos.Length; i++)
            {
                Assert.AreEqual(expectedDtos[i], output.EntryQuantityTypes.ElementAt(i));
            }
        }
    }
}
