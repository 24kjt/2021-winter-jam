using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MovementData {
    public Vector3 endPos;
    public Effect playerEffect;
}

public enum Effect {
    None, Pit, Wall, Win
}

public enum Direction {
    Up, Down, Left, Right
}
