using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameMainScript : MonoBehaviour {

    public List<GameObject> unitList = new List<GameObject>(); // list of units
    public List<player> playerList = new List<player>(); //list of players, details of which should be passed fom the main menu
    public Canvas battleDisplay;                        // Canvas used to display battles
    public GameObject gameMap;
    private float counter;


    // Use this for initialization
    public gameMainScript()
    {
        //add players to player list
        playerList.Add(new player("richard", Color.red, 100));
        playerList.Add(new player("jamie", Color.blue, 50));
    }

    public void Awake()
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
        int damage = unitAScript.GetAttack();
        if (unitAScript.GetAttack() < unitBScript.GetCurHP())
        {
            unitBScript.Damage(damage); // Placeholder calculation, will be replace by more sophisticated method
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
        battleDisplay.enabled = true;                                                       // Display the canvas
        Image[] sprites = battleDisplay.GetComponentsInChildren<Image>();                   // Setup the appropriate sprites for the units involved.
        sprites[1].sprite = unitA.GetComponent<SpriteRenderer>().sprite;
        sprites[2].sprite = unitB.GetComponent<SpriteRenderer>().sprite;
        sprites[2].transform.localRotation = Quaternion.Euler(0, 180, 0);
        // TODO
        // Play attack animation and damage number
        // Wait appropriate time
        // Disable display

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("p")){
            Battle(unitList[0], unitList[1]); //TEST
            
        }
		
	}

}
