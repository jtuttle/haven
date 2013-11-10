using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallView : MonoBehaviour {
    private Wall _model;

    private List<WallPieceView> _wallPieceViews;

    public void Awake() {
        _wallPieceViews = new List<WallPieceView>();
    }
    
    public void SetModel(Wall wall) {
        _model = wall;

        foreach(WallPiece wallPiece in wall.WallPieces) {
            bool onCorner = _model.OnCorner(wallPiece.Coord);
            string prefabName = (onCorner ? "WoodWallCornerView" : "WoodWallSideView");

            WallPieceView wallPieceView = UnityUtils.LoadResource<GameObject>("Prefabs/" + prefabName, true).GetComponent<WallPieceView>();

            Vector3 pos = new Vector3(wallPiece.Coord.X * GameConfig.BLOCK_SIZE, 0, wallPiece.Coord.Y * GameConfig.BLOCK_SIZE);
            wallPieceView.transform.position = pos;

            float angle = 0;

            if(onCorner) {
                if(wallPiece.Coord.X == -wall.Radius && wallPiece.Coord.Y == -wall.Radius)
                    angle = 90.0f;
                else if(wallPiece.Coord.X == -wall.Radius && wallPiece.Coord.Y == wall.Radius)
                    angle = 180.0f;
                else if(wallPiece.Coord.X == wall.Radius && wallPiece.Coord.Y == wall.Radius)
                    angle = 270.0f;
            } else {
                if(wallPiece.Coord.X == -wall.Radius)
                    angle = 90.0f;
                else if(wallPiece.Coord.Y == wall.Radius)
                    angle = 180.0f;
                else if(wallPiece.Coord.X == wall.Radius)
                    angle = 270.0f;
            }
            
            wallPieceView.transform.Rotate(new Vector3(0, angle, 0));
            //wallPieceView.transform.localScale = new Vector3(GameConfig.BLOCK_SIZE, GameConfig.BLOCK_SIZE, GameConfig.BLOCK_SIZE);
            wallPieceView.transform.parent = gameObject.transform;

            _wallPieceViews.Add(wallPieceView);
        }
    }

    public WallPieceView GetTouchedPiece() {
        foreach(WallPieceView wallPieceView in _wallPieceViews) {
            if(wallPieceView.Touching)
                return wallPieceView;
        }

        return null;
    }

    public bool OnWall(Vector3 position) {
        float posX = position.x;
        float posZ = position.z;
        float radius = _model.Radius * GameConfig.BLOCK_SIZE;

        return (posX == -radius || posX == radius) && (posZ > -radius && posZ < radius) ||
            (posZ == -radius || posZ == radius) && (posX > -radius && posX < radius);
    }
}
