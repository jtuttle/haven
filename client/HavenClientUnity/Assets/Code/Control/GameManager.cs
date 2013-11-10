﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : UnitySingleton<GameManager> {
    private GameStateMachine _states;

    public GameModel GameModel;

    public PlayerCamera PlayerCamera;
    public PlayerView PlayerView;
    public MapView MapView;

    private InputManager _inputManager;
    public InputManager Input { get { return _inputManager; } }
	
	public MpClient Client { get; private set; }

    public override void Awake() {
        GameModel = new GameModel();

        _states = new GameStateMachine();

        _inputManager = GetComponent<InputManager>();
		
		Client = new MpClient();
    }

    public void Start() {
        _states.OnStateExit += OnExitState;

        _states.ChangeGameState(new MapEnterState());
		
		Client.Start();
    }

    public void Update() {
        if(_states.CurrentState != null)
            _states.CurrentState.Update();
    }

    private void OnExitState(BaseGameState state) {
        switch(state.GameState) {
            case GameStates.MapEnter:
                _states.ChangeGameState(new MapWalkState());

                break;
            default:
                throw new Exception("Game state not found: " + state.GameState.ToString());
        }
    }
}
