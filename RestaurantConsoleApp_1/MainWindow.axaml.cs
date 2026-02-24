using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace RestaurantConsoleApp_1;

public partial class MainWindow : Window
{
    private Server _server;
    private Cook _cook;

    public MainWindow()
    {
        InitializeComponent();
        _server = new Server();
        _cook = new Cook();
    }

    private void ReceiveRequest_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Парсим количество
            if (!int.TryParse(ChickenQtyTextBox.Text, out int chickenQty) || chickenQty < 0)
            {
                ResultsTextBox.Text = "Error: Enter valid chicken quantity (0 or more)!";
                return;
            }

            if (!int.TryParse(EggQtyTextBox.Text, out int eggQty) || eggQty < 0)
            {
                ResultsTextBox.Text = "Error: Enter valid egg quantity (0 or more)!";
                return;
            }

            // Определяем напиток
            MenuItem drink = MenuItem.NoDrink;
            if (TeaRadio.IsChecked == true) drink = MenuItem.Tea;
            else if (ColaRadio.IsChecked == true) drink = MenuItem.CocaCola;
            else if (PepsiRadio.IsChecked == true) drink = MenuItem.Pepsi;

            // Принимаем заказ
            string result = _server.ReceiveRequest(chickenQty, eggQty, drink);
            ResultsTextBox.Text = result;
        }
        catch (Exception ex)
        {
            ResultsTextBox.Text = $"Error: {ex.Message}";
        }
    }

    private void SendToCook_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string result = _server.SendAllRequestsToCook(_cook);
            ResultsTextBox.Text = result;
        }
        catch (Exception ex)
        {
            ResultsTextBox.Text = $"Error: {ex.Message}";
        }
    }

    private void ServeFood_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string result = _server.ServeFood();
            ResultsTextBox.Text = result;
        }
        catch (Exception ex)
        {
            ResultsTextBox.Text = $"Error: {ex.Message}";
        }
    }
}