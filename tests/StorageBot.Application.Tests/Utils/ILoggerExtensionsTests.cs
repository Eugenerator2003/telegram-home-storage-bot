using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using StorageBot.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.Utils
{
    [TestClass]
    public class ILoggerExtensionsTests
    {
        private static void AssertScope(Func<ILogger, IDisposable?> loggerScopeDelegate,
                                        string[] expectedScopes)
        {
            var logger = new FakeLogger();
            using (loggerScopeDelegate(logger))
            {
                logger.LogInformation("Test log");
            }
            var scopes = logger.Collector
                               .GetSnapshot()[0]
                               .Scopes
                               .Select(s => ((Dictionary<string, object>)s!).Keys)
                               .SelectMany(s => s)
                               .ToArray();
            foreach(var scope in expectedScopes)
            {
                Assert.Contains(scope, scopes);
            }
        }

        [TestMethod]
        public void BeginUserScopeTest()
        {
            AssertScope(
                logger => logger.BeginUserScope(1),
                ["UserId"]
                );
        }

        [TestMethod]
        public void BeginStorageOwnershipScopeTest()
        {
            AssertScope(
                logger => logger.BeginStorageOwnershipScope(1),
                ["StorageOwnershipId"]
                );
        }

        [TestMethod]
        public void BeginTargetUserScopeTest()
        {
            AssertScope(
                logger => logger.BeginTargetUserScope(1),
                ["TargetUserId"]
                );
        }

        [TestMethod]
        public void BeginContainerScopeTest()
        {
            AssertScope(
                logger => logger.BeginContainerScope(1),
                ["ContainerId"]
                );
        }

        [TestMethod]
        public void BeginEntryCategoryScopeTest()
        {
            AssertScope(
                logger => logger.BeginEntryCategoryScope(1),
                ["EntryCategoryId"]
                );
        }

        [TestMethod]
        public void BeginEntryQuantityTypeScopeTest()
        {
            AssertScope(
                logger => logger.BeginEntryQuantityTypeScope(1),
                ["EntryQuantityTypeId"]
                );
        }

        [TestMethod]
        public void BeginContainerEntryScopeTest()
        {
            AssertScope(
                logger => logger.BeginContainerEntryScope(1),
                ["ContainerEntryId"]
                );
        }
    }
}
