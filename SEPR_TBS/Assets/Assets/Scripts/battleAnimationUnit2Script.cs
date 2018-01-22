//HEADER
//game executable can be found at: https://drive.google.com/open?id=18i6A5XMkF-5kVlz-RSsyYvMSnSwSpPnT
//HEADER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class battleAnimationUnit2Script : MonoBehaviour {

    public Transform fightCanvas; //stores reference to the fightCanvas canvas

    public GameObject defendingUnit; //used to store GameObject of defending unit 

    public void defendAnimationIsComplete() //This function is called when unit 2's defend animation is done playing.
    {                                       //it then calls a function from its canvas parent to end the battle scene

        fightCanvas.GetComponent<battleAnimationScript>().fightAnimationComplete();

    }

    public void updateHealthBar(int fullHealth, int currentHealth) //this function updates a battling unit's health bar
    {
        GameObject healthBar = this.transform.GetChild(0).gameObject; //fetch healthbar child

        GameObject bar = healthBar.transform.GetChild(0).gameObject; //get the green coloured bar for scaling with health
        bar.transform.localScale = new Vector3((float)currentHealth / (float)fullHealth, 1, 1); //green bar covers a percentage of the full health bar
                                                                                                //representing remaining health points

        GameObject healthText = healthBar.transform.GetChild(1).gameObject; //get the text indicating unit's health

        healthText.GetComponent<Text>().text = currentHealth.ToString() + "/" + fullHealth.ToString(); //display new health points in text
    }

}
