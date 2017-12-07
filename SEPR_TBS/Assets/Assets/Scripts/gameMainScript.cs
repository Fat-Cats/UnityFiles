using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMainScript : MonoBehaviour {

    public List<player> playerList = new List<player>(); //list of players, details of which should be passed fom the main menu


	// Use this for initialization
	public gameMainScript()
    {
        //add players to player list
        playerList.Add(new player("richard", Color.red, 100));
        playerList.Add(new player("jamie", Color.blue, 50));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
