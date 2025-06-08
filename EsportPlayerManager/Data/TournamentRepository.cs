using System.Collections.Generic;
using Dapper;
using EsportPlayerManager.Models;
using Npgsql;

namespace EsportPlayerManager.Data;

public class TournamentRepository
{
    private readonly string _connectionString;

    public TournamentRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Tournament> GetTournaments()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return connection.Query<Tournament>("SELECT * FROM tournaments").AsList();
    }
    
}