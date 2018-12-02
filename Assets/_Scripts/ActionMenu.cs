using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour {

    public Button endTurnButton;
    public Button buildButton;
    public GameObject actionMenuBox;
    public GameObject buildMenuBox;
    public GameObject cancelBox;


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
    
    public void disableActionMenuBox() {
        actionMenuBox.SetActive(false);
    }

    public void enableActionMenuBox() {
        print("TEST");
        actionMenuBox.SetActive(true);
    }

    public void disableBuildMenuBox() {
        buildMenuBox.SetActive(false);
    }

    public void enableBuildMenuBox() {
        buildMenuBox.SetActive(true);
    }

    public void toggleBuildMenuBox() {
        if (buildMenuBox.activeSelf == true) {
            buildMenuBox.SetActive(false);
        }
        else {
            buildMenuBox.SetActive(true);
        }
    }

    public void disableCancelBox() {
        cancelBox.SetActive(false);
    }

    public void enableCancelBox() {
        cancelBox.SetActive(true);
    }

    public void enableCancelBoxOnly() {
        
        disableActionMenuBox();
        disableBuildMenuBox();
        enableCancelBox();
    }

    public void disableAll() {
       
        disableActionMenuBox();
        disableBuildMenuBox();
        disableCancelBox();
    }

  

    

}
