using StorageBot.Host.Presentation;
using StorageBot.Host.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.Utilities
{
    [TestClass]
    public class HtmlMessageTests
    {
        [TestMethod]
        public void InitializeEmptyTest()
        {
            var message = new HtmlMessage();
            Assert.IsEmpty(message.ToString());
        }

        [TestMethod]
        [DataRow("string1")]
        [DataRow("message11")]
        public void InitializeTest(string text)
        {
            var message = new HtmlMessage(text);
            Assert.AreEqual(text, message.ToString());
        }

        [TestMethod]
        [DataRow("", "3123")]
        [DataRow("info", "some info")]
        public void AddTextTest(string baseText, string newLineText)
        {
            var message = new HtmlMessage(baseText);
            message.AddText(newLineText);
            var expectedText = $"{baseText}{newLineText}";
            Assert.AreEqual(expectedText, message.ToString());
        }

        [TestMethod]
        [DataRow("", "new line")]
        [DataRow("text", "new line text")]
        public void AddLineTest(string baseText, string newLineText)
        {
            var message = new HtmlMessage(baseText);
            message.AddLine(newLineText);
            var expectedText = $"{baseText}{newLineText}{Environment.NewLine}";
            Assert.AreEqual(expectedText, message.ToString());
        }

        [TestMethod]
        public void OpenKeyboardWithOpenKeyboardTest()
        {
            var message = new HtmlMessage();
            message.OpenKeyboard();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.OpenKeyboard();
            });
        }

        [TestMethod]
        public void OpenKeyboardWithClosedKeyboardTest()
        {
            var message = new HtmlMessage();
            message.OpenKeyboard();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.OpenKeyboard();
            });
        }

        [TestMethod]
        public void OpenRowWithoutKeyboardTest()
        {
            var message = new HtmlMessage();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.OpenRow();
            });
        }

        [TestMethod]
        public void OpenRowWithOpenRowTest()
        {
            var message = new HtmlMessage();
            message.OpenKeyboard();
            message.OpenRow();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.OpenRow();
            });
        }

        [TestMethod]
        [DataRow("", 0, 2)]
        [DataRow("some button", 3, 20)]
        public void AddCallbackQueryButtonToKeyboardTest(string buttonText, int controllerId, int actionId)
        {
            var message = new HtmlMessage();
            var callback = new CallbackData(controllerId, actionId);
            message.OpenKeyboard();
            message.AddCallbackQueryButton(buttonText, callback);
            message.CloseKeyboard();
            var expectedText = $"<keyboard><button text=\"{buttonText}\" callback=\"{CallbackDataEncoder.Encode(callback)}\"></keyboard>";
            Assert.AreEqual(expectedText, message.ToString());
        }

        [TestMethod]
        [DataRow("", 0, 2)]
        [DataRow("woooow", 3, 20)]
        public void AddCallbackQueryButtonToKeyboardRowTest(string buttonText, int controllerId, int actionId)
        {
            var message = new HtmlMessage();
            var callback = new CallbackData(controllerId, actionId);
            message.OpenKeyboard();
            message.OpenRow();
            message.AddCallbackQueryButton(buttonText, callback);
            message.CloseRow();
            message.CloseKeyboard();
            var expectedText = $"<keyboard><row><button text=\"{buttonText}\" callback=\"{CallbackDataEncoder.Encode(callback)}\"></row></keyboard>";
            Assert.AreEqual(expectedText, message.ToString());
        }

        [TestMethod]
        [DataRow("", 0, 2)]
        [DataRow("yami", 40, 3)]
        public void AddRowWithCallbackQueryButtonTest(string buttonText, int controllerId, int actionId)
        {
            var message = new HtmlMessage();
            var callback = new CallbackData(controllerId, actionId);
            message.OpenKeyboard();
            message.AddRowWithCallbackQueryButton(buttonText, callback);
            message.CloseKeyboard();
            var expectedText = $"<keyboard><row><button text=\"{buttonText}\" callback=\"{CallbackDataEncoder.Encode(callback)}\"></row></keyboard>";
            Assert.AreEqual(expectedText, message.ToString());
        }

        [TestMethod]
        public void AddRowWithCallbackQueryButtonWithoutOpenKeyboardTest()
        {
            var message = new HtmlMessage();
            var callback = new CallbackData(30, 256);
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.AddRowWithCallbackQueryButton("lolololo", callback);
            });
        }

        [TestMethod]
        public void AddRowWithCallbackQueryButtonWithOpenRowTest()
        {
            var message = new HtmlMessage();
            var callback = new CallbackData(30, 256);
            message.OpenKeyboard();
            message.OpenRow();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.AddRowWithCallbackQueryButton("lolololo", callback);
            });
        }

        [TestMethod]
        public void CloseRowWithClosedKeyboardTest()
        {
            var message = new HtmlMessage();
            message.OpenKeyboard();
            message.AddCallbackQueryButton("olololo", new CallbackData(1, 0));
            message.CloseKeyboard();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.CloseRow();
            });
        }

        [TestMethod]
        public void CloseRowWithoutOpenRowTest()
        {
            var message = new HtmlMessage();
            message.OpenKeyboard();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.CloseRow();
            });
        }

        [TestMethod]
        public void CloseRowWithNoButtonsTest()
        {
            var message = new HtmlMessage();
            message.OpenKeyboard();
            message.OpenRow();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.CloseRow();
            });
        }

        [TestMethod]
        public void CloseKeyboardWithoutKeyboardTest()
        {
            var message = new HtmlMessage();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.CloseKeyboard();
            });
        }

        [TestMethod]
        public void CloseKeyboardWithClosedKeyboardTest()
        {
            var message = new HtmlMessage();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.CloseKeyboard();
            });
        }

        [TestMethod]
        public void CloseKeyboardWithOpenRowTest()
        {
            var message = new HtmlMessage();
            message.OpenKeyboard();
            message.OpenRow();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.CloseKeyboard();
            });
        }

        [TestMethod]
        public void CloseKeyboardWithNoButtonsTest()
        {
            var message = new HtmlMessage();
            message.OpenKeyboard();
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                message.CloseKeyboard();
            });
        }
    }
}
