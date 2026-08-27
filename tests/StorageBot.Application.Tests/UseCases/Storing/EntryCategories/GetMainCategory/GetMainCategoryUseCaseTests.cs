using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetMainCategoryUseCase;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.GetMainCategory
{
    [TestClass]
    public class GetMainCategoryUseCaseTests
    {
        private class GetMainCategoryUseCaseFactory :
            StorageUseCaseFactory<GetMainCategoryUseCase, GetMainCategoryInput, GetMainCategoryOutput>
        {
            public override GetMainCategoryUseCase CreateUseCase()
            {
                return new GetMainCategoryUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new GetMainCategoryInput(3003, 444030, 399);
            var factory = new GetMainCategoryUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new GetMainCategoryInput(3003, 444030, 399);
            var factory = new GetMainCategoryUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new GetMainCategoryInput(3003, 444030, 399);
            var factory = new GetMainCategoryUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTests()
        {
            var input = new GetMainCategoryInput(3003, 444030, 399);
            var factory = new GetMainCategoryUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteSubcategoryNotFoundTest()
        {
            var targetOwnershipId = 20;
            var subcategoryId = 404;
            var input = new GetMainCategoryInput(3003, targetOwnershipId, subcategoryId);
            var factory = new GetMainCategoryUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.SubcategoryNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteNoMainCategoryTest()
        {
            var targetOwnershipId = 20;
            var subcategoryId = 404;
            var input = new GetMainCategoryInput(3003, targetOwnershipId, subcategoryId);
            var factory = new GetMainCategoryUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var category = new EntryCategory("category") { Id = subcategoryId };
            context.TargetOwnership.PersonalStorage.AddEntryCategory(category);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.MainCategoryNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteSuccessTest()
        {
            var targetOwnershipId = 20;
            var subcategoryId = 404;
            var input = new GetMainCategoryInput(3003, targetOwnershipId, subcategoryId);
            var factory = new GetMainCategoryUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var mainCategory = new EntryCategory("main category") { Id = 200 };
            var category = new EntryCategory("category", mainCategory) { Id = subcategoryId };
            context.TargetOwnership.PersonalStorage.AddEntryCategory(mainCategory);
            var output = factory.CreateUseCase().Execute(input);
            var dto = new EntryCategoryDTO(mainCategory.Id, mainCategory.Name);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(dto, output.MainCategory);
        }
    }
}
