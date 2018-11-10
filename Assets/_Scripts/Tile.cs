using System;
using UnityEngine;

public class Tile
{
    public GameObject gameObject;
    public ResourceType resourceType;
    public int resourceNumber;
    public int x;
    public int y;

    public Tile(GameObject gObject,int x, int y)
    {
        this.gameObject = gObject;
        this.x = x;
        this.y = y; 
    }

    public Tile(){
        gameObject = new GameObject();
    }

 

}

