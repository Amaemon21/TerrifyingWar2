using System.Collections.Generic;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;

namespace KINEMATION.FPSAnimationPack.Scripts.Weapon
{
    public class MachineGun : global::Weapon
    {
        [SerializeField] private List<Transform> gunTape;
        [SerializeField, Min(0f)] private float tapeResetTime = 0f;
        
        private static int RELOAD_TAPE = Animator.StringToHash("Reload_Extra");
        private static int GAIT = Animator.StringToHash("Gait");

        private void Update()
        {
            _weaponAnimator.SetFloat(GAIT, _weaponProvider.Animator.GetFloat(GAIT));
        }

        private void LateUpdate()
        {
            int count = gunTape.Count;
            if (WeaponInventoryItemConfig.CurrentAmmo > count) return;

            for (int i = 0; i < count; i++)
            {
                if(i > count - WeaponInventoryItemConfig.CurrentAmmo) continue;
                
                KTransform childWorldTransform = KTransform.Identity;
                if (i < count - 1)
                {
                    childWorldTransform = new KTransform(gunTape[i + 1]);
                }

                gunTape[i].localScale /= 100f;
                if (i < count - 1)
                {
                    gunTape[i + 1].localScale *= 100f;
                    gunTape[i + 1].position = childWorldTransform.position;
                    gunTape[i + 1].rotation = childWorldTransform.rotation;
                }
            }
        }

        public override void OnReload()
        {
            if (WeaponInventoryItemConfig.CurrentAmmo == WeaponInventoryItemConfig.MagazineSize) 
                return;
            
            var reloadHash = WeaponInventoryItemConfig.CurrentAmmo == 0 ? AnimationsConstrains.RELOAD_EMPTY : WeaponInventoryItemConfig.CurrentAmmo > gunTape.Count ? AnimationsConstrains.RELOAD_TAC : RELOAD_TAPE;
            _weaponProvider.Animator.Play(reloadHash, -1, 0f);
            _weaponAnimator.Play(reloadHash, -1, 0f);

            float delay = WeaponInventoryItemConfig.CurrentAmmo > gunTape.Count ? TacReloadDelay : tapeResetTime;
            Invoke(nameof(AddAmmo), delay);
            IsReloading = true;
        }
    }
}