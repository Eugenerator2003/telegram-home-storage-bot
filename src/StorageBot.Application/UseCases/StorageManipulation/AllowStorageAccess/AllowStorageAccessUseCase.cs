using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.UseCases.StorageManipulation.GetAccessibleStorages;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.AllowStorageAccess
{
    public class AllowStorageAccessUseCase
        : UserOwnershipUseCase<AllowStorageAccessInput, AllowStorageAccessOutput>
    {

        public AllowStorageAccessUseCase(ILogger<AllowStorageAccessUseCase> logger, IStringLocalizer localizer, 
                                         UserRepository userRepository, UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override AllowStorageAccessOutput ExecuteWithUserOwnership(AllowStorageAccessInput input, User user, UserOwnership ownership)
        {
            var allowUser = userRepository.GetById(input.AllowUserId);
            if (allowUser == null)
            {
                _logger.LogError("User to allow with id {userId} not found", input.AllowUserId);
                return new AllowStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserToAllowNotFound", input.AllowUserId]);
            }
            var allowOwnership = ownershipRepository.GetByUser(user);
            if (allowOwnership == null)
            {
                _logger.LogError("Ownership of user to allow with id {userId} not found", input.AllowUserId);
                return new AllowStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserToAllowOwnershipNotFound", input.AllowUserId]);
            }
            Output.OutputResult result;
            string message = "";
            try
            {
                allowOwnership.AddOtherStorage(ownership.PersonalStorage);
                result = Output.OutputResult.Success;
            }
            catch (Exception ex) 
            {
                result = Output.OutputResult.Fail;
                message = ex.Message;
            }
            return new AllowStorageAccessOutput(result, message);
        }

        protected override AllowStorageAccessOutput GenerateOwnershipNotFoundOutput(AllowStorageAccessInput input)
        {
            return new AllowStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound", input.UserId]);
        }

        protected override AllowStorageAccessOutput GenerateUserNotFoundOutput(AllowStorageAccessInput input)
        {
            return new AllowStorageAccessOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound", input.UserId]);
        }
    }

}
