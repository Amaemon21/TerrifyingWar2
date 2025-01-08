using System;
using System.Collections.Generic;

public class SaveDataKeysProvider: IKeysProvider
{
    private readonly IReadOnlyDictionary<Type, string> _map = new Dictionary<Type, string>
        {        
            { typeof(PlayerData), "PlayerData" },
            { typeof(InventoryData), "InventoryData" },
        };

    public string Provide<TData>() => _map[typeof(TData)];
    
    public IEnumerable<string> ProvideAll() => _map.Values;
}