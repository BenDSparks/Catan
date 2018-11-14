using System;
using UnityEngine;

public class Player {

    int playerNumber;
    PlayerColor playerColor;
    int settlments;
    int cities;
    int roads;
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
    }
}
