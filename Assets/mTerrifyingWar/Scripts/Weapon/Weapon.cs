using System;
using KINEMATION.KAnimationCore.Runtime.Core;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class Weapon : MonoBehaviour
{
    [HideInInspector] public KTransform rightHandPose;
    [HideInInspector] public KTransform adsPose;
    
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly IStorageService _storagesService;

    [field: SerializeField] public WeaponSettings WeaponSettings { get; private set; }
    [field: SerializeField] public Transform AimPoint { get; private set; }
    [field: SerializeField] public Transform BarrelPoint { get; private set; }
    [field: SerializeField] public WeaponAnimator WeaponAnimator { get; private set; }
    [field: SerializeField] public WeaponAmmo WeaponAmmo { get; private set; }
    
    private bool _canFire = true;
    private bool _isFiring;
    private float _lastShootTime = 0.0f;
    
    private SaveData _saveData;
    private WeaponItemEntity weaponItemEntity;

    public FireMode FireMode { get; private set; } = FireMode.Semi;
    public WeaponContainer WeaponContainer { get; private set; }
    public WeaponInventoryItemConfig WeaponInventoryItemConfig { get; private set; }
    
    public event Action OnShootChanged;

    private void OnEnable()
    {
        _inputService.OnShootStart += HandleFirePressed;
        _inputService.OnShootEnd += HandleFireReleased;
        
        _displayProvider.AmmoView.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _inputService.OnShootStart -= HandleFirePressed;
        _inputService.OnShootEnd -= HandleFireReleased;

        if (_displayProvider.AmmoView != null)
            _displayProvider.AmmoView.gameObject.SetActive(false);
    }
    
    public void Initialize(WeaponInventoryItemConfig weaponInventoryItemConfig, WeaponContainer weaponContainer)
    {
        WeaponInventoryItemConfig = weaponInventoryItemConfig;
        WeaponContainer = weaponContainer;
        
        _storagesService.Load(LoadData);
        
        if (WeaponInventoryItemConfig.ScopeInventoryItemConfig != null)
        {
            Scope scope = Instantiate(weaponInventoryItemConfig.ScopeInventoryItemConfig.Scope, transform);
            scope.transform.localPosition = weaponInventoryItemConfig.ScopeInventoryItemConfig.Position;
            scope.transform.localRotation = Quaternion.identity; 
            AimPoint = scope.AimPoint;
        }

        WeaponAmmo.Setup();
    }

    public void ChangeCanFire(bool canFire)
    {
        _canFire = canFire;
    }
    
    public void OnFireModeChange()
    {
        FireMode = FireMode == FireMode.Auto ? FireMode.Semi : WeaponSettings.fullAuto ? FireMode.Auto : FireMode.Semi;
        WeaponContainer.RecoilAnimation.fireMode = FireMode;
    }
    
    private void Update()
    {
        if (_isFiring && FireMode == FireMode.Auto && CanShoot())
        {
            Shoot();

            if (WeaponInventoryItemConfig.CurrentAmmo <= 0)
            {
                StopFiring();
            }
        }
    }

    private void HandleFirePressed()
    {
        switch (FireMode)
        {
            case FireMode.Semi:
                Shoot();
                break;
            case FireMode.Auto:
                _isFiring = true;
                break;
        }
    }

    private void HandleFireReleased()
    {
        if (FireMode == FireMode.Auto)
        {
            _isFiring = false;
        }

        WeaponContainer.RecoilAnimation.Stop();
        WeaponContainer.RecoilPattern.OnFireEnd();
    }

    private void Shoot()
    {
        if (!CanShoot())
            return;

        WeaponContainer.RecoilPattern.OnFireStart();
        WeaponContainer.RecoilAnimation.Play();

        OnShootChanged?.Invoke();
        
        WeaponInventoryItemConfig.RemoveCurrentAmmo();
        IncrementAmmoSaver();
        WeaponAmmo.HandleDisplayAmmo();
        _lastShootTime = Time.time;
    }

    private void StopFiring()
    {
        _isFiring = false;
        _canFire = false;
        HandleFireReleased();
    }
    
    private bool CanShoot()
    {
        return _canFire && !WeaponAmmo.IsReloading && WeaponInventoryItemConfig.CurrentAmmo > 0 && Time.time >= _lastShootTime + (60f / WeaponSettings.fireRate);
    }

    private void IncrementAmmoSaver()
    {
        weaponItemEntity.CurrentAmmo--;
        
        _storagesService.Save(_saveData);
    }

    private void LoadData(SaveData saveData)
    {
        _saveData = saveData;

        ItemEntity item = _saveData.InventoryEntity.FindItemByID(WeaponInventoryItemConfig.ItemID);

        if (item is WeaponItemEntity weaponInventoryItemData)
        {
            weaponItemEntity = weaponInventoryItemData;
        }
    }
}