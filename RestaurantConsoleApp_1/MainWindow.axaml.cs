using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;

namespace RestaurantConsoleApp_1;

public partial class MainWindow : Window
{
    private readonly Server   _server;
    private readonly CookPool _cookPool;

    // Минимум 2 повара по требованию
    private const int COOK_COUNT = 2;

    public MainWindow()
    {
        InitializeComponent();
        _server   = new Server();
        _cookPool = new CookPool(COOK_COUNT);
    }

    // ── Кнопка 1: Receive (LOCKED внутри Server.ReceiveRequest) ──────────────
    private void ReceiveRequest_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string name = CustomerNameTextBox.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(name))
            {
                ResultsTextBox.Text = "Error: Please enter a customer name!";
                return;
            }

            if (!int.TryParse(ChickenQtyTextBox.Text, out int chickenQty) || chickenQty < 0)
            {
                ResultsTextBox.Text = "Error: Enter valid chicken quantity!";
                return;
            }

            if (!int.TryParse(EggQtyTextBox.Text, out int eggQty) || eggQty < 0)
            {
                ResultsTextBox.Text = "Error: Enter valid egg quantity!";
                return;
            }

            string drink = "NoDrink";
            if      (TeaRadio.IsChecked   == true) drink = "Tea";
            else if (ColaRadio.IsChecked  == true) drink = "CocaCola";
            else if (PepsiRadio.IsChecked == true) drink = "Pepsi";

            string result = _server.ReceiveRequest(chickenQty, eggQty, drink, name);
            ResultsTextBox.Text = result;

            // Очищаем поля для следующего клиента
            CustomerNameTextBox.Text = "";
            ChickenQtyTextBox.Text   = "0";
            EggQtyTextBox.Text       = "0";
            NoDrinkRadio.IsChecked   = true;
        }
        catch (Exception ex)
        {
            ResultsTextBox.Text = $"Error: {ex.Message}";
        }
    }

    // ── Кнопка 2: Send to Cook ────────────────────────────────────────────────
    // Task 1 (повар готовит, SemaphoreSlim) → continuation Task 2 (подача, lock)
    private async void SendToCook_Click(object sender, RoutedEventArgs e)
    {
        SendToCookButton.IsEnabled = false;
        ResultsTextBox.Text = "Kitchen is working...";

        try
        {
            string result = await _server.SendToCookAsync(_cookPool);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                ResultsTextBox.Text = result;
            });
        }
        catch (Exception ex)
        {
            ResultsTextBox.Text = $"Error: {ex.Message}";
        }
        finally
        {
            SendToCookButton.IsEnabled = true;
        }
    }

    // ── Кнопка 3: Serve (LOCKED внутри Server.ServeFood) ─────────────────────
    private void ServeFood_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ResultsTextBox.Text = _server.ServeFood();
        }
        catch (Exception ex)
        {
            ResultsTextBox.Text = $"Error: {ex.Message}";
        }
    }
}