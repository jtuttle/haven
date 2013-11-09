using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallView : MonoBehaviour {
    
    public void SetModel(Wall wall) {
        foreach(WallPiece wallPiece in wall.WallPieces) {
            GameObject wallPieceView = UnityUtils.LoadResource<GameObject>("Prefabs/WallPieceView", true);

            Vector3 pos = new Vector3(wallPiece.Coord.X * GameConfig.BLOCK_SIZE, GameConfig.BLOCK_SIZE / 2, wallPiece.Coord.Y * GameConfig.BLOCK_SIZE);
            wallPieceView.transform.position = pos;
            wallPieceView.transform.localScale = new Vector3(GameConfig.BLOCK_SIZE, GameConfig.BLOCK_SIZE, GameConfig.BLOCK_SIZE);
            wallPieceView.transform.parent = gameObject.transform;
        }
    }
}
