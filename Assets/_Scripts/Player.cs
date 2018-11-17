using System;
using UnityEngine;

public class Player {

    public int playerNumber;
    public PlayerColor playerColor;
    public int settlments;
    public int cities;
    public int roads;
    bool isTurn;

    int brickCount;
    int woodCount;
    int sheepCount;
    int wheatCount;
    int oreCount;
    int goldCount;

    GameObject gameObject; 

    public Player(GameObject gameObject, int playerNumber) {
        this.gameObject = gameObject;
        this.playerNumber = playerNumber;
        settlments = 5;
        cities = 4;
        roads = 15;

        isTurn = false;

        brickCount = 0;
        woodCount = 0;
        sheepCount = 0;
        wheatCount = 0;
        oreCount = 0;
        goldCount = 0;

    }
}
