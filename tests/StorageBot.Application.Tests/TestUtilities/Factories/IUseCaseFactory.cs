using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.TestUtilities.Factories
{
    public interface IUseCaseFactory<T, TInput, TOuput> 
        where T : UseCase<TInput, TOuput>
        where TInput : Input
        where TOuput : Output
    {
        T CreateUseCase();

        IStringLocalizer Localizer { get; set; }

        ILogger<T> Logger { get; set; }
    }
}
