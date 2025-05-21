using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerSettings", menuName = "KINEMATION/FPS Animation Pack/FPS Player Settings")]
public class FPSPlayerSettings : ScriptableObject
{
    public List<Weapon> weaponPrefabs;
    public float grenadeDelay = 0f;
    public float gaitSmoothing = 0f;

    public float defaultFov = 80f;
    
    [Range(0f, 1f)] public float ikWeight = 1f;
    public float aimSpeed = 0f;

    public IKMotion aimingMotion;
    public IKMotion fireModeMotion;

    public List<AudioClip> generalSounds;
}