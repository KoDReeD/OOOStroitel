using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Praktika1.Services;

namespace Praktika1.Windows;

public partial class AuthorizeWindow : Window
{
    private TextBox _textBoxPassword;
    private TextBox _textBoxLogin;
    public AuthorizeWindow()
    {
        InitializeComponent();
        this.AttachDevTools();
        
        InitUI();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void InitUI()
    {
        _textBoxPassword = this.FindControl<TextBox>("TextBoxPassword");
        _textBoxLogin = this.FindControl<TextBox>("TextBoxLogin");
    }

    private void ButtonLogin_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_textBoxLogin.Text) || string.IsNullOrWhiteSpace(_textBoxPassword.Text))
        {
            return;
        }
        
        var user = Helper.Database.Users.Where(
            x => x.Login == _textBoxLogin.Text && x.Password == _textBoxPassword.Text)
            .FirstOrDefault();

        if (user == null)
        {
            return;
        }
        
        MainWindow mainWindow = new MainWindow(user);
        mainWindow.Show();
        this.Close();
    }
    
    private void ButtonGuest_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow(null);
        mainWindow.Show();
        this.Close();
    }
}