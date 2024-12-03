namespace Messenger.Application.Services;

public interface IRedisDistributedCacheManager {
    /// <summary>
    /// Добавляет элемент в массив, связанный с указанным ключом.
    /// </summary>
    Task AddToArrayAsync(string key, string value);

    /// <summary>
    /// Удаляет элемент из массива, связанного с указанным ключом.
    /// </summary>
    Task RemoveFromArrayAsync(string key, string value);

    /// <summary>
    /// Получает весь массив, связанный с указанным ключом.
    /// </summary>
    Task<List<string>?> GetArrayAsync(string key);
}