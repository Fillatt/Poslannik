namespace Poslannik.Framework.Models;

/// <summary>
/// Представляет пользователя мессенджера
/// </summary>
public record User
{
    /// <summary>
    /// Уникальный идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Логин для входа в систему
    /// </summary>
    public required string Login { get; set; }

    /// <summary>Пароль.</summary>
    public required string Password { get; set; }

    /// <summary>
    /// Отображаемое имя пользователя
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>Статус пользователя (если студент - указывается группа)</summary>
    public string? Status { get; set; }
}
