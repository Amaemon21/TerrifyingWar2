using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class Bar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    protected abstract void OnEnable();
    
    protected abstract void OnDisable();

    protected void OnBarChanged(int currentHealth, int maxHealth)
    {
        float targetFillAmount = (float)currentHealth / maxHealth;
        
        _fillImage.DOFillAmount(targetFillAmount, 0.3f).SetEase(Ease.OutQuad);
    }
    
    protected void OnBarChanged(float currentHealth, float maxHealth)
    {
        float targetFillAmount = currentHealth / maxHealth;
        
        _fillImage.DOFillAmount(targetFillAmount, 0.3f).SetEase(Ease.OutQuad);
    }
}