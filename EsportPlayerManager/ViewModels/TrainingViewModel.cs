using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsportPlayerManager.Data;
using EsportPlayerManager.Models;

namespace EsportPlayerManager.ViewModels;

public partial class TrainingViewModel : ObservableObject
{
    private readonly TrainingRepository _trainingRepository;
    private readonly PlayerRepository _playerRepository;
    private readonly MainWindowViewModel _mainVm;

    [ObservableProperty] private Training? _selectedTraining;
    [ObservableProperty] private Player? _selectedPlayer;

    public ObservableCollection<Training> Trainings { get; } = new();

    public TrainingViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainVm = mainWindowViewModel;
        var connectionString = """
                               Host=localhost;
                               Port=5432;
                               Username=postgres;
                               Password=Dominik01;
                               Database=esportmanager
                               """;
        SelectedPlayer = mainWindowViewModel.SelectedPlayer;
        _playerRepository = new PlayerRepository(connectionString);
        _trainingRepository = new TrainingRepository(connectionString);
        LoadTrainings();
    }

    private void LoadTrainings()
    {
        Trainings.Clear();
        foreach (var training in _trainingRepository.GetTrainings())
        {
            Trainings.Add(training);
        }
    }

    [RelayCommand(CanExecute = nameof(CanJoinTraining))]
    private void JoinTraining()
    {
    }
    [RelayCommand]
    public void Train()
    {
        if (SelectedTraining == null || _selectedPlayer == null)
            return;
        SelectedPlayer.Money += SelectedTraining.moneyIncrease;
        SelectedPlayer.Skill += SelectedTraining.skillIncrease;
        SelectedPlayer.Stress += SelectedTraining.stressIncrease;
        _playerRepository.UpdatePlayer(SelectedPlayer);
        _mainVm.SelectedPlayer = new Player
        {
            PlayerId = SelectedPlayer.PlayerId,
            Skill = SelectedPlayer.Skill,
            Stress = SelectedPlayer.Stress,
            Money = SelectedPlayer.Money
        };
        OnPropertyChanged(nameof(SelectedPlayer));
    }

    private bool CanJoinTraining() => SelectedTraining is not null;

    partial void OnSelectedTrainingChanged(Training? value)
    {
        TrainCommand.NotifyCanExecuteChanged();
    }
}