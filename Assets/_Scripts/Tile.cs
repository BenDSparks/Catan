using System;
using UnityEngine;

public class Tile
{
    public GameObject gameObject;
    public ResourceType resourceType;
    public int resourceNumber;
    public int x;
    public int y;
    public bool isRobbed;
    private MeshRenderer meshRenderer;
    private Material startingMaterial;

    public Tile(GameObject gameObject, int x, int y, ResourceType resourceType)
    {
        this.gameObject = gameObject;
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        startingMaterial = meshRenderer.material;
        isRobbed = false;
        this.x = x;
        this.y = y;
        this.resourceType = resourceType;
    }

    public Tile(){
        gameObject = new GameObject();
    }

    

}

