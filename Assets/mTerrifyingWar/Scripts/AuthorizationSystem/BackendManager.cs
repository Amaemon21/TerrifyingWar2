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

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Login request failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterPlayerAsync(string playerName, string login, string password)
    {
        var requestData = new { playerName, login, password };
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
    
    public async Task AddItemAsync(int playerID, string playerName, int itemID, int itemCount)
    {
        var data = new
        {
            playerID,
            playerName,
            itemID,
            itemCount
        };

        string json = JsonConvert.SerializeObject(data);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync($"{ServerUrl}/addItem", content);

        string result = await response.Content.ReadAsStringAsync();
        Debug.Log(result);
    }

    public async Task RemoveItemAsync(int playerID, int itemID, int itemCount)
    {
        var data = new
        {
            playerID,
            itemID,
            itemCount
        };

        string json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync($"{ServerUrl}/removeItem", content);

        string result = await response.Content.ReadAsStringAsync();
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
