using System;
using UnityEngine;

public class Road {

    public GameObject gameObject;
    private int x;
    private int y;
    private MeshRenderer meshRenderer;
    private Material startingMaterial;
    public bool isOccupied;
    public bool isAvailable;
    public int playerNumber;
    private GameObject visual;

    public Road(GameObject gObject, int x, int y) {
        this.gameObject = gObject;
        
        this.x = x;
        this.y = y;
        isOccupied = false;
        isAvailable = false;
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        startingMaterial = meshRenderer.material;
        visual = gameObject.transform.GetChild(0).gameObject;
        playerNumber = -1;
    }

    public int getX() {
        return x;
    }

    public int getY() {
        return y;
    }

    public void resetColor() {
        meshRenderer.material = startingMaterial;
    }

    public void setColor(Material material) {
        meshRenderer.material = material;
    }

    public void showVisual() {
        //meshRenderer.enabled = true;
        visual.SetActive(true);
    }

    public void hideVisual() {
        //meshRenderer.enabled = false;
        visual.SetActive(false);
    }

}
