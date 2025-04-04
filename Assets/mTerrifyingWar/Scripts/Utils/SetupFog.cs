using AtmosphericHeightFog;
using UnityEngine;
using Zenject;

public class SetupFog : MonoBehaviour
{
    [Inject] private readonly PlayerProvider _playerProvider;
    
    [SerializeField] private HeightFogGlobal _heightFogGlobal;
    
    private void Awake()
    {
        _heightFogGlobal.Setup(_playerProvider.MainCamera);
    }
}
