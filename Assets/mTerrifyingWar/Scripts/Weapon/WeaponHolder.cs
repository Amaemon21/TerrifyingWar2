using UnityEngine;
using Zenject;

public class WeaponHolder : MonoBehaviour
{
    [Inject] private readonly Inventory _inventory;
    [Inject] private readonly DiContainer _container;
    [Inject] private readonly AmmoView _ammoView;

    private int _currentWeaponIndex = 1;

    private Weapon _currentWeapon;
    private Weapon _primaryWeapon;
    private Weapon _secondWeapon;
    
    private void OnEnable()
    {
        _inventory.RequestPrimaryWeaponChanged += RequestPrimaryWeapon;
        _inventory.RequestSecondWeaponChanged += RequestSecondWeapon;
    }

    private void OnDisable()
    {
        _inventory.RequestPrimaryWeaponChanged -= RequestPrimaryWeapon;
        _inventory.RequestSecondWeaponChanged -= RequestSecondWeapon;
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
        UpdateWeapon(ref _primaryWeapon, _inventory.PrimaryWeapon, 1);
    }

    private void RequestSecondWeapon()
    {
        UpdateWeapon(ref _secondWeapon, _inventory.SecondWeapon, 2);
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
            _ammoView.gameObject.SetActive(true);
        }
        else
        {
            _ammoView.gameObject.SetActive(false);
        }
    }

    private Weapon CreateWeapon(WeaponInventoryItemConfig weaponConfig)
    {
        if (weaponConfig == null)
            return null;

        var weapon = _container.InstantiatePrefabForComponent<Weapon>(weaponConfig.WeaponHandPrefab, transform);
        weapon.SetupWeapon(weaponConfig);
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