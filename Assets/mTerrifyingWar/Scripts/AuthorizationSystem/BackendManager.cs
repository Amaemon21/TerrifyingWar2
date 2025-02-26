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
    public string PlayerName { get; private set; } = "";
    
    public async Task<bool> CheckUserAsync(string login, string password)
    {
        var requestData = new { login, password };
        string json = JsonConvert.SerializeObject(requestData);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await _client.PostAsync($"{ServerUrl}/checkUser", content);
            string responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"CheckUser error: {response.StatusCode} - {responseString}");
                return false;
            }

            CheckUserResponse result = JsonConvert.DeserializeObject<CheckUserResponse>(responseString);
            PlayerName = result.Success ? result.PlayerName : "";
            return result.Success;
        }
        catch (Exception ex)
        {
            Debug.LogError($"CheckUser request failed: {ex.Message}");
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
            {
                Debug.LogError($"RegisterPlayer error: {response.StatusCode} - {responseString}");
                return false;
            }

            return responseString.Contains("\"success\":true");
        }
        catch (Exception ex)
        {
            Debug.LogError($"RegisterPlayer request failed: {ex.Message}");
            return false;
        }
    }
}

[Serializable]
public class CheckUserResponse
{
    public bool Success;
    public string PlayerName;
}
