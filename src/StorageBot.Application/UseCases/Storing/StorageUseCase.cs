using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Utils;
using StorageBot.Domain.Account;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing
{
    public abstract class StorageUseCase<TInput, TOutput> :
        UserOwnershipUseCase<TInput, TOutput>
        where TInput : StorageInput
        where TOutput : Output
    {
        protected StorageUseCase(
            ILogger<StorageUseCase<TInput, TOutput>> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected sealed override TOutput ExecuteWithUserOwnership(TInput input, User user, UserOwnership ownership)
        {
            using (_logger.BeginStorageOwnershipScope(input.StorageOwnershipId))
            {
                var storageOwnership = ownershipRepository.GetById(input.StorageOwnershipId);
                if (storageOwnership == null)
                {
                    _logger.LogError("Storage ownership not found");
                    return GenerateStorageOwnershipNotFoundOutput(input);
                }
                if (!ownership.OtherStorages.Contains(storageOwnership.PersonalStorage))
                {
                    _logger.LogError("User has no access to personal storage");
                    return GenerateNoAccessOutput(input);
                }
                return ExecuteWithStorage(input, storageOwnership.PersonalStorage);
            }
        }

        protected abstract TOutput ExecuteWithStorage(TInput input, Storage storage);

        protected abstract TOutput GenerateStorageOwnershipNotFoundOutput(TInput input);

        protected abstract TOutput GenerateNoAccessOutput(TInput input);
    }
}
