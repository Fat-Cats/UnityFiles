using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player {

    public string playerName; //string containing the chosen name of this player
    public string collegeRep; //this string indicates which college a player represents
                              //colleges are stored as 3 upper case letters (E.G: "HAL" - halifax, "GOO" - Goodricke...)
                              //each college has a predefined representative colour
    public Color teamColour; //color indicating this college's territory on the map
    public int playerCurrency; //amount of currency this player currently has (can be used to purchase units)

    public player(string name, int currency, string college) //called when a new player is created
    {
        this.playerName = name; //set playerName
        this.playerCurrency = currency; //set currency
        this.collegeRep = college; //set college

        switch(college) //set teamColour based on college
        {
            case "ALC":
                this.teamColour = Color.red;
                break;

            case "CON":
                this.teamColour = Color.magenta;
                break;

            case "DER":
                this.teamColour = new Color((float)154/255, (float)36/255, 0f, (float)61/255); //but it the colour looks good
                break;

            case "GOO":
                this.teamColour = Color.green; 
                break;

            case "HAL":
                this.teamColour = Color.blue;
                break;

            case "JAM":
                this.teamColour = Color.black;
                break;

            case "LAN":
                this.teamColour = Color.yellow;
                break;

            case "VAN":
                this.teamColour = Color.cyan;
                break;

            case "WEN":
                this.teamColour = Color.white; 
                break;
        }
    }
}
