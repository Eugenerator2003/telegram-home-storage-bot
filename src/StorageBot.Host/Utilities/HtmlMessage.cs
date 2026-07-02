using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Utilities
{
    public class HtmlMessage
    {
        private enum KeyboardState
        {
            None,
            Open,
            Close
        }

        private enum KeyboardRowState
        {
            None,
            Open,
            Close
        }

        private KeyboardState keyboardState;
        private KeyboardRowState rowState;
        private bool rowHasButton;
        private readonly StringBuilder builder;

        public HtmlMessage() : this(string.Empty) { }

        public HtmlMessage(string text)
        {
            builder = new StringBuilder(text);
        }

        public void AddText(string text)
        {
            builder.Append(text);
        }

        public void AddLine(string text)
        {
            builder.AppendLine(text);
        }

        public void OpenKeyboard()
        {
            if (keyboardState == KeyboardState.Open)
            {
                throw new InvalidOperationException("Keyboard is already open");
            }
            if (keyboardState == KeyboardState.Close)
            {
                throw new InvalidOperationException("Keyboard is already closed");
            }
            builder.Append("<keyboard>");
            keyboardState = KeyboardState.Open;
        }

        public void OpenRow()
        {
            if (keyboardState != KeyboardState.Open)
            {
                throw new InvalidOperationException("Cannot add row without keyboard");
            }
            if (rowState == KeyboardRowState.Open)
            {
                throw new InvalidOperationException("Keyboard row is already open");
            }
            builder.Append("<row>");
            rowState = KeyboardRowState.Open;
        }

        public void AddCallbackQueryButton(string buttonText, CallbackData data)
        {
            if (keyboardState == KeyboardState.Close)
            {
                throw new InvalidOperationException("Cannot add button to closed keyboard");
            }
            if (rowState == KeyboardRowState.Close)
            {
                throw new InvalidOperationException("Cannot add button to closed keyboard row");
            }
            builder.Append($"<button text=\"{buttonText}\" callback=\"{CallbackDataEncoder.Encode(data)}\">");
            rowHasButton = true;
        }

        public void CloseRow()
        {
            if (keyboardState == KeyboardState.Close)
            {
                throw new InvalidOperationException("Keyboard is already closed");
            }
            if (rowState != KeyboardRowState.Open)
            {
                throw new InvalidOperationException("Cannot close keyboard row without opening");
            }
            if (!rowHasButton)
            {
                throw new InvalidOperationException("Cannot close keyboard row without any button");
            }
            builder.Append("</row>");
            rowState = KeyboardRowState.Close;
            rowHasButton = false;
        }

        public void CloseKeyboard()
        {
            if (keyboardState == KeyboardState.None)
            {
                throw new InvalidOperationException("Keyboard is not open");
            }
            if (keyboardState == KeyboardState.Close)
            {
                throw new InvalidOperationException("Keyboard is already closed");
            }
            if (rowState == KeyboardRowState.Open ||
                rowState == KeyboardRowState.None && !rowHasButton)
            {
                throw new InvalidOperationException("Cannot close keyboard while a keyboard row is open or row is empty");
            }
            builder.Append("</keyboard>");
            keyboardState = KeyboardState.Close;
        }

        public override string ToString()
        {
            if (keyboardState == KeyboardState.Open)
            {
                throw new InvalidOperationException("Cannot get string from message while keyboard is open");
            }

            return builder.ToString();
        }
    }
}
