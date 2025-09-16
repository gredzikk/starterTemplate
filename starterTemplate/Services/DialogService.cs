namespace starterTemplate.Services;

public interface IDialogService
{
    void ShowMessage(string message, string title = "Information");
    bool ShowConfirmation(string message, string title = "Confirmation");
}

public class DialogService : IDialogService
{
    public void ShowMessage(string message, string title = "Information")
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public bool ShowConfirmation(string message, string title = "Confirmation")
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }
}