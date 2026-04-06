using StorageBot.Application.Data.Repositories;
using StorageBot.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.TestUtilities.Factories
{
    public interface IUserUseCaseFactory<T, TInput, TOuput>
        : IUseCaseFactory<T, TInput, TOuput>
        where T : UserUseCase<TInput, TOuput>
        where TInput : UserInput
        where TOuput : Output
    {
        UserRepository UserRepository { get; set; }
    }
}
