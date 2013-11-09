using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Wall Wall { get; private set; }

    public Map(int width, int height) {
        Width = width;
        Height = height;

        Wall = new Wall(Width, Height, 10);
    }
}
