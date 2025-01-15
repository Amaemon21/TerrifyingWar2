using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine
{
    private readonly Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;

    public GameStateMachine(SceneLoader sceneLoader, LoadingScreen loadingScreen)
    {
        _states = new Dictionary<Type, IExitableState>
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
            [typeof(LoadMainMenuState)] = new LoadMainMenuState(this, sceneLoader, loadingScreen),
            [typeof(LoadGameplayState)] = new LoadGameplayState(this, sceneLoader, loadingScreen),
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