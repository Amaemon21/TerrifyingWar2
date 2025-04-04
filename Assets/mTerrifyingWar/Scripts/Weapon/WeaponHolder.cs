using System;
using System.Collections;
using System.Collections.Generic;
using KINEMATION.KAnimationCore.Runtime.Core;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class WeaponHolder : MonoBehaviour
{
    [Inject] private DisplayProvider _displayProvider;
    [Inject] private readonly DiContainer _container;
    [Inject] private readonly FPSPlayerSettings _playerSettings;
    [Inject] private readonly WeaponProvider _weaponProvider;
    
    [SerializeField] private RuntimeAnimatorController _defaultAnimator;

    private int _activeWeaponIndex = 0;
    
    private List<Weapon> _weapons = new();
    
    private Weapon _primaryWeapon;
    private Weapon _secondWeapon;

    private KTransform _root;
    private KTransform _localCamera;

    private void OnEnable()
    {
        _displayProvider.Inventory.RequestPrimaryWeaponChanged += RequestPrimaryWeapon;
        _displayProvider.Inventory.RequestSecondWeaponChanged += RequestSecondWeapon;
    }
    
    private void OnDisable()
    {
        _displayProvider.Inventory.RequestPrimaryWeaponChanged -= RequestPrimaryWeapon;
        _displayProvider.Inventory.RequestSecondWeaponChanged -= RequestSecondWeapon;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnChangeWeapon();
        }
    }

    private void RequestPrimaryWeapon()
    {
        UpdateWeapon(ref _primaryWeapon, _displayProvider.Inventory.PrimaryWeapon);
    }

    private void RequestSecondWeapon()
    {
        UpdateWeapon(ref _secondWeapon, _displayProvider.Inventory.SecondWeapon);
    }
    
    private void UpdateWeapon(ref Weapon weaponSlot, WeaponInventoryItemConfig weaponConfig)
    {
        if (weaponConfig != null)
        {
            weaponSlot = SpawnWeapon(weaponConfig);
        
            if (_activeWeaponIndex == 0)
            {
                if (_primaryWeapon != null)
                {
                    _primaryWeapon.gameObject.SetActive(true);
                    _primaryWeapon.OnEquipped();
                }
            }
            else if (_activeWeaponIndex == 1)
            {
                if (_secondWeapon != null)
                {
                    _secondWeapon.gameObject.SetActive(true);
                    _secondWeapon.OnEquipped();
                }
            }
        }
        else
        {
            var slot = weaponSlot;
            
            StartCoroutine(AfterDelay(weaponSlot.OnUnEquipped(), () =>
            {
                Destroy(slot.gameObject);
                _weaponProvider.Animator.runtimeAnimatorController = _defaultAnimator;
            }));
        }
    }
    
    private void EquipWeapon_Incremental()
    {
        GetActiveWeapon().gameObject.SetActive(false);
        _activeWeaponIndex = (_activeWeaponIndex + 1) % _weapons.Count;
        GetActiveWeapon().OnEquipped();
        StartCoroutine(AfterDelay(0.05f, () => GetActiveWeapon().gameObject.SetActive(true)));
    }

    public void EquipWeapon()
    {
        GetActiveWeapon().gameObject.SetActive(false);
        GetActiveWeapon().OnEquipped(true);
        StartCoroutine(AfterDelay(0.05f, () => GetActiveWeapon().gameObject.SetActive(true)));
    }

    private void OnChangeWeapon()
    {
        if (_weapons.Count <= 1)
            return;

        float delay = GetActiveWeapon().OnUnEquipped();
        StartCoroutine(AfterDelay(delay, EquipWeapon_Incremental));
    }

    private IEnumerator AfterDelay(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
    
    public Weapon GetActiveWeapon()
    {
        if (_weapons.Count <= 0)
        {
            return null;
        }
        
        return _weapons[_activeWeaponIndex];
    }

    private Weapon SpawnWeapon(WeaponInventoryItemConfig weapon)
    {
        if (weapon == null)
            return null;
        
        _root = new KTransform(_weaponProvider.TransformsContainer.CameraPoint);
        _localCamera = _root.GetRelativeTransform(new KTransform(_weaponProvider.TransformsContainer.CameraPoint), false);
        
        Weapon instance = _container.InstantiatePrefabForComponent<Weapon>(weapon.WeaponHandPrefab, _weaponProvider.TransformsContainer.WeaponBone);
        instance.GameObject().SetActive(false);
            
        instance.Initialize(gameObject, weapon);

        KTransform weaponT = new KTransform(_weaponProvider.TransformsContainer.WeaponBone);
        instance.rightHandPose = new KTransform(_weaponProvider.TransformsContainer.RightHand.tip).GetRelativeTransform(weaponT, false);

        KTransform localWeapon = _root.GetRelativeTransform(weaponT, false);

        localWeapon.rotation *= AnimationsConstrains.ANIMATED_OFFSET;

        instance.adsPose.position = _localCamera.position - localWeapon.position;
        instance.adsPose.rotation = Quaternion.Inverse(localWeapon.rotation);

        _weapons.Add(instance);
        return instance;
    }
}