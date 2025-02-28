using System.Collections;
using System.Threading.Tasks;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Zenject;

public class AuthorizationSystem : MonoBehaviour
{
    [Inject] private BackendManager _backendManager;
    [Inject] private GameStateMachine _gameStateMachine;
    
    [SerializeField, BoxGroup("Authorization"), HorizontalLine] private TMP_InputField _loginAutInputField;
    [SerializeField, BoxGroup("Authorization")] private TMP_InputField _passwordAutInputField;
    
    [SerializeField, BoxGroup("Registration"), HorizontalLine] private TMP_InputField _loginRegInputField;
    [SerializeField, BoxGroup("Registration")] private TMP_InputField _nameRegInputField;
    [SerializeField, BoxGroup("Registration")] private TMP_InputField _passwordOneRegInputField;
    [SerializeField, BoxGroup("Registration")] private TMP_InputField _passwordTwoRegInputField;

    [SerializeField, BoxGroup("Buttons"), HorizontalLine] private GameObject _authButton;
    [SerializeField, BoxGroup("Buttons")] private GameObject _regButton;
    
    [SerializeField, BoxGroup("NotificationSystem"), HorizontalLine] private NotificationSystem _notificationSystem;

    private void Awake()
    {
        ClearInputFields();
    }

    public void AuthorizationButtonClick()
    {
        if (IsInputEmpty(_loginAutInputField, _passwordAutInputField))
        {
            _notificationSystem.AddMessage("Введите логин и пароль");
            return;
        }

        StartCoroutine(HandleAuthorization());
    }

    public void RegistrationButtonClick()
    {
        if (IsInputEmpty(_loginRegInputField, _nameRegInputField, _passwordOneRegInputField, _passwordTwoRegInputField))
        {
            _notificationSystem.AddMessage("Все поля должны быть заполнены");
            return;
        }

        if (_passwordOneRegInputField.text != _passwordTwoRegInputField.text)
        {
            _notificationSystem.AddMessage("Пароли не совпадают");
            return;
        }

        StartCoroutine(HandleRegistration());
    }
    
    private IEnumerator HandleAuthorization()
    {
        SetButtonsInteractable(false);
        Task<bool> authTask = _backendManager.CheckUserAsync(_loginAutInputField.text, _passwordAutInputField.text);
        yield return new WaitUntil(() => authTask.IsCompleted);

        if (authTask.Result)
        {
            _gameStateMachine.Enter<LoadMainMenuState, IExitableState>(_gameStateMachine.GetActiveState());
        }
        else
        {
            _notificationSystem.AddMessage("Пользователь не найден");
        }

        SetButtonsInteractable(true);
    }

    private IEnumerator HandleRegistration()
    {
        SetButtonsInteractable(false);
        
        Task<bool> regTask = _backendManager.RegisterPlayerAsync(
            _nameRegInputField.text, 
            _loginRegInputField.text, 
            _passwordOneRegInputField.text
        );

        yield return new WaitUntil(() => regTask.IsCompleted);

        if (regTask.Result)
        {
            _notificationSystem.AddMessage("Регистрация успешна");
            ClearInputFields();
        }
        else
        {
            _notificationSystem.AddMessage("Ошибка регистрации: логин уже используется или сервер недоступен");
        }

        SetButtonsInteractable(true);
    }

    private void ClearInputFields()
    {
        _loginAutInputField.text = "";
        _passwordAutInputField.text = "";
        _loginRegInputField.text = "";
        _nameRegInputField.text = "";
        _passwordOneRegInputField.text = "";
        _passwordTwoRegInputField.text = "";
    }
    
    private bool IsInputEmpty(params TMP_InputField[] fields)
    {
        foreach (TMP_InputField field in fields)
        {
            if (string.IsNullOrEmpty(field.text))
                return true;
        }
        return false;
    }

    private void SetButtonsInteractable(bool state)
    {
        _authButton.SetActive(state);
        _regButton.SetActive(state);
    }
}
