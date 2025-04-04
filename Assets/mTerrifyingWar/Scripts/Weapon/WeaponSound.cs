using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    
    private Weapon _weapon;
    private WeaponSettings _settings;
    
    private bool _isPlaying = false;
        
    private void Awake()
    {
        _weapon = transform.parent.GetComponent<Weapon>();
        _settings = _weapon.WeaponSettings;
    }

    private void OnEnable()
    {
        _weapon.OnShootChanged += PlayFireSound;
    }

    private void OnDisable()
    {
        _weapon.OnShootChanged -= PlayFireSound;
    }

    private void PlayFireSound()
    {
        if (_audioSource == null)
            return;

        _audioSource.pitch = Random.Range(_settings.firePitchRange.x, _settings.firePitchRange.y);
        _audioSource.volume = Random.Range(_settings.fireVolumeRange.x, _settings.fireVolumeRange.y);
        _audioSource.PlayOneShot(PlayerSound.GetRandomAudioClip(_settings.fireSounds));
    }

    public void PlayWeaponSound(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex > _settings.weaponEventSounds.Count - 1)
            return;

        if (!_isPlaying)
        {
            _audioSource.PlayOneShot(_settings.weaponEventSounds[clipIndex]);
            _isPlaying = true;
        }
    }

    public void Stop()
    {
        _isPlaying = false;
    }
}