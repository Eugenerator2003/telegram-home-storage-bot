using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories.CreateEntryCategory;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.CreateEntryCategory
{
    [TestClass]
    public class CreateEntryCategoryUseCaseTests
    {
        private class CreateEntryCategoryUseCaseFactory
            : StorageUseCaseFactory<CreateEntryCategoryUseCase, CreateEntryCategoryInput, CreateEntryCategoryOutput>
        {
            public override CreateEntryCategoryUseCase CreateUseCase()
            {
                return new CreateEntryCategoryUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new CreateEntryCategoryInput(1, 1, null, "cat");
            var factory = new CreateEntryCategoryUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new CreateEntryCategoryInput(1, 1, null, "cat");
            var factory = new CreateEntryCategoryUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new CreateEntryCategoryInput(1, 1, null, "cat");
            var factory = new CreateEntryCategoryUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new CreateEntryCategoryInput(1, 1, null, "cat");
            var factory = new CreateEntryCategoryUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteEmptyInputStringTest()
        {
            var userId = 1;
            var storageOwnershipId = 10;
            var mainCategoryId = new Nullable<int>();
            var name = string.Empty;
            var input = new CreateEntryCategoryInput(userId, storageOwnershipId, mainCategoryId, name);
            var factory = new CreateEntryCategoryUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(storageOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EmptyInputString"], output.Message);
        }

        [TestMethod]
        public void ExecuteMainCategoryNotFoundTest()
        {
            var userId = 1;
            var storageOwnershipId = 10;
            var mainCategoryId = 99;
            var name = "category";
            var input = new CreateEntryCategoryInput(userId, storageOwnershipId, mainCategoryId, name);
            var factory = new CreateEntryCategoryUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(storageOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.MainCategoryNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteNoMainCategoryTest()
        {
            var userId = 1;
            var storageOwnershipId = 10;
            var mainCategoryId = new Nullable<int>();
            var name = "category";
            var input = new CreateEntryCategoryInput(userId, storageOwnershipId, mainCategoryId, name);
            var factory = new CreateEntryCategoryUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(storageOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            var category = context.TargetOwnership.PersonalStorage.EntryCategories.Single();
            Assert.AreEqual(name, category.Name);
        }

        [TestMethod]
        public void ExecuteWithMainCategoryTest()
        {
            var mainCategory = new EntryCategory("main-category") { Id = 999 };
            var userId = 1;
            var storageOwnershipId = 10;
            var mainCategoryId = mainCategory.Id;
            var name = "category";
            var input = new CreateEntryCategoryInput(userId, storageOwnershipId, mainCategoryId, name);
            var factory = new CreateEntryCategoryUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(storageOwnershipId);
            context.TargetOwnership.PersonalStorage.AddEntryCategory(mainCategory);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            var category = context.TargetOwnership.PersonalStorage.EntryCategories.Single().Subcategories.Single();
            Assert.AreEqual(mainCategory, category.MainCategory);
            Assert.AreEqual(name, category.Name);
        }
    }
}
