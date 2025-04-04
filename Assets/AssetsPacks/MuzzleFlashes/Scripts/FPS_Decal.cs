using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshRenderer))]
public class FPSDecal : MonoBehaviour
{
    [SerializeField] private bool _screenSpaceDecals = true;
    [SerializeField] [Range(0, 100)] private float _randomScalePercent = 25;

    private MeshRenderer _meshRenderer;
    private Vector3 _startScale;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _startScale = transform.localScale;
    }

    private void OnEnable()
    {
        ConfigureRenderer();
        ApplyRandomTransform();
        //EnsureDepthTextureMode();
    }

    private void ConfigureRenderer()
    {
        _meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
        _meshRenderer.shadowCastingMode = ShadowCastingMode.Off;

        if (_screenSpaceDecals)
        {
            _meshRenderer.sharedMaterial.DisableKeyword("USE_QUAD_DECAL");
            _meshRenderer.sharedMaterial.SetInt("_ZTest1", (int)CompareFunction.Greater);
        }
        else
        {
            _meshRenderer.sharedMaterial.EnableKeyword("USE_QUAD_DECAL");
            _meshRenderer.sharedMaterial.SetInt("_ZTest1", (int)CompareFunction.LessEqual);
        }
    }

    private void ApplyRandomTransform()
    {
        if (!Application.isPlaying) return;

        transform.localRotation = Quaternion.Euler(Random.Range(0, 360), 90, 90);

        float scaleFactor = _randomScalePercent * 0.01f * _startScale.x;
        float randomScale = Random.Range(_startScale.x - scaleFactor, _startScale.x + scaleFactor);

        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    private void EnsureDepthTextureMode()
    {
        Camera mainCamera = Camera.main;
        
        if (mainCamera != null && mainCamera.depthTextureMode != DepthTextureMode.Depth)
        {
            mainCamera.depthTextureMode = DepthTextureMode.Depth;
        }
    }
}