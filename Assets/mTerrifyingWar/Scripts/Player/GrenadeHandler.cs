using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class GrenadeHandler : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly WeaponProvider _weaponProvider;
    [Inject] private readonly FPSPlayerSettings _playerSettings;
    
    private void Update()
    {
        OnThrowGrenade();
    }
    
    private void OnThrowGrenade()
    {
        if (_inputService.ThrowGrenade)
        {
            _weaponProvider.Animator.SetTrigger(AnimationsConstrains.THROW_GRENADE);
            StartCoroutine(AfterDelay(_weaponProvider.WeaponHolder.GetActiveWeapon().UnEquipDelay, ThrowGrenade));
        }
    }

    private void ThrowGrenade()
    {
        _weaponProvider.WeaponHolder.GetActiveWeapon().gameObject.SetActive(false);
        StartCoroutine(AfterDelay(_playerSettings.grenadeDelay, _weaponProvider.WeaponHolder.EquipWeapon));
    }
    
    private IEnumerator AfterDelay(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
