using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Utilities;
using StorageBot.Domain.Repositories;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries
{
    public abstract class ContainerEntryUseCase<TInput, TOutput> :
        StorageUseCase<TInput, TOutput>
        where TInput : ContainerInput
        where TOutput : Output
    {
        protected ContainerEntryRepository ContainerEntryRepository { get; private set; } = null!;
        protected StorageEntryQuantityTypeRepository StorageEntryQuantityTypeRepository { get; private set; } = null!;

        protected ContainerEntryUseCase(ILogger<ContainerEntryUseCase<TInput, TOutput>> logger, 
                                        IStringLocalizer localizer, UserRepository userRepository, 
                                        UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected sealed override TOutput ExecuteWithStorage(TInput input, Storage storage)
        {
            using (_logger.BeginContainerScope(input.ContainerId)) 
            {
                var containerRepository = new StorageContainerRepository(storage);
                var container = containerRepository.GetById(input.ContainerId);
                if (container == null)
                {
                    _logger.LogError("Container not found");
                    return GenerateContainerNotFoundOutput();
                }
                ContainerEntryRepository = new ContainerEntryRepository(container);
                StorageEntryQuantityTypeRepository = new StorageEntryQuantityTypeRepository(storage);
                return ExecuteWithContainer(input);
            }
        }

        protected abstract TOutput GenerateContainerNotFoundOutput();

        protected abstract TOutput ExecuteWithContainer(TInput input);
    }
}
