using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;

    [SerializeField] private Image _fillImage;

    private void OnEnable()
    {
        _playerHealth.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _playerHealth.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        float targetFillAmount = (float)currentHealth / maxHealth;
        
        _fillImage.DOFillAmount(targetFillAmount, 0.3f).SetEase(Ease.OutQuad);
    }
}