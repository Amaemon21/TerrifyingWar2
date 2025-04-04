using UnityEngine;

namespace KINEMATION.FPSAnimationPack.Scripts.Weapon
{
    public class ManualWeapon : global::Weapon
    {
        private static int RELOAD_START = Animator.StringToHash("Reload_Start");
        private static int RELOAD_LOOP = Animator.StringToHash("Reload_Loop");
        private static int RELOAD_END = Animator.StringToHash("Reload_End");

        private float _startDelay = 0f;
        private float _loopDelay = 0f;

        public override void Initialize(GameObject owner, WeaponInventoryItemConfig weaponInventoryItemConfig)
        {
            base.Initialize(owner, weaponInventoryItemConfig);

            foreach (var clip in WeaponSettings.characterController.animationClips)
            {
                if (!clip.name.Contains("Reload")) continue;

                if (clip.name.Contains("Start"))
                {
                    _startDelay = clip.length;
                    continue;
                }
                
                if (clip.name.Contains("Loop")) _loopDelay = clip.length;
            }
        }

        public override void OnReload()
        {
            _weaponProvider.Animator.Play(RELOAD_START, -1, 0f);
            _weaponAnimator.Play(RELOAD_START, -1, 0f);
            
            Invoke(nameof(OnReloadLoop), _startDelay);
        }

        public void OnReloadLoop()
        {
            if (WeaponInventoryItemConfig.CurrentAmmo == WeaponInventoryItemConfig.MagazineSize)
            {
                OnReloadEnd();
                return;
            }

            //ActiveAmmo++;
            
            _weaponProvider.Animator.CrossFade(RELOAD_LOOP, 0.1f, -1, 0f, 0f);
            _weaponAnimator.Play(RELOAD_LOOP, -1, 0f);
            
            Invoke(nameof(OnReloadLoop), _loopDelay);
        }

        public void OnReloadEnd()
        {
            _weaponProvider.Animator.CrossFade(RELOAD_END, 0.1f, -1, 0f, 0f);
            _weaponAnimator.Play(RELOAD_END, -1, 0f);
        }
    }
}