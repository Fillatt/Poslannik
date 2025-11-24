using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Linq;
using System.Reflection;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// Локатор для автоматического поиска View по ViewModel
    /// </summary>
    public class ViewLocator : IDataTemplate
    {
        /// <summary>
        /// Создает экземпляр View для переданной ViewModel
        /// </summary>
        /// <param name="data">ViewModel, для которой нужно создать View</param>
        /// <returns>Экземпляр View</returns>
        public Control Build(object? data)
        {
            if (data is null)
                return new TextBlock { Text = "ViewModel is null" };

            var viewModelType = data.GetType();
            var viewModelName = viewModelType.Name;
            var viewName = viewModelName.Replace("ViewModel", "View");

            var assembly = Assembly.GetExecutingAssembly();
            var viewType = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == viewName && typeof(Control).IsAssignableFrom(t));

            if (viewType != null)
            {
                return (Control)Activator.CreateInstance(viewType)!;
            }
            else
            {
                return new TextBlock { Text = $"Not Found: {viewName}" };
            }
        }

        /// <summary>
        /// Проверяет, подходит ли данный объект для создания View
        /// </summary>
        /// <param name="data">Объект для проверки</param>
        /// <returns>true, если объект является ViewModelBase</returns>
        public bool Match(object? data)
        {
            return data is ViewModels.ViewModelBase;
        }
    }
}
