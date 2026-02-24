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

        public AllowStorageAccessUseCase(UserRepository userRepository, UserOwnershipRepository ownershipRepository) 
            : base(userRepository, ownershipRepository)
        {
        }

        protected override AllowStorageAccessOutput ExecuteWithUserOwnership(AllowStorageAccessInput input, User user, UserOwnership ownership)
        {
            var allowUser = userRepository.GetById(input.AllowUserId);
            if (allowUser == null)
            {
                return new AllowStorageAccessOutput(Output.OutputResult.FatalFail, "User to allow not found");
            }
            var allowOwnership = ownershipRepository.GetByUser(user);
            if (allowOwnership == null)
            {
                return new AllowStorageAccessOutput(Output.OutputResult.FatalFail, "User does not have a ownership");
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

        protected override AllowStorageAccessOutput GenerateOwnershipNotFoundOutput()
        {
            return new AllowStorageAccessOutput(Output.OutputResult.FatalFail, "User does not have a ownership");
        }

        protected override AllowStorageAccessOutput GenerateUserNotFoundOutput()
        {
            return new AllowStorageAccessOutput(Output.OutputResult.FatalFail, "User not found");
        }
    }

}
