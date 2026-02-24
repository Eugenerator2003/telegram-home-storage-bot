using StorageBot.Domain.Account;
using StorageBot.Application.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.InitializeStorage
{
    public class InitializeStorageUseCase 
        : UserUseCase<InitializeStorageInput, InitializeStorageOutput>
    {
        protected UserOwnershipRepository ownershipRepository;

        public InitializeStorageUseCase(UserRepository userRepository, UserOwnershipRepository ownershipRepository) 
            : base(userRepository)
        {
            this.ownershipRepository = ownershipRepository;
        }

        protected override InitializeStorageOutput ExecuteWithUser(InitializeStorageInput input, User user)
        {
            var ownership = ownershipRepository.GetByUser(user);
            if (ownership != null)
            {
                return new InitializeStorageOutput(Output.OutputResult.FatalFail, "User already have a personal storage");
            }
            ownership = new UserOwnership(user);
            ownershipRepository.Create(ownership);
            return new InitializeStorageOutput(Output.OutputResult.Success);
        }

        protected override InitializeStorageOutput GenerateUserNotFoundOutput()
        {
            return new InitializeStorageOutput(Output.OutputResult.FatalFail, "User not found");
        }
    }
}
