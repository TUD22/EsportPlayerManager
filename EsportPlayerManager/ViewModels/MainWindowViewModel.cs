using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsportPlayerManager.Models;
using EsportPlayerManager.Views;

namespace EsportPlayerManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private object _currentView;
    [ObservableProperty] private Player _selectedPlayer;

    public MainWindowViewModel()
    {
        ShowPlayer();
    }
    
    [RelayCommand]
    public void ShowPlayer()
    {
        CurrentView = new PlayerView()
        {
            DataContext = new PlayerViewModel(this)
        };
    }
    
    public void ShowTraining()
    {
        CurrentView = new TrainingView()
        {
            DataContext = new TrainingViewModel(this)
        };
    }

    [RelayCommand]
    public void ShowTournament()
    {
        CurrentView = new TournamentView
        {
            DataContext = new TournamentViewModel(SelectedPlayer)
        };
    }
}
