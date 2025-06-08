using DG.Tweening;
using UnityEngine;

public class FPSLightCurves : MonoBehaviour
{
    public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float GraphTimeMultiplier = 1, GraphIntensityMultiplier = 1;

    private bool canUpdate;
    private float startTime;
    private Light lightSource;

    private Tween lightTween;

    private void Awake()
    {
        lightSource = GetComponent<Light>();
        lightSource.intensity = LightCurve.Evaluate(0) * GraphIntensityMultiplier;
        lightSource.enabled = false;
    }

    private void OnEnable()
    {
        lightSource.enabled = true;

        lightTween?.Kill();

        lightTween = DOTween.To(
                () => 0f,
                t => lightSource.intensity = LightCurve.Evaluate(t) * GraphIntensityMultiplier,
                GraphTimeMultiplier,
                GraphTimeMultiplier)
            .SetEase(Ease.Linear)
            .OnComplete(() => lightSource.enabled = false);
    }

    private void OnDisable()
    {
        lightTween?.Kill();
    }
}