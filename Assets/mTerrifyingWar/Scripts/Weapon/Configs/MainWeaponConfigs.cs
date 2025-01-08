using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "MainWeaponConfig", menuName = "Weapon/MainWeaponConfig")]
public class MainWeaponConfigs : ScriptableObject
{
    [field: SerializeField, BoxGroup("Main"), HorizontalLine] public WeaponType WeaponType { get; private set; } = WeaponType.AssaultRifle;
    [field: SerializeField, BoxGroup("Main")] public LayerMask HitScanMask { get; private set; }
    
    [field: SerializeField][Range(0.0f, 10.0f)]
    [field: BoxGroup("Main")] public float ReloadTime { get; private set; } = 2.0f;
    
    [field: SerializeField][Range(0.0f, 15.0f)]
    [field: BoxGroup("Main")] public float ReloadFullTime { get; private set; } = 3.0f;
    
    [field: SerializeField, BoxGroup("Spread"), HorizontalLine] public bool ApplySpread { get; private set; } = true;
    [field: SerializeField, BoxGroup("Spread")] public Vector3 SpreadVariance { get; private set; } = new(1.0f, 1.0f, 1.0f);

    [field: SerializeField, BoxGroup("Transform"), HorizontalLine] public Vector3 DefaultPosition { get; private set; }
}
