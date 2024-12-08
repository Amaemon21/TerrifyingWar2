using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(WeaponAnimator))]
[RequireComponent(typeof(WeaponVisualEffects))]
[RequireComponent(typeof(WeaponAudio))]
public class Weapon : MonoBehaviour
{
    [Inject] private readonly InputService _inputService;
    [Inject] private readonly EventBus _eventBus;
    [Inject] private readonly WeaponRecoilAndShake _weaponRecoilAndShake;
    [Inject] private readonly ShootTransform _shootTransform;
    [Inject] private readonly AimPoint _aimPoint;
    [Inject] private readonly AmmoView _ammoView;
    [Inject] private readonly Inventory _inventory;
    
    [SerializeField, BoxGroup("Main")] private WeaponType _type = WeaponType.AssaultRifle;
    [SerializeField, BoxGroup("Main")] private LayerMask _hitScanMask;
    [SerializeField][Range(0.0f, 10.0f), BoxGroup("Main")] private float _reloadTime = 2.0f;
    [SerializeField][Range(0.0f, 15.0f), BoxGroup("Main")] private float _reloadFullTime = 3.0f;

    [SerializeField, BoxGroup("Shake Preset"), HorizontalLine] private ShakePreset _shakePreset;
    
    [SerializeField, BoxGroup("Recoil Preset"), HorizontalLine] private RecoilPreset _recoilPreset;

    [SerializeField, BoxGroup("Spread"), HorizontalLine] private bool _applySpread = true;
    [SerializeField, BoxGroup("Spread")] private Vector3 _spreadVariance = new(1.0f, 1.0f, 1.0f);

    [SerializeField, BoxGroup("Transform"), HorizontalLine] private Vector3 _defaultPosition;
    
    [SerializeField, BoxGroup("State"), HorizontalLine] private bool _canShoot = true;
    [SerializeField, BoxGroup("State")] private bool _canReload = true;
    [SerializeField, BoxGroup("State")] private bool _canScope = true;
    [SerializeField, BoxGroup("State")] private bool _inScope = false;
    [SerializeField, BoxGroup("State")] private bool _isReload = false;
    
    private WeaponAnimator _weaponAnimator;
    private WeaponAudio _weaponAudio;
    private WeaponVisualEffects _weaponVisualEffects;

    private Transform _transform;
    
    private float _lastShootTime = 0.0f;
    private Vector3 _finalDirection;

    private AmmoInventoryItemConfig _ammoInventoryItemConfig = null;
    private WeaponInventoryItemConfig _weaponInventoryItemConfig = null;

    public WeaponInventoryItemConfig WeaponInventoryItemConfig => _weaponInventoryItemConfig;

    public void SetupWeapon(WeaponInventoryItemConfig weaponInventoryItemConfig)
    {
        _weaponInventoryItemConfig = weaponInventoryItemConfig;
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
        _inputService.ReloadChanged += ReloadChecker;
        _inputService.AimChanged += Aiming;
        
        _weaponAudio.PlayEquipSound();
        
        _eventBus.ItemAddedInventoryChanged.AddListener(RequestAmmo);
        _eventBus.RemoveItemToInventoryChanged.AddListener(RemoveAmmo);
        
        RequestAmmo();
    }

    private void OnDisable()
    {
        _inputService.ReloadChanged -= ReloadChecker;
        _inputService.AimChanged -= Aiming;
        
        _eventBus.ItemAddedInventoryChanged.RemoveListener(RequestAmmo);
        _eventBus.RemoveItemToInventoryChanged.RemoveListener(RemoveAmmo);
    }

    private void Update()
    {
        _transform.localPosition = _defaultPosition;

        ShootButtonChecker();
        
        ApplyRecoil();

        PlayWeaponAnimations();
    }
    
    private void ApplyRecoil()
    {
        _weaponRecoilAndShake.Recoil(_recoilPreset.ReturnSpeed, _recoilPreset.Snappiness);
    }
    
    private void ShootButtonChecker()
    {
        if (_canShoot)
        {
            if (Time.time - _lastShootTime > 1.0f / (_weaponInventoryItemConfig.FireRate / 60.0f))
            {
                if (_weaponInventoryItemConfig.CurrentAmmo > 0)
                {
                    if (_type == WeaponType.AssaultRifle)
                    {
                        if (_inputService.IsShoot)
                        {
                            Shoot();
                        }
                    }
                    else if (_type == WeaponType.Pistol)
                    {
                        if (_inputService.IsShoot)
                        {
                            Shoot();
                            _inputService.ResetShoot();
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
                    if (_ammoInventoryItemConfig != null && _ammoInventoryItemConfig.ItemCount > 0 && _weaponInventoryItemConfig.CurrentAmmo < _weaponInventoryItemConfig.MagazineSize)
                    {
                        StartCoroutine(ReloadCoroutine());
                    }
                    else if (_ammoInventoryItemConfig != null && _ammoInventoryItemConfig.ItemCount > 0 && _weaponInventoryItemConfig.CurrentAmmo == 0)
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
            HitScan(new Ray(_shootTransform.transform.position, _shootTransform.transform.forward));

            _weaponAnimator.PlayShootAnimation(_inScope);
        }
        else
        {
            _finalDirection = WeaponUtilities.GetDirection(_transform.forward, _applySpread, _spreadVariance);

            HitScan(new Ray(_shootTransform.transform.position, _finalDirection));

            _weaponAnimator.PlayShootAnimation(_inScope);
        }

        _weaponRecoilAndShake.PlayShakeAnimation(_shakePreset);
        
        _weaponRecoilAndShake.AddRecoil(_recoilPreset);
        
        _weaponInventoryItemConfig.RemoveCurrentAmmo();
        HandleDisplayAmmo();
        
        _weaponVisualEffects.CreateMuzzleFlash();

        _weaponAudio.PlayShootSound();
        
        _weaponVisualEffects.CreateShell();

        _ammoView.PlayShootAnimation();
    }
    
    private void HitScan(Ray ray)
    {
        if (Physics.Raycast(ray, out var hit, int.MaxValue, _hitScanMask))
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
                    bodyPart.TakeDamage(_weaponInventoryItemConfig.Damage);
                }
                
                Destroy(spawnedObject, 2.0f);
            }
        }
        else
        {
            if (!_inScope)
                _weaponVisualEffects.CreateTrail(_weaponVisualEffects.BarrelTransform.position + _finalDirection * 100);
            else
                _weaponVisualEffects.CreateTrail(_shootTransform.transform.position + _shootTransform.transform.forward * 100);
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
                _aimPoint.gameObject.SetActive(!_inScope);
            }
        }
    }

    public void ResetCurrentAmmo()
    {
        _weaponInventoryItemConfig.ResetCurrentAmmo();
    }

    public void ResetAvailableAmmo()
    {
        if (_ammoInventoryItemConfig != null)
        {
            _ammoInventoryItemConfig.ResetCount();
            _inventory.RemoveItem(_ammoInventoryItemConfig);
        }
    }

    private void RequestAmmo()
    {
        if (_weaponInventoryItemConfig != null)
        {
            _ammoInventoryItemConfig = _inventory.RequestAmmo(_weaponInventoryItemConfig.AmmoID);
        }

        HandleDisplayAmmo();
    }

    private void RemoveAmmo(InventoryItemConfig config)
    {
        if (config is AmmoInventoryItemConfig ammoInventoryItem)
        {
            if (ammoInventoryItem.ItemID == _weaponInventoryItemConfig.AmmoID)
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

        _weaponVisualEffects.CreateMag(_weaponInventoryItemConfig);

        if (_weaponInventoryItemConfig.CurrentAmmo > 0)
        {
            _weaponAudio.PlayReloadSound();

            _weaponAnimator.PlayReloadAnimation(false);

            yield return new WaitForSeconds(_reloadTime);

            _weaponAnimator.StopReloadAnimation(false);
        }
        else if (_weaponInventoryItemConfig.CurrentAmmo == 0)
        {
            _weaponAudio.PlayFullReloadSound();
            
            _weaponAnimator.PlayReloadAnimation(true);
            
            yield return new WaitForSeconds(_reloadFullTime);

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
        int amountNeeded = _weaponInventoryItemConfig.MagazineSize - _weaponInventoryItemConfig.CurrentAmmo;

        if (amountNeeded >= _ammoInventoryItemConfig.ItemCount)
        {
            _weaponInventoryItemConfig.AddCurrentAmmo(_ammoInventoryItemConfig.ItemCount);
            _ammoInventoryItemConfig.RemoveCount(amountNeeded);
        }
        else
        {
            _weaponInventoryItemConfig.SetCurrentAmmo();
            _ammoInventoryItemConfig.RemoveCount(amountNeeded);
        }
    }
    
    private void HandleDisplayAmmo()
    {
        if (_weaponInventoryItemConfig != null)
        {
            if (_ammoInventoryItemConfig != null)
            {
                _ammoView.DisplayAmmo(_weaponInventoryItemConfig.CurrentAmmo, _ammoInventoryItemConfig.ItemCount, this);
            }
            else
            {
                _ammoView.DisplayAmmo(_weaponInventoryItemConfig.CurrentAmmo, 0, this);
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
            }
            else
            {
                _canReload = !_isReload;
                _canShoot = !_isReload;
                _canScope = !_isReload;
            }
        }
        else
        {
            _weaponAnimator.SetMovementState(false, false);
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