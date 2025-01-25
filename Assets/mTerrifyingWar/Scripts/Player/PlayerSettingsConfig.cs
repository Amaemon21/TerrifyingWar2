using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsConfig", menuName = "Settings/PlayerSettingsConfig")]
public class PlayerSettingsConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Health"), HorizontalLine] public float MaxHealth { get; private set; } = 100f;
}