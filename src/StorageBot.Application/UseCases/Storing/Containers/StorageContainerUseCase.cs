using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.Repositories;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers
{
    public abstract class StorageContainerUseCase<TInput, TOutput> :
        StorageUseCase<TInput, TOutput>
        where TInput : StorageInput
        where TOutput : Output
    {
        protected StorageContainerRepository StorageContainerRepository { get; private set; } = null!;

        public StorageContainerUseCase(
            ILogger<StorageContainerUseCase<TInput, TOutput>> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override TOutput ExecuteWithStorage(TInput input, Storage storage)
        {
            StorageContainerRepository = new StorageContainerRepository(storage);
            return ExecuteWithRepository(input);
        }

        protected abstract TOutput ExecuteWithRepository(TInput input);
    }
}
