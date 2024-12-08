using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class WeaponsInstaller : MonoInstaller
{
    [FormerlySerializedAs("_weaponRecoil")] [SerializeField] private WeaponRecoilAndShake weaponRecoilAndShake;
    [SerializeField] private ShootTransform _shootTransform;
    [SerializeField] private AmmoView _ammoView;
    [FormerlySerializedAs("_aimImage")] [SerializeField] private AimPoint aimPoint;
    
    public override void InstallBindings()
    {
        Container.Bind<WeaponRecoilAndShake>().FromInstance(weaponRecoilAndShake).AsSingle();
        Container.Bind<ShootTransform>().FromInstance(_shootTransform).AsSingle();
        Container.Bind<AmmoView>().FromInstance(_ammoView).AsSingle();
        Container.Bind<AimPoint>().FromInstance(aimPoint).AsSingle();
    }
}
