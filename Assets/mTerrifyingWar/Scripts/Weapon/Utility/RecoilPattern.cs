using UnityEngine;
using Zenject;

public class RecoilPattern : MonoBehaviour
{
    [Inject] private IInputService _inputService;

    [SerializeField] private RecoilPreset recoilSettings;

    private Vector2 _compensation;
    private Vector2 _targetRecoil;
    private Vector2 _recoil;
    private Vector2 _cachedRecoil;
    private Vector3 _headRecoilOffset;
    private bool _isFiring;

    public Vector2 AccumulatedRecoil {get; private set;}
    
    public void Setup(RecoilPreset settings)
    {
        recoilSettings = settings;
        _compensation = AccumulatedRecoil = _targetRecoil = _cachedRecoil = _recoil = Vector2.zero;
    }

    private void Update()
    {
        if (recoilSettings == null)
            return;

        if (_isFiring)
        {
            Vector2 deltaInput = _inputService.LookDirection;
            _compensation.x += deltaInput.x;
            _compensation.y += deltaInput.y;
        }

        float alpha = KMath.ExpDecayAlpha(recoilSettings.HorizontalSmoothing, Time.deltaTime);
        _recoil.x = Mathf.Lerp(_recoil.x, _targetRecoil.x, alpha);

        alpha = KMath.ExpDecayAlpha(recoilSettings.VerticalSmoothing, Time.deltaTime);
        _recoil.y = Mathf.Lerp(_recoil.y, _targetRecoil.y, alpha);

        if (!_isFiring)
        {
            alpha = KMath.ExpDecayAlpha(recoilSettings.Damping, Time.deltaTime);
            _targetRecoil = Vector2.Lerp(_targetRecoil, Vector2.zero, alpha);
        }

        AccumulatedRecoil = _recoil - _cachedRecoil;
        _cachedRecoil = _recoil;
    }

    public void OnFireStart()
    {
        if (recoilSettings == null)
            return;

        if (!_isFiring)
        {
            _compensation = AccumulatedRecoil = Vector2.zero;
        }

        _isFiring = true;
        _targetRecoil.x += Random.Range(recoilSettings.HorizontalRecoil.x, recoilSettings.HorizontalRecoil.y);
        _targetRecoil.y += Random.Range(recoilSettings.VerticalRecoil.x, recoilSettings.VerticalRecoil.y);
    }

    public void OnFireEnd()
    {
        _isFiring = false;

        _recoil.x *= Compensate(_recoil.x, _compensation.x);
        _recoil.y *= Compensate(_recoil.y, _compensation.y);
        _cachedRecoil = _recoil;
        _targetRecoil = _recoil;
    }

    private float Compensate(float recoil, float compensation)
    {
        float multiplier = 1f;
        bool isOpposite = recoil * compensation <= 0f;

        if (!Mathf.Approximately(compensation, 0f) && isOpposite)
        {
            multiplier -= Mathf.Clamp01(Mathf.Abs(compensation / recoil));
        }

        return multiplier;
    }
}