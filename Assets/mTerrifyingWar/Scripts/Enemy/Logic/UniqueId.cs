using NaughtyAttributes;
using UnityEngine;

public class UniqueId : MonoBehaviour
{
    [field: SerializeField, BoxGroup("ID"), HorizontalLine] public string Id { get; private set; }

    public void SetupId(string id)
    {
        Id = id;
    }
}