using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

    public string playerName;
    public Color teamColour;
    public int playerCurrency;

    public player(string name, Color teamColour, int currency) //called when a new player is created
    {
        this.playerName = name;
        this.teamColour = teamColour;
        this.playerCurrency = currency;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
