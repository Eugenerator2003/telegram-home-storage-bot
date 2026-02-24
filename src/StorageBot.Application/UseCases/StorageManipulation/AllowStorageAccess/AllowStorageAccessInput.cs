using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.AllowStorageAccess
{
    public class AllowStorageAccessInput : UserInput
    {
        public int AllowUserId { get; }

        public AllowStorageAccessInput(int userId, int allowUserId) 
            : base(userId)
        {
            AllowUserId = allowUserId;
        }
    }
}
