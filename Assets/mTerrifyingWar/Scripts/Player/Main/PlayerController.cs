using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly DisplayProvider _displayProvider;
    
    [field: SerializeField] public PlayerSettingsConfig PlayerSettingsConfig {get; private set;}

    [SerializeField] private PlayerSound _playerSound;
    [SerializeField] private WeaponIKHandler _weaponIKHandler;
    [SerializeField] private WeaponContainer _weaponContainer;
    
    public bool IsAiming {get; private set;}

    public bool IsSprinting {get; private set;}
    public bool IsTacSprinting {get; private set;}
    
    private void Update()
    {
        OnAim();
        OnChangeFireMode();
        OnReload();
        OnSprint();
    }
    
    private void OnChangeFireMode()
    {
        if (_inputService.ChangeFireMode)
        {
            FireMode prevFireMode = _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().FireMode;
            _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().OnFireModeChange();

            if (prevFireMode != _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().FireMode)
            {
                _playerSound.PlayFireModeSwitchSound();
                _weaponIKHandler.PlayIkMotion(PlayerSettingsConfig.fireModeMotion);
            }
        }
    }

    private void OnReload()
    {
        if (_inputService.IsReload && _weaponContainer.WeaponHolder.GetCurrentWeaponSlot())
        {
            _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponAmmo.OnReload();
        }
    }
    
    private void OnSprint()
    {
        IsSprinting = _inputService.MoveDirection.magnitude >= 0.01f && _inputService.IsRun;

        if (!IsSprinting)
            IsTacSprinting = false;
    }

    private void OnAim()
    {
        Weapon currentWeaponSlot = _weaponContainer.WeaponHolder.GetCurrentWeaponSlot();
        
        if (currentWeaponSlot == null)
            return;
        
        bool wasAiming = IsAiming;

        IsAiming = _inputService.IsAim;
        _weaponContainer.RecoilAnimation.isAiming = IsAiming;

        if (_displayProvider.AimPoint != null)
            _displayProvider.AimPoint.gameObject.SetActive(!IsAiming);
      
        if (wasAiming != IsAiming)
        {
            _playerSound.PlayAimSound(IsAiming);
            _weaponIKHandler.PlayIkMotion(PlayerSettingsConfig.aimingMotion);
        }
    }
    
  //  public void OnTacSprint(InputValue value)
  //  {
  //      if (!_bSprinting)
  //          return;
  
  //      _bTacSprinting = value.isPressed;
  //  }
    

}