using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buildingScript : MonoBehaviour {

    Transform buyUnitCanvas; //this is stored as a reference to the buyUnitCanvas canvas. it is used to open the buy units menu when a building is clicked
    GameObject gameMap; //this is a reference to the gameMap. it is used to close the game map when using the buy units menu
    GameObject sector; //this stores the sector on which this building is placed

    public void Init(string buildingName, GameObject sector, Transform buyUnitCanvas, GameObject theGameMap) //called to initialize building after instantiation
    {
        this.buyUnitCanvas = buyUnitCanvas; //set buyUnitCanvas
        this.gameMap = theGameMap; //set theGameMap
        this.sector = sector; //set sector on which this building is placed

        this.gameObject.transform.SetParent(sector.gameObject.transform); //change building's parent to the sector that it is placed on

        //create a vector representing this objects position in the game (in reference to the sector on which it is placed)
        Vector3 currentPosition = sector.transform.position; //set currentPosition to the position of the sector it is placed on

        switch (buildingName) //depending on building type, select appropriate sprite and place accordingly
        {
            case "CentralHall":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\CentralHall")[1] as Sprite;
                this.transform.localScale = new Vector3(2, 2, 1); //fine tune building position and scale
                currentPosition.x += -0.19f; 
                currentPosition.y += -0.51f;
                currentPosition.z += -0.5f;
                break;

            case "HesHall":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\HesHall")[1] as Sprite;
                this.transform.localScale = new Vector3(1.7f, 1.7f, 1); //fine tune building position and scale
                currentPosition.x += 0.85f; 
                currentPosition.y += 0.16f;
                currentPosition.z += -0.5f;
                break;

            case "JBMorrell":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\JBMorrell")[1] as Sprite;
                this.transform.localScale = new Vector3(2, 2, 1); //fine tune building position and scale
                currentPosition.x += -0.09f;
                currentPosition.y += 0.55f;
                currentPosition.z += -0.5f;
                break;

            case "Nisa":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\Nisa")[1] as Sprite;
                currentPosition.x += 0.29f; //fine tune building position and scale
                currentPosition.y += 0.47f;
                currentPosition.z += -0.5f;
                break;

            case "RCH":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\RCH")[1] as Sprite;
                this.transform.localScale = new Vector3(0.5f, 1, 1); //new Vector3(1, 2, 1); //fine tune building position and scale
                currentPosition.x += -0.05f;
                currentPosition.y += 0.55f;
                currentPosition.z += -0.5f;
                break;

            case "SportsVillage":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\SportsVillage")[1] as Sprite;
                this.transform.localScale = new Vector3(1.8f, 1.8f, 1); //fine tune building position and scale
                this.transform.Rotate(new Vector3(0f, 0f, -6.07f));
                currentPosition.x += -0.26f;
                currentPosition.y += 0.86f;
                currentPosition.z += -0.5f;
                break;

            case "track":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\track")[1] as Sprite;
                this.transform.localScale = new Vector3(0.2f, 0.2f, 1); //fine tune building position and scale
                this.transform.Rotate( new Vector3(0f, 0f, -50f) );
                currentPosition.x += 0.05f;
                currentPosition.y += -0.29f;
                currentPosition.z += -0.5f;
                break;

            default:
                Debug.Log("that building type is not recognised");
                    break;
        }

        this.gameObject.GetComponent<Transform>().position = currentPosition; //set this buildings position 

        this.gameObject.AddComponent<PolygonCollider2D>(); //add polygoncollider component to this building so that it can be clicked by the user
    }

    void OnMouseDown() //when a building is clicked, it should open the unit buy menu and hide the game map
    {
        gameMapScript theGameMapScript = this.gameMap.GetComponent<gameMapScript>(); //set a reference to the gameMapScript script of the gameMap
        GameObject theSelectedUnit = theGameMapScript.selectedUnit; //set a reference to the gameMap's selected unit to see if a unit was selected before this sector was clicked on

        theGameMapScript.selectedUnit = null; //a sector has now been selected, so set the selectedUnit to null

        if (theSelectedUnit != null && theSelectedUnit.GetComponent<unitScript>().canMoveTo().Contains(this.sector)) //if a unit has been selected, and this building has been clicked, move 
        {                                                                                                                //that unit to this sector, if it is within range
            unitScript theSelectedUnitScript = theSelectedUnit.GetComponent<unitScript>(); //set a reference to the selected unit's unitScript script
            theSelectedUnitScript.moveUnit(this.sector); //move unit to this sector
        }
        else if (theSelectedUnit == null)
        {
            this.buyUnitCanvas.gameObject.SetActive(true); //open the unit buying canvas
            this.gameMap.gameObject.SetActive(false); //close the game map
            buyUnitCanvas.GetComponent<unitCanvasScript>().warningMessage.gameObject.SetActive(false); //do not display the warningMessage
            //set the "spawnPoint" value in the buyUnitCanvas' "unitCanvasScript" script so that newly purchased units will spawn on the same sector
            //as the building that they clicked to open the buy unit menu
            this.buyUnitCanvas.GetComponent<unitCanvasScript>().setSpawnPoint(sector.gameObject);
        }
    }
}
