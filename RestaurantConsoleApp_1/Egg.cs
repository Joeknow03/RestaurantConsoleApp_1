using System;

namespace RestaurantConsoleApp_1;

public class Egg : CookedFood, IDisposable
{
    private readonly int _quality;
    private bool _disposed = false;

    public Egg(int quantity) : base(quantity)
    {
        Random rand = new Random();
        _quality = rand.Next(1, 101);
    }

    // Property для качества
    public int Quality
    {
        get { return _quality; }
    }

    public override void Obtain()
    {
        _isObtained = true;
    }

    public void Crack()
    {
        if (_quality < 25)
            throw new Exception("Rotten egg!");
    }

    public override void Cook()
    {
        // Симуляция готовки яиц
    }

    public override void Serve()
    {
        _isServed = true;
    }

    public override string GetName()
    {
        return "Egg";
    }

    // IDisposable - выбрасываем скорлупу
    public void Dispose()
    {
        if (!_disposed)
        {
            // Выбрасываем скорлупу
            _disposed = true;
        }
    }
}