using System;
using UnityEngine;

public class Settlement {

    public GameObject gameObject;
    private int x;
    private int y;
    public bool isOccupied;
    public bool isAvailable;
    private MeshRenderer meshRenderer;
    private Material startingMaterial;

    public Settlement(GameObject gameObject, int x, int y) {
        this.gameObject = gameObject;
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        startingMaterial = meshRenderer.material;
        this.x = x;
        this.y = y;
        isOccupied = false;
        isAvailable = true;
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
}

