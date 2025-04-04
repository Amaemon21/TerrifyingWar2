using System;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;
using Zenject;

public class Weapon : MonoBehaviour
{
    [HideInInspector] public KTransform rightHandPose;
    [HideInInspector] public KTransform adsPose;
    
    [Inject] protected readonly WeaponProvider _weaponProvider;
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly IInputService _inputService;

    [field: SerializeField] public WeaponSettings WeaponSettings { get; private set; }
    [field: SerializeField] public Transform AimPoint { get; private set; }
    [field: SerializeField] public Transform BarrelPoint { get; private set; }
    
    [SerializeField] private RecoilPreset _recoilPreset;
    
    [SerializeField] protected Animator _weaponAnimator;
    
    protected float TacReloadDelay;
    protected bool IsReloading;
    
    private float _emptyReloadDelay;
    private bool _canFire = true;
    private bool _isFiring;
    private float _lastShootTime = 0.0f;

    public FireMode FireMode { get; private set; } = FireMode.Semi;
    public float UnEquipDelay { get; private set; }
    
    public AmmoInventoryItemConfig AmmoInventoryItemConfig { get; private set; }
    public WeaponInventoryItemConfig WeaponInventoryItemConfig { get; private set; }
    
    public event Action OnShootChanged;

    public virtual void Initialize(GameObject owner, WeaponInventoryItemConfig weaponInventoryItemConfig)
    {
        WeaponInventoryItemConfig = weaponInventoryItemConfig;
        
        AnimationClip idlePose = null;

        foreach (var clip in WeaponSettings.characterController.animationClips)
        {
            if (clip.name.Contains("Reload"))
            {
                if (clip.name.Contains("Tac"))
                    TacReloadDelay = clip.length;

                if (clip.name.Contains("Empty"))
                    _emptyReloadDelay = clip.length;

                continue;
            }

            if (clip.name.ToLower().Contains("unequip"))
            {
                UnEquipDelay = clip.length;
                continue;
            }

            if (idlePose != null)
                continue;

            if (clip.name.Contains("Idle") || clip.name.Contains("Pose"))
                idlePose = clip;
        }

        if (idlePose != null)
        {
            idlePose.SampleAnimation(owner, 0f);
        }
    }
    
    public void OnEquipped_Immediate()
    {
        _weaponProvider.Animator.runtimeAnimatorController = WeaponSettings.characterController;
        _weaponAnimator.Play(AnimationsConstrains.IDLE, -1, 0f);
        _weaponProvider.RecoilAnimation.Init(WeaponSettings.recoilAnimData, WeaponSettings.fireRate, FireMode);
    }

    public void OnEquipped(bool fastEquip = false)
    {
        _weaponProvider.Animator.runtimeAnimatorController = WeaponSettings.characterController;
        _weaponProvider.RecoilAnimation.Init(WeaponSettings.recoilAnimData, WeaponSettings.fireRate, FireMode);

        // Reset the default pose to idle.
        _weaponProvider.Animator.Play(AnimationsConstrains.IDLE, -1, 0f);

        // Play the equip animation.
        if (WeaponSettings.hasEquipOverride)
        {
            _weaponProvider.Animator.Play("IKMovement", -1, 0f);
            _weaponProvider.Animator.Play(fastEquip ? AnimationsConstrains.EQUIP : AnimationsConstrains.EQUIP_OVERRIDE, -1, 0f);
            return;
        }

        // Play the curve-based equipping animation.
        _weaponProvider.Animator.Play(AnimationsConstrains.EQUIP, -1, 0f);
    }

    public float OnUnEquipped()
    {
        _weaponProvider.Animator.SetTrigger(AnimationsConstrains.UNEQUIP);
        return UnEquipDelay + 0.05f;
    }
    
    public void OnFireModeChange()
    {
        FireMode = FireMode == FireMode.Auto ? FireMode.Semi : WeaponSettings.fullAuto ? FireMode.Auto : FireMode.Semi;
        _weaponProvider.RecoilAnimation.fireMode = FireMode;
    }

    private void OnEnable()
    {
        _inputService.OnShootStart += HandleFirePressed;
        _inputService.OnShootEnd += HandleFireReleased;
        
        _displayProvider.Inventory.ItemAddedInventoryChanged += RequestAmmo;
        _displayProvider.Inventory.ItemRemoveInventoryChanged += RemoveAmmo;
        
        RequestAmmo();
    }

    private void OnDisable()
    {
        _inputService.OnShootStart -= HandleFirePressed;
        _inputService.OnShootEnd -= HandleFireReleased;
        
        _displayProvider.Inventory.ItemAddedInventoryChanged -= RequestAmmo;
        _displayProvider.Inventory.ItemRemoveInventoryChanged -= RemoveAmmo;
    }
    
    public virtual void OnReload()
    {
        if (AmmoInventoryItemConfig != null && AmmoInventoryItemConfig.ItemCount > 0 && WeaponInventoryItemConfig.CurrentAmmo < WeaponInventoryItemConfig.MagazineSize)
        {
            if (WeaponInventoryItemConfig.CurrentAmmo == WeaponInventoryItemConfig.MagazineSize)
                return;

            var reloadHash = WeaponInventoryItemConfig.CurrentAmmo == 0 ? AnimationsConstrains.RELOAD_EMPTY : AnimationsConstrains.RELOAD_TAC;
            _weaponAnimator.Play(reloadHash, -1, 0f);

            _weaponProvider.Animator.Play(reloadHash, -1, 0f);
            Invoke(nameof(AddAmmo), WeaponInventoryItemConfig.CurrentAmmo == 0 ? _emptyReloadDelay : TacReloadDelay);
            IsReloading = true;
        }
    }

    private void Update()
    {
        if (_isFiring && FireMode == FireMode.Auto && CanShoot())
        {
            Shoot();
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

        _weaponProvider.RecoilAnimation.Stop();
        _weaponProvider.RecoilPattern.OnFireEnd();
    }

    private void Shoot()
    {
        if (!CanShoot())
            return;

        _weaponProvider.RecoilPattern.OnFireStart();
        _weaponProvider.RecoilAnimation.Play();
        PlayShootingEffects();

        OnShootChanged?.Invoke();
        WeaponInventoryItemConfig.RemoveCurrentAmmo();
        HandleDisplayAmmo();
        _lastShootTime = Time.time;

        if (WeaponInventoryItemConfig.CurrentAmmo <= 0)
        {
            StopFiring();
        }
    }

    private void StopFiring()
    {
        _isFiring = false;
        _canFire = false;
        HandleFireReleased();
    }

    private bool CanShoot()
    {
        return _canFire && !IsReloading && WeaponInventoryItemConfig.CurrentAmmo > 0 && Time.time >= _lastShootTime + (60f / WeaponSettings.fireRate);
    }

    private void PlayShootingEffects()
    {
        if (WeaponSettings.useFireClip)
        {
            _weaponProvider.Animator.Play(AnimationsConstrains.FIRE, -1, 0f);
        }

        _displayProvider.AmmoView.PlayShootAnimation();
        
        _weaponAnimator.Play(WeaponSettings.hasFireOut && WeaponInventoryItemConfig.CurrentAmmo == 1 ? AnimationsConstrains.FIREOUT : AnimationsConstrains.FIRE, -1, 0f);
    }

    public void ResetAvailableAmmo()
    {
        if (AmmoInventoryItemConfig != null)
        {
            AmmoInventoryItemConfig.ResetCount();
            _displayProvider.Inventory.RemoveItem(AmmoInventoryItemConfig);
        }
    }

    private void RequestAmmo()
    {
        if (WeaponInventoryItemConfig != null)
        {
            AmmoInventoryItemConfig = _displayProvider.InventorySystem.RequestAmmo(WeaponInventoryItemConfig.EAmmoType);
        }

        HandleDisplayAmmo();
    }

    private void RemoveAmmo(InventoryItemConfig config)
    {
        if (config is AmmoInventoryItemConfig ammoInventoryItem)
        {
            if (ammoInventoryItem.EAmmoType == WeaponInventoryItemConfig.EAmmoType)
            {
                AmmoInventoryItemConfig = null;
                HandleDisplayAmmo();
            }
        }
    }
    
    protected void AddAmmo()
    {
        int amountNeeded = WeaponInventoryItemConfig.MagazineSize - WeaponInventoryItemConfig.CurrentAmmo;

        if (amountNeeded >= AmmoInventoryItemConfig.ItemCount)
        {
            WeaponInventoryItemConfig.AddCurrentAmmo(AmmoInventoryItemConfig.ItemCount);
            AmmoInventoryItemConfig.RemoveCount(amountNeeded);
        }
        else
        {
            WeaponInventoryItemConfig.SetCurrentAmmo();
            AmmoInventoryItemConfig.RemoveCount(amountNeeded);
        }
        
        HandleDisplayAmmo();
        
        _canFire = true;
        IsReloading = false;
    }
    
    private void HandleDisplayAmmo()
    {
        if (WeaponInventoryItemConfig != null)
        {
            if (AmmoInventoryItemConfig != null)
            {
                _displayProvider.AmmoView.DisplayAmmo(WeaponInventoryItemConfig.CurrentAmmo, AmmoInventoryItemConfig.ItemCount, this);
            }
            else
            {
                _displayProvider.AmmoView.DisplayAmmo(WeaponInventoryItemConfig.CurrentAmmo, 0, this);
            }
        }
    }
}