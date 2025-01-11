using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingScreen : MonoBehaviour
{
    [Inject] private readonly SceneLoader _sceneLoader;
    
    private readonly float _fadeDuration = 0.5f;
    
    [SerializeField] private Image _fill;
    [SerializeField] private Image _arrow;
    
    private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        
        gameObject.SetActive(false);
        _canvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        _sceneLoader.OnProgressUpdated += UpdateProgress;
    }

    private void OnDisable()
    {
        _sceneLoader.OnProgressUpdated -= UpdateProgress;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 1;
        UpdateProgress(_sceneLoader.Progress);
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0, _fadeDuration).OnComplete(() => 
        {
            gameObject.SetActive(false);
        });
    }
    
    private void UpdateProgress(float progress)
    {
        _fill.fillAmount = progress;
        float rotation = Mathf.Lerp(95, -95, progress);
        _arrow.rectTransform.localRotation = Quaternion.Euler(0, 0, rotation);
    }
}