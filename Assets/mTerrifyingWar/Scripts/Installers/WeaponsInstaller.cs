using UnityEngine;
using Zenject;

public class WeaponsInstaller : MonoInstaller
{
    [SerializeField] private WeaponRecoilAndShake weaponRecoilAndShake;
    [SerializeField] private ShootTransform _shootTransform;
    [SerializeField] private WeaponCamera _weaponCamera;
    [SerializeField] private AmmoView _ammoView;
    [SerializeField] private AimPoint aimPoint;
    
    public override void InstallBindings()
    {
        Container.Bind<WeaponRecoilAndShake>().FromInstance(weaponRecoilAndShake).AsSingle();
        Container.Bind<ShootTransform>().FromInstance(_shootTransform).AsSingle();
        Container.Bind<AmmoView>().FromInstance(_ammoView).AsSingle();
        Container.Bind<AimPoint>().FromInstance(aimPoint).AsSingle();
        Container.Bind<WeaponCamera>().FromInstance(_weaponCamera).AsSingle();
    }
}
