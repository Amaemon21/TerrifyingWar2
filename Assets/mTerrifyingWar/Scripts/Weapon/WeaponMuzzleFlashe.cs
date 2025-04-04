using KINEMATION.FPSAnimationPack.Scripts.Weapon;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponMuzzleFlashe : MonoBehaviour
{
    [SerializeField, BoxGroup("Muzzle Flash"), HorizontalLine] private MuzzleFlash[] _muzzlePrefabs;
    [SerializeField, BoxGroup("Muzzle Flash")] private bool _enableMuzzle = true;
    [SerializeField][Range(0.0f, 2.0f), BoxGroup("Muzzle Flash")] private float _scaleFactor = 1.0f;
    [SerializeField][Range(0.0f, 5.0f), BoxGroup("Muzzle Flash")] private float _destroyTime = 2.0f;
    
    private Weapon weapon;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    private void OnEnable()
    {
        weapon.OnShootChanged += CreateMuzzleFlash;
    }

    private void OnDisable()
    {
        weapon.OnShootChanged -= CreateMuzzleFlash;
    }

    private void CreateMuzzleFlash()
    {
        WeaponUtilities.CreateMuzzleFlash(_enableMuzzle, _muzzlePrefabs, weapon.BarrelPoint, _scaleFactor, _destroyTime);
    }
}
