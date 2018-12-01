using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour {

    public Button endTurnButton;
    public GameObject menuBox;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

    public void disableEndTurnButton() {
        endTurnButton.gameObject.SetActive(false);
    }

    public void enableEndTurnButton() {
        endTurnButton.gameObject.SetActive(true);
    }
    
    public void disableMenuBox() {
        menuBox.SetActive(false);
    }

    public void enableMenuBox() {
        menuBox.SetActive(true);
    }

    public void disableAll() {
        disableEndTurnButton();
        disableMenuBox();
    }

    public void enableAll() {
        enableEndTurnButton();
        enableMenuBox();
    }

    

}
