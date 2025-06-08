using System;
using System.Collections.Generic;
using Dapper;
using EsportPlayerManager.Models;
using Npgsql;

namespace EsportPlayerManager.Data;

public class PlayerRepository
{
    private readonly string _connectionString;
    
    public PlayerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public void InitializeDatabase()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Execute("""
        CREATE TABLE IF NOT EXISTS players (
        player_id SERIAL PRIMARY KEY,
        nickname VARCHAR(50) NOT NULL,
        skill INTEGER NOT NULL,
        stress INTEGER NOT NULL
    )
    """);
    }

    public List<Player> GetPlayers()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return connection.Query<Player>("SELECT * FROM players").AsList();
    }

    public void AddPlayer(Player player)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        Console.WriteLine($"Adding player {player.Nickname}");
        connection.Execute(
                "INSERT INTO players (nickname, skill, stress) VALUES (@nickname, 0, 0)", player);
    }

    public void DeletePlayer(int playerId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Execute("DELETE FROM players WHERE player_id = @playerId", new { playerId });
    }
    
    public void UpdatePlayer(Player player)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Execute("""
                               UPDATE players 
                               SET skill = @Skill, stress = @Stress 
                               WHERE player_id = @PlayerId
                           """, player);
    }
}