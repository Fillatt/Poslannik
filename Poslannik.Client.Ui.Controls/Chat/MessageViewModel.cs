using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Framework.Models;
using ReactiveUI;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для отображения сообщения
    /// </summary>
    public class MessageViewModel : ViewModelBase
    {
        private string _text = string.Empty;
        private string? _senderName;
        private bool _isOwnMessage;
        private bool _isPrivateChat;
        private DateTime _dateTime;

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        /// <summary>
        /// Имя отправителя (null для собственных сообщений)
        /// </summary>
        public string? SenderName
        {
            get => _senderName;
            set => this.RaiseAndSetIfChanged(ref _senderName, value);
        }

        public bool IsPrivateChat
        {
            get => _isPrivateChat;
            set => this.RaiseAndSetIfChanged(ref _isPrivateChat, value);
        }

        /// <summary>
        /// Является ли сообщение собственным
        /// </summary>
        public bool IsOwnMessage
        {
            get => _isOwnMessage;
            set => this.RaiseAndSetIfChanged(ref _isOwnMessage, value);
        }

        /// <summary>
        /// Время отправки сообщения
        /// </summary>
        public DateTime DateTime
        {
            get => _dateTime;
            set => this.RaiseAndSetIfChanged(ref _dateTime, value);
        }

        /// <summary>
        /// ID сообщения
        /// </summary>
        public Guid MessageId { get; set; }

        /// <summary>
        /// ID отправителя
        /// </summary>
        public Guid SenderId { get; set; }
    }
}
