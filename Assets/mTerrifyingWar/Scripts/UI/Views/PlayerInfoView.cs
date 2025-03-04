using TMPro;
using UnityEngine;
using Zenject;

public class PlayerInfoView : MonoBehaviour
{
    [Inject] private readonly BackendManager _backendManager;
    
    [SerializeField] private TMP_Text _playerNameText;

    private void Awake()
    {
        _playerNameText.text = _backendManager.PlayerLogin;
    }
}
