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

namespace StorageBot.Application.Tests.TestUtilities.Factories
{
    public static class FactoryExtensions
    {
        public class FactoryUserContext
        {
            public required Mock<User> UserMock { get; set; }

            public required Mock<UserRepository> UserRepositoryMock { get; set; }
        }

        public class FactoryOwnershipContext : FactoryUserContext
        {
            public required UserOwnership UserOwnership { get; set; }

            public required Mock<UserOwnershipRepository> OwnershipRepositoryMock { get; set; }
        }

        public class FactoryTargetOwnershipContext : FactoryOwnershipContext
        {
            public required Mock<User> TargetUserMock { get; set; }

            public required UserOwnership TargetOwnership { get; set; }
        }

        public static FactoryUserContext ConfigureWithUser<T, TInput, TOutput>
            (this IUserUseCaseFactory<T, TInput, TOutput> factory)
            where T : UserUseCase<TInput, TOutput>
            where TInput : UserInput
            where TOutput : Output
        {
            var user = new Mock<User>();
            var repositoryMock = new Mock<UserRepository>();
            repositoryMock.Setup(m => m.GetById(It.IsAny<int>()))
                          .Returns(user.Object);
            factory.UserRepository = repositoryMock.Object;
            return new FactoryUserContext { UserMock = user, UserRepositoryMock = repositoryMock };
        }

        public static FactoryOwnershipContext ConfigureWithUserOwnership<T, TInput, TOutput>
            (this IUserOwnershipUseCaseFactory<T, TInput, TOutput> factory)
            where T : UserUseCase<TInput, TOutput>
            where TInput : UserInput
            where TOutput : Output
        {
            var context = factory.ConfigureWithUser();
            var ownership = new UserOwnership(context.UserMock.Object);
            var ownershipRepositoryMock = new Mock<UserOwnershipRepository>();
            ownershipRepositoryMock.Setup(m => m.GetByUser(It.IsAny<User>()))
                                   .Returns(ownership);
            factory.OwnershipRepository= ownershipRepositoryMock.Object;
            return new FactoryOwnershipContext()
            {
                UserMock = context.UserMock,
                UserRepositoryMock = context.UserRepositoryMock,
                UserOwnership = ownership,
                OwnershipRepositoryMock = ownershipRepositoryMock
            };
        }

        public static FactoryTargetOwnershipContext ConfigureWithTargetOwnership<T, TInput, TOutput>
            (this IStorageUseCaseFactory<T, TInput, TOutput> factory, int targetOwnershipId)
            where T : StorageUseCase<TInput, TOutput>
            where TInput : StorageInput
            where TOutput : Output
        {
            var context = factory.ConfigureWithUserOwnership();
            var userMock = new Mock<User>();
            var targetOwnership = new UserOwnership(userMock.Object);
            targetOwnership.Id = targetOwnershipId;
            context.UserOwnership.AddOtherStorage(targetOwnership.PersonalStorage);
            context.OwnershipRepositoryMock.Setup(m => m.GetById(targetOwnershipId))
                                           .Returns(targetOwnership);
            return new FactoryTargetOwnershipContext()
            {
                UserMock = context.UserMock,
                UserRepositoryMock = context.UserRepositoryMock,
                UserOwnership = context.UserOwnership,
                OwnershipRepositoryMock = context.OwnershipRepositoryMock,
                TargetUserMock = userMock,
                TargetOwnership = targetOwnership,
            };
        }
    }
}
