using System;
using MySql.Data.MySqlClient;
using Zenject;

public class MYSQLProvider : IInitializable
{
    private MySqlConnection _connection;
    
    public long? CurrentPlayerId { get; private set; }
    public string CurrentPlayerName { get; private set; }
    
    public void Initialize()
    {
        ConnectToDatabase();
    }
    
    public void SetupCurrentPlayer(long? currentPlayerId)
    {
        CurrentPlayerId = currentPlayerId;
    }
    
    private void ConnectToDatabase()
    {
        string connectionString = "server=212.109.196.234;port=3306;Database=gameplayDatabase;CharacterSet=utf8mb4;user=AMAEMON;password=VENOm_21rast;POOLING=FALSE;";
        
        _connection = new MySqlConnection(connectionString);
        
        _connection.Open();
    }
    
    public void AddPlayer(string playerName, string login, string password)
    {
        string checkQuery = "SELECT COUNT(*) FROM Players WHERE Login = @Login;";
        string insertQuery = $"INSERT INTO Players (PlayerName, Login, Password) VALUES (@PlayerName, @Login, @Password);";

        MySqlCommand checkCommand = new(checkQuery, _connection);
            
        checkCommand.Parameters.AddWithValue("@Login", login);

        long userExists = (long)checkCommand.ExecuteScalar();

        if (userExists > 0)
            return;
            
        MySqlCommand insertCommand = new(insertQuery, _connection);
            
        insertCommand.Parameters.AddWithValue("@PlayerName", playerName);
        insertCommand.Parameters.AddWithValue("@Login", login);
        insertCommand.Parameters.AddWithValue("@Password", password);
    }
    
    public long? GetUserByIdentity(string login, string password)
    {
        using MySqlCommand command = new($"SELECT Id FROM {DatabaseConstants.PlayersTable} WHERE Login = @Login AND Password = @Password", _connection);
        
        command.Parameters.AddWithValue("@Login", login);
        command.Parameters.AddWithValue("@Password", password);

        object result = command.ExecuteScalar();

        if (result != null) return Convert.ToInt64(result);

        return null;
    }
}