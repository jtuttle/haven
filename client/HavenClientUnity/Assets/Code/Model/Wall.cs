using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall {

    public int Radius { get; private set; }

    public List<WallPiece> WallPieces { get; private set; }

    public Wall(int mapWidth, int mapHeight, int radius) {
        Radius = radius;

        BuildWall(mapWidth, mapHeight);
    }

    private void BuildWall(int mapWidth, int mapHeight) {
        WallPieces = new List<WallPiece>();

        Vector2 center = new Vector2(mapWidth / 2, mapHeight / 2);

        //for(int y = (int)center.y - Radius; y < (int)center.y + Radius; y++) {
        //    for(int x = (int)center.x - Radius; x < (int)center.x + Radius; x++) {
        //        XY coord = new XY(x, y);
        //        WallPieces.Add(new WallPiece(coord));
        //    }
        //}

        for(int y = 0; y < Radius * 2; y++) {
            for(int x = 0; x < Radius * 2; x++) {
                if(y == 0) {
                    XY coord = new XY((int)(center.x - Radius) + x, (int)(center.y - Radius) + y);
                    WallPieces.Add(new WallPiece(coord));
                } else if(y == Radius * 2 - 1) {
                    XY coord = new XY((int)(center.x - Radius) + x, (int)(center.y - Radius) + y);
                    WallPieces.Add(new WallPiece(coord));
                } else if(x == 0) {
                    XY coord = new XY((int)(center.x - Radius) + x, (int)(center.y - Radius) + y);
                    WallPieces.Add(new WallPiece(coord));
                } else if(x == Radius * 2 - 1) {
                    XY coord = new XY((int)(center.x - Radius) + x, (int)(center.y - Radius) + y);
                    WallPieces.Add(new WallPiece(coord));
                }
            }
        }
    }
}
