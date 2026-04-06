using Moq;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.UseCases;
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
    }
}
