using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.StorageManipulation.GetAccessibleStorages;
using StorageBot.Application.UseCases.StorageManipulation.GetUserOwnership;
using StorageBot.Host.Presentation;
using StorageBot.Host.Routing.Attributes.Actions.Updates;
using StorageBot.Host.Routing.Attributes.Controllers;
using StorageBot.Host.Utilities;
using StorageBot.TelegramContracts.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Controllers
{
    [ControllerId(2)]
    public class StoragesController
        (GetAccessibleStoragesUseCase getAccessibleStoragesUseCase,
         GetUserOwnershipUseCase getUserOwnershipUseCase)
        : MenuBotController
    {
        [ActionId(0)]
        public async Task StoragesMenu()
        {
            _ = AnswerCallbackQueryWithContext();
            var input = new GetAccessibleStoragesInput(ApplicationUserId);
            var output = getAccessibleStoragesUseCase.Execute(input);
            var message = new HtmlMessage(Localizer["View.Storages.MenuTitle"]);
            message.OpenKeyboard();
            foreach (var userId in output.UserIds)
            {
                string text;
                if (userId == ApplicationUserId)
                {
                    text = Localizer["View.Storages.Personal"];
                }
                else
                {
                    var user = TelegramUserService.ResolveTelegramUserByUserId(userId);
                    text = $"@{user.Username}";
                }
                var callback = GetCallbackDataForAction<StoragesController>(nameof(ChooseStorage), 1);
                callback.Values[0] = userId;
                message.AddRowWithCallbackQueryButton(text, callback);
            }
            message.CloseKeyboard();
            await StartOrEditMenu(message);
        }

        [ActionId(1)]
        public async Task ChooseStorage()
        {
            var context = await AnswerCallbackQueryWithContext();
            var callback = CallbackDataEncoder.Decode(context.Data);
            var userId = callback.Values[0];
            var storagesInput = new GetAccessibleStoragesInput(ApplicationUserId);
            var storagesOutput = getAccessibleStoragesUseCase.Execute(storagesInput);
            if (!storagesOutput.UserIds.Contains(userId))
            {
                await ShowErrorMessage(Localizer["View.Storages.InvalidStorageOption"]);
            }
            else
            {
                var ownershipInput = new GetUserOwnershipInput(ApplicationUserId);
                var ownershipOutput = getUserOwnershipUseCase.Execute(ownershipInput);
                if (ownershipOutput.Result == Output.OutputResult.Success)
                {
                    await CurrentOwnershipStore.SetCurrentOwnership(TelegramUser, ownershipOutput.OwnershipId);
                    SetRedirect<HomeController>(nameof(HomeController.Menu));
                }
                else
                {
                    await ShowErrorMessage(ownershipOutput.Message);
                }
            }
        }

        [ActionId(2)]
        public async Task AboutThisStorage()
        {
            _ = AnswerCallbackQueryWithContext();
            await SendMessageWithDelete(new HtmlMessage(Localizer["View.Placeholder"]), 1000);   
        }
    }
}
