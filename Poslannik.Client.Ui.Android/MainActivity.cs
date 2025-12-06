using Android.App;
using Android.Content.PM;
using Android.Views;
using Autofac;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;

namespace Poslannik.Client.Ui.Android
{
    [Activity(
        Label = "ЧГУ Посланник",
        Theme = "@style/MyTheme.NoActionBar",
        Icon = "@drawable/poslanik",
        MainLauncher = true,
        WindowSoftInputMode = SoftInput.AdjustResize,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
    public class MainActivity : AvaloniaMainActivity<App>
    {
        protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
        {
            return base.CustomizeAppBuilder(builder)
                .WithInterFont()
                .UseReactiveUI();
        }

        /// <summary>
        /// Обработка системной кнопки "Назад" на Android
        /// </summary>
        [System.Obsolete]
        public override void OnBackPressed()
        {
            var navigationService = App.Container?.Resolve<INavigationService>();

            if (navigationService != null && navigationService.CanNavigateBack)
            {
                // Если есть куда вернуться - выполняем навигацию назад
                navigationService.NavigateBack();
            }
            else
            {
                // Если вернуться некуда - стандартное поведение (выход из приложения)
                base.OnBackPressed();
            }
        }
    }
}