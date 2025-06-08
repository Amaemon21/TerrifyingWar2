using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

public class FPS_PerPlatformSettings : MonoBehaviour
{
    [Inject] private readonly PlayerProvider _playerProvider;
    
    private bool defaultOpaueColorUsing;
    private bool defaultDepthUsing;
    
    private UniversalAdditionalCameraData addCamData;

    private void OnEnable()
    {
        if (addCamData != null)
        {
            defaultOpaueColorUsing = addCamData.requiresColorTexture;
            defaultDepthUsing = addCamData.requiresDepthTexture;
            addCamData.requiresColorTexture = true;
            addCamData.requiresDepthTexture = true;
        }
    }

    private void OnDisable()
    {
        if (addCamData != null)
        {
            addCamData.requiresColorTexture = defaultOpaueColorUsing;
            addCamData.requiresDepthTexture = defaultDepthUsing;
        }
    }
}