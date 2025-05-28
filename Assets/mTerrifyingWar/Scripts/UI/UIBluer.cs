using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBluer : MonoBehaviour
{
    [SerializeField] private Image _bluerImage;

    [SerializeField] private float _duration;
    
    private void Awake()
    {
        DeactiveBluer();
    }

    public void ActiveBluer()
    {
        _bluerImage.DOFade(1, _duration).SetLink(_bluerImage.gameObject);
    }

    public void DeactiveBluer()
    {
        _bluerImage.DOFade(0, _duration).SetLink(_bluerImage.gameObject);
    }
}
