using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases
{
    public abstract class UseCase<TInput, TOutput> 
        where TInput : Input 
        where TOutput : Output
    {
        public abstract TOutput Execute(TInput input);
    }
}
