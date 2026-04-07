using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.UseCases.StorageManipulation.AllowStorageAccess;
using StorageBot.Domain.Account;
using StorageBot.Domain.Exceptions.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.DisallowStorageAccess
{
    public class DisallowStorageAccessUseCase
        : UserOwnershipUseCase<DisallowStorageAccessInput, DisallowStorageAccessOutput>
    {
        public DisallowStorageAccessUseCase(ILogger<DisallowStorageAccessUseCase> logger, IStringLocalizer localizer,
                                            UserRepository userRepository, UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override DisallowStorageAccessOutput ExecuteWithUserOwnership(DisallowStorageAccessInput input, User user, UserOwnership ownership)
        {
            if (input.UserId == input.DisallowUserId)
            {
                _logger.LogError("User failed to disallow personal storage as other user storage");
                return new DisallowStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.DisallowPersonalStorage"]);
            }
            var disallowUser = userRepository.GetById(input.DisallowUserId);
            if (disallowUser == null)
            {
                _logger.LogError("User to disallow {userId} not found", input.DisallowUserId);
                return new DisallowStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserToDisallowNotFound", input.DisallowUserId]);
            }
            var allowOwnership = ownershipRepository.GetByUser(user);
            if (allowOwnership == null)
            {
                _logger.LogError("Ownership of user to disallow {userId} not found", input.DisallowUserId);
                return new DisallowStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipToDisallownNotFound", input.DisallowUserId]);
            }
            Output.OutputResult result;
            string message = "";
            try
            {
                allowOwnership.RemoveOtherStorage(ownership.PersonalStorage);
                result = Output.OutputResult.Success;
                _logger.LogInformation("User disallowed access to personal storage to user {otherUserId}", input.DisallowUserId);
            }
            catch (NoStorageToRemoveException ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.NoStorageToRemoveException"];
                _logger.LogError(ex, "User can't disallow access to personal storage to user {otherUserId}: target user doesn't have a storage access", input.DisallowUserId);
            }
            catch (Exception ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.ServerError"];
                _logger.LogError(ex, "User can't disallow access to personal storage to user {otherUserId}", input.DisallowUserId);
            }
            return new DisallowStorageAccessOutput(result, message);
        }

        protected override DisallowStorageAccessOutput GenerateOwnershipNotFoundOutput(DisallowStorageAccessInput input)
        {
            return new DisallowStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound", input.UserId]);
        }

        protected override DisallowStorageAccessOutput GenerateUserNotFoundOutput(DisallowStorageAccessInput input)
        {
            return new DisallowStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound", input.UserId]);
        }
    }
}
