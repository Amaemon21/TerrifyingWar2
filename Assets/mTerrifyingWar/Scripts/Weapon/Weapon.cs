using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(WeaponAnimator))]
[RequireComponent(typeof(WeaponVisualEffects))]
[RequireComponent(typeof(WeaponAudio))]
public class Weapon : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly PlayerProvider _playerProvider;

    [SerializeField, BoxGroup("Main Weapon Config"), HorizontalLine] private MainWeaponConfigs _mainWeaponConfigs;

    [SerializeField, BoxGroup("Shake Preset"), HorizontalLine] private ShakePreset _shakePreset;
    
    [SerializeField, BoxGroup("Recoil Preset"), HorizontalLine] private RecoilPreset _recoilPreset;
    
    private bool _canShoot = true;
    private bool _canReload = true;
    private bool _canScope = true;
    private bool _inScope = false;
    private bool _isReload = false;
    
    private WeaponAnimator _weaponAnimator;
    private WeaponAudio _weaponAudio;
    private WeaponVisualEffects _weaponVisualEffects;
    
    private Transform _transform;
    
    private float _lastShootTime = 0.0f;
    private Vector3 _finalDirection;

    private AmmoInventoryItemConfig _ammoInventoryItemConfig = null;
    public WeaponInventoryItemConfig WeaponInventoryItemConfig { get; private set; }

    public void SetupWeapon(WeaponInventoryItemConfig weaponInventoryItemConfig)
    {
        WeaponInventoryItemConfig = weaponInventoryItemConfig;
    }
    
    private void Awake()
    {
        _transform = transform;

        _weaponAnimator = GetComponent<WeaponAnimator>();
        _weaponAudio = GetComponent<WeaponAudio>();
        _weaponVisualEffects = GetComponent<WeaponVisualEffects>();
    }

    private void OnEnable()
    {
        _displayProvider.Inventory.ItemAddedInventoryChanged += RequestAmmo;
        _displayProvider.Inventory.ItemRemoveInventoryChanged += RemoveAmmo;
        
        RequestAmmo();
    }

    private void OnDisable()
    {
        _displayProvider.Inventory.ItemAddedInventoryChanged -= RequestAmmo;
        _displayProvider.Inventory.ItemRemoveInventoryChanged -= RemoveAmmo;
    }

    private void Update()
    {
        _transform.localPosition = _mainWeaponConfigs.DefaultPosition;

        ShootButtonChecker();
        ReloadChecker();
        Aiming();
        
        ApplyRecoil();

        PlayWeaponAnimations();
    }
    
    private void ApplyRecoil()
    {
        _playerProvider.WeaponRecoilAndShake.Recoil(_recoilPreset.ReturnSpeed, _recoilPreset.Snappiness);
    }
    
    private void ShootButtonChecker()
    {
        if (_canShoot)
        {
            if (Time.time - _lastShootTime > 1.0f / (WeaponInventoryItemConfig.FireRate / 60.0f))
            {
                if (WeaponInventoryItemConfig.CurrentAmmo > 0)
                {
                    if (_mainWeaponConfigs.WeaponType == WeaponType.AssaultRifle)
                    {
                        if (_inputService.IsShoot)
                        {
                            Shoot();
                        }
                    }
                    else if (_mainWeaponConfigs.WeaponType  == WeaponType.Pistol)
                    {
                        if (_inputService.IsShoot)
                        {
                            Shoot();
                            //_inputService.ResetShoot();
                        }
                    }
                }
            }
        }
    }

    private void ReloadChecker()
    {
        if (_canReload)
        {
            if (!_inScope)
            {
                if (!_inputService.IsRun)
                {
                    if (_inputService.IsReload)
                    {
                        if (_ammoInventoryItemConfig != null && _ammoInventoryItemConfig.ItemCount > 0 && WeaponInventoryItemConfig.CurrentAmmo < WeaponInventoryItemConfig.MagazineSize)
                        {
                            StartCoroutine(ReloadCoroutine());
                        }
                    }
                    else if (_ammoInventoryItemConfig != null && _ammoInventoryItemConfig.ItemCount > 0 && WeaponInventoryItemConfig.CurrentAmmo == 0)
                    {
                        StartCoroutine(ReloadCoroutine());
                    }
                }
            }
        }
    }
    
    private void Shoot()
    {
        _lastShootTime = Time.time;

        if (_inScope)
        {
            HitScan(new Ray(_playerProvider.MainCamera.transform.position, _playerProvider.MainCamera.transform.forward));

            _weaponAnimator.PlayShootAnimation(_inScope);
        }
        else
        {
            _finalDirection = WeaponUtilities.GetDirection(_transform.forward, _mainWeaponConfigs.ApplySpread, _mainWeaponConfigs.SpreadVariance);

            HitScan(new Ray(_playerProvider.MainCamera.transform.position, _finalDirection));

            _weaponAnimator.PlayShootAnimation(_inScope);
        }

        _weaponVisualEffects.CreateMuzzleFlash();
        _weaponAudio.PlayShootSound();
        
        _playerProvider.WeaponRecoilAndShake.PlayShakeAnimation(_shakePreset);
        _playerProvider.WeaponRecoilAndShake.AddRecoil(_recoilPreset);
        
        WeaponInventoryItemConfig.RemoveCurrentAmmo();
        
        HandleDisplayAmmo();
        
        _weaponVisualEffects.CreateShell();

        _displayProvider.AmmoView.PlayShootAnimation();
    }
    
    private void HitScan(Ray ray)
    {
        if (Physics.Raycast(ray, out var hit, int.MaxValue, _mainWeaponConfigs.HitScanMask))
        {
            _weaponVisualEffects.CreateTrail(hit.point);

            Collider hitCollider = hit.collider;

            if (hitCollider.TryGetComponent(out Impact impact))
            {
                GameObject spawnedObject = Instantiate(impact.ImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));

                spawnedObject.transform.parent = hitCollider.transform;

                if (spawnedObject.TryGetComponent(out ImpactSound impactSound))
                {
                    impactSound.PlaySound();
                }
                
                if (hitCollider.TryGetComponent(out BodyPart bodyPart))
                {
                    Vector3 force = ray.direction * 150;
                    Vector3 hitPosition = hit.point;

                    bodyPart.Hit(force, hitPosition);
                    bodyPart.TakeDamage(WeaponInventoryItemConfig.Damage);
                }
                
                Destroy(spawnedObject, 2.0f);
            }
        }
        else
        {
            if (!_inScope)
                _weaponVisualEffects.CreateTrail(_weaponVisualEffects.BarrelTransform.position + _finalDirection * 100);
            else
                _weaponVisualEffects.CreateTrail(_playerProvider.MainCamera.transform.position + _playerProvider.MainCamera.transform.forward * 100);
        }
    }
    
    private void Aiming()
    {
        if (!_inputService.IsRun)
        {
            if (_canScope)
            {
                _inScope = _inputService.IsAim;

                _weaponAnimator.SetAimState(_inScope);
                _displayProvider.AimPoint.gameObject.SetActive(!_inScope);
                
                DOTween.Kill(_playerProvider.MainCamera);
                DOTween.Kill(_playerProvider.WeaponCamera);
                
                _playerProvider.WeaponCamera.DOFieldOfView(_inScope ? 50 : 60, 0.5f).SetId(_playerProvider.WeaponCamera);
                _playerProvider.MainCamera.DOFieldOfView(_inScope ? 50 : 60, 0.5f).SetId(_playerProvider.MainCamera);
            }
        }
    }


    public void ResetCurrentAmmo()
    {
        WeaponInventoryItemConfig.ResetCurrentAmmo();
    }

    public void ResetAvailableAmmo()
    {
        if (_ammoInventoryItemConfig != null)
        {
            _ammoInventoryItemConfig.ResetCount();
            _displayProvider.Inventory.RemoveItem(_ammoInventoryItemConfig);
        }
    }

    private void RequestAmmo()
    {
        if (WeaponInventoryItemConfig != null)
        {
            _ammoInventoryItemConfig = _displayProvider.InventorySystem.RequestAmmo(WeaponInventoryItemConfig.AmmoID);
        }

        HandleDisplayAmmo();
    }

    private void RemoveAmmo(InventoryItemConfig config)
    {
        if (config is AmmoInventoryItemConfig ammoInventoryItem)
        {
            if (ammoInventoryItem.ItemID == WeaponInventoryItemConfig.AmmoID)
            {
                _ammoInventoryItemConfig = null;
                HandleDisplayAmmo();
            }
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        _canShoot = false;
        _canScope = false;
        _canReload = false;
        _isReload = true;

        _weaponVisualEffects.CreateMag(WeaponInventoryItemConfig);

        if (WeaponInventoryItemConfig.CurrentAmmo > 0)
        {
            _weaponAudio.PlayReloadSound();

            _weaponAnimator.PlayReloadAnimation(false);

            yield return new WaitForSeconds(_mainWeaponConfigs.ReloadTime);

            _weaponAnimator.StopReloadAnimation(false);
        }
        else if (WeaponInventoryItemConfig.CurrentAmmo == 0)
        {
            _weaponAudio.PlayFullReloadSound();
            
            _weaponAnimator.PlayReloadAnimation(true);
            
            yield return new WaitForSeconds(_mainWeaponConfigs.ReloadFullTime);

            _weaponAnimator.StopReloadAnimation(true);
        }

        _canShoot = true;
        _canScope = true;
        _canReload = true;
        _isReload = false;

        AddAmmo();
        HandleDisplayAmmo();
    }
    
    private void AddAmmo()
    {
        int amountNeeded = WeaponInventoryItemConfig.MagazineSize - WeaponInventoryItemConfig.CurrentAmmo;

        if (amountNeeded >= _ammoInventoryItemConfig.ItemCount)
        {
            WeaponInventoryItemConfig.AddCurrentAmmo(_ammoInventoryItemConfig.ItemCount);
            _ammoInventoryItemConfig.RemoveCount(amountNeeded);
        }
        else
        {
            WeaponInventoryItemConfig.SetCurrentAmmo();
            _ammoInventoryItemConfig.RemoveCount(amountNeeded);
        }
    }
    
    private void HandleDisplayAmmo()
    {
        if (WeaponInventoryItemConfig != null)
        {
            if (_ammoInventoryItemConfig != null)
            {
                _displayProvider.AmmoView.DisplayAmmo(WeaponInventoryItemConfig.CurrentAmmo, _ammoInventoryItemConfig.ItemCount, this);
            }
            else
            {
                _displayProvider.AmmoView.DisplayAmmo(WeaponInventoryItemConfig.CurrentAmmo, 0, this);
            }
        }
    }
    
    private void PlayWeaponAnimations()
    {
        if (_inputService.MoveDirection.sqrMagnitude >= 0.01f)
        {
            _weaponAnimator.SetMovementState(true, _inputService.IsRun);

            if (_inputService.IsRun)
            {
                _canReload = false;
                _canShoot = false;
                _canScope = false;
                _inScope = false;
                
                _weaponAnimator.SetAimWalkState(false);
            }
            else
            {
                _canReload = !_isReload;
                _canShoot = !_isReload;
                _canScope = !_isReload;
                
                _weaponAnimator.SetAimWalkState(_inScope);
            }
        }
        else
        {
            _weaponAnimator.SetMovementState(false, false);
            _weaponAnimator.SetAimWalkState(false);
        }
    }

    public void HideWeapon(Action callback = null)
    {
        if (!_inScope)
        {
            if (!_isReload)
            {
                if (!_inputService.IsRun)
                {
                    _weaponAnimator.PlayHideAnimation();
                    StartCoroutine(Hide(callback));
                }
            }
        }
    }

    private IEnumerator Hide(Action callback)
    {
        yield return new WaitForSeconds(0.78f);
        gameObject.SetActive(false);
        callback?.Invoke();
    }
}