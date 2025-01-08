using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BloobOverlay : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Image _bloobOverlay;
    [SerializeField] private float _fadeDuration = 0.5f;

    private void OnEnable()
    {
        _playerHealth.HealthChanged += UpdateBloobOverlay;
    }

    private void OnDisable()
    {
        _playerHealth.HealthChanged -= UpdateBloobOverlay;
    }

    private void UpdateBloobOverlay(int currentHealth, int maxHealth)
    {
        float alpha = 1 - (float)currentHealth / maxHealth;
        _bloobOverlay.DOFade(alpha, _fadeDuration);
    }
}