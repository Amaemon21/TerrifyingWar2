using TMPro;
using UnityEngine;
using Zenject;

public class PlayerInfoView : MonoBehaviour
{
    [Inject] private readonly MYSQLProvider _mySQLProvider;
    
    [SerializeField] private TMP_Text _playerNameText;

    private void Awake()
    {
        if (_mySQLProvider.CurrentPlayerId != null)
        {
            _playerNameText.text = _mySQLProvider.CurrentPlayerId.ToString();
        }
    }
}
