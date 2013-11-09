using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Wall Wall { get; private set; }

    public Map(int size) {
        // make map size odd so that we can have a center square
        if(size % 2 != 0)
            size = size + 1;

        Width = size;
        Height = size;
        
        Wall = new Wall(5);
    }
}
