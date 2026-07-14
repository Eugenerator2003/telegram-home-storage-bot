using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.GetUserOwnership
{
    public class GetUserOwnershipInput : Input
    {
        public int UserId { get; }

        public GetUserOwnershipInput(int userId)
        {
            UserId = userId;
        }
    }
}
