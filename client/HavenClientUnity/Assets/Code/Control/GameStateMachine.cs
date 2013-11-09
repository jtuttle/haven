using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates {
    None,
    Loading, MainMenu,
    Shop, 
    MapEnter, MapWalk, MapDesign, MapExit, 
    PlayerPlace,
    MiniMap
}

public class GameStateMachine {
    public delegate void StateChangeDelegate(BaseGameState state);
    public event StateChangeDelegate OnStateExit = delegate { };

    public BaseGameState CurrentState { get; private set; }

    private Stack<BaseGameState> _stateStack;

    public GameStateMachine() {
        _stateStack = new Stack<BaseGameState>();
    }

    public void ChangeGameState(BaseGameState newState, bool pushPreviousState = false) {
        // destroy or push old state
        if(CurrentState != null) {
            CurrentState.OnExit -= OnExit;

            if(pushPreviousState)
                _stateStack.Push(CurrentState);
            else
                CurrentState.Dispose();
        }

        // set new state
        CurrentState = newState;

        Debug.Log("Changed state to " + CurrentState.GameState.ToString());

        CurrentState.OnExit += OnExit;
        CurrentState.EnterState();
    }

    public void RestorePreviousState(GameStates state = GameStates.None) {
        if(_stateStack.Count == 0) throw new Exception("No previous states available");

        // set target state to the previous state if none specified
        if(state == GameStates.None)
            state = (_stateStack.Peek() as BaseGameState).GameState;

        while(_stateStack.Count > 0 && CurrentState.GameState != state) {
            CurrentState.OnExit -= OnExit;
            CurrentState.Dispose();

            CurrentState = (BaseGameState)_stateStack.Pop();
        }

        if(CurrentState == null) throw new Exception("Previous state not found: " + state.ToString());

        Debug.Log("Restored state to " + CurrentState.GameState.ToString());

        CurrentState.OnExit += OnExit;
        CurrentState.EnterState();
    }

    private void OnExit(BaseGameState exitingState) {
        OnStateExit(exitingState);
    }
}
