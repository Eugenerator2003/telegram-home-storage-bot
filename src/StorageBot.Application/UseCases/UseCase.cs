using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
        protected readonly ILogger<UseCase<TInput, TOutput>> _logger;

        protected readonly IStringLocalizer _localizer;

        protected UseCase(ILogger<UseCase<TInput, TOutput>> logger, IStringLocalizer localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public abstract TOutput Execute(TInput input);
    }
}
