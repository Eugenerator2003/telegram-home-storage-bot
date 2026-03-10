using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.UseCases.StorageManipulation.DisallowStorageAccess;
using StorageBot.Domain.Account;
using StorageBot.Domain.Exceptions.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.RelinquishStorageAccess
{
    public class RelinquishStorageAccessUseCase :
        UserOwnershipUseCase<RelinquishStorageAccessInput, RelinquishStorageAccessOutput>
    {
        public RelinquishStorageAccessUseCase(
            ILogger<UserOwnershipUseCase<RelinquishStorageAccessInput, RelinquishStorageAccessOutput>> logger, 
            IStringLocalizer localizer, UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override RelinquishStorageAccessOutput ExecuteWithUserOwnership(RelinquishStorageAccessInput input, User user, UserOwnership ownership)
        {
            var owner = userRepository.GetById(input.StorageOwnerId);
            if (owner == null)
            {
                _logger.LogError("Storage owner {userId} not found", input.StorageOwnerId);
                return new RelinquishStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnerOfStorageToRelinquishNotFound", input.StorageOwnerId]);
            }
            var ownerOwnership = ownershipRepository.GetByUser(owner);
            if (ownerOwnership == null)
            {
                _logger.LogError("Ownership of user {userId} not found", input.StorageOwnerId);
                return new RelinquishStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipWithStorageToRelinquishNotFound", input.StorageOwnerId]);
            }

            Output.OutputResult result;
            string message = "";
            try
            {
                ownership.RemoveOtherStorage(ownerOwnership.PersonalStorage);
                result = Output.OutputResult.Success;
                _logger.LogInformation("User {usedId} relinquished access to personal storage of user {storageOwnerId}", user.Id, input.StorageOwnerId);
            }
            catch (NoStorageToRemoveException ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.NoStorageToRemoveException"];
                _logger.LogError(ex, "User {userId} can't relinquish access to personal storage to user {storageOwnerId}", input.UserId, input.StorageOwnerId);
            }
            catch (Exception ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.ServerError"];
                _logger.LogError(ex, "User {userId} can't relinquish access to personal storage to user {storageOwnerId}", input.UserId, input.StorageOwnerId);
            }

            return new RelinquishStorageAccessOutput(result, message);
        }

        protected override RelinquishStorageAccessOutput GenerateOwnershipNotFoundOutput(RelinquishStorageAccessInput input)
        {
            return new RelinquishStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound", input.UserId]);
        }

        protected override RelinquishStorageAccessOutput GenerateUserNotFoundOutput(RelinquishStorageAccessInput input)
        {
            return new RelinquishStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound", input.UserId]);
        }

        protected override RelinquishStorageAccessOutput? ValidateBeforeExecute(RelinquishStorageAccessInput input)
        {
            if (input.UserId == input.StorageOwnerId)
            {
                _logger.LogError("User {userId} failed to relinquish access to personal storage", input.UserId);
                return new RelinquishStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.RelinquishAccessToPersonalStorage"]);
            }
            return base.ValidateBeforeExecute(input);
        }
    }
}
