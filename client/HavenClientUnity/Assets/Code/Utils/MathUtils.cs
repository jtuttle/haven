using UnityEngine;
using System.Collections;

public class MathUtils {
    public static int Clamp(int value, int min, int max) {
        return (value < min) ? min : (value > max) ? max : value;
    }
}
