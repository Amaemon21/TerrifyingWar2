using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BloobOverlay : MonoBehaviour
{
    [Inject] private PlayerProvider _playerProvider;
    
    [SerializeField] private Image _bloobOverlay;
    [SerializeField] private float _fadeDuration = 0.5f;

    private void OnEnable()
    {
        _playerProvider.PlayerHealth.HealthChanged += UpdateBloobOverlay;
    }

    private void OnDisable()
    { 
        _playerProvider.PlayerHealth.HealthChanged  -= UpdateBloobOverlay;
    }

    private void UpdateBloobOverlay(int currentHealth, int maxHealth)
    {
        float alpha = 1 - (float)currentHealth / maxHealth;
        _bloobOverlay.DOFade(alpha, _fadeDuration);
    }
}