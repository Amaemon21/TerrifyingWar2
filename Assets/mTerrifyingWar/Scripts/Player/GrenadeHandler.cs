using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class GrenadeHandler : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    
    [SerializeField] private WeaponContainer _weaponContainer;
    [SerializeField] private PlayerSettingsConfig playerSettingsConfig;

    private Weapon _currentWeapon;
    private float _delay = 1;
    
    private void Update()
    {
        OnThrowGrenade();
    }
    
    private void OnThrowGrenade()
    {
        if (_inputService.ThrowGrenade)
        {
            _weaponContainer.HandAnimator.SetTrigger(AnimationsConstrains.THROW_GRENADE);
            
            _currentWeapon = _weaponContainer.WeaponHolder.GetCurrentWeaponSlot();

            if (_currentWeapon != null)
                _delay = _currentWeapon.WeaponAnimator.UnEquipDelay;
            
            StartCoroutine(AfterDelay(_delay, ThrowGrenade));
        }
    }

    private void ThrowGrenade()
    {
        if (_currentWeapon != null)
            _currentWeapon.gameObject.SetActive(false);

        StartCoroutine(AfterDelay(playerSettingsConfig.grenadeDelay, _weaponContainer.WeaponHolder.ForceEquipCurrentWeapon));
    }
    
    private IEnumerator AfterDelay(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}