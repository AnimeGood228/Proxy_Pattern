using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Proxy_Pattern
{
    using System;
    using System.Collections.Generic;
    using System.Timers;
    public interface ISubject
    {
        string Request(string request);
    }
    public class Proxy : ISubject
    {
        private RealSubject _realSubject;
        private Dictionary<string, (string result, DateTime timestamp)> _cache;
        private readonly TimeSpan _cacheDuration;
        private readonly Timer _cacheCleanupTimer;

        public Proxy(TimeSpan cacheDuration)
        {
            _realSubject = new RealSubject();
            _cache = new Dictionary<string, (string result, DateTime timestamp)>();
            _cacheDuration = cacheDuration;

            // очистка кэша
            _cacheCleanupTimer = new Timer(60000); // каждые 60 секунд
            _cacheCleanupTimer.Elapsed += CleanupCache;
            _cacheCleanupTimer.Start();
        }

        public string Request(string request)
        {
            // Проверка прав доступа 
            if (!HasAccess())
            {
                return "Доступ запрещен.";
            }

            // Проверка кэша
            if (_cache.TryGetValue(request, out var cachedResult))
            {
                if (DateTime.Now - cachedResult.timestamp < _cacheDuration)
                {
                    return cachedResult.result; // Возвращаем кэшированный результат
                }
                else
                {
                    _cache.Remove(request); // Удаляем устаревший кэш
                }
            }

            // Обработка запроса с помощью RealSubject
            string result = _realSubject.Request(request);

            // Сохранение результата в кэш
            _cache[request] = (result, DateTime.Now);

            return result;
        }

        private bool HasAccess()
        {
            
            return false; // доступ
        }

        private void CleanupCache(object sender, ElapsedEventArgs e)
        {
            // Очистка устаревших записей из кэша
            var keysToRemove = new List<string>();
            foreach (var entry in _cache)
            {
                if (DateTime.Now - entry.Value.timestamp >= _cacheDuration)
                {
                    keysToRemove.Add(entry.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }
    }
}
