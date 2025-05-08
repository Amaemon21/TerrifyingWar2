using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class BackendManager 
{
    private readonly string ServerUrl = "http://212.109.196.234:3000";
    
    private readonly HttpClient _client = new();
    
    public int PlayerId { get; private set; }
    public string PlayerLogin { get; private set; } = "";
    
    private bool _isLoggedIn = false;
    
    public async Task<bool> LoginAsync(string login, string password)
    {
        var requestData = new { login, password };
        string json = JsonConvert.SerializeObject(requestData);
        using StringContent content = new(json, Encoding.UTF8, "application/json");

        try
        {
            using HttpResponseMessage response = await _client.PostAsync($"{ServerUrl}/login", content).ConfigureAwait(false);
            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return false;

            LoginResponse result = JsonConvert.DeserializeObject<LoginResponse>(responseString);
            
            if (result == null || !result.Success)
                return false;

            if (result.PlayerId <= 0)
                return false;

            PlayerId = result.PlayerId;
            PlayerLogin = login;

            _isLoggedIn = true;
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Login request failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterPlayerAsync(string email, string login, string password)
    {
        var requestData = new { email, login, password };
        string json = JsonConvert.SerializeObject(requestData);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await _client.PostAsync($"{ServerUrl}/addPlayer", content);
            string responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return false;

            return responseString.Contains("\"success\":true");
        }
        catch (Exception ex)
        {
            Debug.LogError($"RegisterPlayer request failed: {ex.Message}");
            return false;
        }
    }
    
    public async Task AddItemAsync(string itemID, int itemCount)
    {
        if (!_isLoggedIn)
            return;
        
        var data = new
        {
            playerID = PlayerId,
            itemID = itemID,
            itemCount = itemCount
        };

        string json = JsonConvert.SerializeObject(data);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync($"{ServerUrl}/addItem", content);

        string result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            Debug.Log($"Item added successfully: {result}");
        }
        else
        {
            Debug.LogError($"Error adding item: {result}");
        }
    }
    
    public async Task RemoveItemAsync(string itemID, int itemCount)
    {
        if (!_isLoggedIn)
            return;
        
        var data = new
        {
            playerID = PlayerId,
            itemID = itemID,
            itemCount = itemCount
        };

        string json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync($"{ServerUrl}/removeItem", content);

        string result = await response.Content.ReadAsStringAsync();
        
        if (response.IsSuccessStatusCode)
        {
            Debug.Log($"Item remove successfully: {result}");
        }
        else
        {
            Debug.LogError($"Error remove item: {result}");
        }
    }
    
    public async Task RemoveAllItemAsync()
    {
        if (!_isLoggedIn)
            return;
        
        var data = new
        {
            playerID = PlayerId
        };

        string json = JsonConvert.SerializeObject(data);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync($"{ServerUrl}/removeAllItems", content);
        string result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            Debug.Log($"Все предметы удалены: {result}");
        }
        else
        {
            Debug.LogError($"Ошибка при удалении предметов: {result}");
        }
    }
    
    public async Task<bool> SendPlayerProgressAsync(PlayerProgress progress)
    {
        if (!_isLoggedIn)
            return false;
        
        try
        {
            string json = JsonConvert.SerializeObject(progress);
            using StringContent content = new(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _client.PostAsync($"{ServerUrl}/progress", content).ConfigureAwait(false);
            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"Error sending progress: {responseString}");
                return false;
            }

            Debug.Log($"Progress sent successfully: {responseString}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SendPlayerProgress request failed: {ex.Message}");
            return false;
        }
    }

}

[Serializable]
public class LoginResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("playerId")] 
    public int PlayerId { get; set; }
}

[Serializable]
public class PlayerProgress
{
    public int ID_Player;
    public int Money;
    public int HP;
    public int Level;
    public int Experience;
    
    public PlayerProgress(int idPlayer, int money, int hp, int level, int experience)
    {
        ID_Player = idPlayer;
        Money = money;
        HP = hp;
        Level = level;
        Experience = experience;
    }
}