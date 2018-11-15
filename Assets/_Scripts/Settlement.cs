using System;
using UnityEngine;

public class Settlement {

    public GameObject gameObject;
    private int x;
    private int y;
    public bool isOccupied;


    public Settlement(GameObject gObject, int x, int y) {
        this.gameObject = gObject;
        this.x = x;
        this.y = y;
        isOccupied = false;
    }

    public int getX() {
        return x;
    }

    public int getY() {
        return y;
    }
}

