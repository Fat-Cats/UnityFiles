using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMainScript : MonoBehaviour {

    public List<player> playerList = new List<player>(); //list of players, details of which should be passed fom the main menu
    public player currentPlayer; //stores the player that is currently making their move

	// Use this for initialization
	void Start()
    {
        //add players to player list
        playerList.Add(new player("1", 100, "ALC"));
        playerList.Add(new player("2", 50, "CON"));
        playerList.Add(new player("3", 100, "DER"));
        playerList.Add(new player("4", 50, "GOO"));
        playerList.Add(new player("5", 100, "HAL"));
        playerList.Add(new player("6", 50, "JAM"));
        playerList.Add(new player("7", 100, "LAN"));
        playerList.Add(new player("8", 50, "VAN"));
        playerList.Add(new player("9", 100, "WEN"));

        this.currentPlayer = playerList[0]; //currentPlayer does not change for testing purposes

        this.transform.GetChild(0).GetComponent<gameMapScript>().gameMapStart();
    }
	
	// Update is called once per frame
	void Update () {
    }


}
