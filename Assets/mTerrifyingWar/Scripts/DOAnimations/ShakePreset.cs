using UnityEngine;

[CreateAssetMenu(menuName = "Source/Camera/Shake Preset", fileName = "Shake Preset", order = 0)]
public class ShakePreset : ScriptableObject
{
    [SerializeField] private ShakePositionRotationSettings shakeSettings;

    public ShakePositionRotationSettings ShakeSettings => shakeSettings;
}
