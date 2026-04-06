using Microsoft.Extensions.Localization;
using Moq;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.TestUtilities
{
    public class UseCaseTester
    {
        public static void UserNotFoundTest<TUseCase, TInput, TOuput>(
            IUserUseCaseFactory<TUseCase, TInput, TOuput> factory,
            TInput input)
            where TUseCase : UserUseCase<TInput, TOuput>
            where TInput : UserInput
            where TOuput : Output
        {
            User? user = null;
            var userRepositoryMock = new Mock<UserRepository>();
            userRepositoryMock.Setup(m => m.GetById(input.UserId))
                              .Returns(user);
            factory.UserRepository = userRepositoryMock.Object;
            var useCase = factory.CreateUseCase();
            var output = useCase.Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.UserNotFound"], output.Message);
        }

        public static void UserOwnershipNotFoundTest<TUseCase, TInput, TOuput>(
            IUserOwnershipUseCaseFactory<TUseCase, TInput, TOuput> factory,
            TInput input)
            where TUseCase : UserOwnershipUseCase<TInput, TOuput>
            where TInput : UserInput
            where TOuput : Output
        {
            User user = new Mock<User>().Object;
            var userRepositoryMock = new Mock<UserRepository>();
            userRepositoryMock.Setup(m => m.GetById(input.UserId))
                              .Returns(user);
            factory.UserRepository = userRepositoryMock.Object;
            UserOwnership? userOwnership = null;
            var ownershipRepositoryMock = new Mock<UserOwnershipRepository>();
            ownershipRepositoryMock.Setup(m => m.GetByUser(user))
                                   .Returns(userOwnership);
            factory.OwnershipRepository = ownershipRepositoryMock.Object;
            var useCase = factory.CreateUseCase();
            var output = useCase.Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.UserOwnershipNotFound"], output.Message);
        }

        public static void StorageOwnershipNotFoundTest<TUseCase, TInput, TOuput>(
            IStorageUseCaseFactory<TUseCase, TInput, TOuput> factory,
            TInput input)
            where TUseCase : StorageUseCase<TInput, TOuput>
            where TInput : StorageInput
            where TOuput : Output
        {
            User user = new Mock<User>().Object;
            var userRepositoryMock = new Mock<UserRepository>();
            userRepositoryMock.Setup(m => m.GetById(input.UserId))
                              .Returns(user);
            factory.UserRepository = userRepositoryMock.Object;
            UserOwnership userOwnership = new UserOwnership(user);
            var ownershipRepositoryMock = new Mock<UserOwnershipRepository>();
            ownershipRepositoryMock.Setup(m => m.GetByUser(user))
                                   .Returns(userOwnership);
            UserOwnership? storageOwnership = null;
            ownershipRepositoryMock.Setup(m => m.GetById(input.StorageOwnershipId))
                                   .Returns(storageOwnership);
            factory.OwnershipRepository = ownershipRepositoryMock.Object;
            var useCase = factory.CreateUseCase();
            var output = useCase.Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.StorageOwnershipNotFound"], output.Message);
        }

        public static void NoStorageAccessTest<TUseCase, TInput, TOuput>(
            IStorageUseCaseFactory<TUseCase, TInput, TOuput> factory,
            TInput input)
            where TUseCase : StorageUseCase<TInput, TOuput>
            where TInput : StorageInput
            where TOuput : Output
        {
            User user = new Mock<User>().Object;
            var userRepositoryMock = new Mock<UserRepository>();
            userRepositoryMock.Setup(m => m.GetById(input.UserId))
                              .Returns(user);
            factory.UserRepository = userRepositoryMock.Object;
            UserOwnership userOwnership = new UserOwnership(user);
            var ownershipRepositoryMock = new Mock<UserOwnershipRepository>();
            ownershipRepositoryMock.Setup(m => m.GetByUser(user))
                                   .Returns(userOwnership);
            UserOwnership? storageOwnership = new UserOwnership(new Mock<User>().Object);
            ownershipRepositoryMock.Setup(m => m.GetById(input.StorageOwnershipId))
                                   .Returns(storageOwnership);
            factory.OwnershipRepository = ownershipRepositoryMock.Object;
            var useCase = factory.CreateUseCase();
            var output = useCase.Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.NoStorageAccess"], output.Message);
        }
    }
}
