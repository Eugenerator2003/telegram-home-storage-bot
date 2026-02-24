using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases
{
    public abstract class UserInput : Input
    {
        public int UserId { get; }

        protected UserInput(int userId)
        {
            UserId = userId;
        }
    }
}
