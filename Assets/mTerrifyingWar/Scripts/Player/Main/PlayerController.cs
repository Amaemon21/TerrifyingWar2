using KINEMATION.ProceduralRecoilAnimationSystem.Runtime;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly WeaponProvider _weaponProvider;
    [Inject] private readonly FPSPlayerSettings _playerSettings;

    private PlayerSound _playerSound;
    private WeaponIKHandler _weaponIKHandler;

    private int _tacSprintLayerIndex;
    private int _triggerDisciplineLayerIndex;
    private int _rightHandLayerIndex;

    private bool _isAiming;

    private float _smoothGait;
    private bool _sprinting;
    private bool _tacSprinting;
    
    public float AdsWeight {get; private set;}
    
    private void Awake()
    {
        _playerSound = GetComponent<PlayerSound>();
        _weaponIKHandler = GetComponent<WeaponIKHandler>();

        _triggerDisciplineLayerIndex = _weaponProvider.Animator.GetLayerIndex("TriggerDiscipline");
        _rightHandLayerIndex = _weaponProvider.Animator.GetLayerIndex("RightHand");
        _tacSprintLayerIndex = _weaponProvider.Animator.GetLayerIndex("TacSprint");
    }

    private void Update()
    {
        OnAim();

        UpdateAnimatorLayers();
        
        OnChangeFireMode();
        OnReload();
        OnSprint();
    }
    
    private void OnChangeFireMode()
    {
        if (_inputService.ChangeFireMode)
        {
            FireMode prevFireMode = _weaponProvider.WeaponHolder.GetActiveWeapon().FireMode;
            _weaponProvider.WeaponHolder.GetActiveWeapon().OnFireModeChange();

            if (prevFireMode != _weaponProvider.WeaponHolder.GetActiveWeapon().FireMode)
            {
                _playerSound.PlayFireModeSwitchSound();
                _weaponIKHandler.PlayIkMotion(_playerSettings.fireModeMotion);
            }
        }
    }

    private void OnReload()
    {
        if (_inputService.IsReload)
        {
            _weaponProvider.WeaponHolder.GetActiveWeapon().OnReload();
        }
    }
    
    private void OnSprint()
    {
        if (_inputService.MoveDirection.magnitude >= 0.01f)
        {
            _sprinting = _inputService.IsRun;
        }
        else
        {
            _sprinting = false;
        }
        
        if (!_sprinting)
            _tacSprinting = false;
    }

  //  public void OnTacSprint(InputValue value)
  //  {
  //      if (!_bSprinting)
  //          return;
  
  //      _bTacSprinting = value.isPressed;
  //  }

    private void OnAim()
    {
        bool wasAiming = _isAiming;
        _isAiming = _inputService.IsAim;
        _weaponProvider.RecoilAnimation.isAiming = _isAiming;

        if (wasAiming != _isAiming)
        {
            _playerSound.PlayAimSound(_isAiming);
            _weaponIKHandler.PlayIkMotion(_playerSettings.aimingMotion);
        }
    }
    
    private void UpdateAnimatorLayers()
    {
        AdsWeight = Mathf.Clamp01(AdsWeight + _playerSettings.aimSpeed * Time.deltaTime * (_isAiming ? 1f : -1f));

        _smoothGait = Mathf.Lerp(_smoothGait, GetDesiredGait(), KMath.ExpDecayAlpha(_playerSettings.gaitSmoothing, Time.deltaTime));

        _weaponProvider.Animator.SetFloat(AnimationsConstrains.GAIT, _smoothGait);
        _weaponProvider.Animator.SetLayerWeight(_tacSprintLayerIndex, Mathf.Clamp01(_smoothGait - 2f));

        if ( _weaponProvider.WeaponHolder.GetActiveWeapon() == null) 
            return;
        
        bool triggerAllowed = _weaponProvider.WeaponHolder.GetActiveWeapon().WeaponSettings.useSprintTriggerDiscipline;

        _weaponProvider.Animator.SetLayerWeight(_triggerDisciplineLayerIndex, triggerAllowed ? _weaponProvider.Animator.GetFloat(AnimationsConstrains.TAC_SPRINT_WEIGHT) : 0f);
        
        _weaponProvider.Animator.SetLayerWeight(_rightHandLayerIndex, _weaponProvider.Animator.GetFloat(AnimationsConstrains.RIGHT_HAND_WEIGHT));
    }
    
    private float GetDesiredGait()
    {
        if (_tacSprinting)
            return 3f;

        if (_sprinting)
            return 2f;

        return _inputService.MoveDirection.magnitude;
    }
}