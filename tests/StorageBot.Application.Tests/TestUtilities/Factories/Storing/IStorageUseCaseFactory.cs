using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.TestUtilities.Factories.Storing
{
    public interface IStorageUseCaseFactory<T, TInput, TOuput>
        : IUserOwnershipUseCaseFactory<T, TInput, TOuput>
        where T : StorageUseCase<TInput, TOuput>
        where TInput : StorageInput
        where TOuput : Output
    {
        
    }
}
