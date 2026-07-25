using Moq;
using StorageBot.Application.Services;
using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetEntryCategories;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.GetEntryCategories
{
    [TestClass]
    public class GetEntryCategoriesUseCaseTests
    {
        private class GetEntryCategoriesUseCaseFactory
            : StorageUseCaseFactory<GetEntryCategoriesUseCase, GetEntryCategoriesInput, GetEntryCategoriesOutput>
        {
            public static int PageSize { get; set; } = 3;

            public override GetEntryCategoriesUseCase CreateUseCase()
            {
                var pageSizeProviderMock = new Mock<IPageSizeService>();
                pageSizeProviderMock.Setup(c => c.GetPageSize()).Returns(PageSize);
                return new GetEntryCategoriesUseCase(Logger, Localizer, UserRepository, OwnershipRepository, pageSizeProviderMock.Object);
            }
        }

        private static EntryCategoryDTO[] FillStorageAndGetPage(Storage storage, int count, int returnPage)
        {
            return FillAndGetPage(c => storage.AddEntryCategory(c),
                                  () => storage.EntryCategories, 
                                  count, returnPage );
        }

        private static EntryCategoryDTO[] FillCategoryAndGetPage(EntryCategory mainCategory, int count, int returnPage)
        {
            return FillAndGetPage(c => mainCategory.AddSubcategory(c),
                                  () => mainCategory.Subcategories,
                                  count, returnPage);
        }

        private static EntryCategoryDTO[] FillAndGetPage(
            Action<EntryCategory> addMethod, 
            Func<IEnumerable<EntryCategory>> getAllMethod, 
            int count, int returnPage)
        {
            for (int i = 0; i < count; i++)
            {
                var id = i + 1;
                var category = new EntryCategory($"category {id}") { Id = id };
                addMethod.Invoke(category);
            }
            var expected = getAllMethod.Invoke()
                                  .Skip((returnPage - 1) * GetEntryCategoriesUseCaseFactory.PageSize)
                                  .Take(GetEntryCategoriesUseCaseFactory.PageSize)
                                  .Select(t => new EntryCategoryDTO(t.Id, t.Name));
            return expected.ToArray();
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new GetEntryCategoriesInput(1, 1, 1, 1);
            var factory = new GetEntryCategoriesUseCaseFactory();
            factory.ConfigureWithTargetOwnership(1);
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTests()
        {
            var input = new GetEntryCategoriesInput(1, 1, 1, 1);
            var factory = new GetEntryCategoriesUseCaseFactory();
            factory.ConfigureWithTargetOwnership(1);
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new GetEntryCategoriesInput(1, 1, 1, 1);
            var factory = new GetEntryCategoriesUseCaseFactory();
            factory.ConfigureWithTargetOwnership(1);
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTests()
        {
            var input = new GetEntryCategoriesInput(1, 1, 1, 1);
            var factory = new GetEntryCategoriesUseCaseFactory();
            factory.ConfigureWithTargetOwnership(1);
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteMainCategoryNotFoundTests()
        {
            var targetOwnershipId = 2;
            int page = 2;
            int? mainCategoryId = 6;
            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.MainCategoryNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteEmptyStorageTests()
        {
            var targetOwnershipId = 2;
            int page = 2;
            int? mainCategoryId = null;
            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var pagination = new PaginationDTO(page, GetEntryCategoriesUseCaseFactory.PageSize, 0);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(0, output.SubCategories);
            Assert.AreEqual(pagination, output.Pagination);
        }

        [TestMethod]
        public void ExecuteStorageFirstPageTest()
        {
            var targetOwnershipId = 2;
            int page = 1;
            int? mainCategoryId = null;
            int count = 100;
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);

            var dtos = FillStorageAndGetPage(context.TargetOwnership.PersonalStorage, count, page);
            var pagination = new PaginationDTO(page, GetEntryCategoriesUseCaseFactory.PageSize, count);

            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(dtos.Length, output.SubCategories);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var d in dtos )
            {
                Assert.Contains(d, output.SubCategories);
            }
        }

        [TestMethod]
        public void ExecuteStorageZeroPageTest()
        {
            var targetOwnershipId = 2;
            int page = 0;
            int? mainCategoryId = null;
            int count = 100;
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);

            var dtos = FillStorageAndGetPage(context.TargetOwnership.PersonalStorage, count, page);
            var pagination = new PaginationDTO(1, GetEntryCategoriesUseCaseFactory.PageSize, count);

            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(dtos.Length, output.SubCategories);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var d in dtos)
            {
                Assert.Contains(d, output.SubCategories);
            }
        }

        [TestMethod]
        public void ExecuteStorageIntermediatePageTest()
        {
            var targetOwnershipId = 2;
            int page = 23;
            int? mainCategoryId = null;
            int count = 100;
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);

            var dtos = FillStorageAndGetPage(context.TargetOwnership.PersonalStorage, count, page);
            var pagination = new PaginationDTO(page, GetEntryCategoriesUseCaseFactory.PageSize, count);

            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(dtos.Length, output.SubCategories);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var d in dtos)
            {
                Assert.Contains(d, output.SubCategories);
            }
        }

        [TestMethod]
        public void ExecuteStorageLastPageTest()
        {
            var targetOwnershipId = 2;
            int page = 34;
            int? mainCategoryId = null;
            int count = 100;
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);

            var dtos = FillStorageAndGetPage(context.TargetOwnership.PersonalStorage, count, page);
            var pagination = new PaginationDTO(page, GetEntryCategoriesUseCaseFactory.PageSize, count);

            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(dtos.Length, output.SubCategories);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var d in dtos)
            {
                Assert.Contains(d, output.SubCategories);
            }
        }

        [TestMethod]
        public void ExecuteEmptyMainCategoryTests()
        {
            var targetOwnershipId = 2;
            int page = 2;
            int? mainCategoryId = 2;
            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var mainCategory = new EntryCategory("main-category") { Id = mainCategoryId.Value };
            context.TargetOwnership.PersonalStorage.AddEntryCategory(mainCategory);
            var pagination = new PaginationDTO(page, GetEntryCategoriesUseCaseFactory.PageSize, 0);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(0, output.SubCategories);
            Assert.AreEqual(pagination, output.Pagination);
        }

        [TestMethod]
        public void ExecuteMainCategoryFirstPageTest()
        {
            var targetOwnershipId = 2;
            int page = 0;
            int? mainCategoryId = 100;
            int count = 100;
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);

            var mainCategory = new EntryCategory("main-category") { Id = mainCategoryId.Value };
            context.TargetOwnership.PersonalStorage.AddEntryCategory(mainCategory);
            var dtos = FillCategoryAndGetPage(mainCategory, count, page);
            var pagination = new PaginationDTO(1, GetEntryCategoriesUseCaseFactory.PageSize, count);

            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(dtos.Length, output.SubCategories);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var d in dtos)
            {
                Assert.Contains(d, output.SubCategories);
            }
        }

        [TestMethod]
        public void ExecuteMainCategoryZeroPageTest()
        {
            var targetOwnershipId = 2;
            int page = 0;
            int? mainCategoryId = 1000;
            int count = 100;
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);

            var mainCategory = new EntryCategory("main-category") { Id = mainCategoryId.Value };
            context.TargetOwnership.PersonalStorage.AddEntryCategory(mainCategory);
            var dtos = FillCategoryAndGetPage(mainCategory, count, page);
            var pagination = new PaginationDTO(1, GetEntryCategoriesUseCaseFactory.PageSize, count);

            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(dtos.Length, output.SubCategories);
            Assert.AreEqual(pagination, output.Pagination);
            foreach (var d in dtos)
            {
                Assert.Contains(d, output.SubCategories);
            }
        }

        [TestMethod]
        public void ExecuteMainCategoryIntermediatePageTest()
        {
            var targetOwnershipId = 2;
            int page = 23;
            int? mainCategoryId = 319239129;
            int count = 100;
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);

            var mainCategory = new EntryCategory("main-category") { Id = mainCategoryId.Value };
            context.TargetOwnership.PersonalStorage.AddEntryCategory(mainCategory);
            var dtos = FillCategoryAndGetPage(mainCategory, count, page);
            var pagination = new PaginationDTO(page, GetEntryCategoriesUseCaseFactory.PageSize, count);

            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(pagination, output.Pagination);
            Assert.HasCount(dtos.Length, output.SubCategories);
            foreach (var d in dtos)
            {
                Assert.Contains(d, output.SubCategories);
            }
        }

        [TestMethod]
        public void ExecuteMainCategoryLastPageTest()
        {
            var targetOwnershipId = 2;
            int page = 34;
            int? mainCategoryId = 20000;
            int count = 100;
            var factory = new GetEntryCategoriesUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);

            var mainCategory = new EntryCategory("main-category") { Id = mainCategoryId.Value };
            context.TargetOwnership.PersonalStorage.AddEntryCategory(mainCategory);
            var dtos = FillCategoryAndGetPage(mainCategory, count, page);
            var pagination = new PaginationDTO(page, GetEntryCategoriesUseCaseFactory.PageSize, count);

            var input = new GetEntryCategoriesInput(1, targetOwnershipId, mainCategoryId, page);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(dtos.Length, output.SubCategories);
            foreach (var d in dtos)
            {
                Assert.Contains(d, output.SubCategories);
            }
        }
    }
}
