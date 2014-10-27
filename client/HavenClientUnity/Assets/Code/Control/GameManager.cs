using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

public class GameManager : UnitySingleton<GameManager> {
    private GameStateMachine _states;

    public GameModel GameModel;

    public PlayerCamera PlayerCamera;
    public PlayerView PlayerView;
    public MapView MapView;

    private InputManager _inputManager;
    public InputManager Input { get { return _inputManager; } }
	
	public MpClient Client { get; private set; }
	public MpHandler Multiplayer { get; private set; }

    public override void Awake() {
        GameModel = new GameModel();

        _states = new GameStateMachine();

        _inputManager = GetComponent<InputManager>();

		/*
		Client = new MpClient();
		Client.Host = GameConfig.MpHost;
		Client.Port = GameConfig.MpPort;
		Multiplayer = new MpHandler(Client);
		*/
    }

    public void Start() {
        _states.OnStateExit += OnExitState;

        _states.ChangeGameState(new MapEnterState());
		
		//Client.Start();
    }

    public void Update() {
        if(_states.CurrentState != null)
            _states.CurrentState.Update();
		//Multiplayer.DoDelegatedWork();
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
