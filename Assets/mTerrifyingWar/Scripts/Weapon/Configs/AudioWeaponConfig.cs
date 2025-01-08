using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioWeaponConfig", menuName = "Weapon/AudioWeaponConfig")]
public class AudioWeaponConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Audio")] public AudioClip EmptyMagSound { get; private set; }
    [field: SerializeField, BoxGroup("Audio")] public AudioClip ReloadSound { get; private set; }
    [field: SerializeField, BoxGroup("Audio")] public AudioClip FullReloadSound { get; private set; }
    [field: SerializeField, BoxGroup("Audio")] public AudioClip EquipSound { get; private set; }
    

    [field: SerializeField, Space, BoxGroup("Audio")] public AudioClip[] ShootSounds { get; private set; }
}