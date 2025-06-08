using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsportPlayerManager.Data;
using EsportPlayerManager.Models;

namespace EsportPlayerManager.ViewModels;

public partial class TournamentViewModel : ObservableObject
{
    private readonly TournamentRepository _tournamentRepository;

    [ObservableProperty] private Tournament? _selectedTournament;

    public ObservableCollection<Tournament> Tournaments { get; } = new();

    public TournamentViewModel(Player selectedPlayer)
    {
        _tournamentRepository = new TournamentRepository("""
                                                         Host=localhost;
                                                         Port=5432;
                                                         Username=postgres;
                                                         Password=Dominik01;
                                                         Database=esportmanager
                                                         """);
        LoadTournaments();
    }

    private void LoadTournaments()
    {
        Tournaments.Clear();
        foreach (var tournament in _tournamentRepository.GetTournaments())
        {
            Tournaments.Add(tournament);
        }
    }

    [RelayCommand(CanExecute = nameof(CanJoinTournament))]
    private void JoinTournament()
    {
    }

    private bool CanJoinTournament() => SelectedTournament is not null;

    partial void OnSelectedTournamentChanged(Tournament? value)
    {
        JoinTournamentCommand.NotifyCanExecuteChanged();
    }
}