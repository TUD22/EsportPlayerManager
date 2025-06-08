using System;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dapper;
using EsportPlayerManager.Data;
using EsportPlayerManager.Models;
using Npgsql;
using Tmds.DBus.Protocol;

namespace EsportPlayerManager.ViewModels;

public partial class PlayerViewModel: ObservableObject
{
    private readonly MainWindowViewModel _mainVm;
    private readonly PlayerRepository _playerRepository;
    [ObservableProperty] private int _skill;
    [ObservableProperty] private string _nickname;
    [ObservableProperty] private int _playerId;
    [ObservableProperty] private int _stress;
    [ObservableProperty] public Player? _selectedPlayer;

    public ObservableCollection<Player> Players { get; } = new();
    public ICommand AddPlayerCommand { get; }
    public ICommand LoadPlayersCommand { get; }
    
    public ICommand DeletePlayerCommand { get; }

    public PlayerViewModel(MainWindowViewModel mainVm)
    {
        _mainVm = mainVm;
        _playerRepository = new PlayerRepository("""
                                                 Host=localhost;
                                                 Port=5432;
                                                 Username=postgres;
                                                 Password=Dominik01;
                                                 Database=esportmanager
                                                 """);
        AddPlayerCommand = new RelayCommand(AddPlayer);
        LoadPlayersCommand = new RelayCommand(LoadPlayers);
        DeletePlayerCommand = new RelayCommand(DeletePlayer, CanDeletePlayer);
        _playerRepository.InitializeDatabase();
        LoadPlayers();
    }
    
    private void LoadPlayers()
    {
        Players.Clear();
        foreach (var player in _playerRepository.GetPlayers())
        {
            Players.Add(player);
        }
    }
    
    private bool CanDeletePlayer() => SelectedPlayer is not null;

    private void DeletePlayer()
    {
        if (SelectedPlayer == null) return;
        
        _playerRepository.DeletePlayer(SelectedPlayer.PlayerId);
        Players.Remove(SelectedPlayer);
        SelectedPlayer = null;
    }
    private void AddPlayer()
    {
        var player = new Player
        {
            PlayerId = _playerId,
            Nickname = _nickname,
            Skill = _skill,
            Stress = _stress
        };

        try
        {
            _playerRepository.AddPlayer(player);
            Players.Add(player);
            ClearFields();
        }
        catch(PostgresException ex) when (ex.SqlState == "23502")
        {
            Console.WriteLine($"Error adding player: {ex.Message}");
        }
        LoadPlayers();
    }

    private void ClearFields()
    {
        Nickname = string.Empty;
    }

    partial void OnSelectedPlayerChanged(Player? value)
    {
        _mainVm.SelectedPlayer = value;
        (DeletePlayerCommand as RelayCommand)?.NotifyCanExecuteChanged();
    }
}