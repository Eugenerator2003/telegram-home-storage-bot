using Moq;
using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.StorageManipulation.RelinquishStorageAccess;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.RelinquishStorageAccess
{
    [TestClass]
    public class RelinquishStorageAccessUseCaseTests
    {
        private class RelinquishStorageAccessUseCaseFactory
            : UserOwnershipUseCaseFactory<RelinquishStorageAccessUseCase, RelinquishStorageAccessInput, RelinquishStorageAccessOutput>
        {
            public override RelinquishStorageAccessUseCase CreateUseCase()
            {
                return new RelinquishStorageAccessUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new RelinquishStorageAccessInput(10, 21);
            var factory = new RelinquishStorageAccessUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new RelinquishStorageAccessInput(21, 10);
            var factory = new RelinquishStorageAccessUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }


        [TestMethod]
        public void ExecuteRelinquishPersonalStorageTest()
        {
            var userId = 999;
            var storageOwnerId = userId;
            var input = new RelinquishStorageAccessInput(userId, storageOwnerId);
            var factory = new RelinquishStorageAccessUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.RelinquishAccessToPersonalStorage"], output.Message);
        }

        [TestMethod]
        public void ExecuteStorageOnwerNotFoundTest()
        {
            var userId = 999;
            var storageOwnerId = 1999;
            var input = new RelinquishStorageAccessInput(userId, storageOwnerId);
            var factory = new RelinquishStorageAccessUseCaseFactory();
            var context = factory.ConfigureWithUserOwnership();
            context.UserRepositoryMock.Setup(r => r.GetById(storageOwnerId))
                                      .Returns(null as User);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.OwnerOfStorageToRelinquishNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteOnwershipWithStorageToRelinquishNotFoundTest()
        {
            var userId = 999;
            var storageOwnerId = 1999;
            var input = new RelinquishStorageAccessInput(userId, storageOwnerId);
            var factory = new RelinquishStorageAccessUseCaseFactory();
            var context = factory.ConfigureWithUserOwnership();
            var userMock = new Mock<User>();
            context.UserRepositoryMock.Setup(r => r.GetById(storageOwnerId))
                                      .Returns(userMock.Object);
            context.OwnershipRepositoryMock.Setup(r => r.GetByUser(userMock.Object))
                                           .Returns(null as UserOwnership);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.OwnershipWithStorageToRelinquishNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteSuccessTest()
        {
            var userId = 999;
            var storageOwnerId = 1999;
            var input = new RelinquishStorageAccessInput(userId, storageOwnerId);
            var factory = new RelinquishStorageAccessUseCaseFactory();
            var context = factory.ConfigureWithUserOwnership();
            var userMock = new Mock<User>();
            context.UserRepositoryMock.Setup(r => r.GetById(storageOwnerId))
                                      .Returns(userMock.Object);
            UserOwnership storageOwnership = new UserOwnership(userMock.Object);
            context.UserOwnership.AddOtherStorage(storageOwnership.PersonalStorage);
            context.OwnershipRepositoryMock.Setup(r => r.GetByUser(userMock.Object))
                                           .Returns(storageOwnership);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
        }

        [TestMethod]
        public void ExecuteNoStorageToRemoveTest()
        {
            var userId = 999;
            var storageOwnerId = 1999;
            var input = new RelinquishStorageAccessInput(userId, storageOwnerId);
            var factory = new RelinquishStorageAccessUseCaseFactory();
            var context = factory.ConfigureWithUserOwnership();
            var userMock = new Mock<User>();
            context.UserRepositoryMock.Setup(r => r.GetById(storageOwnerId))
                                      .Returns(userMock.Object);
            UserOwnership storageOwnership = new UserOwnership(userMock.Object);
            context.OwnershipRepositoryMock.Setup(r => r.GetByUser(userMock.Object))
                                           .Returns(storageOwnership);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.NoStorageToRemove"], output.Message);
        }
    }
}
