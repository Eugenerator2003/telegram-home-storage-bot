using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.GetAccessibleStorages
{
    public class GetAccessibleStoragesInput : UserInput
    {
        public GetAccessibleStoragesInput(int userId) 
            : base(userId)
        {
        }
    }
}
