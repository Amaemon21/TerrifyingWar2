using System.Collections.Generic;
using KINEMATION.ProceduralRecoilAnimationSystem.Runtime;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponSettings", menuName = "Weapon/Weapon Settings")]
public class WeaponSettings : ScriptableObject
{
    [BoxGroup("General"), HorizontalLine] public RuntimeAnimatorController characterController;
    [BoxGroup("General")] public RecoilAnimData recoilAnimData;
    [BoxGroup("General")] public RecoilPreset RecoilPreset;
    
    [BoxGroup("IK"), HorizontalLine] public Vector3 ikOffset;
    [BoxGroup("IK")] public Vector3 leftClavicleOffset;
    [BoxGroup("IK")] public Vector3 rightClavicleOffset;
    [BoxGroup("IK")] public Vector3 aimPointOffset;
    [BoxGroup("IK")] public Quaternion rightHandSprintOffset = Quaternion.identity;
    [BoxGroup("IK")] [Range(0f, 1f)] public float adsBlend = 0f;
    
    [BoxGroup("Settings"), HorizontalLine] [Min(0f)] public float fireRate = 600f;
    [BoxGroup("Settings")] [Min(0f)] public float aimFov = 70f;
    
    [BoxGroup("Settings")] public bool fullAuto;
    [BoxGroup("Settings")] public bool useFireClip;
    [BoxGroup("Settings")] public bool hasEquipOverride;
    [BoxGroup("Settings")] public bool hasFireOut;
    [BoxGroup("Settings")] public bool useSprintTriggerDiscipline = true;
    
    [BoxGroup("SFX"), HorizontalLine] public List<AudioClip> fireSounds;
    [BoxGroup("SFX")] public List<AudioClip> weaponEventSounds;
    [BoxGroup("SFX")] public Vector2 firePitchRange = Vector2.one;
    [BoxGroup("SFX")] public Vector2 fireVolumeRange = Vector2.one;
}