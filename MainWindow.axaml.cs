using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Logof_Client;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        try
        {
            var temppath = "kaspersons.csv";
            var result = DataImport.ImportKasAddressList(temppath);
            if (result.Item1)
            {
                var check_result = AddressCheck.Perform(result.Item2);
                foreach (var item in check_result)
                {
                    Console.WriteLine();
                    Console.Write(item.Item1 + " ");
                    foreach (var error in item.Item2) Console.Write(error + ", ");
                }
            }
        }
        catch
        {
        }
    }

    private void MnuExit_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void MnuAbout_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void MnuSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void MnuHelp_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void MnuGithub_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}