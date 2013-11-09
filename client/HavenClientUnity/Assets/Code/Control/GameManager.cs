using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : UnitySingleton<GameManager> {
    private GameStateMachine _states;

    public Camera GameCamera;

    private InputManager _inputManager;
    public InputManager Input { get { return _inputManager; } }

    public override void Awake() {
        _states = new GameStateMachine();

        _inputManager = GetComponent<InputManager>();
    }

    public void Start() {
        _states.OnStateExit += OnExitState;

        _states.ChangeGameState(new MapEnterState());
    }

    public void Update() {
        if(_states.CurrentState != null)
            _states.CurrentState.Update();
    }

    private void OnExitState(BaseGameState state) {
        switch(state.GameState) {
            case GameStates.MapEnter:
                
                break;
            default:
                throw new Exception("Game state not found: " + state.GameState.ToString());
        }
    }
}
