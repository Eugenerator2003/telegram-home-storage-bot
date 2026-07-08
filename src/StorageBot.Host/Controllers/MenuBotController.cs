using StorageBot.Host.Presentation;
using StorageBot.TelegramContracts.Account;
using StorageBot.TelegramContracts.States.Chat;
using StorageBot.TelegramContracts.States.ControllerState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;

namespace StorageBot.Host.Controllers
{
    public abstract class MenuBotController : BaseBotController
    {
        protected IMenuMessageStore MenuStore { get; private set; } = null!;
        protected IUserInputLinesStore InputLinesStore { get; private set; } = null!;
        protected IUserInputStateStore InputStateStore { get; private set; } = null!;
        protected ICurrentOwnershipStore CurrentOwnershipStore { get; private set; } = null!;
        protected ITelegramUserService TelegramUserService { get; private set; } = null!;

        protected ITelegramUser TelegramUser
        {
            get
            {
                _user ??= TelegramUserService.GetUser(Context.Chat);
                return _user;
            }
        }

        private ITelegramUser? _user;

        public override void SetDependencies(ControllerDependencies dependencies)
        {
            base.SetDependencies(dependencies);
            MenuStore = dependencies.MenuStore;
            InputLinesStore = dependencies.LinesStore;
            InputStateStore = dependencies.StateStore;
            CurrentOwnershipStore = dependencies.CurrentOwnershipStore;
            TelegramUserService = dependencies.TelegramUserService;
        }

        protected async Task SetInputState<TController>(string actionName) where TController : BaseBotController
        {
            var navigation = Navigation.GetAction<TController>(actionName);
            var state = new InputState(navigation.ControllerId, navigation.ActionId);
            await InputStateStore.SetState(TelegramUser, state);
        }

        protected async Task StartOrEditMenu(HtmlMessage message)
        {
            var menuId = await MenuStore.GetMenuMessageId(TelegramUser);
            if (menuId == -1)
            {
                var sendedMessageId = await SendMessage(message);
                await MenuStore.SetMenuMessageId(TelegramUser, sendedMessageId);
            }
            else
            {
                await EditMessage(menuId, message);
            }
        }

        protected async Task DeleteMenuIfExist()
        {
            var menuId = await MenuStore.GetMenuMessageId(TelegramUser);
            if (menuId != -1)
            {
                await DeleteMessage(menuId);
            }
        }
    }
}
