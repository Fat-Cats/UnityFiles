﻿//HEADER
//game executable can be found at: https://drive.google.com/open?id=18i6A5XMkF-5kVlz-RSsyYvMSnSwSpPnT
//HEADER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class battleAnimationUnit1Script : MonoBehaviour {

    public Transform fightCanvas; //stores the fightCanvas canvas, used to call the "defendAnimation" function when the attack animation is complete
    public Transform myHealthBar; //The health bar of the attacking unit

    public GameObject attackingUnit; //stores the GameObject of the attacking unit

    public void attackAnimationIsComplete() //This function is called when unit 1's attack animation is done playing.
    {                                       //it then calls a function from its canvas parent to start battle unit 2's defending animation

        int attackingUnitCurHealth = attackingUnit.GetComponent<unitScript>().curHP; //fetch new health point values
        int attackingUnitMaxHealth = attackingUnit.GetComponent<unitScript>().maxHP; 

        updateHealthBar(attackingUnitMaxHealth, attackingUnitCurHealth); //display new health point values on health bar

        fightCanvas.GetComponent<battleAnimationScript>().defendAnimation(); //call a function from canvas which plays the defending animation
    }

    public void updateHealthBar(int fullHealth, int currentHealth) //this function updates a battling unit's health bar
    {
        GameObject healthBar = this.transform.GetChild(0).gameObject; //fetch healthbar which is a child GameObject

        GameObject bar = healthBar.transform.GetChild(0).gameObject; //get the green coloured bar which scales with health
        bar.transform.localScale = new Vector3( (float)currentHealth/(float)fullHealth, 1, 1) ; //green bar covers a percentage of the full health bar
                                                                                                //representing remaining health points

        GameObject healthText = healthBar.transform.GetChild(1).gameObject; //get the text indicating unit's health

        healthText.GetComponent<Text>().text = currentHealth.ToString() + "/" + fullHealth.ToString(); //display new health points in text
    }
}
