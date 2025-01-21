using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class DebugerHealth : MonoBehaviour
{
    [Inject] private PlayerHealth _playerHealth;

    [SerializeField] private float _value;
        
    [Button("TakeDamage")]
    public void TakeDamage()
    {
        _playerHealth.TakeDamage(_value);
    }

    [Button("Heal")]
    public void Heal()
    {
        _playerHealth.Heal(_value);
    }
}