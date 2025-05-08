using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class GrenadeHandler : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    
    [SerializeField] private WeaponContainer _weaponContainer;
    [SerializeField] private FPSPlayerSettings _playerSettings;
    
    private void Update()
    {
        OnThrowGrenade();
    }
    
    private void OnThrowGrenade()
    {
        if (_inputService.ThrowGrenade)
        {
            _weaponContainer.HandAnimator.SetTrigger(AnimationsConstrains.THROW_GRENADE);
            StartCoroutine(AfterDelay(_weaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponAnimator.UnEquipDelay, ThrowGrenade));
        }
    }

    private void ThrowGrenade()
    {
        _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().gameObject.SetActive(false);
        //StartCoroutine(AfterDelay(_playerSettings.grenadeDelay, _weaponProvider.WeaponHolder.EquipWeapon));
    }
    
    private IEnumerator AfterDelay(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
