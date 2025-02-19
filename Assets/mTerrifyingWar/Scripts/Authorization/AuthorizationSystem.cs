using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Zenject;

public class AuthorizationSystem : MonoBehaviour
{
    [Inject] private readonly MYSQLProvider _mysqlProvider;
    [Inject] private readonly GameStateMachine _gameStateMachine;
    
    [SerializeField, BoxGroup("Authorization"), HorizontalLine] private TMP_InputField _loginAutInputField;
    [SerializeField, BoxGroup("Authorization")] private TMP_InputField _passwordAutInputField;
    
    [SerializeField, BoxGroup("Registration"), HorizontalLine] private TMP_InputField _loginRegInputField;
    [SerializeField, BoxGroup("Registration")] private TMP_InputField _emailRegInputField;
    [SerializeField, BoxGroup("Registration")] private TMP_InputField _passwordOneRegInputField;
    [SerializeField, BoxGroup("Registration")] private TMP_InputField _passwordTwoRegInputField;

    private void Awake()
    {
        _loginAutInputField.text = string.Empty;
        _passwordAutInputField.text = string.Empty;
        _loginRegInputField.text = string.Empty;
        _emailRegInputField.text = string.Empty;
        _passwordOneRegInputField.text = string.Empty;
        _passwordTwoRegInputField.text = string.Empty;
    }

    public void EnterButtonClick()
    {
        if (string.IsNullOrEmpty(_loginAutInputField.text))
            return;
        
        if (string.IsNullOrEmpty(_passwordAutInputField.text))
            return;
        
        _mysqlProvider.SetupCurrentPlayer(_mysqlProvider.GetUserByIdentity(_loginAutInputField.text, _passwordAutInputField.text));
        
        if (_mysqlProvider.CurrentPlayerId.HasValue)
        {
            _gameStateMachine.Enter<LoadMainMenuState, IExitableState>(_gameStateMachine.GetActiveState());
        }
        else
        {
            Debug.Log("Player not found");
        }
    }

    public void RegistrationButtonClick()
    {
        
    }
}