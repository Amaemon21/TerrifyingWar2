using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponAudio : MonoBehaviour
{
    [SerializeField, BoxGroup("Audio")] private AudioClip _emptyMagSound;
    [SerializeField, BoxGroup("Audio")] private AudioClip _reloadSound;
    [SerializeField, BoxGroup("Audio")] private AudioClip _fullReloadSound;
    [SerializeField, BoxGroup("Audio")] private AudioClip _equipSound;
    
    [Space]
    [SerializeField, BoxGroup("Audio")] private AudioClip[] _shootSounds;
    
    [SerializeField, BoxGroup("Audio"), HorizontalLine] private AudioSource _audioSource;

    public void Awake()
    {
        _audioSource.playOnAwake = false;
    }
    
    public void PlayEquipSound()
    {
        if (_equipSound != null)
        {
            _audioSource.PlayOneShot(_equipSound);
        }
    }

    public void PlayShootSound()
    {
        if (_shootSounds.Length == 0) return;

        int randomIndex = Random.Range(0, _shootSounds.Length);
        _audioSource.PlayOneShot(_shootSounds[randomIndex]);
    }

    public void PlayReloadSound()
    {
        if (_reloadSound != null)
        {
            _audioSource.PlayOneShot(_reloadSound);
        }
    }
    
    public void PlayFullReloadSound()
    {
        if (_fullReloadSound != null)
        {
            _audioSource.PlayOneShot(_fullReloadSound);
        }
    }

    public void PlayEmptyClipSound()
    {
        if (_emptyMagSound != null)
        {
            _audioSource.PlayOneShot(_emptyMagSound);
        }
    }
}