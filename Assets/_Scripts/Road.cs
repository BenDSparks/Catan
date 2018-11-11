using System;
using UnityEngine;

public class Road {

    public GameObject gameObject;
    private int x;
    private int y;


    public Road(GameObject gObject, int x, int y) {
        this.gameObject = gObject;
        this.x = x;
        this.y = y;
    }

    public int getX() {
        return x;
    }

    public int getY() {
        return y;
    }
}
