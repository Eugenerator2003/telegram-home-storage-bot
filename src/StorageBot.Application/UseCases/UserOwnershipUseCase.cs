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

        protected UserOwnershipUseCase(UserRepository userRepository, UserOwnershipRepository ownershipRepository) 
            : base(userRepository)
        {
            this.ownershipRepository = ownershipRepository;
        }

        protected override TOutput ExecuteWithUser(TInput input, User user)
        {
            UserOwnership? ownership = ownershipRepository.GetByUser(user);
            if (ownership == null)
            {
                return GenerateOwnershipNotFoundOutput();
            }
            return ExecuteWithUserOwnership(input, user, ownership);
        }

        protected abstract TOutput ExecuteWithUserOwnership(TInput input, User user, UserOwnership ownership);

        protected abstract TOutput GenerateOwnershipNotFoundOutput();
    }
}
