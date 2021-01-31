using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MovementData {
    public Vector3 endPos;
    public Effect playerEffect;
}

public enum Effect {
    None, Pit, Wall, Win, Helmet
}

public enum Direction {
    Up, Down, Left, Right
}

public struct grid{
    public static float cellSizeX = 1.2f;
    public static float cellSizeY = 1f;
    public static int gridWidth = 9;
    public static int gridHeight = 9;
}
