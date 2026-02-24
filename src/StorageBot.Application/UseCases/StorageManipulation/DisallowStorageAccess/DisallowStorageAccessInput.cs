using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.DisallowStorageAccess
{
    public class DisallowStorageAccessInput : UserInput
    {
        public int DisallowUserId { get; }

        public DisallowStorageAccessInput(int userId, int disallowUserId) 
            : base(userId)
        {
            DisallowUserId = disallowUserId;
        }
    }
}
