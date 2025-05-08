using UnityEngine;
using Zenject;

public class WeaponAnimator : MonoBehaviour
{
    [Inject] private readonly PlayerProvider _playerProvider;
    
    [SerializeField] private Animator _weaponAnimator;
    
    private Weapon _weapon;
    private WeaponSettings _weaponSettings;
    
    public float TacReloadDelay { get; private set; }
    public float EmptyReloadDelay { get; private set; }
    public float UnEquipDelay { get; private set; }

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
        _weaponSettings = _weapon.WeaponSettings;
    }

    private void OnEnable()
    {
        _weapon.OnShootChanged += PlayerFire;
    }

    private void OnDisable()
    {
        _weapon.OnShootChanged -= PlayerFire;
    }

    public virtual void Initialize(GameObject owner)
    {
        AnimationClip idlePose = null;

        foreach (var clip in _weaponSettings.characterController.animationClips)
        {
            if (clip.name.Contains("Reload"))
            {
                if (clip.name.Contains("Tac"))
                    TacReloadDelay = clip.length;

                if (clip.name.Contains("Empty"))
                    EmptyReloadDelay = clip.length;

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

    private void PlayerFire()
    {
        if (_weaponSettings.useFireClip)
        {
            _playerProvider.WeaponContainer.HandAnimator.Play(AnimationsConstrains.FIRE, -1, 0f);
        }
        
        _weaponAnimator.Play(_weaponSettings.hasFireOut && _weapon.WeaponInventoryItemConfig.CurrentAmmo == 1 ? AnimationsConstrains.FIREOUT : AnimationsConstrains.FIRE, -1, 0f);
    }

    public void PlayReload()
    {
        int reloadHash = _weapon.WeaponInventoryItemConfig.CurrentAmmo == 0 ? AnimationsConstrains.RELOAD_EMPTY : AnimationsConstrains.RELOAD_TAC;
        _weaponAnimator.Play(reloadHash, -1, 0f);
        _playerProvider.WeaponContainer.HandAnimator.Play(reloadHash, -1, 0f);
    }
    
    public void OnEquipped_Immediate()
    {
        _playerProvider.WeaponContainer.HandAnimator.runtimeAnimatorController = _weaponSettings.characterController;
        _weaponAnimator.Play(AnimationsConstrains.IDLE, -1, 0f);
        _playerProvider.WeaponContainer.RecoilAnimation.Init(_weaponSettings.recoilAnimData, _weaponSettings.fireRate, _weapon.FireMode);
    }
    
    public void OnEquipped(bool fastEquip = false)
    {
        _playerProvider.WeaponContainer.HandAnimator.runtimeAnimatorController = _weaponSettings.characterController;
        _playerProvider.WeaponContainer.RecoilAnimation.Init(_weaponSettings.recoilAnimData, _weaponSettings.fireRate, _weapon.FireMode);

        // Reset the default pose to idle.
        _playerProvider.WeaponContainer.HandAnimator.Play(AnimationsConstrains.IDLE, -1, 0f);

        // Play the equip animation.
        if (_weaponSettings.hasEquipOverride)
        {
            _playerProvider.WeaponContainer.HandAnimator.Play("IKMovement", -1, 0f);
            _playerProvider.WeaponContainer.HandAnimator.Play(fastEquip ? AnimationsConstrains.EQUIP : AnimationsConstrains.EQUIP_OVERRIDE, -1, 0f);
            return;
        }

        // Play the curve-based equipping animation.
        _playerProvider.WeaponContainer.HandAnimator.Play(AnimationsConstrains.EQUIP, -1, 0f);
    }

    public float OnUnEquipped()
    {
        _playerProvider.WeaponContainer.HandAnimator.SetTrigger(AnimationsConstrains.UNEQUIP);
        return UnEquipDelay + 0.05f;
    }
}
