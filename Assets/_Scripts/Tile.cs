using System;
using UnityEngine;

public class Tile
{
    public GameObject gameObject;
    public ResourceType resourceType;
    public int resourceNumber;

    public Tile(GameObject gObject)
    {
        this.gameObject = gObject;
    }

    public Tile(){
        gameObject = new GameObject();
    }
}

