using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories.DeleteEntryCategoryUseCase;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.DeleteEntryCategory
{
    [TestClass]
    public class DeleteEntryCategoryUseCaseTests
    {
        private class DeleteEntryCategoryUseCaseFactory
            : StorageUseCaseFactory<DeleteEntryCategoryUseCase, DeleteEntryCategoryInput, DeleteEntryCategoryOutput>
        {
            public override DeleteEntryCategoryUseCase CreateUseCase()
            {
                return new DeleteEntryCategoryUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new DeleteEntryCategoryInput(1, 1, 1);
            var factory = new DeleteEntryCategoryUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new DeleteEntryCategoryInput(1, 1, 1);
            var factory = new DeleteEntryCategoryUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipFoundTest()
        {
            var input = new DeleteEntryCategoryInput(1, 1, 1);
            var factory = new DeleteEntryCategoryUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new DeleteEntryCategoryInput(1, 1, 1);
            var factory = new DeleteEntryCategoryUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteNotFoundTest()
        {
            var targetOwnershipId = 10;
            var entryCategory = new EntryCategory("category") { Id = 100 };
            var input = new DeleteEntryCategoryInput(1, targetOwnershipId, entryCategory.Id);
            var factory = new DeleteEntryCategoryUseCaseFactory();
            factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EntryCategoryNotFound"], output.Message);
        }


        [TestMethod]
        public void ExecuteStorageCategoryTest()
        {
            var targetOwnershipId = 10;
            var entryCategory = new EntryCategory("category") { Id = 100 };
            var input = new DeleteEntryCategoryInput(1, targetOwnershipId, entryCategory.Id);
            var factory = new DeleteEntryCategoryUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            context.TargetOwnership.PersonalStorage.AddEntryCategory(entryCategory);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(0, context.TargetOwnership.PersonalStorage.EntryCategories);
        }

        [TestMethod]
        public void ExecuteSubCategoryTest()
        {
            var targetOwnershipId = 10;
            var entryCategory = new EntryCategory("category") { Id = 1 };
            var subCategory1 = new EntryCategory("sub-category-1", entryCategory) { Id = 2 };
            var subCategory2 = new EntryCategory("sub-category-2", entryCategory) { Id = 3 };
            var factory = new DeleteEntryCategoryUseCaseFactory();
            var input = new DeleteEntryCategoryInput(1, targetOwnershipId, subCategory2.Id);
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            context.TargetOwnership.PersonalStorage.AddEntryCategory(entryCategory);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.DoesNotContain(subCategory2, subCategory1.Subcategories);
        }
    }
}
