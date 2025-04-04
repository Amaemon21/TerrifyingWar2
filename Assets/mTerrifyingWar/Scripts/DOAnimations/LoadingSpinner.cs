using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSpinner : MonoBehaviour
{
    private Image _loadingBar;
    private Transform _loadingBarTransform;
    
    private float _loadingDuration = 3f;
    private float _rotateDuration = 1.5f;
    
    private void Awake()
    {
        _loadingBar = GetComponent<Image>();
        _loadingBarTransform = transform;
    }

    private void Start()
    {
        StartLoading();
        RotateLoading();
    }
    
    private void StartLoading()
    {
        _loadingBar.fillAmount = 0f;
        _loadingBar.DOFillAmount(1f, _loadingDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    }
    
    private void RotateLoading()
    {
        _loadingBarTransform.DORotate(new Vector3(0f, 0f, -360f), _rotateDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1).SetLink(gameObject);
    }
}
