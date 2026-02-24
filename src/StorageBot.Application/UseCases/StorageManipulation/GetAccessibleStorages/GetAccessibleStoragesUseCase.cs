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
        public GetAccessibleStoragesUseCase(UserRepository userRepository, UserOwnershipRepository ownershipRepository) 
            : base(userRepository, ownershipRepository)
        {
        }

        protected override GetAccessibleStoragesOutput ExecuteWithUserOwnership(GetAccessibleStoragesInput input, User user, UserOwnership ownership)
        {
            var userIds = ownership.OtherStorages.Select(s => s.UserOwnership.User.Id).ToList();
            return new GetAccessibleStoragesOutput(Output.OutputResult.Success, userIds, "");
        }

        protected override GetAccessibleStoragesOutput GenerateOwnershipNotFoundOutput()
        {
            return new GetAccessibleStoragesOutput(Output.OutputResult.FatalFail, "User does not have a ownership");
        }

        protected override GetAccessibleStoragesOutput GenerateUserNotFoundOutput()
        {
            return new GetAccessibleStoragesOutput(Output.OutputResult.FatalFail, "User not found");
        }
    }
}
