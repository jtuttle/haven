using UnityEngine;
using System.Collections;
using System;

public class MapWalkState : BaseGameState {
    private PlayerView _playerView;
    private MapView _mapView;

    public MapWalkState() 
        : base(GameStates.MapWalk) {

        _playerView = GameManager.Instance.PlayerView;
        _mapView = GameManager.Instance.MapView;
    }

    public override void EnterState() {
        base.EnterState();

        GameManager.Instance.Input.OnAxialInput += OnAxialInput;
        GameManager.Instance.Input.GetButton(ButtonId.Confirm).OnPress += OnConfirmPress;
    }

    public override void ExitState() {
        GameManager.Instance.Input.OnAxialInput -= OnAxialInput;

        base.ExitState();
    }

    public override void Dispose() {
        
        base.Dispose();
    }

    private void OnAxialInput(float h, float v) {
        _playerView.Move(h, v, GameManager.Instance.PlayerCamera.camera);
		PlayerMoved();
    }

    private void OnConfirmPress() {
        WallPieceView touchedPiece = _mapView.WallView.GetTouchedPiece();
        
        if(touchedPiece != null) {
            MovePlayerToWall(touchedPiece);
        }
    }

    private void MovePlayerToWall(WallPieceView wallPieceView) {

        float pieceScaleY = wallPieceView.transform.localScale.y;
        _playerView.transform.position = wallPieceView.transform.position + new Vector3(0, pieceScaleY, 0);

        GameManager.Instance.GameModel.Player.OnWall = true;

        //_playerView.transform.position = 
		PlayerMoved();
    }
	
	private void PlayerMoved() {
		GameManager.Instance.Client.SendPosition(_playerView.transform.position.x,
			_playerView.transform.position.y, _playerView.transform.position.z);
	}
}
