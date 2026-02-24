using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases
{
    public abstract class UserOwnershipUseCase<TInput, TOutput>
        : UserUseCase<TInput, TOutput>
        where TInput : UserInput
        where TOutput : Output
    {
        protected readonly UserOwnershipRepository ownershipRepository;

        protected UserOwnershipUseCase(ILogger<UserOwnershipUseCase<TInput, TOutput>> logger, IStringLocalizer localizer, 
                                       UserRepository userRepository, UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository)
        {
            this.ownershipRepository = ownershipRepository;
        }

        protected override TOutput ExecuteWithUser(TInput input, User user)
        {
            UserOwnership? ownership = ownershipRepository.GetByUser(user);
            if (ownership == null)
            {
                return GenerateOwnershipNotFoundOutputWithLog(input);
            }
            return ExecuteWithUserOwnership(input, user, ownership);
        }

        protected abstract TOutput ExecuteWithUserOwnership(TInput input, User user, UserOwnership ownership);

        protected abstract TOutput GenerateOwnershipNotFoundOutput();

        private TOutput GenerateOwnershipNotFoundOutputWithLog(TInput input)
        {
            _logger.LogError("Ownership of user with id {userId} not found", input.UserId);
            return GenerateOwnershipNotFoundOutput();
        }
    }
}
