using StorageBot.Host.Presentation;
using StorageBot.Host.Routing.Attributes.Actions.Required;
using StorageBot.Host.Routing.Attributes.Actions.Updates;
using StorageBot.Host.Routing.Attributes.Controllers;
using StorageBot.TelegramContracts.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Controllers
{
    [ControllerId(1)]
    public class ErrorHandlerСontroller : BaseBotController
    {
        private static readonly int _Delay = 7500;

        [ActionId(0)]
        [UnexpectedMessageAction]
        public async Task HandleUnexpectedMessage()
        {
            await AnswerToText(Localizer["View.UnexpectedMessage"]);
        }

        [ActionId(1)]
        [UnregisteredCommandAction]
        public async Task UnregisteredCommandAction()
        {
            await AnswerToText(Localizer["View.UnregisteredCommand"]);
        }

        [ActionId(2)]
        [UnknownCallbackQueryAction]
        public async Task HandleUnknownCallbackQuery()
        {
            var message = new HtmlMessage(Localizer["View.UnknownCallbackQuery"]);
            await SendMessageWithDelete(message, _Delay);
        }

        private async Task AnswerToText(string answer)
        {
            var context = ExtractContext<MessageUpdateContext>();
            var task1 = DeleteMessage(context.Message.Id, _Delay);
            var message = new HtmlMessage(Localizer["View.UnexpectedMessage"]);
            var task2 = SendMessageWithDelete(message, _Delay);
            Task.WaitAll(task1, task2);
        }
    }
}
