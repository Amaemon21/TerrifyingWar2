using Newtonsoft.Json;

public class JsonProjectSettings
{
    public void Initialize()
    {
        ApplyProjectSerializationSettings();
    }
    
    private void ApplyProjectSerializationSettings()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new JsonEntityConverter());
        
        JsonConvert.DefaultSettings = () => settings;
    }
}