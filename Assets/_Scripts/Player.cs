using System;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    public int playerNumber;
    public PlayerColor playerColor;
    public int settlmentCount;
    public int cityCount;
    public int roadCount;
    bool isTurn;


    public int brickCount;
    public int woodCount;
    public int sheepCount;
    public int wheatCount;
    public int oreCount;
    public int goldCount;

    public List<Settlement> settlements;
    public List<Road> roads;

    GameObject gameObject; 

    public Player(GameObject gameObject, int playerNumber) {
        this.gameObject = gameObject;
        this.playerNumber = playerNumber;
        settlmentCount = 5;
        cityCount = 4;
        roadCount = 15;

        isTurn = false;

        brickCount = 0;
        woodCount = 0;
        sheepCount = 0;
        wheatCount = 0;
        oreCount = 0;
        goldCount = 0;

        settlements = new List<Settlement>();
        roads = new List<Road>();
    }
}
