using DG.Tweening;
using UnityEngine;

public class PlayHitAnimation : MonoBehaviour
{
    [SerializeField] private float _hitDuration = 0.2f;
    [SerializeField] private float _rotationStrength = 10f;

    [Space(10)] 
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _audioClip;
    
    private Transform _transform;
    
    private void Awake()
    {
        _transform = transform;
    }
    
    private void OnPlayHitAnimation()
    {
        _transform.DOShakeRotation(_hitDuration, new Vector3(_rotationStrength, _rotationStrength, 0), vibrato: 10, randomness: 90);
        
        _audioSource.PlayOneShot(_audioClip);
    }
}
