using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.TestUtilities.Factories
{
    public abstract class UseCaseFactory<T, TInput, TOuput> : IUseCaseFactory<T, TInput, TOuput>
        where T : UseCase<TInput, TOuput>
        where TInput : Input
        where TOuput : Output
    {
        public IStringLocalizer Localizer { get; set; } = new TestLocalizer();

        public ILogger<T> Logger { get; set; } = new FakeLogger<T>();

        public abstract T CreateUseCase();
    }
}
