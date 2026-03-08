using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantConsoleApp_1;
/// Пул поваров на основе SemaphoreSlim.
/// SemaphoreSlim выбран потому что поваров НЕСКОЛЬКО —
/// нужно ограничить параллельность ровно N слотами.
/// WaitAsync() не блокирует UI поток пока ждёт свободного повара.

public class CookPool
{
    private readonly Cook[]       _cooks;
    private readonly SemaphoreSlim _semaphore;
    private readonly Queue<Cook>   _available;
    private readonly object        _queueLock = new();

    public CookPool(int cookCount)
    {
        if (cookCount < 1) throw new ArgumentException("Need at least 1 cook.");

        _cooks = new Cook[cookCount];
        for (int i = 0; i < cookCount; i++)
            _cooks[i] = new Cook(i + 1);

        _semaphore = new SemaphoreSlim(cookCount, cookCount);
        _available = new Queue<Cook>(_cooks);
    }

    
    /// Асинхронно берёт свободного повара из пула.
    /// Если все заняты — ждёт без блокировки UI.
    /// Dispose на CookScope возвращает повара в пул.
    public async Task<CookScope> AcquireAsync()
    {
        await _semaphore.WaitAsync();

        Cook cook;
        lock (_queueLock)
        {
            cook = _available.Dequeue();
        }

        return new CookScope(cook, ReturnCook);
    }

    private void ReturnCook(Cook cook)
    {
        lock (_queueLock)
        {
            _available.Enqueue(cook);
        }
        _semaphore.Release();
    }

    public sealed class CookScope : IDisposable
    {
        public Cook Cook { get; }
        private readonly Action<Cook> _release;
        private bool _disposed;

        internal CookScope(Cook cook, Action<Cook> release)
        {
            Cook     = cook;
            _release = release;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _release(Cook);
        }
    }
}