using System;

namespace RestaurantConsoleApp_1;

public class EggOrder
{
    private readonly int _quantity;
    private readonly int _quality;
    private static int _instanceCount = 0;
    private readonly bool _shouldReturnQuality;

    public EggOrder(int quantity)
    {
        _quantity = quantity;
        _instanceCount++;
        
        // random quality
        Random rand = new Random();
        _quality = rand.Next(1, 101);
        
        // На 2м, 4м, 6м и т.д. экземпляре возвращаем null
        _shouldReturnQuality = _instanceCount % 2 != 0;
    }

    public int GetQuantity()
    {
        return _quantity;
    }

    public int? GetQuality()
    {
        return _shouldReturnQuality ? _quality : null;
    }

    public void Crack()
    {
        // if quality is less than 25, egg is rotten
        if (_quality < 25)
            throw new Exception("Rotten egg!");
    }

    public void DiscardShell()
    {
        // shell throw simulation
    }

    public void Cook()
    {
        //cooking simulation
    }
}