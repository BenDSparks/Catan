using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCardUI : MonoBehaviour {


    public Text brickText;
    public Text woodText;
    public Text sheepText;
    public Text wheatText;
    public Text oreText;





	// Use this for initialization
	void Start () {
        brickText.text = "0";
        woodText.text = "0";
        sheepText.text = "0";
        wheatText.text = "0";
        oreText.text = "0";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setResources(Player player) {
        //print(player.brickCount + " " + player.woodCount + " " + player.sheepCount + " " + player.wheatCount + " " + player.woodCount);
        //print(player.brickCount.ToString() + " " + player.woodCount.ToString() + " " + player.sheepCount.ToString() + " " + player.wheatCount.ToString() + " " + player.woodCount.ToString());

        brickText.text = player.brickCount.ToString();
        woodText.text = player.woodCount.ToString();
        sheepText.text = player.sheepCount.ToString();
        wheatText.text = player.wheatCount.ToString();
        oreText.text = player.oreCount.ToString();
    }
}
