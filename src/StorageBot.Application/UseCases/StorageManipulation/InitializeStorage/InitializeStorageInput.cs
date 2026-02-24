using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.InitializeStorage
{
    public class InitializeStorageInput : UserInput
    {
        public InitializeStorageInput(int userId) 
            : base(userId)
        {
        }
    }
}
