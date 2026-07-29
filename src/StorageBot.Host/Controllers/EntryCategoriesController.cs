using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories.CreateEntryCategory;
using StorageBot.Application.UseCases.Storing.EntryCategories.DeleteEntryCategoryUseCase;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetEntryCategories;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetMainCategoryUseCase;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetSingleEntryCategory;
using StorageBot.Application.UseCases.Storing.EntryCategories.UpdateEntryCategoryNameUseCase;
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
    [ControllerId(4)]
    public class EntryCategoriesController
        (GetEntryCategoriesUseCase getEntryCategoriesUseCase,
         GetMainCategoryUseCase getMainCategoryUseCase,
         GetSingleEntryCategoryUseCase getSingleEntryCategoryUseCase,
         CreateEntryCategoryUseCase createEntryCategoryUseCase,
         UpdateEntryCategoryNameUseCase updateEntryCategoryNameUseCase,
         DeleteEntryCategoryUseCase deleteEntryCategoryUseCase) : MenuBotController
    {
        [ActionId(0)]
        public async Task CategoriesMenu()
        {
            var context = await AnswerCallbackQueryWithBuffer();
            var data = CallbackDataEncoder.Decode(context.Data);
            int page = 0;
            int? currentCategoryId = null;
            if (data.Values.Length != 0)
            {
                currentCategoryId = data.Values[0] > 0 ? data.Values[0] : null;
                if (data.Values.Length > 1)
                {
                    page = data.Values[1];
                }
            }
            var itemsInput = new GetEntryCategoriesInput(ApplicationUserId, CurrentOwnershipId, currentCategoryId, page);
            var itemsOutput = getEntryCategoriesUseCase.Execute(itemsInput);
            if (itemsOutput.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(itemsOutput.Message);
                return;
            }
            var message = new HtmlMessage(Localizer["View.EntryCategories.MenuTitle"]);
            if (itemsOutput.MainCategory != null)
            {
                message.AddLine(Localizer["View.EntryCategories.CurrentCategory", itemsOutput.MainCategory.Name]);
            }
            else
            {
                message.AddLine(Localizer["View.EntryCategories.NoCurrentCategory"]);
            }
            message.OpenKeyboard();
            var callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(CategoriesMenu), [0]);
            foreach (var dto in itemsOutput.SubCategories)
            {
                callback.Values[0] = dto.Id;
                message.AddRowWithCallbackQueryButton(dto.Name, callback);
            }
            if (currentCategoryId != null)
            {
                var mainCategoryInput = new GetMainCategoryInput(ApplicationUserId, CurrentOwnershipId, currentCategoryId.Value);
                var mainCategoryOutput = getMainCategoryUseCase.Execute(mainCategoryInput);
                callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(About), [currentCategoryId.Value]);
                message.AddRowWithCallbackQueryButton(Localizer["View.EntryCategories.About", itemsOutput.MainCategory!.Name], callback);
                if (mainCategoryOutput.Result == Output.OutputResult.FatalFail)
                {
                    await ShowErrorMessage(mainCategoryOutput.Message);
                    return;
                }
                else if (mainCategoryOutput.Result == Output.OutputResult.Success)
                {
                    callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(CategoriesMenu), [mainCategoryOutput.MainCategory.Id]);
                    message.AddRowWithCallbackQueryButton(Localizer["View.EntryCategories.ToMainCategory"], callback);
                }
                else
                {
                    callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(CategoriesMenu));
                    message.AddRowWithCallbackQueryButton(Localizer["View.EntryCategories.ToRoot"], callback);
                }
            }
            callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(CreateReadMainCategory), [currentCategoryId.HasValue ? currentCategoryId.Value : -1]);
            message.AddRowWithCallbackQueryButton(Localizer["View.EntryCategories.Create"], callback);
            message.OpenRow();
            callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(CategoriesMenu), [currentCategoryId ?? -1, 0]);
            if (itemsOutput.Pagination.HasPrevious())
            {
                callback.Values[1] = page - 1;
                message.AddCallbackQueryButton(Localizer["View.PreviousPage"], callback);
            }
            if (itemsOutput.Pagination.HasNext())
            {
                callback.Values[1] = page + 1;
                message.AddCallbackQueryButton(Localizer["View.NextPage"], callback);
            }
            message.CloseRow();
            if (currentCategoryId == null)
            {
                callback = GetCallbackDataForAction<HomeController>(nameof(HomeController.Menu));
                message.AddRowWithCallbackQueryButton(Localizer["View.BackToMenu"], callback);
            }
            message.CloseKeyboard();
            await StartOrEditMenu(message);
        }

        [ActionId(1)]
        public async Task About()
        {
            var context = await AnswerCallbackQueryWithBuffer();
            var callback = CallbackDataEncoder.Decode(context.Data);
            var categoryId = callback.Values[0];
            var getInput = new GetSingleEntryCategoryInput(ApplicationUserId, CurrentOwnershipId, categoryId);
            var outputInput = getSingleEntryCategoryUseCase.Execute(getInput);
            if (outputInput.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(outputInput.Message);
                return;
            }
            var message = new HtmlMessage(Localizer["View.EntryCategories.About", outputInput.EntryCategory.Name]);
            message.OpenKeyboard();
            callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(EditSetCategory), [categoryId]);
            message.AddRowWithCallbackQueryButton(Localizer["View.EntryCategories.Edit"], callback);
            callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(Delete), [categoryId]);
            message.AddRowWithCallbackQueryButton(Localizer["View.EntryCategories.Delete"], callback);
            callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(CategoriesMenu), [categoryId]);
            message.AddRowWithCallbackQueryButton(Localizer["View.EntryCategories.BackToItems"], callback);
            message.CloseKeyboard();
            await StartOrEditMenu(message);
        }

        [ActionId(2)]
        public async Task CreateReadMainCategory()
        {
            var context = await AnswerCallbackQuery();
            var data = CallbackDataEncoder.Decode(context.Data);
            var categoryId = data.Values[0] > 0 ? data.Values[0] : 0;
            if (categoryId > 0)
            {
                string[] lines = [categoryId.ToString()];
                await InputLinesStore.SetLines(TelegramUser, lines);
            }
            SetRedirect<EntryCategoriesController>(nameof(CreateInputName));
        }

        [ActionId(3)]
        public async Task CreateInputName()
        {
            var message = new HtmlMessage(Localizer["View.EntryCategories.InputName"]);
            await SetInputState<EntryCategoriesController>(nameof(CreateReadName));
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
                SetRedirect<EntryCategoriesController>(nameof(CreateInputName));
            }
            else
            {
                int? mainCategoryId = null;
                var lines = await InputLinesStore.GetLines(TelegramUser);
                if (lines.Length > 0)
                {
                    await InputLinesStore.ClearLines(TelegramUser);
                    mainCategoryId = int.Parse(lines[0]);
                }
                var input = new CreateEntryCategoryInput(ApplicationUserId, CurrentOwnershipId, mainCategoryId, name);
                var output = createEntryCategoryUseCase.Execute(input);
                if (output.Result != Output.OutputResult.Success)
                {
                    await ShowErrorMessage(output.Message);
                }
                else
                {
                    int[] values = [mainCategoryId ?? -1];
                    SetRedirectWithCallbackData<EntryCategoriesController>(nameof(CategoriesMenu), values);
                }
            }
        }

        [ActionId(5)]
        public async Task EditSetCategory()
        {
            var context = await AnswerCallbackQuery();
            var categoryId = CallbackDataEncoder.Decode(context.Data).Values[0];
            string[] lines = [categoryId.ToString()];
            await InputLinesStore.SetLines(TelegramUser, lines);
            await SetInputState<EntryCategoriesController>(nameof(EditInputName));
        }

        [ActionId(6)]
        public async Task EditInputName()
        {
            var message = new HtmlMessage(Localizer["View.EntryCategories.InputName"]);
            await SetInputState<EntryCategoriesController>(nameof(EditReadName));
            await StartOrEditMenu(message);
        }

        [ActionId(7)]
        public async Task EditReadName()
        {
            var context = ExtractContext<MessageUpdateContext>();
            var name = context.Message.Text;
            await DeleteMessage(context.Message.Id);
            if (string.IsNullOrEmpty(name))
            {
                var message = new HtmlMessage(Localizer["View.EmptyInputString"]);
                _ = SendMessageWithDelete(message, 1000);
                SetRedirect<EntryCategoriesController>(nameof(EditInputName));
            }
            else
            {
                var lines = await InputLinesStore.GetLines(TelegramUser);
                await InputLinesStore.ClearLines(TelegramUser);
                var categoryId = int.Parse(lines[0]);
                var input = new UpdateEntryCategoryNameInput(ApplicationUserId, CurrentOwnershipId, categoryId, name);
                var output = updateEntryCategoryNameUseCase.Execute(input);
                if (output.Result != Output.OutputResult.Success)
                {
                    await ShowErrorMessage(output.Message);
                }
                else
                {
                    SetRedirectWithCallbackData<EntryCategoriesController>(nameof(About), [categoryId]);
                }
            }
        }

        [ActionId(8)]
        public async Task Delete()
        {
            var context = await AnswerCallbackQuery();
            var categoryId = CallbackDataEncoder.Decode(context.Data).Values[0];
            var input = new GetSingleEntryCategoryInput(ApplicationUserId, CurrentOwnershipId, categoryId);
            var output = getSingleEntryCategoryUseCase.Execute(input);
            if (output.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(output.Message);
            }
            else
            {
                var message = new HtmlMessage(Localizer["View.EntryCategories.About", output.EntryCategory.Name]);
                message.OpenKeyboard();
                var callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(Delete), [categoryId]);
                message.AddRowWithCallbackQueryButton(Localizer["View.ConfirmDelete"], callback);
                callback = GetCallbackDataForAction<EntryCategoriesController>(nameof(About), [categoryId]);
                message.AddRowWithCallbackQueryButton(Localizer["View.CancelDelete"], callback);
                message.CloseKeyboard();
                await StartOrEditMenu(message);
            }
        }

        [ActionId(9)]
        public async Task ConfirmDelete()
        {
            var context = await AnswerCallbackQuery();
            var categoryId = CallbackDataEncoder.Decode(context.Data).Values[0];
            var input = new DeleteEntryCategoryInput(ApplicationUserId, CurrentOwnershipId, categoryId);
            var output = deleteEntryCategoryUseCase.Execute(input);
            if (output.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(output.Message);
            }
            else
            {
                SetRedirectWithCallbackData<EntryCategoriesController>(nameof(CategoriesMenu));
            }
        }
    }
}
