using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "MainWeaponConfig", menuName = "Weapon/MainWeaponConfig")]
public class WeaponConfigs : ScriptableObject
{
    [field: SerializeField, BoxGroup("Main"), HorizontalLine] public WeaponType WeaponType { get; private set; } = WeaponType.AssaultRifle;
    [field: SerializeField, BoxGroup("Main")] public LayerMask HitScanMask { get; private set; }
    
    [field: SerializeField, BoxGroup("Spread"), HorizontalLine] public bool ApplySpread { get; private set; } = true;
    [field: SerializeField, BoxGroup("Spread")] public Vector3 SpreadVariance { get; private set; } = new(1.0f, 1.0f, 1.0f);
    [field: SerializeField, BoxGroup("Transform"), HorizontalLine] public Vector3 DefaultPosition { get; private set; }
    
    [field: SerializeField, BoxGroup("Audio"), HorizontalLine] public AudioClip EmptyMagSound { get; private set; }
    [field: SerializeField, BoxGroup("Audio")] public AudioClip ReloadSound { get; private set; }
    [field: SerializeField, BoxGroup("Audio")] public AudioClip FullReloadSound { get; private set; }
    [field: SerializeField, BoxGroup("Audio")] public AudioClip EquipSound { get; private set; }
    [field: SerializeField, Space, BoxGroup("Audio")] public AudioClip[] ShootSounds { get; private set; }
}
