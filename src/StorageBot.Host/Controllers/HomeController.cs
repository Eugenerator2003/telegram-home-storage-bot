using StorageBot.Application.UseCases.StorageManipulation.InitializeStorage;
using StorageBot.Host.Localization;
using StorageBot.Host.Routing.Attributes.Actions.Required;
using StorageBot.Host.Routing.Attributes.Actions.Updates;
using StorageBot.Host.Routing.Attributes.Controllers;
using StorageBot.Host.Presentation;
using StorageBot.TelegramContracts.Account;
using StorageBot.TelegramContracts.Context;
using StorageBot.TelegramContracts.States.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using StorageBot.Host.Utilities;

namespace StorageBot.Host.Controllers
{
    [ControllerId(0)]
    public class HomeController
        (InitializeStorageUseCase initilializeStorageUseCase,
         IUserLocalizationService localizationService)
        : MenuBotController
    {

        [ActionId(0)]
        [Command("start")]
        public async Task Start()
        {
            await DeleteMenuIfExist();
            var userId = TelegramUserService.ResolveApplicationUserIdByChat(Context.Chat);
            var initializeStorageInput = new InitializeStorageInput(userId);
            initilializeStorageUseCase.Execute(initializeStorageInput);
            var menuCallback = GetCallbackDataForRequiredAction<MenuActionAttribute>();
            var localizationCallback = GetCallbackDataForRequiredAction<LocalizationMenuActionAttribute>();
            var message = new HtmlMessage(Localizer["View.Welcome"]);
            message.OpenKeyboard();
            message.AddRowWithCallbackQueryButton(Localizer["View.ToMenu"], menuCallback);
            message.AddRowWithCallbackQueryButton(Localizer["View.ToLocalizationMenu"], localizationCallback);
            message.CloseKeyboard();
            await BotClient.SendMessage(Context.Chat, message.ToString());
        }

        [ActionId(1)]
        [MenuAction]
        public async Task Menu()
        {
            await DeleteMenuIfExist();
            var storageUserId = await CurrentOwnershipStore.GetCurrentOwnership(TelegramUser);
            var telegramUser = TelegramUserService.ResolveTelegramUserByOwnershipId(storageUserId);

            var emptyCallback = new CallbackData(0, 0, 0);
            var localizationCallback = GetCallbackDataForRequiredAction<LocalizationMenuActionAttribute>();

            var message = new HtmlMessage();
            message.AddLine(Localizer["View.MenuTitle"]);
            message.AddText(Localizer["View.CurrentOwnershipInfo", telegramUser.Firstname, telegramUser.Lastname, telegramUser.Username]);
            message.OpenKeyboard();
            message.AddRowWithCallbackQueryButton(Localizer["View.MyContainers"], emptyCallback);
            message.AddRowWithCallbackQueryButton(Localizer["View.MyCategories"], emptyCallback);
            message.AddRowWithCallbackQueryButton(Localizer["View.MyQuantityType"], emptyCallback);
            message.AddRowWithCallbackQueryButton(Localizer["View.MoreAboutOnwrship"], emptyCallback);
            message.AddRowWithCallbackQueryButton(Localizer["View.MyStorages"], emptyCallback);
            message.AddRowWithCallbackQueryButton(Localizer["View.ToLocalizationMenu"], localizationCallback);
            message.CloseKeyboard();
            var sendedMessage = await BotClient.SendMessage(Context.Chat, message.ToString());
            await MenuStore.SetMenuMessageId(TelegramUser, sendedMessage.Id);
        }

        [ActionId(2)]
        [LocalizationMenuAction]
        public async Task LocalizationMenu()
        {            
            var message = new HtmlMessage();
            var menuCallback = GetCallbackDataForRequiredAction<MenuActionAttribute>();
            message.AddRowWithCallbackQueryButton(Localizer["View.ToMenu"], menuCallback);
            var localizations = localizationService.GetAvailableLocalizations();
            for (int i = 0; i < localizations.Length; i++)
            {
                var localizationCallback = GetCallbackDataForAction<HomeController>(nameof(SetLocalization), 1);
                localizationCallback.Values[0] = i;
                message.AddRowWithCallbackQueryButton(localizations[i], localizationCallback);
            }
            await StartOrEditMenu(message);
        }

        [ActionId(3)]
        public async Task SetLocalization()
        {
            var context = ExtractContext<CallbackQueryUpdateContext>();
            await AnswerCallbackQuery(context.QueryId);
            var callback = CallbackDataEncoder.Decode(context.Data);
            var index = callback.Values[0];
            var localizations = localizationService.GetAvailableLocalizations();
            if (index < 0 || index >= localizations.Length)
            {
                await ShowErrorMessage(Localizer["View.WrongLocalization"]);
            }
            else
            {
                localizationService.SetLocalization(TelegramUser, localizations[index]);
            }
        }
    }
}
