using UnityEngine;
using Zenject;

public class PlayerAnimator : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    
    [field: SerializeField] public PlayerSettingsConfig PlayerSettingsConfig {get; private set;}
    
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private WeaponContainer _weaponContainer;
    
    private readonly string TriggerDiscipline = "TriggerDiscipline";
    private readonly string RightHand = "RightHand";
    private readonly string TacSprint = "TacSprint";
    
    private int _tacSprintLayerIndex;
    private int _triggerDisciplineLayerIndex;
    private int _rightHandLayerIndex;
    
    private float _smoothGait;
    
    public float AdsWeight {get; private set;}
    
    private void Awake()
    {
        _triggerDisciplineLayerIndex = _weaponContainer.HandAnimator.GetLayerIndex(TriggerDiscipline);
        _rightHandLayerIndex = _weaponContainer.HandAnimator.GetLayerIndex(RightHand);
        _tacSprintLayerIndex = _weaponContainer.HandAnimator.GetLayerIndex(TacSprint);
    }
    
    private void Update()
    {
        UpdateAnimatorLayers();  
    }
    
    private void UpdateAnimatorLayers()
    {
        AdsWeight = Mathf.Clamp01(AdsWeight + PlayerSettingsConfig.aimSpeed * Time.deltaTime * (_playerController.IsAiming ? 1f : -1f));

        _smoothGait = Mathf.Lerp(_smoothGait, GetDesiredGait(), KMath.ExpDecayAlpha(PlayerSettingsConfig.gaitSmoothing, Time.deltaTime));

        _weaponContainer.HandAnimator.SetFloat(AnimationsConstrains.GAIT, _smoothGait);
        _weaponContainer.HandAnimator.SetLayerWeight(_tacSprintLayerIndex, Mathf.Clamp01(_smoothGait - 2f));

        if ( _weaponContainer.WeaponHolder.GetCurrentWeaponSlot() == null) 
            return;
        
        bool triggerAllowed = _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponSettings.useSprintTriggerDiscipline;

        _weaponContainer.HandAnimator.SetLayerWeight(_triggerDisciplineLayerIndex, triggerAllowed ? _weaponContainer.HandAnimator.GetFloat(AnimationsConstrains.TAC_SPRINT_WEIGHT) : 0f);
        
        _weaponContainer.HandAnimator.SetLayerWeight(_rightHandLayerIndex, _weaponContainer.HandAnimator.GetFloat(AnimationsConstrains.RIGHT_HAND_WEIGHT));
    }
    
    private float GetDesiredGait()
    {
        if (_playerController.IsTacSprinting)
            return 3f;

        if (_playerController.IsSprinting)
            return 2f;

        return _inputService.MoveDirection.magnitude;
    }
}