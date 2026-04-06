using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.TestUtilities.Factories.Storing
{
    public abstract class StorageUseCaseFactory<T, TInput, TOuput>
        : UserOwnershipUseCaseFactory<T, TInput, TOuput>, IStorageUseCaseFactory<T, TInput, TOuput>
            where T : StorageUseCase<TInput, TOuput>
            where TInput : StorageInput
            where TOuput : Output
    {
    }
}
