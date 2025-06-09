using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsportPlayerManager.Data;
using EsportPlayerManager.Models;

public partial class TournamentViewModel : ObservableObject
{
    private readonly TournamentRepository _tournamentRepository;
    private readonly PlayerRepository _playerRepository;
    private readonly Player _selectedPlayer;

    [ObservableProperty] private Tournament? _selectedTournament;
    [ObservableProperty]
    private string _tournamentResultMessage = string.Empty;

    public ObservableCollection<Tournament> Tournaments { get; } = new();

    public TournamentViewModel(Player selectedPlayer)
    {
        _selectedPlayer = selectedPlayer;
        var connectionString = """
                               Host=localhost;
                               Port=5432;
                               Username=postgres;
                               Password=Dominik01;
                               Database=esportmanager
                               """;
        _tournamentRepository = new TournamentRepository(connectionString);
        _playerRepository = new PlayerRepository(connectionString);
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
        if (SelectedTournament == null)
            return;
        var rnd = new Random();

        int playerScore = rnd.Next(1, 11) * _selectedPlayer.Skill * 3 - _selectedPlayer.Stress * 5;
        int opponentScore = rnd.Next(1, 11) * SelectedTournament.MinSkillRequired * 3;

        bool won = playerScore > opponentScore;

        _selectedPlayer.Stress += SelectedTournament.StressIncrease;
        _selectedPlayer.Money -= SelectedTournament.EntryFee;

        if (won)
        {
            _selectedPlayer.Money += SelectedTournament.Prize;
        }

        _playerRepository.UpdatePlayer(_selectedPlayer);
        OnPropertyChanged(nameof(_selectedPlayer));
        TournamentResultMessage = won
            ? $"Wygrałeś turniej! Twój wynik: {playerScore}, Przeciwnik: {opponentScore}"
            : $"Przegrałeś turniej. Twój wynik: {playerScore}, Przeciwnik: {opponentScore}";
    }

    private bool CanJoinTournament()
    {
        return SelectedTournament is not null &&
               _selectedPlayer.Skill >= SelectedTournament.MinSkillRequired &&
               _selectedPlayer.Money >= SelectedTournament.EntryFee;
    }

    partial void OnSelectedTournamentChanged(Tournament? value)
    {
        JoinTournamentCommand.NotifyCanExecuteChanged();
    }
}
