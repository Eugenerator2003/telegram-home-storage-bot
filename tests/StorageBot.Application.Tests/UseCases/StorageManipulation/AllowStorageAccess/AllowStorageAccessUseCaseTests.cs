using Moq;
using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.StorageManipulation.AllowStorageAccess;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.AllowStorageAccess
{
    [TestClass]
    public class AllowStorageAccessUseCaseTests
    {
        private class AllowStorageAccessUseCaseFactory
            : UserOwnershipUseCaseFactory<AllowStorageAccessUseCase, AllowStorageAccessInput, AllowStorageAccessOutput>
        {
            public override AllowStorageAccessUseCase CreateUseCase()
            {
                return new AllowStorageAccessUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new AllowStorageAccessInput(3123, 2131);
            var factory = new AllowStorageAccessUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new AllowStorageAccessInput(3123, 2131);
            var factory = new AllowStorageAccessUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void ExecuteAllowSelfTest()
        {
            var input = new AllowStorageAccessInput(1, 1);
            var factory = new AllowStorageAccessUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.AllowPersonalStorageAsOther"], output.Message);
        }

        [TestMethod]
        public void ExecuteAllowUserNotFoundTest()
        {
            var userId = 999;
            var allowUserId = 1999;
            var input = new AllowStorageAccessInput(userId, allowUserId);
            var factory = new AllowStorageAccessUseCaseFactory();
            var context = factory.ConfigureWithUserOwnership();
            context.UserRepositoryMock.Setup(r => r.GetById(allowUserId))
                                      .Returns(null as User);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.UserToAllowNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteAllowUserOwnershipNotFoundTest()
        {
            var userId = 999;
            var allowUserId = 1999;
            var input = new AllowStorageAccessInput(userId, allowUserId);
            var factory = new AllowStorageAccessUseCaseFactory();
            var context = factory.ConfigureWithUserOwnership();
            var userMock = new Mock<User>();
            context.UserRepositoryMock.Setup(r => r.GetById(allowUserId))
                                      .Returns(userMock.Object);
            context.OwnershipRepositoryMock.Setup(r => r.GetByUser(userMock.Object))
                                           .Returns(null as UserOwnership);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.UserOwnershipToAllowNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteSuccessTest()
        {
            var userId = 999;
            var allowUserId = 1999;
            var input = new AllowStorageAccessInput(userId, allowUserId);
            var factory = new AllowStorageAccessUseCaseFactory();
            var context = factory.ConfigureWithUserOwnership();
            var userMock = new Mock<User>();
            context.UserRepositoryMock.Setup(r => r.GetById(allowUserId))
                                      .Returns(userMock.Object);
            var allowOwnership = new UserOwnership(userMock.Object);
            context.OwnershipRepositoryMock.Setup(r => r.GetByUser(userMock.Object))
                                           .Returns(allowOwnership);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
        }

        [TestMethod]
        public void ExecuteUserAlreadyHaveAccessTest()
        {
            var userId = 999;
            var allowUserId = 1999;
            var input = new AllowStorageAccessInput(userId, allowUserId);
            var factory = new AllowStorageAccessUseCaseFactory();
            var context = factory.ConfigureWithUserOwnership();
            var userMock = new Mock<User>();
            context.UserRepositoryMock.Setup(r => r.GetById(allowUserId))
                                      .Returns(userMock.Object);
            var allowOwnership = new UserOwnership(userMock.Object);
            allowOwnership.AddOtherStorage(context.UserOwnership.PersonalStorage);
            context.OwnershipRepositoryMock.Setup(r => r.GetByUser(userMock.Object))
                                           .Returns(allowOwnership);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.UserAlreadyHaveStorageAccess"], output.Message);
        }
    }
}
