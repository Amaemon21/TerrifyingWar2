using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Renderer))]
public class FPSShaderColorGradient : MonoBehaviour
{
    [SerializeField] private RFX4_ShaderProperties ShaderColorProperty = RFX4_ShaderProperties._TintColor;
    [SerializeField] private Gradient Color = new();
    [SerializeField] private float TimeMultiplier = 1f;
    [SerializeField] private float ColorMultiplier = 1f;
    [SerializeField] private bool IsLoop = false;

    private int _propertyID;
    private Color _startColor;
    private Renderer _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;
    private Tween _colorTween;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        _propertyID = Shader.PropertyToID(ShaderColorProperty.ToString());
        _startColor = _renderer.sharedMaterial.GetColor(_propertyID);
    }

    private void OnEnable()
    {
        AnimateGradient();
    }

    private void OnDisable()
    {
        _colorTween?.Kill();
    }

    private void AnimateGradient()
    {
        _colorTween?.Kill();

        float duration = TimeMultiplier > 0 ? TimeMultiplier : 1f;
        _colorTween = DOTween.To(() => 0f, t => ApplyColor(t), 1f, duration).SetLoops(IsLoop ? -1 : 0).SetEase(Ease.Linear);
    }

    private void ApplyColor(float time)
    {
        _renderer.GetPropertyBlock(_materialPropertyBlock);
        Color evaluatedColor = Color.Evaluate(time) * _startColor * ColorMultiplier;
        _materialPropertyBlock.SetColor(_propertyID, evaluatedColor);
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }

    public enum RFX4_ShaderProperties
    {
        _TintColor,
        _Cutoff,
        _Color,
        _EmissionColor,
        _MaskPow,
        _Cutout,
        _Speed,
        _BumpAmt,
        _MainColor,
        _Distortion,
        _FresnelColor
    }
}