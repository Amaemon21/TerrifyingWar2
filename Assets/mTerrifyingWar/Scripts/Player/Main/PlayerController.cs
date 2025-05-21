using DG.Tweening;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly IGameplayFactory _gameplayFactory;
    [Inject] private readonly PlayerProvider _playerProvider;
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly IStorageService _storagesService;
    
    [field: SerializeField] public FPSPlayerSettings PlayerSettings {get; private set;}

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
    }

    private void OnEnable()
    {
        _gameplayFactory.CreatePlayerChanged += Setup;
    }

    private void OnDisable()
    {
        _gameplayFactory.CreatePlayerChanged -= Setup;
    }

    private void Setup()
    {
        _triggerDisciplineLayerIndex = _playerProvider.WeaponContainer.HandAnimator.GetLayerIndex("TriggerDiscipline");
        _rightHandLayerIndex = _playerProvider.WeaponContainer.HandAnimator.GetLayerIndex("RightHand");
        _tacSprintLayerIndex = _playerProvider.WeaponContainer.HandAnimator.GetLayerIndex("TacSprint");
    }
    
    private void Update()
    {
        OnAim();

        UpdateAnimatorLayers();
        
        OnChangeFireMode();
        OnReload();
        OnSprint();

        if (Input.GetKeyDown(KeyCode.F5))
        {
            _storagesService.Save();
        }
    }
    
    private void OnChangeFireMode()
    {
        if (_inputService.ChangeFireMode)
        {
            FireMode prevFireMode = _playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot().FireMode;
            _playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot().OnFireModeChange();

            if (prevFireMode != _playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot().FireMode)
            {
                _playerSound.PlayFireModeSwitchSound();
                _weaponIKHandler.PlayIkMotion(PlayerSettings.fireModeMotion);
            }
        }
    }

    private void OnReload()
    {
        if (_inputService.IsReload && _playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot())
        {
            _playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponAmmo.OnReload();
        }
    }
    
    private void OnSprint()
    {
        _sprinting = _inputService.MoveDirection.magnitude >= 0.01f && _inputService.IsRun;

        if (!_sprinting)
            _tacSprinting = false;
    }

    private void OnAim()
    {
        var currentWeaponSlot = _playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot();
        if (currentWeaponSlot == null)
            return;

        float targetFov = _playerProvider.MainCamera.fieldOfView;
        bool wasAiming = _isAiming;

        _isAiming = _inputService.IsAim;
        _playerProvider.WeaponContainer.RecoilAnimation.isAiming = _isAiming;

        if (_displayProvider.AimPoint != null)
            _displayProvider.AimPoint.gameObject.SetActive(!_isAiming);
      
        if (wasAiming != _isAiming)
        {
            _playerSound.PlayAimSound(_isAiming);
            _weaponIKHandler.PlayIkMotion(PlayerSettings.aimingMotion);
        }
      
        targetFov = _isAiming 
            ? currentWeaponSlot.WeaponSettings.aimFov 
            : PlayerSettings.defaultFov;

        _playerProvider.MainCamera.DOFieldOfView(targetFov, 0.5f).SetLink(_playerProvider.MainCamera.gameObject);
    }
    
  //  public void OnTacSprint(InputValue value)
  //  {
  //      if (!_bSprinting)
  //          return;
  
  //      _bTacSprinting = value.isPressed;
  //  }
  
    private void UpdateAnimatorLayers()
    {
        AdsWeight = Mathf.Clamp01(AdsWeight + PlayerSettings.aimSpeed * Time.deltaTime * (_isAiming ? 1f : -1f));

        _smoothGait = Mathf.Lerp(_smoothGait, GetDesiredGait(), KMath.ExpDecayAlpha(PlayerSettings.gaitSmoothing, Time.deltaTime));

        _playerProvider.WeaponContainer.HandAnimator.SetFloat(AnimationsConstrains.GAIT, _smoothGait);
        _playerProvider.WeaponContainer.HandAnimator.SetLayerWeight(_tacSprintLayerIndex, Mathf.Clamp01(_smoothGait - 2f));

        if ( _playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot() == null) 
            return;
        
        bool triggerAllowed = _playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponSettings.useSprintTriggerDiscipline;

        _playerProvider.WeaponContainer.HandAnimator.SetLayerWeight(_triggerDisciplineLayerIndex, triggerAllowed ? _playerProvider.WeaponContainer.HandAnimator.GetFloat(AnimationsConstrains.TAC_SPRINT_WEIGHT) : 0f);
        
        _playerProvider.WeaponContainer.HandAnimator.SetLayerWeight(_rightHandLayerIndex, _playerProvider.WeaponContainer.HandAnimator.GetFloat(AnimationsConstrains.RIGHT_HAND_WEIGHT));
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