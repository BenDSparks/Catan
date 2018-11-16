using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlementData : MonoBehaviour {


    private int x, y;

    public void setPosition(int x, int y) {
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
