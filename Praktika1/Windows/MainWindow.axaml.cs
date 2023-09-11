using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Metsys.Bson;
using Microsoft.EntityFrameworkCore;
using Praktika1.Context;
using Praktika1.Models;
using Helper = Praktika1.Services.Helper;

namespace Praktika1.Windows;

public partial class MainWindow : Window
{
    private ListBox _productsListBox;

    public Boolean IsVis { get; set; }
    
    private TextBlock _textBlockFio;
    private ComboBox _comboBoxManufacturer;
    private ComboBox _comboBoxSort;
    private TextBox _textBoxSeach;
    private TextBlock _textBlockCount;

    private User user;

    private Grid _gridRow1;
    private Grid _gridRow2;
    private Grid _gridRow4;

    public MainWindow()
    {
        InitializeComponent();
        this.AttachDevTools();

        InitUI();
        SetComboBoxManufacturer();
        FoundLoad();
    }

    public MainWindow(User user)
    {
        InitializeComponent();
        this.AttachDevTools();

        InitUI();
        SetComboBoxManufacturer();
        
        this.user = user;
        
        FoundLoad();

        if (user != null)
        {
            SetFIO(user);
        }

    }

    private void SetFIO(User user)
    {
        string fio = $"{user.Surname} {user.Name} {user.Patronymic}";
        _textBlockFio.Text = fio;
    }

    public void SetComboBoxManufacturer()
    {
        var manufacturerList = Helper.Database.Manufacturers.ToList();
        manufacturerList.Insert(0, new Manufacturer()
        {
            Id = 0,
            Title = "Все производители"
        });

        _comboBoxManufacturer.Items = manufacturerList;
        _comboBoxManufacturer.SelectedIndex = 0;
    }

    private void InitUI()
    {
        _gridRow1 = this.FindControl<Grid>("GridRow1");
        _gridRow1.Background = Helper.DopColor;

        _gridRow2 = this.FindControl<Grid>("GridRow2");
        _gridRow2.Background = Helper.DopColor;
        
        _gridRow4 = this.FindControl<Grid>("GridRow4");
        _gridRow4.Background = Helper.DopColor;

        _productsListBox = this.FindControl<ListBox>("ProductListBox");
        _comboBoxManufacturer = this.FindControl<ComboBox>("ComboBoxManufacturer");
        _comboBoxManufacturer.SelectionChanged += ComboBoxManufacturer_OnSelectionChanged;
        
        _comboBoxSort = this.FindControl<ComboBox>("ComboBoxSort");
        _comboBoxSort.SelectionChanged += ComboBoxSort_OnSelectionChanged;
        _textBoxSeach = this.FindControl<TextBox>("TextBoxSeach");

        _textBlockFio = this.FindControl<TextBlock>("TextBlockFIO");
        _textBlockCount = this.FindControl<TextBlock>("TextBlockCount");

    }

    private void FoundLoad()
    {
        Manufacturer selectedProduct = _comboBoxManufacturer.SelectedItem as Manufacturer;

        List<Product> productsList;

        //  ФИЛЬТРАЦИЯ
        if (selectedProduct.Id > 0)
        {
            productsList = Helper.Database.Products
                .Include(x => x.Manufacturer)
                .Include(x => x.Provider)
                .Include(x => x.Category)
                .Where(x => x.Manufacturerid == selectedProduct.Id)
                .ToList();
        }
        else
        {
            productsList = Helper.Database.Products
                .Include(x => x.Manufacturer)
                .Include(x => x.Provider)
                .Include(x => x.Category)
                .ToList();
        }
        
        //  ПОИСК
        string seachText = _textBoxSeach.Text?.ToLower() ?? "";

        if (!string.IsNullOrWhiteSpace(seachText))
        {
            productsList = productsList.Where(x =>
                (x.Articlenumber != null && x.Articlenumber.ToLower().Contains(seachText)) ||
                (x.Title != null && x.Title.ToLower().Contains(seachText)) ||
                (x.Manufacturer.Title != null && x.Manufacturer.Title.ToLower().Contains(seachText)) ||
                (x.Provider.Title != null && x.Provider.Title.ToLower().Contains(seachText)) ||
                (x.Category.Title != null && x.Category.Title.ToLower().Contains(seachText)) ||
                (x.Description != null && x.Description.ToLower().Contains(seachText))
            ).ToList();
        }

        //  СОРТИРОВКА
        int sortIndex = _comboBoxSort.SelectedIndex;
        
        //  А-Я
        if (sortIndex == 0)
        {
            productsList = productsList.OrderBy(x => x.Price).ToList();
        }
        //Я-А
        else
        {
            productsList = productsList.OrderByDescending(x => x.Price).ToList();
        }
        
        var list = productsList
            .Select(x => new
            {
                x.Articlenumber,
                x.Title,
                x.Description,
                Manufacturer = x.Manufacturer.Title,
                x.Price,
                BackColor = x.Quantityinstock > 0 ? Brushes.White : Brushes.LightGray,
                BtnColor = Helper.VnimanieColor,
                isExists = x.Quantityinstock > 0 ? "В наличии" : "Нет в наличии",
                PhotoPath = string.IsNullOrWhiteSpace(x.Photo) ?  new Bitmap(@"..\..\..\Resources\noPicture.png") : new Bitmap(@"..\..\..\Resources\ProductPhotos\" + x.Photo),
                isAdmin = user?.Roleid == 1 ? true : false
            })
            .ToList();
        
        
        _productsListBox.Items = list;

        _textBlockCount.Text = $"{list.Count} из {Helper.Database.Products.Count()}";
    }
    

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void ComboBoxManufacturer_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        FoundLoad();
    }

    private void ComboBoxSort_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        FoundLoad();
    }

    private void TextBoxSeach_OnKeyUp(object? sender, KeyEventArgs e)
    {
        FoundLoad();
    }

    private void ButtonExit_OnClick(object? sender, RoutedEventArgs e)
    {
        AuthorizeWindow authorizeWindow = new AuthorizeWindow();
        authorizeWindow.Show();
        this.Close();
    }

    private void ButtonAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        AddEditWindow addEditWindow = new AddEditWindow(null);
        addEditWindow.ShowDialog(this);
        
        addEditWindow.Closed += (o, args) =>
        {
            FoundLoad();
        };
    }

    private void ButtonEdit_OnClick(object? sender, RoutedEventArgs e)
    {
        var btn = sender as Button;
        AddEditWindow addEditWindow = new AddEditWindow(btn.Tag?.ToString());
        addEditWindow.ShowDialog(this);

        addEditWindow.Closed += (o, args) =>
        {
            FoundLoad();
        };
    }
    
    private void ButtonDelete_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }

    private void TextBoxSeach_OnTextInput(object? sender, TextInputEventArgs e)
    {
        
        FoundLoad();
    }
}