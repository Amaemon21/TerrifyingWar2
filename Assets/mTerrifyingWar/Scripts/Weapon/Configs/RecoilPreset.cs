using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Source/Camera/Recoil Preset", fileName = "Recoil Preset", order = 0)]
public class RecoilPreset : ScriptableObject
{
    [field: SerializeField, BoxGroup("Recoil"), HorizontalLine] public Vector2 HorizontalRecoil { get; private set; }
    [field: SerializeField, BoxGroup("Recoil")] public Vector2 VerticalRecoil { get; private set; }
    [field: SerializeField, BoxGroup("Recoil")] [Min(0f)] public float HorizontalSmoothing { get; private set; }
    [field: SerializeField, BoxGroup("Recoil")] [Min(0f)] public float VerticalSmoothing { get; private set; }
    [field: SerializeField, BoxGroup("Recoil")] [Min(0f)] public float Damping { get; private set; }
}