using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для профиля другого пользователя (собеседника)
    /// </summary>
    public class UserProfileViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private string? _displayName;
        private Guid? _userId;

        public UserProfileViewModel(IUserService userService)
        {
            _userService = userService;
            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
        }

        /// <summary>
        /// ID пользователя для отображения
        /// </summary>
        public Guid? UserId
        {
            get => _userId;
            set
            {
                this.RaiseAndSetIfChanged(ref _userId, value);
                if (value.HasValue)
                {
                    _ = LoadUserDataAsync();
                }
            }
        }

        /// <summary>
        /// Отображаемое имя пользователя (Имя Фамилия)
        /// </summary>
        public string? DisplayName
        {
            get => _displayName;
            set => this.RaiseAndSetIfChanged(ref _displayName, value);
        }

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Загрузка данных пользователя
        /// </summary>
        private async Task LoadUserDataAsync()
        {
            if (!UserId.HasValue)
                return;

            try
            {
                var user = await _userService.GetUserByIdAsync(UserId.Value);
                if (user != null)
                {
                    DisplayName = user.DisplayName;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки данных пользователя: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик возврата назад
        /// </summary>
        private void OnNavigateBack()
        {
            NavigationService.NavigateBack();
        }
    }
}
