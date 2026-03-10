using StorageBot.Domain.Account;
using StorageBot.Application.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;

namespace StorageBot.Application.UseCases.StorageManipulation.InitializeStorage
{
    public class InitializeStorageUseCase 
        : UserUseCase<InitializeStorageInput, InitializeStorageOutput>
    {
        protected UserOwnershipRepository ownershipRepository;

        public InitializeStorageUseCase(ILogger<InitializeStorageUseCase> logger,IStringLocalizer localizer, 
                                        UserRepository userRepository, UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository)
        {
            this.ownershipRepository = ownershipRepository;
        }

        protected override InitializeStorageOutput ExecuteWithUser(InitializeStorageInput input, User user)
        {
            var ownership = ownershipRepository.GetByUser(user);
            if (ownership != null)
            {
                _logger.LogError("User {userId} already have a ownership", user.Id);
                return new InitializeStorageOutput(Output.OutputResult.FatalFail, _localizer["Case.UserAlreadyHaveStorage", input.UserId]);
            }
            ownership = new UserOwnership(user);
            ownershipRepository.Create(ownership);
            _logger.LogInformation("Create a ownership to user {userId}", user.Id);
            return new InitializeStorageOutput(Output.OutputResult.Success);
        }

        protected override InitializeStorageOutput GenerateUserNotFoundOutput(InitializeStorageInput input)
        {
            return new InitializeStorageOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound", input.UserId]);
        }
    }
}
