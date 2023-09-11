using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Praktika1.Models;
using Praktika1.Services;

namespace Praktika1.Windows;

public partial class AddEditWindow : Window
{
    private Product _currentProduct = new Product();

    private ComboBox _comboBoxManufacturer;
    private ComboBox _comboBoxProvider;
    private ComboBox _comboBoxCategory;

    private StackPanel _stackPanel;

    private Image _photoImage;
    
    private string oldPhotoName = "";
    private string newPhotoFullPath;

    private string? _startArticul;

    public AddEditWindow()
    {
        InitializeComponent();
        this.AttachDevTools();
    }

    public void InitUI()
    {
        _comboBoxManufacturer = this.FindControl<ComboBox>("ComboBoxManufacturer");
        _comboBoxProvider = this.FindControl<ComboBox>("ComboBoxProvider");
        _comboBoxCategory = this.FindControl<ComboBox>("ComboBoxCategory");

        _stackPanel = this.FindControl<StackPanel>("StackPanelName");

        _photoImage = this.FindControl<Image>("PhotoImage");

        _stackPanel.Background = Helper.DopColor;
    }

    public AddEditWindow(string? articul)
    {
        InitializeComponent();
        this.AttachDevTools();

        InitUI();
        SetAllComboboxes();

        if (articul != null)
        {
            var product = Helper.Database.Products.Find(articul);
            if (product == null)
            {
                this.Close();
            }

            _currentProduct = product;
        }

        oldPhotoName = _currentProduct.Photo;
        _startArticul = _currentProduct.Articlenumber;

        if (_startArticul == null)
        {
            _currentProduct.Articlenumber = Guid.NewGuid().ToString();
        }

        try
        {
            var photo =  new Bitmap(@"..\..\..\Resources\ProductPhotos\" + _currentProduct.Photo);
            _photoImage.Source = photo;
        }
        catch (Exception e)
        {
            var photo = new Bitmap(@"..\..\..\Resources\noPicture.png");
            _photoImage.Source = photo;
        }
        
        DataContext = _currentProduct;

        _comboBoxManufacturer.SelectedItem = _currentProduct.Manufacturer;
        _comboBoxProvider.SelectedItem = _currentProduct.Provider;
        _comboBoxCategory.SelectedItem = _currentProduct.Category;
    }

    private void SetAllComboboxes()
    {
        var manufacturerList = Helper.Database.Manufacturers.ToList();
        var providerList = Helper.Database.Providers.ToList();
        var categoryList = Helper.Database.Productcategories.ToList();

        _comboBoxManufacturer.Items = manufacturerList;
        _comboBoxProvider.Items = providerList;
        _comboBoxCategory.Items = categoryList;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void ButtonSave_OnClick(object? sender, RoutedEventArgs e)
    {
        StringBuilder errors = new StringBuilder();

        var curProd = DataContext as Product;
        
        if (oldPhotoName != curProd.Photo)
        {
            try
            {
                string path = Path.Combine((Directory.GetParent(Environment.CurrentDirectory).Parent.Parent) + @"\Resources\ProductPhotos\" + curProd.Photo);
                File.Copy(newPhotoFullPath, path, true);
            }
            catch (Exception exception)
            {
            }
            
        }
        
        curProd.Manufacturerid = (_comboBoxManufacturer.SelectedItem as Manufacturer).Id;
        curProd.Providerid = (_comboBoxProvider.SelectedItem as Provider).Id;
        curProd.Categoryid = (_comboBoxCategory.SelectedItem as Productcategory).Id;

        if (_startArticul == null)
        {
            Helper.Database.Products.Add(curProd);
        }
        else
        {
            Helper.Database.Products.Update(curProd);
        }

        try
        {
            Helper.Database.SaveChanges();
            this.Close();
        }
        catch (Exception exception)
        {
            
        }
        
    }

    private async void ButtonAddPhoto_OnClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions = { "jpg", "jpeg", "png", "bmp" } });
        
        var result = await dialog.ShowAsync(this);
        
        if (result != null)
        {
            newPhotoFullPath = result[0];
            var bitmap = new Bitmap(result[0]);
            string fileName = System.IO.Path.GetFileName(result[0]);
            
            _photoImage.Source = bitmap;
            (DataContext as Product).Photo = fileName;
        }
    }
}