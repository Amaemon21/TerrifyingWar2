using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsConfig", menuName = "Settings/PlayerSettingsConfig")]
public class PlayerSettingsConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Sensitivity"), HorizontalLine] public float SensitivityX { get; private set; }
    [field: SerializeField, BoxGroup("Sensitivity")] public float SensitivityY { get; private set; }

    public void SetSensetivityX(float value)
    {
        SensitivityX = Mathf.Round(value * 10) / 10;
    }
    
    public void SetSensetivityY(float value)
    {
        SensitivityY = Mathf.Round(value * 10) / 10;
    }
}