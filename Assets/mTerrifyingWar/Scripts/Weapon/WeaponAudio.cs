using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class WeaponAudio : MonoBehaviour
{
    [SerializeField, BoxGroup("Audio Weapon Config")] private AudioWeaponConfig _audioWeaponConfig;
    
    private AudioSource _audioSource;

    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        PlayEquipSound();
    }

    private void PlayEquipSound()
    {
        if (_audioWeaponConfig.EquipSound != null)
        {
            _audioSource.PlayOneShot(_audioWeaponConfig.EquipSound);
        }
    }

    public void PlayShootSound()
    {
        if (_audioWeaponConfig.ShootSounds.Length == 0) return;

        int randomIndex = Random.Range(0, _audioWeaponConfig.ShootSounds.Length);
        _audioSource.PlayOneShot(_audioWeaponConfig.ShootSounds[randomIndex]);
    }

    public void PlayReloadSound()
    {
        if (_audioWeaponConfig.ReloadSound != null)
        {
            _audioSource.PlayOneShot(_audioWeaponConfig.ReloadSound);
        }
    }
    
    public void PlayFullReloadSound()
    {
        if (_audioWeaponConfig.FullReloadSound != null)
        {
            _audioSource.PlayOneShot(_audioWeaponConfig.FullReloadSound);
        }
    }

    public void PlayEmptyClipSound()
    {
        if (_audioWeaponConfig.EmptyMagSound != null)
        {
            _audioSource.PlayOneShot(_audioWeaponConfig.EmptyMagSound);
        }
    }
}