using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimatorWeaponConfig", menuName = "Weapon/AnimatorWeaponConfig")]
public class AnimatorWeaponConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Animation"), HorizontalLine] public string ShootName { get; private set; } = "Shoot";
    [field: SerializeField, BoxGroup("Animation")] public string[] AimShootsName { get; private set; }
    [field: SerializeField, BoxGroup("Animation")] public string ReloadBoolName { get; private set; } = "Reload";
    [field: SerializeField, BoxGroup("Animation")] public string ReloadFullBoolName { get; private set; } = "Reload Full";
    [field: SerializeField, BoxGroup("Animation")] public string AimBoolName { get; private set; } = "Aim";
    [field: SerializeField, BoxGroup("Animation")] public string AimWalkBoolName { get; private set; } = "AimWalk";
    [field: SerializeField, BoxGroup("Animation")] public string HideWaponName { get; private set; } = "Hide";
}