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
