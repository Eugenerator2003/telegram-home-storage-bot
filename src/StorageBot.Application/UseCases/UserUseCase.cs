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

        protected UserUseCase(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override TOutput Execute(TInput input)
        {
            User? user = userRepository.GetById(input.UserId);
            if (user == null)
            {
                return GenerateUserNotFoundOutput();
            }
            return ExecuteWithUser(input, user);
        }

        protected abstract TOutput ExecuteWithUser(TInput input, User user);

        protected abstract TOutput GenerateUserNotFoundOutput();
    }
}
