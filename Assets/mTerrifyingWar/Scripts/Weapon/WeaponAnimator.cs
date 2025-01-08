using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class WeaponAnimator : MonoBehaviour
{
    private AnimatorWeaponConfig _animatorWeaponConfig;

    private Animator _animator;

    private void Awake()
    {
        _animatorWeaponConfig = Resources.Load<AnimatorWeaponConfig>("Configs/WeaponsConfigs/AnimatorWeaponConfig");
        
        _animator = GetComponent<Animator>();   
        
        _animator.applyRootMotion = false;
        _animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
    }

    public void PlayShootAnimation(bool isAiming)
    {
        _animator.Play(isAiming ? _animatorWeaponConfig.AimShootsName[Random.Range(0, _animatorWeaponConfig.AimShootsName.Length)] : _animatorWeaponConfig.ShootName);
    }

    public void SetAimState(bool isAiming)
    {
        _animator.SetBool(_animatorWeaponConfig.AimBoolName, isAiming);
    }
    
    public void SetAimWalkState(bool isAiming)
    {
        _animator.SetBool(_animatorWeaponConfig.AimWalkBoolName, isAiming);
    }

    public void PlayReloadAnimation(bool isFullReload)
    {
        string reloadName = isFullReload ? _animatorWeaponConfig.ReloadFullBoolName : _animatorWeaponConfig.ReloadBoolName;
        _animator.SetBool(reloadName, true);
    }

    public void StopReloadAnimation(bool isFullReload)
    {
        string reloadName = isFullReload ? _animatorWeaponConfig.ReloadFullBoolName : _animatorWeaponConfig.ReloadBoolName;
        _animator.SetBool(reloadName, false);
    }

    public void SetMovementState(bool isWalking, bool isRunning)
    {
        _animator.SetBool("Walk", isWalking);
        _animator.SetBool("Run", isRunning);
    }

    public void PlayHideAnimation()
    {
        _animator.Play(_animatorWeaponConfig.HideWaponName);
    }
}