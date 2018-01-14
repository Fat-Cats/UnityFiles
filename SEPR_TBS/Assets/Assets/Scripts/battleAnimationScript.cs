using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class battleAnimationScript : MonoBehaviour {

    //stores the canvas' used to represent battling unit's
    public GameObject battlingUnit1;
    public GameObject battlingUnit2;

    unitScript defenderUnitScript; //scripts of battling unit's 
    unitScript attackerUnitScript; //used to get stats of either unit

    //stores the battling unit's animators
    Animator unit1Anim;
    Animator unit2Anim;

    //public Transform canvas;
    public GameObject gameMap;

    public Camera cameraRef; //Used to change the background color during animation from green to black

    void Start()
    {
        unit1Anim = battlingUnit1.GetComponent<Animator>(); //fetch both unit's animator components
        unit2Anim = battlingUnit2.GetComponent<Animator>();
    }


    public void fightAnimation (GameObject attackingUnit, GameObject defendingUnit)
    {
        defenderUnitScript = defendingUnit.GetComponent<unitScript>(); //set battling unit's unit scripts
        attackerUnitScript = attackingUnit.GetComponent<unitScript>();

        //Calculate damage done to each unit and consequently their new health points

        System.Random percentageOfCritical = new System.Random(); //used to calculate random numbers used in chance based attacks
        float randomInteger; //used in calculating whether units get critical hits

        //get unit stats
        int defenderHealth = defenderUnitScript.curHP;
        int attackerHealth = attackerUnitScript.curHP;
        double defenderCritical = defenderUnitScript.critical;
        double attackerCritical = attackerUnitScript.critical;
        double attackerAccuracy = attackerUnitScript.accuracy;
        int defenderDefence = defenderUnitScript.defence;
        int attackerAttack = attackerUnitScript.attack;

        //new defending unit health is:
        //defenderCurrentHP - (attackerAttack - defenderDefence) [ * attackerCritical / attackerAccuracy is the chance for a critical to occur ]

        int defenderNewHealth; //used to store the defender's new health

        randomInteger = percentageOfCritical.Next(0, 100) / 100; //get random 2 digit float between 0.00 and 1.00

        if (randomInteger < attackerAccuracy) //determine if critical is applied
        {
            defenderNewHealth = Convert.ToInt32(Math.Floor(defenderHealth - ((attackerAttack - defenderDefence) * attackerCritical)));
        }
        else
        {
            defenderNewHealth = defenderHealth - (attackerAttack - defenderDefence);
        }
        if (defenderNewHealth < 0) { defenderNewHealth = 0; }; //health values cannot be negative
        //========================================================================
        //Need to place code here to kill unit in game
        //========================================================================
        
        //attacker is only damaged if the defender achieves a critical (the chance of which is their critical stat). If they do, the damage done to the
        //attacker is the defending units defence

        int attackerNewHealth; //used to store the attacker's new health

        randomInteger = percentageOfCritical.Next(0, 100) / 100; //get random 2 digit float between 0.00 and 1.00

        if (randomInteger < defenderCritical) //if the defender has a critical, the attacker is damaged by the defenders defence
        {
            attackerNewHealth = attackerHealth - defenderDefence;
        }
        else
        {
            attackerNewHealth = attackerHealth;
        }
        if (defenderNewHealth < 0) { defenderNewHealth = 0; }; //health values cannot be negative
        //========================================================================
        //Need to place code here to kill unit in game
        //========================================================================

        battlingUnit1.GetComponent<battleAnimationUnit1Script>().attackingUnit = attackingUnit; //set units in battle animation scripts, so that their health
        battlingUnit2.GetComponent<battleAnimationUnit2Script>().defendingUnit = defendingUnit; //stats can be read and the health bar can be changed accordingly

        battlingUnit1.GetComponent<Image>().sprite = attackingUnit.GetComponent<SpriteRenderer>().sprite; //set battlingUnit canvas' to appropriate sprites
        battlingUnit2.GetComponent<Image>().sprite = defendingUnit.GetComponent<SpriteRenderer>().sprite;

        gameMap.SetActive(false); //stop displaying the map in the background of the scene

        this.gameObject.SetActive(true); //display canvas of battle scene

        cameraRef.backgroundColor = Color.black; //change background colour to black before playing fight animation

        //set healthbar values of both unit's before the attack animation is commenced and new values are applied
        battlingUnit1.GetComponent<battleAnimationUnit1Script>().updateHealthBar(attackerUnitScript.maxHP, attackerHealth);
        battlingUnit2.GetComponent<battleAnimationUnit2Script>().updateHealthBar(defenderUnitScript.maxHP, defenderHealth);

        //set new health point values (which were calculated earlier in code)
        attackerUnitScript.curHP = attackerNewHealth;
        defenderUnitScript.curHP = defenderNewHealth;

        unit1Anim.Play("attackAnimation"); //play the attacking unit's battle animation
        //once this animation has exited, the function "attackAnimationIsComplete" in "battleAnimationUnit1Script" will be called

    }

    public void defendAnimation ()
    {
        unit1Anim.Play("New State"); //set attacker's animation to idle

        //update health bar
        battlingUnit2.GetComponent<battleAnimationUnit2Script>().updateHealthBar(defenderUnitScript.maxHP, defenderUnitScript.curHP);

        unit2Anim.Play("defendAnimation"); //start the defender's defend animation
    }

    public void fightAnimationComplete()
    {
        unit2Anim.Play("New State"); //set defender's animation to idle

        cameraRef.backgroundColor = new Color32(147, 179, 128, 0); //after fight animation is complete, change background colour back to green (to match grass)

        this.gameObject.SetActive(false); //stop displaying the battle animation scene

        gameMap.SetActive(true); //display the game map

        attackerUnitScript.killIfDead(); //check if either unit is dead, if so delete that unit from the game
        defenderUnitScript.killIfDead();
    }
}
