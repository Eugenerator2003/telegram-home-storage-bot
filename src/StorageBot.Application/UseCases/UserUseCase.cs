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
    public abstract class UserUseCase<TInput, TOutput> 
        : UseCase<TInput, TOutput>
        where TInput : UserInput
        where TOutput : Output
    {
        protected readonly UserRepository userRepository;

        protected UserUseCase(ILogger<UserUseCase<TInput, TOutput>> logger, IStringLocalizer localizer, UserRepository userRepository)
            : base(logger, localizer)
        {
            this.userRepository = userRepository;
        }

        protected override sealed TOutput ExecuteAfterValidation(TInput input)
        {
            User? user = userRepository.GetById(input.UserId);
            if (user == null)
            {
                return GenerateUserNotFoundOutputWithLog(input);
            }
            return ExecuteWithUser(input, user);
        }

        protected abstract TOutput ExecuteWithUser(TInput input, User user);

        protected abstract TOutput GenerateUserNotFoundOutput(TInput input);

        private TOutput GenerateUserNotFoundOutputWithLog(TInput input)
        {
            _logger.LogError("User with id {userId} not found", input.UserId);
            return GenerateUserNotFoundOutput(input);
        }
    }
}
