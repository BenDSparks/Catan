using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour {

    public int diceCount = 2;
    public int[] diceValues;
    public int diceTotal;

    // Use this for initialization
    void Start () {
        diceValues = new int[diceCount];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RollTheDice() {
        diceTotal = 0;
        for (int i = 0; i < diceValues.Length; i++) {
            diceValues[i] = Random.Range(1, 7);
            diceTotal += diceValues[i];
        }

        Debug.Log("Rolled: " + diceValues + " (" + diceTotal + ")");
    }
    
}
