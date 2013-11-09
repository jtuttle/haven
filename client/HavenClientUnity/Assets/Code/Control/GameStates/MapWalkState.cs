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

    public override void Update() {
        Vector3 pos = _playerView.transform.position;
        XY mapCoord = _mapView.GetMapCoordFromWorldCoord(pos);
    }

    public override void Dispose() {
        
        base.Dispose();
    }

    private void OnAxialInput(float h, float v) {
        _playerView.Move(h, v, GameManager.Instance.PlayerCamera.camera);
    }

    private void OnConfirmPress() {
        WallPieceView touchedPiece = _mapView.WallView.GetTouchedPiece();

        if(!_playerView.Model.OnWall) {
            if(touchedPiece != null) {
                _playerView.DoAscendWallTween(touchedPiece);
                GameManager.Instance.GameModel.Player.OnWall = true;
            }
        } else {
            if(touchedPiece != null) {
                Vector3 pos = _playerView.transform.position;
                XY mapCoord = _mapView.GetMapCoordFromWorldCoord(pos);

                bool onCorner = GameManager.Instance.GameModel.Map.Wall.OnCorner(mapCoord);
                bool onHori = GameManager.Instance.GameModel.Map.Wall.OnHorizontal(mapCoord);
                bool onVert = GameManager.Instance.GameModel.Map.Wall.OnVertical(mapCoord);

                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                if(onCorner && (v != 0 || h != 0)) {
                    //bool outside 
                    //_playerView.DoDescendWallTween(touchedPiece, (pos.z > 0 && v > 0) || (pos.z < 0 && v < 0));
                } else if(onHori && v != 0) {
                    DescendFromWall(touchedPiece, new Vector3(0, 0, v < 0 ? -1 : 1));
                } else if(onVert && h != 0) {
                    DescendFromWall(touchedPiece, new Vector3(h < 0 ? -1 : 1, 0, 0));
                }
            }
        }
    }

    private void DescendFromWall(WallPieceView wallPieceView, Vector3 direction) {
        GameManager.Instance.GameModel.Player.OnWall = false;
        _playerView.DoDescendWallTween(wallPieceView, direction);
    }
}
