using ClientMessenger.Messages;
using ClientMessenger.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;


namespace ClientMessenger.ViewModels;

public partial class MainViewModel : ViewModel
{
    [ObservableProperty]
    private ViewModel? _currentViewModel;

    public MainViewModel()
    {
        CurrentViewModel = App.Container.GetInstance<RegistrationUserViewModel>();
        var factory = App.Container.GetInstance<ViewModelFactory>();
        WeakReferenceMessenger.Default.Register<ChangeViewMessage>(this, (_, message) =>
        {
            CurrentViewModel = factory.CreateViewModel(message.Value);
        });
    }
    
    
}