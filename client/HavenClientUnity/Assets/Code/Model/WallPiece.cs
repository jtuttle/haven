using UnityEngine;
using System.Collections;

public class WallPiece {
    public XY Coord { get; private set; }

    public int Health { get; private set; }

    public WallPiece(XY coord) {
        Coord = coord;
        Health = 3;
    }
}
