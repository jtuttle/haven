using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : UnitySingleton<GameManager> {
    private GameStateMachine _states;

    public Camera GameCamera;

    public Map CurrentMap { get; private set; }
    public XY CurrentCoord { get; private set; }

    private InputManager _inputManager;
    public InputManager Input { get { return _inputManager; } }

    public override void Awake() {
        _states = new GameStateMachine();

        _inputManager = GetComponent<InputManager>();
    }

    public void Start() {
        _states.OnStateExit += OnExitState;

        _states.ChangeGameState(new MapEnterState(CurrentMap));
    }

    public void Update() {
        if(_states.CurrentState != null)
            _states.CurrentState.Update();
    }

    public void UpdateCurrentCoord(Vector3 from, Vector3 to) {
        int dx = to.x < from.x ? -1 : (to.x > from.x ? 1 : 0);
        int dy = to.z < from.z ? -1 : (to.z > from.z ? 1 : 0);
        CurrentCoord = CurrentCoord + new XY(dx, dy);
    }

    private void OnExitState(BaseGameState state) {
        switch(state.GameState) {
            case GameStates.MapEnter:
                
                break;
            case GameStates.MapWalk:

                break;
            case GameStates.MapDesign:
                
                break;
            case GameStates.PlayerPlace:
                
                break;
            default:
                throw new Exception("Game state not found: " + state.GameState.ToString());
        }
    }
}
