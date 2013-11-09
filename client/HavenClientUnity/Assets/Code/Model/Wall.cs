using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall {
    public int Radius { get; private set; }

    public List<WallPiece> WallPieces { get; private set; }

    public Wall(int radius) {
        Radius = radius;

        BuildWall();
    }

    public List<WallPiece> GetCrossPieces(XY coord) {
        List<WallPiece> pieces = new List<WallPiece>();

        foreach(WallPiece piece in WallPieces) {
            if(piece.Coord.X == coord.X || piece.Coord.Y == coord.Y)
                pieces.Add(piece);
        }

        return pieces;
    }

    public bool OnHorizontal(XY mapCoord) {
        return Mathf.Abs(mapCoord.Y) == Radius;
    }

    public bool OnVertical(XY mapCoord) {
        return Mathf.Abs(mapCoord.X) == Radius;
    }

    public bool OnCorner(XY mapCoord) {
        return OnHorizontal(mapCoord) && OnVertical(mapCoord);
    }

    private void BuildWall() {
        WallPieces = new List<WallPiece>();

        for(int y = -Radius; y <= Radius; y++) {
            for(int x = -Radius; x <= Radius; x++) {
                if(y == -Radius || y == Radius || x == -Radius || x == Radius)
                    WallPieces.Add(new WallPiece(new XY(x, y)));
            }
        }
    }
}
