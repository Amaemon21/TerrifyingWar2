using UnityEngine;
using Zenject;

public class PlayerLook : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    
    [SerializeField] private PlayerSettingsConfig _playerSettingsConfig;
    [SerializeField] private RecoilPattern _recoilPattern;
    [SerializeField] private Transform _cameraTransform;
    
    private Vector3 _headRecoilOffset;
    private Vector2 _playerInput;
    private Transform _transform;
    
    private void Awake()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        Look();
    }
    
    private void Look()
    {
        float deltaMouseX = _inputService.LookDirection.x * (_playerSettingsConfig.SensitivityX / 10f);
        float deltaMouseY = -_inputService.LookDirection.y  * (_playerSettingsConfig.SensitivityY / 10f);
            
        _playerInput.y += deltaMouseY;
        _playerInput.x += deltaMouseX;

        _playerInput += _recoilPattern.AccumulatedRecoil;
        deltaMouseX += _recoilPattern.AccumulatedRecoil.x;
        
        Vector2 pitchClamp = Vector2.Lerp(new Vector2(-90f, 90f), new Vector2(-30, 0f), 0);

        _playerInput.y = Mathf.Clamp(_playerInput.y, pitchClamp.x, pitchClamp.y);
            
        _transform.rotation *= Quaternion.Euler(0f, deltaMouseX, 0f);
        _cameraTransform.localRotation = Quaternion.Euler(_playerInput.y, 0, 0);
    }
}