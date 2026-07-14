using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Utilities;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.GetUserOwnership
{
    public class GetUserOwnershipUseCase : UseCase<GetUserOwnershipInput, GetUserOwnershipOutput>
    {
        private readonly UserRepository userRepository;
        private readonly UserOwnershipRepository userOwnershipRepository;

        public GetUserOwnershipUseCase(
            ILogger<UseCase<GetUserOwnershipInput, GetUserOwnershipOutput>> logger, 
            IStringLocalizer localizer,
            UserRepository userRepository,
            UserOwnershipRepository userOwnershipRepository) 
            : base(logger, localizer)
        {
            this.userRepository = userRepository;
            this.userOwnershipRepository = userOwnershipRepository;
        }

        protected override GetUserOwnershipOutput ExecuteAfterValidation(GetUserOwnershipInput input)
        {
            using (_logger.BeginUserScope(input.UserId))
            {
                var user = userRepository.GetById(input.UserId);
                if (user == null)
                {
                    _logger.LogError("User not found");
                    return new GetUserOwnershipOutput(-1, Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
                }
                var ownership = userOwnershipRepository.GetByUser(user);
                if (ownership == null)
                {
                    _logger.LogError("User ownership not found");
                    return new GetUserOwnershipOutput(-1, Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
                }
                else
                {
                    _logger.LogInformation("Ownership retrieved");
                    return new GetUserOwnershipOutput(ownership.Id, Output.OutputResult.Success, string.Empty);
                }
            }
        }
    }
}
