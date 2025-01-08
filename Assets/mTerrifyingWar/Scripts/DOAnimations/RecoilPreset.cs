using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Source/Camera/Recoil Preset", fileName = "Recoil Preset", order = 0)]
public class RecoilPreset : ScriptableObject
{
    [field: SerializeField, BoxGroup("Recoil"), HorizontalLine] public float RecoilX { get; private set; } = 1.5f;
    [field: SerializeField, BoxGroup("Recoil")] public float RecoilY { get; private set; } = 2.0f;
    [field: SerializeField, BoxGroup("Recoil")] public float RecoilZ { get; private set; } = 1.0f;
    [field: SerializeField, BoxGroup("Recoil")] public float Snappiness { get; private set; } = 2.0f;
    [field: SerializeField, BoxGroup("Recoil")] public float ReturnSpeed { get; private set; } = 5.0f;
}