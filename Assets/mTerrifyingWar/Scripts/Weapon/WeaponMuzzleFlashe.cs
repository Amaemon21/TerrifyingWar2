using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Weapon))]
public class WeaponMuzzleFlashe : MonoBehaviour
{
    [Inject] private readonly DiContainer _container;
    
    [SerializeField, BoxGroup("Main"), HorizontalLine] private Weapon _weapon;
    
    [SerializeField, BoxGroup("Muzzle Flash"), HorizontalLine] private MuzzleFlash[] _muzzlePrefabs;
    [SerializeField, BoxGroup("Muzzle Flash")] private bool _enableMuzzle = true;
    [SerializeField][Range(0.0f, 2.0f), BoxGroup("Muzzle Flash")] private float _scaleFactor = 1.0f;
    [SerializeField][Range(0.0f, 5.0f), BoxGroup("Muzzle Flash")] private float _destroyTime = 2.0f;

    private void OnEnable()
    {
        _weapon.OnShootChanged += CreateMuzzleFlash;
    }

    private void OnDisable()
    {
        _weapon.OnShootChanged -= CreateMuzzleFlash;
    }

    private void CreateMuzzleFlash()
    {
        WeaponUtilities.CreateMuzzleFlash(_container, _enableMuzzle, _muzzlePrefabs, _weapon.BarrelPoint, _scaleFactor, _destroyTime);
    }
}