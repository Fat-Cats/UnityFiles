using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player {

    public string playerName; //string containing the chosen name of this player
    public Color teamColour; //color indicating this players territory on the map
    public int playerCurrency; //amount of currency this player currently has (can be used to purchase units)

    public player(string name, Color teamColour, int currency) //called when a new player is created
    {
        this.playerName = name;
        this.teamColour = teamColour;
        this.playerCurrency = currency;
    }

    /*public void init (string name, Color teamColour, int currency)
    {
        this.playerName = name;
        this.teamColour = teamColour;
        this.playerCurrency = currency;
    }*/



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
