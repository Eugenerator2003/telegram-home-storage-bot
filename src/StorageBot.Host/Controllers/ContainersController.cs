using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.ContainerEntries.DeleteContainerEntry;
using StorageBot.Application.UseCases.Storing.Containers.CreateContainer;
using StorageBot.Application.UseCases.Storing.Containers.DeleteContainer;
using StorageBot.Application.UseCases.Storing.Containers.GetContainers;
using StorageBot.Application.UseCases.Storing.Containers.GetOuterContainer;
using StorageBot.Application.UseCases.Storing.Containers.GetSingleContainer;
using StorageBot.Application.UseCases.Storing.Containers.UpdateContainerName;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetSingleEntryCategory;
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
    [ControllerId(5)]
    public class ContainersController
        (GetContainersUseCase getContainersUseCase,
         GetOuterContainerUseCase getOuterContainerUseCase,
         GetSingleContainerUseCase getSingleContainerUseCase,
         CreateContainerUseCase createContainerUseCase,
         UpdateContainerNameUseCase updateContainerNameUseCase,
         DeleteContainerUseCase deleteContainerUseCase) : MenuBotController
    {
        [ActionId(0)]
        public async Task ContainersMenu()
        {
            var context = await AnswerCallbackQueryWithBuffer();
            var callback = CallbackDataEncoder.Decode(context.Data);
            int page = 0;
            int? containerId = null;
            if (callback.Values.Length > 0)
            {
                containerId = containerId.HasValue ? containerId.Value : null;
                if (callback.Values.Length > 1)
                {
                    page = callback.Values[1];
                }
            }
            var itemsInput = new GetContainersInput(ApplicationUserId, CurrentOwnershipId, page, containerId);
            var itemsOutput = getContainersUseCase.Execute(itemsInput);
            if (itemsOutput.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(itemsOutput.Message);
                return;
            }
            var message = new HtmlMessage(Localizer["View.Containers.MenuTitle"]);
            if (itemsOutput.OuterContainer != null)
            {
                message.AddLine(Localizer["View.Containers.CurrentContainer", itemsOutput.OuterContainer.Name]);
            }
            else
            {
                message.AddLine(Localizer["View.Containers.NoCurrentContainer"]);
            }
            message.OpenKeyboard();
            callback = GetCallbackDataForAction<ContainersController>(nameof(ContainersMenu), [0]);
            foreach (var item in itemsOutput.InnerContainers)
            {
                callback.Values[0] = item.Id;
                message.AddRowWithCallbackQueryButton(item.Name, callback);
            }
            if (containerId != null)
            {
                var outerContainerInput = new GetOuterContainerInput(ApplicationUserId, CurrentOwnershipId, containerId.Value);
                var outerContainerOutput = getOuterContainerUseCase.Execute(outerContainerInput);
                callback = GetCallbackDataForAction<ContainersController>(nameof(About), [containerId.Value]);
                message.AddRowWithCallbackQueryButton(Localizer["View.Containers.About", itemsOutput.OuterContainer!.Name], callback);
                if (outerContainerOutput.Result != Output.OutputResult.Success)
                {
                    await ShowErrorMessage(outerContainerOutput.Message);
                    return;
                }
                else if (outerContainerOutput.Result == Output.OutputResult.Success)
                {
                    callback = GetCallbackDataForAction<ContainersController>(nameof(ContainersMenu), [outerContainerOutput.OuterContainer.Id, page]);
                    message.AddRowWithCallbackQueryButton(Localizer["View.Containers.ToOuterContainer"], callback);
                }
                else
                {
                    callback = GetCallbackDataForAction<ContainersController>(nameof(ContainersMenu));
                    message.AddRowWithCallbackQueryButton(Localizer["View.Containers.ToRoot"], callback);
                }
            }
            callback = GetCallbackDataForAction<ContainersController>(nameof(CreateReadOuterContainer), [containerId.HasValue ? containerId.Value : -1]);
            message.AddRowWithCallbackQueryButton(Localizer["View.Containers.Create"], callback);
            message.OpenRow();
            callback = GetCallbackDataForAction<ContainersController>(nameof(ContainersMenu), [containerId ?? -1, page]);
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
            callback = GetCallbackDataForAction<HomeController>(nameof(HomeController.Menu));
            message.AddRowWithCallbackQueryButton(Localizer["View.BackToMenu"], callback);
            message.CloseKeyboard();
            await StartOrEditMenu(message);
        }

        [ActionId(1)]
        public async Task About()
        {
            var context = await AnswerCallbackQueryWithBuffer();
            var callback = CallbackDataEncoder.Decode(context.Data);
            int containerId = callback.Values[0];
            var getInput = new GetSingleContainerInput(ApplicationUserId, CurrentOwnershipId, containerId);
            var getOutput = getSingleContainerUseCase.Execute(getInput);
            if (getOutput.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(getOutput.Message);
                return;
            }
            var message = new HtmlMessage(Localizer["View.Container.About", getOutput.Container.Name]);
            message.OpenKeyboard();
            callback = GetCallbackDataForAction<ContainersController>(nameof(EditSetContainer), [containerId]);
            message.AddRowWithCallbackQueryButton(Localizer["View.Containers.Edit"], callback);
            callback = GetCallbackDataForAction<ContainersController>(nameof(Delete), [containerId]);
            message.AddRowWithCallbackQueryButton(Localizer["View.Containers.Delete"], callback);
            callback = GetCallbackDataForAction<ContainersController>(nameof(ContainersMenu), [containerId]);
            message.AddRowWithCallbackQueryButton(Localizer["View.Containers.BackToItems"], callback);
            message.CloseKeyboard();
            await StartOrEditMenu(message);
        }

        [ActionId(2)]
        public async Task CreateReadOuterContainer()
        {
            var context = await AnswerCallbackQuery();
            var outerContainerId = CallbackDataEncoder.Decode(context.Data).Values[0];
            if (outerContainerId > 0)
            {
                await InputLinesStore.SetLines(TelegramUser, [outerContainerId.ToString()]);
            }
            SetRedirect<ContainersController>(nameof(CreateInputName));
        }

        [ActionId(3)]
        public async Task CreateInputName()
        {
            var message = new HtmlMessage(Localizer["View.Containers.InputName"]);
            await SetInputState<EntryCategoriesController>(nameof(CreateReadName));
            await StartOrEditMenu(message);
        }

        [ActionId(4)]
        public async Task CreateReadName()
        {
            var context = ExtractContext<MessageUpdateContext>();
            var name = context.Message.Text;
            if (string.IsNullOrEmpty(name))
            {

                var message = new HtmlMessage(Localizer["View.EmptyInputString"]);
                _ = SendMessageWithDelete(message, 1000);
                SetRedirect<ContainersController>(nameof(CreateInputName));
            }
            else
            {
                int? outerContainerId = null;
                var lines = await InputLinesStore.GetLines(TelegramUser);
                if (lines.Length > 0)
                {
                    await InputLinesStore.ClearLines(TelegramUser);
                    outerContainerId = int.Parse(lines[0]);
                }
                var createInput = new CreateContainerInput(ApplicationUserId, CurrentOwnershipId, outerContainerId, name);
                var createOutput = createContainerUseCase.Execute(createInput);
                if (createOutput.Result != Output.OutputResult.Success)
                {
                    await ShowErrorMessage(createOutput.Message);
                }
                else
                {
                    SetRedirectWithCallbackData<ContainersController>(nameof(ContainersMenu), [outerContainerId ?? -1]);
                }
            }
        }

        [ActionId(5)]
        public async Task EditSetContainer()
        {
            var context = await AnswerCallbackQuery();
            var container  = CallbackDataEncoder.Decode(context.Data).Values[0];
            await InputLinesStore.SetLines(TelegramUser, [container.ToString()]);
            SetRedirect<ContainersController>(nameof(EditInputName));
        }

        [ActionId(6)]
        public async Task EditInputName()
        {
            var message = new HtmlMessage(Localizer["View.Containers.InputName"]);
            await SetInputState<ContainersController>(nameof(EditReadName));
            await StartOrEditMenu(message);
        }

        [ActionId(7)]
        public async Task EditReadName()
        {
            var context = ExtractContext<MessageUpdateContext>();
            var name = context.Message.Text;
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
                var containerId = int.Parse(lines[0]);
                var input = new UpdateContainerNameInput(ApplicationUserId, CurrentOwnershipId, containerId, name);
                var output = updateContainerNameUseCase.Execute(input);
                if (output.Result != Output.OutputResult.Success)
                {
                    await ShowErrorMessage(output.Message);
                }
                else
                {
                    SetRedirectWithCallbackData<ContainersController>(nameof(About), [containerId]);
                }
            }
        }

        [ActionId(8)]
        public async Task Delete()
        {
            var context = await AnswerCallbackQuery();
            var containerId = CallbackDataEncoder.Decode(context.Data).Values[0];
            var input = new GetSingleContainerInput(ApplicationUserId, CurrentOwnershipId, containerId);
            var output = getSingleContainerUseCase.Execute(input);
            if (output.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(output.Message);
            }
            else
            {
                var message = new HtmlMessage(Localizer["View.Container.About", output.Container.Name]);
                message.OpenKeyboard();
                var callback = GetCallbackDataForAction<ContainersController>(nameof(DeleteConfirm), [containerId]);
                message.AddRowWithCallbackQueryButton(Localizer["View.ConfirmDelete"], callback);
                callback = GetCallbackDataForAction<ContainersController>(nameof(About), [containerId]);
                message.AddRowWithCallbackQueryButton(Localizer["View.CancelDelete"], callback);
                message.CloseKeyboard();
                await StartOrEditMenu(message);
            }
        }

        [ActionId(9)]
        public async Task DeleteConfirm()
        {
            var context = await AnswerCallbackQuery();
            var containerId = CallbackDataEncoder.Decode(context.Data).Values[0];
            var input = new DeleteContainerInput(ApplicationUserId, CurrentOwnershipId, containerId);
            var output = deleteContainerUseCase.Execute(input);
            if (output.Result != Output.OutputResult.Success)
            {
                await ShowErrorMessage(output.Message);
            }
            else
            {
                SetRedirectWithCallbackData<ContainersController>(nameof(ContainersMenu));
            }
        }
    }
}
