using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.Storing;
using StorageBot.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes
{
    public abstract class StorageEntryQuantityTypeUseCase<TInput, TOutput>
        : StorageUseCase<TInput, TOutput>
        where TInput : StorageInput
        where TOutput : Output
    {
        protected StorageEntryQuantityTypeRepository QuantityTypeRepository { get; private set; } = null!;

        public StorageEntryQuantityTypeUseCase(
            ILogger<StorageEntryQuantityTypeUseCase<TInput, TOutput>> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected sealed override TOutput ExecuteWithStorage(TInput input, Storage storage)
        {
            QuantityTypeRepository = new StorageEntryQuantityTypeRepository(storage);
            return ExecuteWithRepository(input);
        }

        protected abstract TOutput ExecuteWithRepository(TInput input);
    }
}
