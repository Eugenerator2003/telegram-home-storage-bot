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
    public abstract class UserUseCaseFactory<T, TInput, TOuput>
        : UseCaseFactory<T, TInput, TOuput>, IUserUseCaseFactory<T, TInput, TOuput>
            where T : UserUseCase<TInput, TOuput>
            where TInput : UserInput
            where TOuput : Output
    {
        public UserRepository UserRepository { get; set; } = new Mock<UserRepository>().Object;
    }
}
