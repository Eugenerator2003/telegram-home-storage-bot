using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories.UpdateEntryCategoryNameUseCase;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.UpdateEntryCategoryName
{
    [TestClass]
    public class UpdateEntryCategoryNameUseCaseTests
    {
        private class UpdateEntryCategoryNameUseCaseFactory
            : StorageUseCaseFactory<UpdateEntryCategoryNameUseCase, UpdateEntryCategoryNameInput, UpdateEntryCategoryNameOutput>
        {
            public override UpdateEntryCategoryNameUseCase CreateUseCase()
            {
                return new UpdateEntryCategoryNameUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new UpdateEntryCategoryNameInput(1, 1, 1, "aa");
            var factory = new UpdateEntryCategoryNameUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new UpdateEntryCategoryNameInput(1, 1, 1, "aa");
            var factory = new UpdateEntryCategoryNameUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new UpdateEntryCategoryNameInput(1, 1, 1, "aa");
            var factory = new UpdateEntryCategoryNameUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new UpdateEntryCategoryNameInput(1, 1, 1, "aa");
            var factory = new UpdateEntryCategoryNameUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteCategoryNotFoundTest()
        {
            var targetOwnershipId = 1;
            var input = new UpdateEntryCategoryNameInput(1, targetOwnershipId, 1, "aa");
            var factory = new UpdateEntryCategoryNameUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EntryCategoryNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteEmptyInputStringTest()
        {
            var categoryId = 230;
            var targetOwnershipId = 1;
            var input = new UpdateEntryCategoryNameInput(1, targetOwnershipId, categoryId, string.Empty);
            var factory = new UpdateEntryCategoryNameUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var category = new EntryCategory("category1") { Id = categoryId };
            var storage = context.TargetOwnership.PersonalStorage;
            storage.AddEntryCategory(category);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EmptyInputString"], output.Message);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var targetOwnershipId = 1;
            var categoryId = 1;
            var name = "name";
            var input = new UpdateEntryCategoryNameInput(1, targetOwnershipId, categoryId, name);
            var factory = new UpdateEntryCategoryNameUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var category = new EntryCategory("category1") { Id = categoryId };
            var storage = context.TargetOwnership.PersonalStorage;
            storage.AddEntryCategory(category);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(name, storage.FindEntryCategory(categoryId)?.Name);
        }
    }
}
