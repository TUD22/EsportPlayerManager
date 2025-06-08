using System.Collections.Generic;
using Dapper;
using EsportPlayerManager.Models;
using Npgsql;

namespace EsportPlayerManager.Data;

public class TrainingRepository
{
    private readonly string _connectionString;

    public TrainingRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Training> GetTrainings()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return connection.Query<Training>("SELECT * FROM trainings").AsList();
    }
    
}