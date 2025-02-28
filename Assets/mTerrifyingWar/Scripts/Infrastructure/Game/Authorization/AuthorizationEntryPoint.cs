using UnityEngine;
using Zenject;

public class AuthorizationEntryPoint : MonoBehaviour
{
    [Inject] private readonly GameStateMachine _gameStateMachine;
    [Inject] private readonly DiContainer _container;
    
    public void Run()
    {
        AuthorizationState authorizationState = _container.Instantiate<AuthorizationState>();

        _gameStateMachine.AddState(authorizationState);
        _gameStateMachine.Enter<AuthorizationState>();
    }
}