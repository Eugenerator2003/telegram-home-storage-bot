using Moq;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.TestUtilities.Factories
{
    public abstract class UserOwnershipUseCaseFactory<T, TInput, TOuput>
        : UserUseCaseFactory<T, TInput, TOuput>, IUserOwnershipUseCaseFactory<T, TInput, TOuput>
            where T : UserOwnershipUseCase<TInput, TOuput>
            where TInput : UserInput
            where TOuput : Output
    {
        public UserOwnershipRepository OwnershipRepository { get; set; } = new Mock<UserOwnershipRepository>().Object;
    }
}
