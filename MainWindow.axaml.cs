using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace Logof_Client;

public partial class MainWindow : Window
{
    public static MainWindow _instance;
    public Uri filePath;

    public MainWindow()
    {
        InitializeComponent();
        _instance = this;
        // try
        // {
        //     var temppath = "kaspersons.csv";
        //     var result = DataImport.ImportKasAddressList(new Uri(temppath));
        //     if (result.Item1)
        //     {
        //         var check_result = new AddressCheck().Perform(result.Item2);
        //         foreach (var item in check_result.Result)
        //         {
        //             Console.WriteLine();
        //             Console.Write(item.Item1 + " ");
        //             foreach (var error in item.Item2) Console.Write(error + ", ");
        //         }
        //     }
        // }
        // catch
        // {
        // }
    }

    private async void StartAddressCheck(Uri path)
    {
        var addresses = DataImport.ImportKasAddressList(path); // Ihr Code hier
        var progressWindow = new ProgressWindow();

        // Fenster anzeigen (nicht blockierend)
        progressWindow.Show(_instance);

        var processor = new AddressCheck(progressWindow);
        var result = await processor.Perform(addresses.Item2);

        // Nach Verarbeitung schließen
        progressWindow.Close();

        // Ergebnis anzeigen, z.B. als Dialog
        new ResultWindow(result).Show();
        //await MessageBox.Show(_instance, $"{result.Count} Einträge fehlerhaft.", "Fertig");
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

    private async void BtnChooseFile_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = GetTopLevel(this);
        var file = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "KAS-CSV-Datei auswählen",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType(".csv-Datei")
                {
                    Patterns = new[] { "*.csv" }
                    //Patterns = new[] { "*" }
                }
            }
        });

        if (file == null) return;
        TbFilename.Text = file[0].Path.ToString();
        filePath = file[0].Path;
    }

    private void BtnCheck_OnClick(object? sender, RoutedEventArgs e)
    {
        if (filePath == null)
        {
            MessageBox.Show(null, "Bitte zunächst eine Datei auswählen", "Datei fehlt");
            return;
        }

        StartAddressCheck(filePath);
        // var result = DataImport.ImportKasAddressList(filePath);
        // if (result.Item1)
        // {
        //     var check_result = new AddressCheck().Perform(result.Item2);
        //     foreach (var item in check_result.Result)
        //     {
        //         Console.WriteLine();
        //         Console.Write(item.Item1 + " ");
        //         foreach (var error in item.Item2) Console.Write(error + ", ");
        //     }
        // }
    }
}