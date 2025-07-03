using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Logof_Client;

public class MessageBox : Window
{
    public MessageBox()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static Task<MessageBoxResult> Show(Window parent, string text, string title,
        MessageBoxButton buttons = MessageBoxButton.Ok)
    {
        try
        {
            var msgbox = new MessageBox
            {
                Title = title
            };
            msgbox.FindControl<TextBlock>("Text").Text = text;
            var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");

            var res = MessageBoxResult.Ok;

            void AddButton(string caption, MessageBoxResult r, bool def = false)
            {
                var btn = new Button { Content = caption };
                btn.Click += (_, __) =>
                {
                    res = r;
                    msgbox.Close();
                };
                buttonPanel.Children.Add(btn);
                if (def)
                    res = r;
            }

            if (buttons == MessageBoxButton.Ok || buttons == MessageBoxButton.OkCancel)
                AddButton("Ok", MessageBoxResult.Ok, true);
            if (buttons == MessageBoxButton.YesNo || buttons == MessageBoxButton.YesNoCancel)
            {
                AddButton("Yes", MessageBoxResult.Yes);
                AddButton("No", MessageBoxResult.No, true);
            }

            if (buttons == MessageBoxButton.OkCancel || buttons == MessageBoxButton.YesNoCancel)
                AddButton("Cancel", MessageBoxResult.Cancel, true);


            var tcs = new TaskCompletionSource<MessageBoxResult>();
            msgbox.Closed += delegate { tcs.TrySetResult(res); };
            if (parent != null)
                msgbox.ShowDialog(parent);
            else msgbox.Show();

            return tcs.Task;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while showing messagebox: " + ex.Message);
            return Task.FromResult(MessageBoxResult.Error);
        }
    }
}

public enum MessageBoxButton
{
    Ok,
    OkCancel,
    YesNo,
    YesNoCancel
}

public enum MessageBoxResult
{
    Ok,
    Cancel,
    Yes,
    No,
    Error
}