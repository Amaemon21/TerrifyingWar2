using System;
using System.Collections;
using System.Collections.Generic;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;
using Zenject;

public class WeaponHolder : MonoBehaviour
{
    [Inject] private readonly IGameplayFactory _gameplayFactory;
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly DiContainer _container;
    
    [SerializeField] private RuntimeAnimatorController _defaultAnimator;

    private readonly List<Weapon> _weapons = new();

    private Weapon _primaryWeapon;
    private Weapon _secondaryWeapon;

    private int _currentWeaponIndex = 0;
    
    private WeaponContainer _weaponContainer;
    
    private KTransform _root;
    private KTransform _localCamera;
    
    private SaveData _saveData;

    private void Awake()
    {
        _weaponContainer = GetComponent<WeaponContainer>();
    }

    private void OnEnable()
    {
        _gameplayFactory.CreateHudChanged += Setup;
    }

    private void Setup()
    {
        _displayProvider.Inventory.RequestPrimaryWeaponChanged += HandlePrimaryWeaponChanged;
        _displayProvider.Inventory.RequestSecondWeaponChanged += HandleSecondaryWeaponChanged;
    }

    private void OnDisable()
    {
        _gameplayFactory.CreateHudChanged -= Setup;
        
        _displayProvider.Inventory.RequestPrimaryWeaponChanged -= HandlePrimaryWeaponChanged;
        _displayProvider.Inventory.RequestSecondWeaponChanged -= HandleSecondaryWeaponChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeWeapon(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeWeapon(1);
    }

    private void HandlePrimaryWeaponChanged()
    {
        AssignWeapon(ref _primaryWeapon, _displayProvider.Inventory.PrimaryWeapon);
    }

    private void HandleSecondaryWeaponChanged()
    {
        AssignWeapon(ref _secondaryWeapon, _displayProvider.Inventory.SecondaryWeapon);
    }

    private void AssignWeapon(ref Weapon weaponSlot, WeaponInventoryItemConfig weaponConfig)
    {
        if (weaponConfig == null)
        {
            RemoveWeapon(weaponSlot);
            weaponSlot = null;
            return;
        }

        if (weaponSlot != null)
        {
            RemoveWeapon(weaponSlot);
            weaponSlot = null;
        }

        Weapon newWeapon = SpawnWeapon(weaponConfig);
        weaponSlot = newWeapon;

        if (GetCurrentWeaponSlot() == weaponSlot)
        {
            weaponSlot.gameObject.SetActive(true);
            weaponSlot.WeaponAnimator.OnEquipped();
        }
    }

    private void RemoveWeapon(Weapon weapon)
    {
        if (weapon == null) 
            return;

        _weapons.Remove(weapon);
        Destroy(weapon.gameObject);
        ResetAnimatorController();
    }

    private void ChangeWeapon(int weaponIndex)
    {
        if (_currentWeaponIndex == weaponIndex)
            return;

        var currentWeapon = GetCurrentWeaponSlot();

        if (currentWeapon != null)
        {
            float unequipDelay = currentWeapon.WeaponAnimator.OnUnEquipped();

            StartCoroutine(ExecuteAfterDelay(unequipDelay, () =>
            {
                EquipWeapon(weaponIndex);
            }));
        }
        else
        {
            EquipWeapon(weaponIndex);
        }
    }

    private void EquipWeapon(int weaponIndex)
    {
        if (GetCurrentWeaponSlot() != null)
            GetCurrentWeaponSlot().gameObject.SetActive(false);

        _currentWeaponIndex = weaponIndex;

        var weaponToEquip = GetCurrentWeaponSlot();

        if (weaponToEquip != null)
        {
            weaponToEquip.gameObject.SetActive(true);
            weaponToEquip.WeaponAnimator.OnEquipped();
        }
        else
        {
            ResetAnimatorController();
        }
    }

    public Weapon GetCurrentWeaponSlot()
    {
        return _currentWeaponIndex switch
        {
            0 => _primaryWeapon,
            1 => _secondaryWeapon,
            _ => null
        };
    }

    public void ForceEquipCurrentWeapon()
    {
        var weapon = GetCurrentWeaponSlot();

        if (weapon == null)
        {
            ResetAnimatorController();
            return;
        }

        weapon.gameObject.SetActive(false);
        weapon.WeaponAnimator.OnEquipped(true);

        StartCoroutine(ExecuteAfterDelay(0.05f, () => weapon.gameObject.SetActive(true)));
    }

    private IEnumerator ExecuteAfterDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    private Weapon SpawnWeapon(WeaponInventoryItemConfig weapon)
    {
        if (weapon == null)
            return null;
        
        _root = new KTransform(_weaponContainer.TransformsContainer.CameraPoint);
        _localCamera = _root.GetRelativeTransform(new KTransform(_weaponContainer.TransformsContainer.CameraPoint), false);
        
        Weapon instance = _container.InstantiatePrefabForComponent<Weapon>(weapon.WeaponHandPrefab, _weaponContainer.TransformsContainer.WeaponBone);
        instance.gameObject.SetActive(false);
        
        instance.Initialize(weapon, _weaponContainer);
        
        instance.WeaponAnimator.Initialize(gameObject);

        KTransform weaponT = new KTransform(_weaponContainer.TransformsContainer.WeaponBone);
        instance.rightHandPose = new KTransform(_weaponContainer.TransformsContainer.RightHand.tip).GetRelativeTransform(weaponT, false);

        KTransform localWeapon = _root.GetRelativeTransform(weaponT, false);

        localWeapon.rotation *= AnimationsConstrains.ANIMATED_OFFSET;

        instance.adsPose.position = _localCamera.position - localWeapon.position;
        instance.adsPose.rotation = Quaternion.Inverse(localWeapon.rotation);

        _weapons.Add(instance);
        return instance;
    }
    
    private void ResetAnimatorController()
    {
        _weaponContainer.HandAnimator.runtimeAnimatorController = _defaultAnimator;
    }
}