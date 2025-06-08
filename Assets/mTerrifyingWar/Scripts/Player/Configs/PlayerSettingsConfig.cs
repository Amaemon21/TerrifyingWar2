using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/Player Settings")]
public class PlayerSettingsConfig : ScriptableObject
{
    public float grenadeDelay = 0f;
    public float gaitSmoothing = 0f;

    public float defaultFov = 80f;
    
    [Range(0f, 1f)] public float ikWeight = 1f;
    public float aimSpeed = 0f;

    public IKMotion aimingMotion;
    public IKMotion fireModeMotion;

    public List<AudioClip> generalSounds;
    
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