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
            ILogger<RelinquishStorageAccessUseCase> logger,
            IStringLocalizer localizer, UserRepository userRepository,
            UserOwnershipRepository ownershipRepository)
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override RelinquishStorageAccessOutput ExecuteWithUserOwnership(RelinquishStorageAccessInput input, User user, UserOwnership ownership)
        {
            using (_logger.BeginScope(new Dictionary<string, object> { ["StorageOwnerId"] = input.StorageOwnerId }))
            {
                var owner = userRepository.GetById(input.StorageOwnerId);
                if (owner == null)
                {
                    _logger.LogError("Storage owner not found");
                    return new RelinquishStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnerOfStorageToRelinquishNotFound"]);
                }
                var ownerOwnership = ownershipRepository.GetByUser(owner);
                if (ownerOwnership == null)
                {
                    _logger.LogError("Ownership of target user not found");
                    return new RelinquishStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipWithStorageToRelinquishNotFound"]);
                }

                Output.OutputResult result;
                string message = "";
                try
                {
                    ownership.RemoveOtherStorage(ownerOwnership.PersonalStorage);
                    result = Output.OutputResult.Success;
                    _logger.LogInformation("User relinquished access to personal storage of taget user");
                }
                catch (NoStorageToRemoveException ex)
                {
                    result = Output.OutputResult.FatalFail;
                    message = _localizer["Case.NoStorageToRemove"];
                    _logger.LogError(ex, "User can't relinquish access to personal storage to target user");
                }
                catch (Exception ex)
                {
                    result = Output.OutputResult.FatalFail;
                    message = _localizer["Case.ServerError"];
                    _logger.LogError(ex, "User can't relinquish access to personal storage to taget user");
                }
                return new RelinquishStorageAccessOutput(result, message);
            }
        }

        protected override RelinquishStorageAccessOutput GenerateOwnershipNotFoundOutput(RelinquishStorageAccessInput input)
        {
            return new RelinquishStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
        }

        protected override RelinquishStorageAccessOutput GenerateUserNotFoundOutput(RelinquishStorageAccessInput input)
        {
            return new RelinquishStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }

        protected override RelinquishStorageAccessOutput? ValidateBeforeExecute(RelinquishStorageAccessInput input)
        {
            if (input.UserId == input.StorageOwnerId)
            {
                _logger.LogError("User failed to relinquish access to personal storage");
                return new RelinquishStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.RelinquishAccessToPersonalStorage"]);
            }
            return base.ValidateBeforeExecute(input);
        }
    }
}
