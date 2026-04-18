using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.TestUtilities.Factories.Storing.ContainerEntries
{
    public abstract class ContainerEntryUseCaseFactory<T, TIn, TOut>
        : StorageUseCaseFactory<T, TIn, TOut>,  IContainerEntryUseCaseFactory<T, TIn, TOut>
        where T : ContainerEntryUseCase<TIn, TOut>
        where TIn : ContainerInput
        where TOut: Output
    {
    }
}
