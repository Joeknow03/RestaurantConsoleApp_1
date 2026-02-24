using System;

namespace RestaurantConsoleApp_1;

public class EggOrder : Order
{
    private readonly int _quality;
    private static int _instanceCount;
    private readonly bool _shouldReturnQuality;

    public EggOrder(int quantity) : base(quantity)
    {
        _instanceCount++;
        
        Random rand = new Random();
        _quality = rand.Next(1, 101);
        
        _shouldReturnQuality = _instanceCount % 2 != 0;
    }

    public int? GetQuality()
    {
        return _shouldReturnQuality ? _quality : null;
    }

    public void Crack()
    {
        if (_quality < 25)
            throw new Exception("Rotten egg!");
    }

    public void DiscardShell()
    {
        // Симуляция выбрасывания скорлупы
    }

    public override void Cook()  // Переопределяем метод
    {
        // Симуляция приготовления яиц
    }
}