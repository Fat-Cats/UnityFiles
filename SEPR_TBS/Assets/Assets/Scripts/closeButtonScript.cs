﻿//HEADER
//game executable can be found at: https://drive.google.com/open?id=18i6A5XMkF-5kVlz-RSsyYvMSnSwSpPnT
//HEADER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class closeButtonScript : MonoBehaviour, IPointerClickHandler {

    public GameObject buyMenuCanvas; //references the "buyUnitCanvas" canvas, which is used to purchase units
    public GameObject gameMap; //references the "gameMap" GameObject, which is used to display the game map (including unit's, buildings, sectors etc...)

    public void OnPointerClick(PointerEventData eventData) //when this button is clicked, close the buy menu
    {
        buyMenuCanvas.SetActive(false); //close the buyMenuCanvas
        gameMap.SetActive(true); //open the game map
    }

    void Start () { //at the start of the game, this script is run
        buyMenuCanvas = this.transform.parent.gameObject; //sets a reference to the buyMenuCanvas (which is it's parent) 
        GameObject theGameMain = buyMenuCanvas.GetComponent<unitCanvasScript>().gameMain; //gets a reference to the game main gameobject
        gameMap = theGameMain.transform.GetChild(0).gameObject; //set a reference to the game map gameobject


    }
}
