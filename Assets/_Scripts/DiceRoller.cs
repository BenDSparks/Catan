using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour {

    private int diceCount = 2;
    public int[] diceValues;
    public int diceTotal;
    public Button button;
    public GameObject diceBox;
    private Text diceTotalText;
    public Sprite[] redDie;
    public Sprite[] whiteDie;
    public Image redDieImage;
    public Image whiteDieImage;

    // Use this for initialization
    void Start () {
        //print("Dice count " + diceCount);
        diceValues = new int[diceCount];
        diceTotalText = diceBox.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RollTheDice() {
        diceTotal = 0;
        for (int i = 0; i < diceValues.Length; i++) {
            diceValues[i] = Random.Range(1, 7);
            //print("i: " + i + " " + diceValues[i]);
            diceTotal += diceValues[i];
            
        }
        diceTotalText.text = diceTotal.ToString();
        whiteDieImage.sprite = whiteDie[diceValues[0] - 1];
        redDieImage.sprite = redDie[diceValues[1] - 1];
        //Debug.Log("Rolled: " + diceValues[0] + " " + diceValues[1] + " (" + diceTotal + ")");


    }

    public void disableRollDiceButton() {
        button.gameObject.SetActive(false);
    }

    public void enableRollDiceButton() {
        button.gameObject.SetActive(true);
    }
    
    public void disableDiceBox() {
        diceBox.SetActive(false);
    }

    public void enableDiceBox() {
        diceBox.SetActive(true);
    }

    public void disableAll() {
        disableRollDiceButton();
        disableDiceBox();
    }

    public void enableAll() {
        enableRollDiceButton();
        enableDiceBox();
    }

    

}
