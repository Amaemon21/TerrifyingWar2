using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine
{
    private readonly Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;

    public GameStateMachine(SceneLoader sceneLoader, LoadingScreen loadingScreen, CursorStateService cursorStateService)
    {
        _states = new Dictionary<Type, IExitableState>
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
            [typeof(LoadAuthorizationState)] = new LoadAuthorizationState(this, sceneLoader, loadingScreen, cursorStateService),
            [typeof(LoadMainMenuState)] = new LoadMainMenuState(this, sceneLoader, loadingScreen, cursorStateService),
            [typeof(LoadGameplayState)] = new LoadGameplayState(this, sceneLoader, loadingScreen, cursorStateService),
        };
    }

    public void Enter<TState>() where TState : class, IState
    {
        IState state = ChangedState<TState>();
        state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
        TState state = ChangedState<TState>();
        state.Enter(payload);
    }

    public bool CheckIsState<TState>() where TState : class, IExitableState
    {
        Type stateType = typeof(TState);
    
        if (_states.ContainsKey(stateType))
        {
            return true;
        }

        return false;
    }
    
    public void AddState<TState>(TState state) where TState : class, IExitableState
    {
        Type stateType = typeof(TState);
    
        if (!_states.ContainsKey(stateType))
        {
            _states[stateType] = state;
        }
        else
        {
            Debug.Log($"State of type {stateType} already exists in the state machine."); 
        }
    }

    public void RemoveState<TState>(TState state) where TState : class, IExitableState
    {
        Type stateType = typeof(TState);
    
        if (_states.ContainsKey(stateType))
        {
            _states.Remove(stateType);
        }
        else
        {
            Debug.Log($"State of type {stateType} already exists in the state machine."); 
        }
    }
    
    public IExitableState GetActiveState()
    {
        return _activeState;
    }

    private TState ChangedState<TState>() where TState : class, IExitableState
    {
        _activeState?.Exit();

        TState state = GetState<TState>();
        _activeState = state;

        return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState
    {
        return _states[typeof(TState)] as TState;
    }
}