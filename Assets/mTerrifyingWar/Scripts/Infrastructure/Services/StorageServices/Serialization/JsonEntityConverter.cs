using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonEntityConverter : JsonConverter<Entity>
{
    private static readonly JsonSerializer _entityInternalSerializer = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
    
    public override void WriteJson(JsonWriter writer, Entity value, JsonSerializer serializer)
    {
        _entityInternalSerializer.Serialize(writer, value);
    }

    public override Entity ReadJson(JsonReader reader, Type objectType, Entity existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);
        EntityType type = jsonObject.GetValue("EntityType").ToObject<EntityType>();

        return type switch
        {
            EntityType.PlayerEntity => jsonObject.ToObject<PlayerEntity>(_entityInternalSerializer),
            EntityType.ItemEntity => jsonObject.ToObject<ItemEntity>(_entityInternalSerializer),
            EntityType.WeaponItemEntity => jsonObject.ToObject<WeaponItemEntity>(_entityInternalSerializer),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}