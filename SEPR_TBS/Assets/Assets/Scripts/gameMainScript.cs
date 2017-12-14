using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameMainScript : MonoBehaviour {

    public List<GameObject> unitList = new List<GameObject>(); // list of units
    public List<player> playerList = new List<player>(); //list of players, details of which should be passed fom the main menu
    public Canvas battleDisplay;                        // Canvas used to display battles
    public GameObject gameMap;
    Animator battleAnimatorA;
    Animator battleAnimatorB;


    // Use this for initialization
    public gameMainScript()
    {
        //add players to player list
        playerList.Add(new player("richard", Color.red, 100));
        playerList.Add(new player("jamie", Color.blue, 50));
    }

    public void Start()
    {
        GameObject temp = GameObject.Find("DisplayBattle");  // Finding the Object holding the canvas
        if (temp != null)
        {
            battleDisplay = temp.GetComponent<Canvas>(); // Finding the Canvas itself
        }
        battleDisplay.enabled = false; // Initially disable the Battle Display Canvas
    }

    public void Battle(GameObject unitA, GameObject unitB) // This unit is the aggressor, enemy unit is the defender. This function will deal with the changing of stats, as well as call the function in GameManager to display the battle.
    {
        unitScript unitAScript = unitA.GetComponent<unitScript>();
        unitScript unitBScript = unitB.GetComponent<unitScript>();
        int damage = unitAScript.GetAttack();  // Placeholder calculation, will be replace by more sophisticated method
        if (unitAScript.GetAttack() < unitBScript.GetCurHP())
        {
            unitBScript.Damage(damage);
            DisplayBattle(unitA, unitB, damage, false);
        }
        else
        {
            unitBScript.Kill();
            DisplayBattle(unitA, unitB, damage, true);
        }

    }

    public void DisplayBattle(GameObject unitA, GameObject unitB, int damage, bool isDead) // unitA is aggressor, unitB is defender. Will bring up background map and correct units, display attack animation and correct damage number.
    {  
        battleDisplay.enabled = true;       // Display the canvas
        GameObject temp1 = GameObject.Find("unitA");
        battleAnimatorA = temp1.GetComponent<Animator>();
        GameObject temp2 = GameObject.Find("unitB");
        battleAnimatorB = temp2.GetComponent<Animator>();
        temp1.GetComponent<Image>().sprite = unitA.GetComponent<SpriteRenderer>().sprite;
        temp2.GetComponent<Image>().sprite = unitB.GetComponent<SpriteRenderer>().sprite; // setup correct sprites
        temp2.GetComponent<Image>().transform.localRotation = Quaternion.Euler(0, 180, 0);
        battleAnimatorA.SetBool("Battle", true);                                          // Play the battle animation for both units (at the moment only attacking unit)
        Invoke ("Animating", 1);                                                          // Timer to stop animation playing twice
        

        // TODO
        // Display damage number
        // Alter animation for unitB if isDead is true.
    }

    private void Animating()                                                              // This function stops the attack animation playing more than once, also times the end of the battle.
    {
        battleAnimatorA.SetBool("AnimationPlaying", true);
        Invoke("EndDisplayBattle", 1);
    }

    private void EndDisplayBattle()                                                       // This function resets any variables used and causes the battle canvas to be hidden again.
    {
        battleAnimatorA.SetBool("Battle", false);
        battleAnimatorA.SetBool("AnimationPlaying", false);
        battleDisplay.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("p")){
            Battle(unitList[0], unitList[1]); //TEST
            
        }
		
	}

}
