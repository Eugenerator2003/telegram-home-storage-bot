using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.QuantityTypes.AddEntryQuantityType;
using StorageBot.Application.UseCases.Storing.QuantityTypes.DeleteEntryQuantityType;
using StorageBot.Application.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes;
using StorageBot.Application.UseCases.Storing.QuantityTypes.GetSingleEntryQuantityType;
using StorageBot.Application.UseCases.Storing.QuantityTypes.UpdateEntryQuantityTypeName;
using StorageBot.Host.Presentation;
using StorageBot.Host.Routing.Attributes.Actions.Updates;
using StorageBot.Host.Routing.Attributes.Controllers;
using StorageBot.Host.Utilities;
using StorageBot.TelegramContracts.Context;
using StorageBot.TelegramContracts.States.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Controllers
{
    [ControllerId(3)]
    public class QuantityTypesController
        (GetEntryQuantityTypesUseCase getEntryQuantityTypesUseCase,
         CreateEntryQuantityTypeUseCase createEntryQuantityTypeUseCase,
         GetSingleEntryQuantityTypeUseCase getSingleEntryQuantityTypeUseCase,
         UpdateEntryQuantityTypeNameUseCase updateEntrtyQuantityTypeNameUseCase,
         DeleteEntryQuantityTypeUseCase deleteEntryQuantityTypeUseCase)
        : MenuBotController
    {
        [ActionId(0)]
        public async Task QuantityTypesMenu()
        {
            var context = await AnswerCallbackQueryWithBuffer();
            var data = CallbackDataEncoder.Decode(context.Data);
            int page = 0;
            if (data.Values.Length != 0)
            {
                page = data.Values[0];
            }
            var input = new GetEntryQuantityTypesInput(ApplicationUserId, CurrentOwnershipId, page);
            var output = getEntryQuantityTypesUseCase.Execute(input);
            if (output.Result == Output.OutputResult.FatalFail)
            {
                await ShowErrorMessage(output.Message);
                return;
            }
            var message = new HtmlMessage(Localizer["View.QuantityTypes.MenuTitle"]);
            message.OpenKeyboard();
            foreach (var item in output.EntryQuantityTypes)
            {
                data = GetCallbackDataForAction<QuantityTypesController>(nameof(About), 2);
                data.Values[0] = item.Id;
                data.Values[1] = page;
                message.AddRowWithCallbackQueryButton(item.UnitName, data);
            }
            message.OpenRow();
            if (output.Pagination.HasPrevious())
            {
                data = GetCallbackDataForAction<QuantityTypesController>(nameof(QuantityTypesMenu), 1);
                data.Values[0] = page - 1;
                message.AddCallbackQueryButton(Localizer["View.PreviousPage"], data);
            }
            data = GetCallbackDataForAction<QuantityTypesController>(nameof(CreateSetState));
            message.AddCallbackQueryButton(Localizer["View.QuantityTypes.Create"], data);
            if (output.Pagination.HasNext())
            {
                data = GetCallbackDataForAction<QuantityTypesController>(nameof(QuantityTypesMenu), 1);
                data.Values[0] = page + 1;
                message.AddCallbackQueryButton(Localizer["View.NextPage"], data);
            }
            message.CloseRow();
            data = GetCallbackDataForAction<HomeController>(nameof(HomeController.Menu));
            message.AddRowWithCallbackQueryButton(Localizer["View.BackToMenu"], data);
            await StartOrEditMenu(message);
        }

        [ActionId(1)]
        public async Task About()
        {
            var context = await AnswerCallbackQueryWithBuffer();
            var data = CallbackDataEncoder.Decode(context.Data);
            var quantityTypeId = data.Values[0];
            int page = 0;
            if (data.Values.Length > 1)
            {
                page = data.Values[1];
            }
            var input = new GetSingleEntryQuantityTypeInput(ApplicationUserId, CurrentOwnershipId, quantityTypeId);
            var output = getSingleEntryQuantityTypeUseCase.Execute(input);
            if (output.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(output.Message);
                return;
            }
            var message = new HtmlMessage(Localizer["View.QuantityType.About", output.QuantityTypeDTO.UnitName, output.QuantityTypeDTO.IsInteger]);
            message.OpenKeyboard();
            data = GetCallbackDataForAction<QuantityTypesController>(nameof(EditSetQuantityType), 1);
            data.Values[0] = quantityTypeId;
            message.AddRowWithCallbackQueryButton(Localizer["View.QuantityTypes.Edit"], data);
            data = GetCallbackDataForAction<QuantityTypesController>(nameof(Delete), 1);
            data.Values[0] = quantityTypeId;
            message.AddRowWithCallbackQueryButton(Localizer["View.QuantityTypes.Delete"], data);
            data = GetCallbackDataForAction<QuantityTypesController>(nameof(QuantityTypesMenu), 1);
            data.Values[0] = page;
            message.AddRowWithCallbackQueryButton(Localizer["View.QuantityTypes.BackToItems"], data);
            message.CloseKeyboard();
            await StartOrEditMenu(message);
        }

        [ActionId(2)]
        public async Task CreateSetState()
        {
            await AnswerCallbackQuery();
            SetRedirect<QuantityTypesController>(nameof(CreateInputName));
        }

        [ActionId(3)]
        public async Task CreateInputName()
        {
            var message = new HtmlMessage(Localizer["View.QuantityTypes.InputName"]);
            await SetInputState<QuantityTypesController>(nameof(CreateReadName));
            await StartOrEditMenu(message);
        }

        [ActionId(4)]
        public async Task CreateReadName()
        {
            var context = ExtractContext<MessageUpdateContext>();
            await DeleteMessage(context.Message.Id);
            var name = context.Message.Text;
            if (string.IsNullOrEmpty(name))
            {
                var message = new HtmlMessage(Localizer["View.EmptyInputString"]);
                _ = SendMessageWithDelete(message, 1000);
                SetRedirect<QuantityTypesController>(nameof(CreateInputName));
            }
            else
            {
                await InputLinesStore.SetLines(TelegramUser, [name]);
                var message = new HtmlMessage(Localizer["View.QuantityTypes.InputIsInteger"]);
                message.OpenKeyboard();
                var data = GetCallbackDataForAction<QuantityTypesController>(nameof(CreateReadIsInteger), 1);
                data.Values[0] = 1;
                message.AddCallbackQueryButton(Localizer["View.QuantityTypes.IsInteger"], data);
                data = GetCallbackDataForAction<QuantityTypesController>(nameof(CreateReadIsInteger), 1);
                data.Values[0] = 0;
                message.AddCallbackQueryButton(Localizer["View.QuantityTypes.IsNotInteger"], data);
                message.CloseKeyboard();
                await StartOrEditMenu(message);
            }
        }

        [ActionId(5)]
        public async Task CreateReadIsInteger()
        {
            var context = await AnswerCallbackQuery();
            var data = CallbackDataEncoder.Decode(context.Data);
            var isInteger = data.Values[0] == 1;
            var lines = await InputLinesStore.GetLines(TelegramUser);
            var unitName = lines[0];
            var input = new CreateEntryQuantityTypeInput(ApplicationUserId, CurrentOwnershipId, isInteger, unitName);
            var output = createEntryQuantityTypeUseCase.Execute(input);
            if (output.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(output.Message);
            }
            else
            {
                await InputLinesStore.ClearLines(TelegramUser);
                SetRedirect<QuantityTypesController>(nameof(QuantityTypesMenu));
            }
        }

        [ActionId(6)]
        public async Task EditSetQuantityType()
        {
            var context = await AnswerCallbackQuery();
            var data = CallbackDataEncoder.Decode(context.Data);
            var entryQuantityId = data.Values[0];
            await InputLinesStore.SetLines(TelegramUser, [entryQuantityId.ToString()]);
            SetRedirect<QuantityTypesController>(nameof(EditInputName));
        }

        [ActionId(7)]
        public async Task EditInputName()
        {
            var message = new HtmlMessage(Localizer["View.QuantityTypes.InputName"]);
            await SetInputState<QuantityTypesController>(nameof(EditReadName));
            await StartOrEditMenu(message);
        }

        [ActionId(8)]
        public async Task EditReadName()
        {
            var context = ExtractContext<MessageUpdateContext>();
            var name = context.Message.Text;
            await DeleteMessage(context.Message.Id);
            if (string.IsNullOrEmpty(name))
            {
                var message = new HtmlMessage(Localizer["View.EmptyInputString"]);
                _ = SendMessageWithDelete(message, 1000);
                SetRedirect<QuantityTypesController>(nameof(EditInputName));
            }
            else
            {
                var lines = await InputLinesStore.GetLines(TelegramUser);
                await InputLinesStore.ClearLines(TelegramUser);
                var entryQuantityId = int.Parse(lines[0]);
                var input = new UpdateEntryQuantityTypeNameInput(ApplicationUserId, CurrentOwnershipId, entryQuantityId, name);
                var output = updateEntrtyQuantityTypeNameUseCase.Execute(input);
                if (output.Result != Output.OutputResult.Success)
                {
                    await ShowErrorMessage(output.Message);
                }
                else
                {
                    SetRedirectWithCallbackData<QuantityTypesController>(nameof(About), [entryQuantityId]);
                }
            }
        }

        [ActionId(9)]
        public async Task Delete()
        {
            var context = await AnswerCallbackQuery();
            var data = CallbackDataEncoder.Decode(context.Data);
            var quantityTypeId = data.Values[0];
            var input = new GetSingleEntryQuantityTypeInput(ApplicationUserId, CurrentOwnershipId, quantityTypeId);
            var output = getSingleEntryQuantityTypeUseCase.Execute(input);
            if (output.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(output.Message);
            }
            else
            {
                var message = new HtmlMessage(Localizer["View.QuantityType.About", output.QuantityTypeDTO.UnitName, output.QuantityTypeDTO.IsInteger]);
                message.OpenKeyboard();
                var callback = GetCallbackDataForAction<QuantityTypesController>(nameof(ConfirmDelete), 1);
                callback.Values[0] = quantityTypeId;
                message.AddRowWithCallbackQueryButton(Localizer["View.ConfirmDelete"], callback);
                callback = GetCallbackDataForAction<QuantityTypesController>(nameof(About), 1);
                callback.Values[0] = quantityTypeId;
                message.AddRowWithCallbackQueryButton(Localizer["View.CancelDelete"], callback);
                message.CloseKeyboard();
                await StartOrEditMenu(message);
            }
        }

        [ActionId(10)]
        public async Task ConfirmDelete()
        {
            var context = await AnswerCallbackQuery();
            var data = CallbackDataEncoder.Decode(context.Data);
            var quantityTypeId = data.Values[0];
            var getInput = new GetSingleEntryQuantityTypeInput(ApplicationUserId, CurrentOwnershipId, quantityTypeId);
            var getOutput = getSingleEntryQuantityTypeUseCase.Execute(getInput);
            if (getOutput.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(getOutput.Message);
            }
            else
            {
                var deleteInput = new DeleteEntryQuantityTypeInput(ApplicationUserId, CurrentOwnershipId, quantityTypeId);
                var deleteOutput = deleteEntryQuantityTypeUseCase.Execute(deleteInput);
                if (deleteOutput.Result != Output.OutputResult.Success)
                {
                    await ShowErrorMessage(deleteOutput.Message);
                }
                else
                {
                    SetRedirectWithCallbackData<QuantityTypesController>(nameof(QuantityTypesMenu));
                }
            }
        }
    }
}
