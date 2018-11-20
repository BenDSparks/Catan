using System;
using UnityEngine;

public class Settlement {

    public GameObject gameObject;
    private int x;
    private int y;
    public bool isOccupied;
    public bool isAvailable;
    public bool isCity;
    private MeshRenderer meshRenderer;
    private Material startingMaterial;
    private GameObject visual;

    public Settlement(GameObject gameObject, int x, int y) {
        this.gameObject = gameObject;
        this.x = x;
        this.y = y;
        isOccupied = false;
        isAvailable = true;
        isCity = false;
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        startingMaterial = meshRenderer.material;
        visual = gameObject.transform.GetChild(0).gameObject;
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

    public void showCollider() {
        BoxCollider collider = visual.GetComponent<BoxCollider>();

        
        collider.enabled = true;
        
    }

    public void hideCollider() {
        BoxCollider collider = visual.GetComponent<BoxCollider>();

        collider.enabled = false;

    }
}

