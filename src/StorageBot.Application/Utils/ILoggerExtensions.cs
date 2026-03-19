using Microsoft.Extensions.Logging;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Utils
{
    public static class ILoggerExtensions
    {
        public static IDisposable? BeginUserScope(this ILogger logger, int userId)
        {
            return logger?.BeginScope(new Dictionary<string, object> { ["UserId"] = userId });
        }

        public static IDisposable? BeginStorageOwnershipScope(this ILogger logger, int storageOwnershipId)
        {
            return logger?.BeginScope(new Dictionary<string, object> { ["StorageOwnershipId"] = storageOwnershipId });
        }

        public static IDisposable? BeginTargetUserScope(this ILogger logger, int targetUserId)
        {
            return logger?.BeginScope(new Dictionary<string, object> { ["TagetUserId"] = targetUserId });
        }

        public static IDisposable? BeginContainerScope(this ILogger logger, int containerId)
        {
            return logger?.BeginScope(new Dictionary<string, object> { ["ContainerId"] = containerId });
        }

        public static IDisposable? BeginEntryCategoryScope(this ILogger logger, int entryCategoryId)
        {
            return logger?.BeginScope(new Dictionary<string, object> { ["EntryCategoryId"] = entryCategoryId });
        }

        public static IDisposable? BeginEntryQuantityTypeScope(this ILogger logger, int entryQuantityTypeScopeId)
        {
            return logger?.BeginScope(new Dictionary<string, object> { ["EntryQuantityTypeScopeId"] = entryQuantityTypeScopeId });
        }
    }
}
