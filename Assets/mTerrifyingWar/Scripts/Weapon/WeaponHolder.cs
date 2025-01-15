using UnityEngine;
using Zenject;

public class WeaponHolder : MonoBehaviour
{
    [Inject] private DisplayProvider _displayProvider;
    [Inject] private IGameplayFactory _gameplayFactory;
    [Inject] private readonly DiContainer _container;
    
    [SerializeField] private WeaponRecoilAndShake _weaponRecoilAndShake;
    [SerializeField] private WeaponCamera _weaponCamera;

    private int _currentWeaponIndex = 1;

    private Weapon _currentWeapon;
    private Weapon _primaryWeapon;
    private Weapon _secondWeapon;
    
    private void OnEnable()
    {
        _gameplayFactory.CreateHudChanged += Setup;
    }

    private void OnDisable()
    {
        _displayProvider.Inventory.RequestPrimaryWeaponChanged -= RequestPrimaryWeapon;
        _displayProvider.Inventory.RequestSecondWeaponChanged -= RequestSecondWeapon;
    }

    private void Setup()
    {
        _displayProvider.Inventory.RequestPrimaryWeaponChanged += RequestPrimaryWeapon;
        _displayProvider.Inventory.RequestSecondWeaponChanged += RequestSecondWeapon;
    }

    private void Update()
    {
        HandleWeaponSwitchInput();
    }

    private void HandleWeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(1, _primaryWeapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(2, _secondWeapon);
        }
    }

    private void RequestPrimaryWeapon()
    {
        UpdateWeapon(ref _primaryWeapon, _displayProvider.Inventory.PrimaryWeapon, 1);
    }

    private void RequestSecondWeapon()
    {
        UpdateWeapon(ref _secondWeapon, _displayProvider.Inventory.SecondWeapon, 2);
    }

    private void UpdateWeapon(ref Weapon weaponSlot, WeaponInventoryItemConfig weaponConfig, int slotIndex)
    {
        if (weaponConfig != null)
        {
            DestroyWeapon(weaponSlot);
            weaponSlot = CreateWeapon(weaponConfig);

            if (_currentWeaponIndex == slotIndex)
            {
                SwitchWeapon(slotIndex, weaponSlot);
            }
            else
            {
                weaponSlot.gameObject.SetActive(false);
            }
        }
        else
        {
            DestroyWeapon(weaponSlot);
            weaponSlot = null;
        }
    }

    private void SwitchWeapon(int weaponIndex, Weapon newWeapon)
    {
        if (_currentWeapon == newWeapon)
            return;

        if (_currentWeapon != null)
        {
            _currentWeapon.HideWeapon(() =>
            {
                _currentWeapon = newWeapon;
                _currentWeaponIndex = weaponIndex;
                UpdateWeaponVisibility();
            });
        }
        else
        {
            _currentWeapon = newWeapon;
            _currentWeaponIndex = weaponIndex;
            UpdateWeaponVisibility();
        }
    }

    private void UpdateWeaponVisibility()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.gameObject.SetActive(true);
            _displayProvider.AmmoView.gameObject.SetActive(true);
        }
        else
        {
            _displayProvider.AmmoView.gameObject.SetActive(false);
        }
    }

    private Weapon CreateWeapon(WeaponInventoryItemConfig weaponConfig)
    {
        if (weaponConfig == null)
            return null;

        var weapon = _container.InstantiatePrefabForComponent<Weapon>(weaponConfig.WeaponHandPrefab, transform);
        weapon.SetupWeapon(weaponConfig, _weaponRecoilAndShake, _weaponCamera);
        weapon.gameObject.SetActive(false);
        
        return weapon;
    }

    private void DestroyWeapon(Weapon weapon)
    {
        if (weapon == null)
            return;
        
        if (_currentWeapon == weapon)
        {
            _currentWeapon = null;
            UpdateWeaponVisibility();
        }

        Destroy(weapon.gameObject);
    }
}