using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class WorkbenchInteractObject : InteractObject
{
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly UIWindowService _windowService;
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly PlayerProvider _playerProvider;
    
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    
    [field: SerializeField] public FactoryWeaponItem FactoryWeaponItem { get; private set; }

    private void Awake()
    {
        _cinemachineCamera.gameObject.SetActive(false);
    }

    protected override void OnInteract()
    {
        _displayProvider.WorkbenchSystem.Setup(this);
        
        _windowService.OpenWindow(WindowType.Workbench);
        
        if (_playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot())
            _playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponAnimator.OnUnEquipped();
        
        _cinemachineCamera.gameObject.SetActive(true);
        _playerProvider.CinematicCamera.gameObject.SetActive(false);
    }

    public void Exit()
    {
        _playerProvider.CinematicCamera.gameObject.SetActive(true);
        _cinemachineCamera.gameObject.SetActive(false);
     
        if (_playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot())
            _playerProvider.WeaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponAnimator.OnEquipped();
    }
}