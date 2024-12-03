using System.Text.Json;
using Messenger.Application;
using Messenger.Application.Services;
using StackExchange.Redis;

namespace Messenger.Infrastructure.Impl;

public class RedisDistributedCacheManager : IRedisDistributedCacheManager {
    private readonly IDatabase _db;
    private readonly RedisOptions _redisOptions;

    public RedisDistributedCacheManager(RedisOptions redisOptions) {
        _redisOptions = redisOptions;
        var connectionString = redisOptions.ConnectionString;
        var redis = ConnectionMultiplexer.Connect(connectionString);
        _db = redis.GetDatabase();
    }
    /// <summary>
    /// Добавляет элемент в массив, связанный с указанным ключом.
    /// </summary>
    public async Task AddToArrayAsync(string key, string value) {
        await _db.ListLeftPushAsync(key, value);
    }

    /// <summary>
    /// Удаляет элемент из массива, связанного с указанным ключом.
    /// </summary>
    public async Task RemoveFromArrayAsync(string key, string value) {
        await _db.ListRemoveAsync(key, value);
    }

    /// <summary>
    /// Получает весь массив, связанный с указанным ключом.
    /// </summary>
    public async Task<List<string>?> GetArrayAsync(string key) {
        var list = await _db.ListRangeAsync(key);
        if (!list.Any()) {
            return null;
        }
        return list.Select(x => x.ToString()).ToList();
    }
}