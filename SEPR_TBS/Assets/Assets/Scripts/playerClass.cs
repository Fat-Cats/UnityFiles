using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player {

    public string playerName; //string containing the chosen name of this player
    public string collegeRep; //this string indicates which college a player represents
                              //colleges are stored as 3 upper case letters (E.G: "HAL" - halifax, "GOO" - Goodricke...)
                              //each college has a predefined representative colour
    public Color teamColour; //color indicating this players territory on the map
    public int playerCurrency; //amount of currency this player currently has (can be used to purchase units)

    public player(string name, int currency, string college) //called when a new player is created
    {
        this.playerName = name; //set locally stored playerName
        this.playerCurrency = currency; //set locally stored currency
        this.collegeRep = college; //set locally stored college
        switch(college)
        {
            case "ALC": //Alcuin's team colour is red
                this.teamColour = Color.red;
                break;

            case "CON":
                this.teamColour = Color.magenta;
                break;

            case "DER":
                this.teamColour = Color.grey;
                break;

            case "GOO":
                this.teamColour = Color.green;
                break;

            case "HAL":
                this.teamColour = Color.blue;
                break;

            case "JAM":
                this.teamColour = Color.white;
                break;

            case "LAN":
                this.teamColour = Color.yellow;
                break;

            case "VAN":
                this.teamColour = Color.cyan; //new Color(0.5f, 0, 1, 1);
                break;

            case "WEN":
                this.teamColour = Color.black; //new Color(1, 0.5f, 0, 1);
                break;
        }
    }
}
