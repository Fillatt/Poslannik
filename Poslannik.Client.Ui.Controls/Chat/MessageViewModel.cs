using System;
using System.Text;
using Poslannik.Framework.Models;
using ReactiveUI;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для отображения сообщения в чате
    /// </summary>
    public class MessageViewModel : ReactiveObject
    {
        private readonly Message _message;
        private readonly Guid _currentUserId;

        public MessageViewModel(Message message, Guid currentUserId)
        {
            _message = message;
            _currentUserId = currentUserId;
        }

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public Guid Id => _message.Id;

        /// <summary>
        /// Текст сообщения (расшифрованный)
        /// </summary>
        public string Text
        {
            get
            {
                try
                {
                    return Encoding.UTF8.GetString(_message.EncryptedMessage);
                }
                catch
                {
                    return "[Ошибка декодирования сообщения]";
                }
            }
        }

        /// <summary>
        /// Отправлено текущим пользователем?
        /// </summary>
        public bool IsSentByCurrentUser => _message.SenderId == _currentUserId;

        /// <summary>
        /// Время отправки
        /// </summary>
        public DateTime SentAt => _message.SentAt;

        /// <summary>
        /// Форматированное время отправки
        /// </summary>
        public string FormattedTime => _message.SentAt.ToLocalTime().ToString("HH:mm");

        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public Guid SenderId => _message.SenderId;
    }
}
