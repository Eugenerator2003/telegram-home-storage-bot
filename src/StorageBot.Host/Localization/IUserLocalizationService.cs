using StorageBot.TelegramContracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Localization
{
    public interface IUserLocalizationService
    {
        void SetLocalization(ITelegramUser telegramUser, string localizationName);

        string[] GetAvailableLocalizations();
    }
}
