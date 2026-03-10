using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.UseCases.StorageManipulation.InitializeStorage;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.GetAccessibleStorages
{
    public class GetAccessibleStoragesUseCase : 
        UserOwnershipUseCase<GetAccessibleStoragesInput, GetAccessibleStoragesOutput>
    {
        public GetAccessibleStoragesUseCase(ILogger<GetAccessibleStoragesUseCase> logger, IStringLocalizer localizer, 
                                            UserRepository userRepository, UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override GetAccessibleStoragesOutput ExecuteWithUserOwnership(GetAccessibleStoragesInput input, User user, UserOwnership ownership)
        {
            var userIds = ownership.OtherStorages.Select(s => s.UserOwnership.User.Id)
                                                 .ToList();
            userIds.Insert(0, ownership.PersonalStorage.Id);
            _logger.LogInformation("Get storages for user {userId}", input.UserId);
            return new GetAccessibleStoragesOutput(Output.OutputResult.Success, userIds, "");
        }

        protected override GetAccessibleStoragesOutput GenerateOwnershipNotFoundOutput(GetAccessibleStoragesInput input)
        {
            return new GetAccessibleStoragesOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound", input.UserId]);
        }

        protected override GetAccessibleStoragesOutput GenerateUserNotFoundOutput(GetAccessibleStoragesInput input)
        {
            return new GetAccessibleStoragesOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound", input.UserId]);
        }
    }
}
